using MySql.Data.MySqlClient;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BasicTemplate.Base
{
    internal class DBMS
    {
        private static MySqlConnection UserConn;
        private static ConnectionInfo SFTPConn;

        private static bool bIsUserConnected;
        private static bool bHaveAuth;

        private const string ServerIP = "*SERVER*";
        private const string ServerPort = "*PORT*";
        private const string DB = "*DBNAME*";

        private const string FileTable = "*FileTable*";

        private const int ScanTimeout = 30000;

        public static bool Connect(string Id, string Pwd)
        {
            string ConnStr = string.Format("Server = {0};" +
                                            "Port = {1};" +
                                            "Database = {2};" +
                                            "Uid = {3};" +
                                            "Pwd = {4};", 
                                            ServerIP,ServerPort,DB,Id, Pwd);

            try
            {
                // Try connection
                UserConn = new MySqlConnection(ConnStr);
                UserConn.Open();

                bIsUserConnected = true;

                // Does user have auth?
                MySqlCommand Cmd = new MySqlCommand(
                    // Query
                    string.Format("SELECT * FROM {0}.UserInfoList WHERE UserName IN('{0}')", Id)
                    // Connection
                    , UserConn
                    );

                using (MySqlDataReader Reader = Cmd.ExecuteReader())
                {
                    if (Reader.Read())
                        bHaveAuth = Reader.GetBoolean(Reader.GetOrdinal("Auth"));
                }

                // SFTP Connection
                SFTPConn =
                    new ConnectionInfo(ServerIP, Id, new PasswordAuthenticationMethod(Id, Pwd));

                return true;
            }
            catch
            {
                return false;
            }
        }


        public static UploadResult UploadFile(string Id, string Filename, string BasePath)
        {
            short Out;

            using (SftpClient SFTP = new SftpClient(SFTPConn))
            {

                // Check there is a duplicated file.
                bool bFileExist = false;

                var Files = SFTP.ListDirectory(BasePath);

                foreach (SftpFile F in Files)
                {
                    if (F.IsDirectory) continue;
                    else
                    {
                        string[] Sp = F.Name.Split(new string[] { "." }, StringSplitOptions.None);

                        if (Sp[0] == Filename)
                            bFileExist = true;
                    }
                }

                // Check malware.
                if (!File.Exists(Filename))
                {
                    return UploadResult.FileNotFound;
                }

                var _Process = new Process();

                string Arg = string.Format("-Scan -ScanType 3 -File \"{0}\"", Filename);

                var _StartInfo = new ProcessStartInfo("C:\\program files\\windows defender\\mpcmdrun.exe")
                {
                    Arguments = Arg,
                    CreateNoWindow = true,
                    ErrorDialog = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false
                };

                _Process.StartInfo = _StartInfo;
                _Process.Start();
                _Process.WaitForExit(ScanTimeout);

                // Timeout...
                if (!_Process.HasExited)
                {
                    _Process.Kill();
                    return UploadResult.Timeout;
                }

                // If this file is corrupted...
                var EventList = new List<EventRecord>();

                if (_Process.ExitCode == 2)
                    return UploadResult.MalwareDetected;

                // Extract Event log...
                var q = new EventLogQuery("Microsoft-Windows-Windows Defender/Operational", PathType.LogName,
                    string.Format("*[System[TimeCreated[@SystemTime>='{0}']]]",
                        DateTime.Now.AddDays(-1).ToString("s")));

                var r = new EventLogReader(q);

                EventRecord er = r.ReadEvent();
                while (er != null)
                {
                    // Check defender event codes for more details.
                    if (er.Id.Equals(1006) || er.Id.Equals(1001))
                        EventList.Add(er);
                    er = r.ReadEvent();
                }

                EventList.Reverse();



                // Try uploading...
                foreach (SftpFile F in Files)
                {
                    if (F.IsDirectory) continue;
                    else
                    {
                        string[] Sp = F.Name.Split(new string[] { "." }, StringSplitOptions.None);

                        if (Sp[0] == Filename)
                            bFileExist = true;
                    }

                }

                if (!bFileExist)
                {
                    using (FileStream _FileStream = File.Open(BasePath, FileMode.Open))
                        SFTP.UploadFile(_FileStream, BasePath + "/" + Filename);

                    using (
                    MySqlCommand Cmd = new MySqlCommand(
                        // Query
                        string.Format("INSERT INTO " + FileTable +
                            " (CreateDate, UploadUser, Filename, ScanID, SecurityID) " +
                            "VALUES(@CreateDate, @UploadUser, @Filename, @ScanID, @SecurityID)")
                        // Connection
                        , UserConn
                        )
                    )
                    {
                        Cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                        Cmd.Parameters.AddWithValue("@UploadUser", UserConn);
                        Cmd.Parameters.AddWithValue("@Filename", Id);
                        Cmd.Parameters.AddWithValue("@ScanID", (string)EventList[0].Properties[2].Value);
                        Cmd.Parameters.AddWithValue("@SecurityID", (string)EventList[0].Properties[9].Value);

                        Cmd.ExecuteNonQuery();
                    };


                    return UploadResult.UploadeComplete;
                }

                return UploadResult.Error;
            }

        }
        private enum ScanResult
        {
            [Description("No threat found")]
            NoThreatFound,

            [Description("Threat found")]
            ThreatFound,

            [Description("The file could not be found")]
            FileNotFound,

            [Description("Timeout")]
            Timeout,

            [Description("Error")]
            Error
        }

    }

    public enum UploadResult
    {
        [Description("Upload complete")]
        UploadeComplete,

        [Description("Another file exists")]
        AnotherFileExists,

        [Description("File not found")]
        FileNotFound,

        [Description("Malware detected")]
        MalwareDetected,

        [Description("Timeout")]
        Timeout,

        [Description("Error")]
        Error
    }

}
