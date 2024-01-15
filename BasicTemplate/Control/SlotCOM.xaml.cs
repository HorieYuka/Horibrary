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

        private string _Log;
        public string Log
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
        }

        public void ConnectDevice()
        {
            string Baudrate = ModelConstDevice1.BaudrateList[BaudrateIdx];

            if (!string.IsNullOrEmpty(Baudrate) && !Baudrate.Equals("Auto"))
                Serial = new SerialPort(Port, int.Parse(Baudrate));
            else
            {
                Serial = new SerialPort(Port);
                Serial.BaudRate
                    ModelConstDevice1.BaudrateList.
                BaudrateIdx.find

            }
            IsConnected = true;

        }

        public string ReadDevice()
        {
            string Out = "";

            if (Serial != null)
                Out = Serial.ReadLine();

            return Out;
        }


        public void WriteDevice(string str)
        { if (Serial != null) Serial.WriteLine(str); }

        public void DisconnectDevice()
        { if (Serial != null) Serial.Dispose(); IsConnected = false; }

    }
}
