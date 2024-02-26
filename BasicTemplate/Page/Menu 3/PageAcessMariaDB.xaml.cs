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
    /// PageAcessMariaDB.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PageAcessMariaDB : UserControl
    {
        public PageAcessMariaDB()
        {
            InitializeComponent();
        }
    }

    class vmPageAcessMariaDB : ObservableObject
    {
        public PackIconKind PageIcon => PackIconKind.DeveloperBoard;
        public string PageName => "MariaDB";
        public short PageNum => 2;

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

        public vmPageAcessMariaDB() 
        {
            // Create examples
            ExampleList = new List<IExample>();
            ExampleList =
            [
                new vmExampleCommonDB(),
                new vmExampleVirusDetect()
            ];
        }
    }
}
