using BasicTemplate.Base;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace BasicTemplate.Control
{
    /// <summary>
    /// SlotGPIB.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SlotGPIB : UserControl
    {
        public SlotGPIB()
        {
            InitializeComponent();
        }
    }

    class vmSlotGPIB : ObservableObject
    {

        public int Idx { get; set; }
        public string Port { get; set; }
        public string DeviceName { get; set; }


        private Paragraph Pl;

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

        private int _BaudrateIdx;
        public int BaudrateIdx
        {
            get => _BaudrateIdx;
            set
            {
                _BaudrateIdx = value;
                OnPropertyChanged("BaudrateIdx");
            }
        }

        private RichTextBox _Log;
        public RichTextBox Log
        {
            get => _Log;
            set
            {
                _Log = value;
                OnPropertyChanged("Log");
            }
        }

        public vmSlotGPIB(string _DeviceName)
        {
            DeviceName = _DeviceName;

            Log = new RichTextBox();
            Log.Document.Blocks.Clear();
            Log.Document.LineHeight = 5;
            Log.Document.FontWeight = FontWeights.Bold;

            Pl = new Paragraph();
            Log.Document.Blocks.Add(Pl);
        }

        public void ConnectDevice()
        {
            DisconnectDevice();



            IsConnected = true;

        }

        public void ReadDevice()
        {

        }


        public void ClearLog()
            => Pl.Inlines.Clear();

        public void WriteDevice(string str)
        {


        }

        public void DisconnectDevice()
        {  }
    }
}