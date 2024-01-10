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
    /// ExampleBasicPlot.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ExampleBasicPlot : UserControl
    {
        public ExampleBasicPlot()
        {
            InitializeComponent();
        }
    }

    class vmExampleBasicPlot : ObservableObject, IExample
    {
        public string ExampleName => "기본적인 그래프 생성하기";
        public short ExampleNum => 0;

        public WpfPlot PlotBase { get; set; }

        public string SampleCount { get; set; }

        private ICommand _CreatePlotCmd;
        public ICommand CreatePlotCmd
        {
            get
            {
                if (_CreatePlotCmd == null)
                    _CreatePlotCmd = new BaseCommand(p => {

                        PlotBase.Plot.Clear();

                        int Value = 0;
                        if (!int.TryParse(SampleCount, out Value))
                        {
                            Helper.UpdateBelowStatus(null, "입력값에 오류가 있습니다.", null);
                            return;
                        }

                        RandomDataGenerator Ran = new RandomDataGenerator();
                        double[] Samples = Ran.RandomSample(Value);

                        PlotBase.Plot.AddSignal(Samples);
                        PlotBase.Render();

                        Helper.UpdateBelowStatus(null, "샘플링 갯수 : " + Value , null);
                    });
                return _CreatePlotCmd;
            }
        }

        public vmExampleBasicPlot()
        {
            // Create plot
            PlotBase = new WpfPlot();

            // Set default value
            SampleCount = "5000";
        }



    }
}
