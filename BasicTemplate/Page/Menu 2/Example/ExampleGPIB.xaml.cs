using BasicTemplate.Base;
using BasicTemplate.Control;
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
using ClassLib;

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
                        ClassLib.Class1.m();
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

                        ListGPIB[Idx].ConnectDevice();
                        CurrentSess = ListGPIB[Idx];
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

                        ListGPIB[Idx].DisconnectDevice();
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
            => ListGPIB = new ObservableCollection<vmSlotGPIB>();
    }
}
