using BasicTemplate.Base;
using BasicTemplate.Control;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Management;
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

namespace BasicTemplate.Example
{
    /// <summary>
    /// ExampleCOMs.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ExampleCOM : UserControl
    {
        public ExampleCOM()
        {
            InitializeComponent();
        }
    }

    class vmExampleCOM : ObservableObject, IExample
    {
        public string ExampleName => "COM 장비";
        public short ExampleNum => 0;

        public ObservableCollection<vmSlotCOM> ListCOM { get; set; }
        public int SelectedIdx { get; set; }


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

        private vmSlotCOM _CurrentSess;
        public vmSlotCOM CurrentSess

        {
            get => _CurrentSess;
            set
            {
                _CurrentSess = value;
                OnPropertyChanged("CurrentSess");
            }
        }


        private ICommand _FindCOMs;
        public ICommand FindCOMs
        {
            get
            {
                if (_FindCOMs == null)
                    _FindCOMs = new BaseCommand(p =>
                    {

                        ListCOM.Clear();

                        using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'"))
                        {
                            var Portnames = SerialPort.GetPortNames();
                            var Searcher = searcher.Get().Cast<ManagementBaseObject>().ToList().Select(p => p["Caption"].ToString());
                            var COMs = Portnames.Select(n => Searcher.FirstOrDefault(s => s.Contains(n))).ToList();

                            for (int i = 0; i < COMs.Count(); i++)
                            {
                                if (COMs[i] == null) continue;
                                else
                                {
                                    ListCOM.Add(new vmSlotCOM(
                                            _Idx: i,
                                            _Port: Portnames[i],
                                            _Name: COMs[i]
                                            ));
                                }

                            }
                        }

                    });
                return _FindCOMs;
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
                            CurrentSess.ReadDevice();
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
                            CurrentSess.WriteDevice(Word);

                        Word = "";
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
                        int Idx = (int)p;

                        ListCOM[Idx].ConnectDevice();
                        CurrentSess = ListCOM[Idx];
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
                        int Idx = (int)p;

                        ListCOM[Idx].DisconnectDevice();
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



        public vmExampleCOM()
            => ListCOM = new ObservableCollection<vmSlotCOM>();
    }

}
