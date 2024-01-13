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

    class vmSlotCom : ObservableObject
    {
        public EventHandler ConnectEvt;
        public EventHandler DisconnectEvt;

        public ModelCOM Model;
        private SerialPort Serial;

        public vmSlotCom(ModelCOM _Model)
            => Model = _Model;

        private ICommand _ConDeviceCmd;
        public ICommand CreatePlotCmd
        {
            get
            {
                if (_ConDeviceCmd == null)
                    _ConDeviceCmd = new BaseCommand(p =>
                    {
                        Serial = new SerialPort(Model.Port, Model.Baudrate);
                    });
                return _ConDeviceCmd;
            }
        }

        private ICommand _DisConDeviceCmd;
        public ICommand DisConDeviceCmd
        {
            get
            {
                if (_DisConDeviceCmd == null)
                    _DisConDeviceCmd = new BaseCommand(p =>
                    {



                    });
                return _DisConDeviceCmd;
            }
        }
    }
}
