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

        public int SelectedIdx { get; set; }

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

        public ObservableCollection<vmSlotCOM> ListCOM { get; set; }

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

        private ICommand _ReadCOM;
        public ICommand ReadCOM
        {
            get
            {
                if (_ReadCOM == null)
                    _ReadCOM = new BaseCommand(p =>
                    {

                    });
                return _ReadCOM;
            }
        }

        private ICommand _WriteCOM;
        public ICommand WriteCOM
        {
            get
            {
                if (_WriteCOM == null)
                    _WriteCOM = new BaseCommand(p =>
                    {

                    });
                return _WriteCOM;
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



        public vmExampleCOM()
        {
            ListCOM = new ObservableCollection<vmSlotCOM>();
        }
    }

}
