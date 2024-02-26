using BasicTemplate.Base;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace BasicTemplate.Control
{
    /// <summary>
    /// SlotCamera.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SlotCamera : UserControl
    {
        public SlotCamera()
        {
            InitializeComponent();
        }
    }

    class vmSlotCamera : ObservableObject
    {
        public int DeviceIdx { get; set; }
        public string PnpName { get; set; }

        private VideoCapture Capture;

        private bool _IsConnected;
        public bool IsConnected
        {
            get => _IsConnected;
            set
            {
                _IsConnected = value;
                OnPropertyChanged("IsConnected");
            }
        }

        public vmSlotCamera(int _DeviceIdx, string _PnpName)
        {
            DeviceIdx = _DeviceIdx;
            PnpName = _PnpName;

            Capture = new VideoCapture(PnpName, VideoCapture.API.Any);
        }

        public void CaptureStart()
            => Capture.Start();

        public void CaptureStop()
            => Capture.Stop();

        private void Capture_ImageGrabbed(object? sender, EventArgs e)
        {
            int a = 1;
        }
    }
}
