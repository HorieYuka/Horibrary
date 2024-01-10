using BasicTemplate.Base;
using ScottPlot;
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
    /// ExampleLivePlot.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ExampleLivePlot : UserControl
    {
        public ExampleLivePlot()
        {
            InitializeComponent();
        }
    }

    class vmExampleLivePlot : ObservableObject, IExample
    {
        public string ExampleName => "실시간 그래프, 크로스 헤어 생성하기";
        public short ExampleNum => 2;

        public WpfPlot sPlot { get; set; }


        public vmExampleLivePlot()
        {
            // Create plot
            sPlot = new WpfPlot();

        }


    }
}
