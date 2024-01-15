using BasicTemplate.Base;
using Ivi.Visa;
using System.IO.Ports;
using System.Linq;
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


        private IMessageBasedSession Device { get; set; }
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

            Device = GlobalResourceManager.Open(DeviceName) as IMessageBasedSession;

            IsConnected = true;

        }

        public string ReadDevice()
        {
            string Out = "";

            if (Device != null)
            {
                try
                {
                    Out = Device.RawIO.ReadString();

                    if (string.IsNullOrEmpty(Out))
                    {
                        Pl.Inlines.Add(new Run(" " + Out + "\n")
                        {
                            FontSize = 16,
                            Foreground = new SolidColorBrush(Colors.Blue)
                        });
                    }
                }
                catch
                {
                    Pl.Inlines.Add(new Run(" " + "Read time out. (3000ms)" + "\n")
                    {
                        FontSize = 16,
                        Foreground = new SolidColorBrush(Colors.Red)
                    });
                }
            }

            return Out;
        }


        public void ClearLog()
            => Pl.Inlines.Clear();

        public void WriteDevice(string str)
        {
            if (!string.IsNullOrEmpty(str) && Device != null)
            {
                Device.RawIO.Write(str);

                Pl.Inlines.Add(new Run(" " + str + "\n")
                {
                    FontSize = 16,
                    Foreground = new SolidColorBrush(Colors.Green)
                });

            }
            ReadDevice();

        }

        public void DisconnectDevice()
        { if (Device != null) Device.Dispose(); IsConnected = false; }
    }
}