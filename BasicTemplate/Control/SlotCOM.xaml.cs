using BasicTemplate.Base;
using System;
using System.Collections.Generic;
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
    /// SlotCOM.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SlotCOM : UserControl
    {
        public SlotCOM()
        {
            InitializeComponent();
        }
    }

    class vmSlotCOM : ObservableObject
    {
        public int Idx { get; set; }
        public string Port { get; set; }
        public string Name { get; set; }

        private SerialPort Serial;
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

        public vmSlotCOM(int _Idx, string _Name, string _Port)
        {
            Idx = _Idx;
            Name = _Name;
            Port = _Port;
            BaudrateIdx = -1;

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

            if (BaudrateIdx != -1)
            {
                string Baudrate = ModelConstDevice1.BaudrateList[BaudrateIdx];
                Serial = new SerialPort(Port, int.Parse(Baudrate));
            }
            else
            {
                Serial = new SerialPort(Port);
                BaudrateIdx =
                    Array.FindIndex(ModelConstDevice1.BaudrateList, s => s.StartsWith(Serial.BaudRate.ToString()));
            }

            Serial.ReadBufferSize = ModelConstDevice1.GetBufferLength;
            Serial.ReadTimeout = 3000;
            Serial.Open();

            Serial.DataReceived += DataReceived;
            
            IsConnected = true;

        }

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string Out = "";

            if (Serial != null)
            {
                try
                {
                    Out = Serial.ReadLine();

                    string[] Sp = Out.Split(new string[] { "\n", "\r" }, StringSplitOptions.None);

                    if (Sp.Length != 0)
                    {
                        string Str = "";

                        for (int i = 0; i < Sp.Length; i++)
                        {
                            if (string.IsNullOrEmpty(Sp[i])) continue;

                            Str += Sp[i];

                        }

                        UiInvoke(delegate
                        {
                            Pl.Inlines.Add(new Run(" " + Str + "\n")
                            {
                                FontSize = 16,
                                Foreground = new SolidColorBrush(Colors.Green)
                            });

                        });
                    }
                }
                catch { /* It's not Line */ };
            }
        }

        public void ClearLog()
            => Pl.Inlines.Clear();

        public void WriteDevice(string str)
        {
            if (!string.IsNullOrEmpty(str) && Serial != null)
            {
                Serial.WriteLine(str);

                Pl.Inlines.Add(new Run(" " + str + "\n") { 
                    FontSize = 16, 
                    Foreground = new SolidColorBrush(Colors.Blue) });

            }
        }

        public void DisconnectDevice()
        { if (Serial != null) Serial.Dispose(); IsConnected = false; }

    }
}
