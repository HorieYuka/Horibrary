using BasicTemplate.Base;
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

namespace BasicTemplate.Example
{
    /// <summary>
    /// ExampleGPIB.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ExampleGPIB : UserControl
    {
        public ExampleGPIB()
        {
            InitializeComponent();
        }
    }

    class vmExampleGPIB : ObservableObject, IExample
    {
        public string ExampleName => "GPIB 장비";
        public short ExampleNum => 1;

        public vmExampleGPIB()
        {
        }
    }
}
