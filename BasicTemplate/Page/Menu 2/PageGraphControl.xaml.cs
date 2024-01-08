using BasicTemplate.Base;
using BasicTemplate.Control;
using BasicTemplate.Example;
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

namespace BasicTemplate.Page
{
    /// <summary>
    /// PageGraphControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PageGraphControl : UserControl
    {
        public PageGraphControl()
        {
            InitializeComponent();
        }
    }

    class vmPageGraphControl : ObservableObject, IPage
    {
        public PackIconKind PageIcon => PackIconKind.ChartBox;
        public string PageName => "그래프";
        public short PageNum => 3;

        public List<ISlot> SlotList { get; set; }
        public List<IExample> ExampleList { get; set; }

        private IExample _CurrentExample;
        public IExample CurrentExample
        {
            get => _CurrentExample;
            set
            {
                _CurrentExample = value;
                OnPropertyChanged("CurrentExample");
            }
        }

        private int _ExampleIdx;
        public int ExampleIdx
        {
            get => _ExampleIdx;
            set
            {
                _ExampleIdx = value;
                if(_ExampleIdx >= 0) CurrentExample = ExampleList[_ExampleIdx];
            }
        }

        internal vmPageGraphControl()
        {
            // Create examples
            ExampleList =
            [
                new vmExampleBasicPlot(),
                new vmExampleLivePlot(),
                new vmExampleLiveChairPlot(),
            ];

            // Create example slots;
            ExampleIdx = -1;
            SlotList = new List<ISlot>();
            for (int i = 0; i < ExampleList.Count; i++)
                SlotList.Add(new vmExampleSlot(ExampleList[i]));

        }

    }
}
