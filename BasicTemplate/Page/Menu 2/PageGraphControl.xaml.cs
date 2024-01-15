using BasicTemplate.Base;
using BasicTemplate.Control;
using BasicTemplate.Example;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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
using System.Windows.Threading;

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


        public List<IExample> ExampleList { get; set; }

        private short _CurrentTabIdx;
        public short CurrentTabIdx
        {
            get => _CurrentTabIdx;
            set
            {
                _CurrentTabIdx = value;
                OnPropertyChanged("CurrentTabIdx");
            }
        }

        internal vmPageGraphControl()
        {
            // Create examples
            ExampleList = new List<IExample>();
            ExampleList =
            [
                new vmExampleBasicPlot(),
                new vmExampleLivePlot(),
                new vmExampleLiveChairPlot(),
            ];

        }

    }
}
