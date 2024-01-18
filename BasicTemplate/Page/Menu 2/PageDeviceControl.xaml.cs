using BasicTemplate.Base;
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
    /// PageDeviceControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PageDeviceControl : UserControl
    {
        public PageDeviceControl()
        {
            InitializeComponent();
        }
    }

    class vmPageDeviceControl : ObservableObject, IPage
    {
        public PackIconKind PageIcon => PackIconKind.DeveloperBoard;
        public string PageName => "장치";
        public short PageNum => 4;

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

        internal vmPageDeviceControl()
        {
            // Create examples
            ExampleList =
            [
                new vmExampleCOM(),
                new vmExampleGPIB(),
                new vmExampleCameraDraw()
            ];
            

        }

    }
}
