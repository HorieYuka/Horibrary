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
    /// ExampleCameraDraw.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ExampleCameraDraw : UserControl
    {
        public ExampleCameraDraw()
        {
            InitializeComponent();
        }
    }

    class vmExampleCameraDraw : ObservableObject , IExample
    {
        public string ExampleName => "카메라";
        public short ExampleNum => 0;

        public ObservableCollection<vmSlotCamera> ListCamera { get; set; }


        private vmGridCameraSess _DisplaySess;
        public vmGridCameraSess DisplaySess
        {
            get => _DisplaySess;
            set
            {
                _DisplaySess = value;
                OnPropertyChanged("DisplaySess");
            }
        }

        private vmListviewDraggable _ListSess;
        public vmListviewDraggable ListSess
        {
            get => _ListSess;
            set
            {
                _ListSess = value;
                OnPropertyChanged("ListSess");
            }
        }


        private ICommand _FindCameras;
        public ICommand FindCameras
        {
            get
            {
                if (_FindCameras == null)
                    _FindCameras = new BaseCommand(p =>
                    {
                        ListCamera.Clear();

                        using (var Searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE PNPClass = 'Camera'"))
                        {
                            var Cameras = Searcher.Get().Cast<ManagementBaseObject>().ToList();
                            
                            for(int i = 0; i < Cameras.Count(); i++)
                            {
                                ListCamera.Add(new vmSlotCamera( 
                                    _DeviceIdx : i,
                                    _PnpName: Cameras[i]["Caption"].ToString())
                                    );
                            }
                        };

                    });
                return _FindCameras;
            }
        }

        private ICommand _AddCameraSessCmd;
        public ICommand AddCameraSessCmd
        {
            get
            {
                if (_AddCameraSessCmd == null)
                    _AddCameraSessCmd = new BaseCommand(p =>
                    {
                        var TargetCameraSess = (vmSlotCamera)p;

                        if (ListCamera.Contains(TargetCameraSess))
                            return;
                        else
                            ListCamera.Add(TargetCameraSess);

                    });
                return _AddCameraSessCmd;
            }
        }


        public vmExampleCameraDraw() 
        {
            ListCamera = new ObservableCollection<vmSlotCamera>();

            ListCamera.Add(new vmSlotCamera(1, "Dummy"));

            DisplaySess = new vmGridCameraSess();
            ListSess = new vmListviewDraggable(ListCamera);

        }
    }
}
