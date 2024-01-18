using BasicTemplate.Base;
using BasicTemplate.Control;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wrapper;

namespace BasicTemplate.Example
{
    /// <summary>
    /// ExampleGPIB.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ExampleGPIB : UserControl
    {
        public ExampleGPIB()
        {
            InitializeComponent();
        }
    }

    class vmExampleGPIB : ObservableObject, IExample
    {
        public string ExampleName => "GPIB 장비";
        public short ExampleNum => 1;

        public ObservableCollection<vmSlotGPIB> ListGPIB { get; set; }
        public int SelectedIdx { get; set; }

        private LibraryVISA Lib;


        private string _Word;
        public string Word

        {
            get => _Word;
            set
            {
                _Word = value;
                OnPropertyChanged("Word");
            }
        }

        private vmSlotGPIB _CurrentSess;
        public vmSlotGPIB CurrentSess

        {
            get => _CurrentSess;
            set
            {
                _CurrentSess = value;
                OnPropertyChanged("CurrentSess");
            }
        }


        private ICommand _FindGPIBs;
        public ICommand FindGPIBs
        {
            get
            {
                if (_FindGPIBs == null)
                    _FindGPIBs = new BaseCommand(p =>
                    {
                        LibraryVISA Lib = new LibraryVISA();
                        Lib.FindResource();

                        int DeviceCount = Lib.GetDeviceCount();
                        unsafe
                        {
                            for (int i = 0; i < DeviceCount; i++)
                            {
                                string Info = new string(Lib.GetDeviceInfo(i));
                                ListGPIB.Add(new vmSlotGPIB(Info, i));
                            }
                        }
                        
                    });
                return _FindGPIBs;
            }
        }

        private ICommand _ReadDeviceCmd;
        public ICommand ReadDeviceCmd
        {
            get
            {
                if (_ReadDeviceCmd == null)
                    _ReadDeviceCmd = new BaseCommand(p =>
                    {
                        if (CurrentSess != null)
                        {
                            string Str = "";

                            unsafe
                            {
                                var Bptr = Lib.IORead(0);

                                byte[] Barr = new byte[ModelConstDevice2.IO_OUT_BUFLEN];
                                Marshal.Copy((IntPtr) Bptr, Barr, 0, ModelConstDevice2.IO_OUT_BUFLEN);

                                Str = Encoding.Default.GetString(Barr);  
                            }

                            CurrentSess.UpdateReadLog(Str);
                        }
                    });
                return _ReadDeviceCmd;
            }
        }

        private ICommand _WriteDeviceCmd;
        public ICommand WriteDeviceCmd
        {
            get
            {
                if (_WriteDeviceCmd == null)
                    _WriteDeviceCmd = new BaseCommand(p =>
                    {
                        if (CurrentSess != null)
                        {
                            unsafe
                            {
                                byte[] Bts = Encoding.ASCII.GetBytes(Word);

                                fixed (byte* Bptr = Bts)
                                {
                                    sbyte* Sptr = (sbyte*)Bptr;

                                    Lib.IOWrite(0, Sptr, (uint)Word.Length);
                                }
                            }
                            CurrentSess.UpdateWriteLog(Word);
                        }
                            
                    });
                return _WriteDeviceCmd;
            }
        }

        private ICommand _ConDeviceCmd;
        public ICommand ConDeviceCmd
        {
            get
            {
                if (_ConDeviceCmd == null)
                    _ConDeviceCmd = new BaseCommand(p =>
                    {
                        int DeviceIdx = (int)p;

                        Lib.OpenSession(DeviceIdx);
                        CurrentSess = ListGPIB[DeviceIdx];

                        CurrentSess.IsConnected = true;
                    });
                return _ConDeviceCmd;
            }
        }

        private ICommand _DisconDeviceCmd;
        public ICommand DisconDeviceCmd
        {
            get
            {
                if (_DisconDeviceCmd == null)
                    _DisconDeviceCmd = new BaseCommand(p =>
                    {
                        int DeviceIdx = (int)p;

                        Lib.CloseSession(DeviceIdx);
                        CurrentSess = null;

                        CurrentSess.IsConnected = false;
                    });
                return _DisconDeviceCmd;
            }
        }

        private ICommand _ClearLogCmd;
        public ICommand ClearLogCmd
        {
            get
            {
                if (_ClearLogCmd == null)
                    _ClearLogCmd = new BaseCommand(p =>
                    {
                        CurrentSess.ClearLog();
                    });
                return _ClearLogCmd;
            }
        }
        private ICommand _ClearTextCmd;
        public ICommand ClearTextCmd
        {
            get
            {
                if (_ClearTextCmd == null)
                    _ClearTextCmd = new BaseCommand(p =>
                    {
                        Word = "";

                    });
                return _ClearTextCmd;
            }
        }

        public vmExampleGPIB()
        {
            ListGPIB = new ObservableCollection<vmSlotGPIB>();
            Lib = new LibraryVISA();
        }

    }
}
