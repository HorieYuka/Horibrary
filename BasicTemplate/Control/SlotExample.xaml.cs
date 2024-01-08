using BasicTemplate.Base;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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
    /// CustomSlot.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SlotExample : UserControl
    {
        public SlotExample()
        {
            InitializeComponent();
        }
    }

    class vmSlotExample : ObservableObject, ISlot
    {
        public string SlotText { get; set; }

        private SolidColorBrush _SlotBackground;
        public SolidColorBrush SlotBackground
        {
            get => _SlotBackground;
            set
            {
                _SlotBackground = value;
                OnPropertyChanged("SlotBackground");
            }
        }

        private ICommand _DeleteSlotCmd;
        public ICommand DeleteSlotCmd
        {
            get
            {
                if (_DeleteSlotCmd == null)
                {
                    _DeleteSlotCmd = new BaseCommand(p =>
                    {
                    });
                }
                return _DeleteSlotCmd;
            }
        }

        public PackIconKind DeviceIcon => PackIconKind.AccessPoint;

        public EventHandler EvtHdr_1;
        public EventHandler EvtHdr_2;

        public void SlotEnabler(bool En)
        {
            if (En)
                SlotBackground = Brushes.LightGreen;
            else
                SlotBackground = Brushes.Transparent;
        }


        public vmSlotExample(IExample _ExampleInfo)
        {
            SlotText = _ExampleInfo.ExampleName;
        }


    }
}
