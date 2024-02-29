using LibraryAMSI.Native;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Net.Mime;


namespace LibraryAMSI
{
    class LibraryAMSI
    {
        // 10MB
        private const int FileScanBlockSize = 10 * 1000 * 1000;

        readonly AMSIHandleSession SessionHandle;
        readonly static Dictionary<Guid, (DetectionEngine DetectionEngine, Func<uint, bool> IsEnabled)> Detectors
    = new Dictionary<Guid, (DetectionEngine DetectionEngine, Func<uint, bool> IsEnabled)>()
    {
                { new Guid("17AD7D40-BA12-9C46-7131-94903A54AD8B"), (DetectionEngine.Avast, state => true) },
                { new Guid("0E9420C4-06B3-7FA0-3AB1-6E49CB52ECD9"), (DetectionEngine.AVG, state => true) },
                { new Guid("92356E98-E159-03AA-2BF0-6FE55F131038"), (DetectionEngine.BitDefender, state => state != 270336) },
                { new Guid("ea21bce8-a461-99c3-3a0d-4c964e75494e"), (DetectionEngine.BitDefenderFree, state => state != 270336) },
                { new Guid("77DEAFED-8149-104B-25A1-21771CA47CD1"), (DetectionEngine.ESET, state => true) },
                { new Guid("ec1d6f37-e411-475a-df50-12ff7fe4ac70"), (DetectionEngine.ESETSecurity, state => true) },
                { new Guid("E5E70D32-0101-4F12-8FB0-D96ACA4F34C0"), (DetectionEngine.ESETSecurity, state => true) },
                { new Guid("545C8713-0744-B079-87F8-349A6D5C8CF0"), (DetectionEngine.GDataAntivirus, state => true) },
                { new Guid("AE1D740B-8F0F-D137-211D-873D44B3F4AE"), (DetectionEngine.KasperskyAntivirus, state => true) },
                { new Guid("86367591-4BE4-AE08-2FD9-7FCB8259CD98"), (DetectionEngine.KasperskyEndpointSecurity, state => true) },
                { new Guid("63DF5164-9100-186D-2187-8DC619EFD8BF"), (DetectionEngine.Norton360, state => true) },
                { new Guid("A2708B76-6835-6565-CB96-694212954A75"), (DetectionEngine.NortonSecurity, state => true) },
                { new Guid("D68DDC3A-831F-4fae-9E44-DA132C1ACF46"), (DetectionEngine.WindowsDefender, state => state != 393472) },
    };

        internal bool bIsDetectorEnable;
        internal HashSet<DetectionEngineInfo> AvailableEngineList;
        internal DetectionEngineInfo DetectorNowInUse;

        private AMSIHandleContext ContextHandle;

        public LibraryAMSI()
        {
            int Result;

            // Initialize AMSI
            using (var CurrentProcess = Process.GetCurrentProcess())
            {
                Result = NativeAMSI.AmsiInitialize(
                    $"{AppDomain.CurrentDomain.FriendlyName} ({CurrentProcess.Id})", 
                    out ContextHandle);

                // Check initialize is okay. Failed if it's not zero.
                if (Result != 0)
                    return;

                // Check context is vaild
                if (ContextHandle.IsInvalid) 
                    return;
            }


            // Create AMSI Session
            Result = NativeAMSI.AmsiOpenSession(ContextHandle, out SessionHandle);
            // Check session is okay. Failed if it's not zero.
            if (Result != 0)
                return;

            // Build the list of available detectors...
            AvailableEngineList = new HashSet<DetectionEngineInfo>();
          
            try
            {
                string wmiScope = $@"\\{Environment.MachineName}\root\SecurityCenter2";
                using (var searcher = new ManagementObjectSearcher(wmiScope, "SELECT * FROM AntiVirusProduct"))
                    foreach (ManagementObject engine in searcher.Get())
                        using (engine)
                        {
                            var instanceGuid = new Guid((string)engine["instanceGuid"]);
                            var productState = (uint)engine["productState"];
                            var displayName = (string)engine["displayName"];
                            string companyName = null;
                            string pathToSignedProductExe = null;
                            string versionInfo = null;

                            if (Detectors.TryGetValue(instanceGuid, out var engineInfo))
                            {
                                foreach (var property in engine.Properties)
                                {
                                    if (property.Name == nameof(companyName))
                                        companyName = (string)property.Value;
                                    else if (property.Name == nameof(pathToSignedProductExe))
                                        pathToSignedProductExe = (string)property.Value;
                                    else if (property.Name == nameof(versionInfo))
                                        versionInfo = (string)property.Value;
                                }

                                AvailableEngineList.Add(new DetectionEngineInfo()
                                {
                                    EngineType = engineInfo.DetectionEngine,
                                    InstanceGuid = instanceGuid,
                                    DisplayName = displayName,
                                    CompanyName = companyName,
                                    PathToSignedProductExe = pathToSignedProductExe,
                                    VersionInfo = versionInfo,
                                    ProductState = productState,
                                    IsEnabled = engineInfo.IsEnabled(productState)
                                });
                            }
                        }
            }
            catch
            {
                // Cannot find detector
                return;
            }

            // Choose detector
            DetectorNowInUse = AvailableEngineList
                .Where(engine => engine.IsEnabled).DefaultIfEmpty(DetectionEngineInfo.Empty).FirstOrDefault();

            // Does it find detector engine?
            if (DetectorNowInUse.EngineType == DetectionEngine.Unknown)
                return;
            else
                bIsDetectorEnable = true;

        }


        public bool IsAvailable()
          => NativeAMSI.IsDllImportPossible();

        public bool ScanFile(string Filepath)
        {
            var Result = NativeAMSI.AmsiScanBuffer(ContextHandle, Buf, Len, Filename, SessionHandle);

            if (Result != 0)return false;
            else return true;

        }

    }

    public class DetectionEngineInfo
    {
        public DetectionEngine EngineType { get; set; }
        public Guid InstanceGuid { get; set; }
        public string DisplayName { get; set; }
        public string CompanyName { get; set; }
        public uint ProductState { get; set; }
        public string PathToSignedProductExe { get; set; }
        public string VersionInfo { get; set; }
        public bool IsEnabled { get; set; }

        public static DetectionEngineInfo Empty
            => new DetectionEngineInfo();
    }

    public enum DetectionEngine
    {
        Unknown,
        Avast,
        AVG,
        BitDefender,
        BitDefenderFree,
        ESET,
        ESETSecurity,
        GDataAntivirus,
        KasperskyEndpointSecurity,
        KasperskyAntivirus,
        Norton360,
        NortonSecurity,
        SophosHome,
        WindowsDefender,
        Other
    }
}
