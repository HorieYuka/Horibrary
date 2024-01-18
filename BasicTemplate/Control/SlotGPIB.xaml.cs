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

        public int DeviceIdx { get; set; }
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

        public vmSlotGPIB(string _DeviceName, int _DeviceIdx)
        {
            DeviceName = _DeviceName;
            DeviceIdx = _DeviceIdx;

            Log = new RichTextBox();
            Log.Document.Blocks.Clear();
            Log.Document.LineHeight = 5;
            Log.Document.FontWeight = FontWeights.Bold;

            Pl = new Paragraph();
            Log.Document.Blocks.Add(Pl);
        }

        public void ClearLog()
            => Pl.Inlines.Clear();

        public void UpdateReadLog(string str)
        {
            string[] Sp = str.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            if (Sp.Count() != 2)
            {
                Pl.Inlines.Add(new Run(" " + "Request time out." + "\n")
                {
                    FontSize = 16,
                    Foreground = new SolidColorBrush(Colors.Red)
                });
            }
            else
            {
                Pl.Inlines.Add(new Run(" " + Sp[0] + "\n")
                {
                    FontSize = 16,
                    Foreground = new SolidColorBrush(Colors.Green)
                });
            }
        }

        public void UpdateWriteLog(string str)
        {
            Pl.Inlines.Add(new Run(" " + str + "\n")
            {
                FontSize = 16,
                Foreground = new SolidColorBrush(Colors.Blue)
            });


        }

    }
}