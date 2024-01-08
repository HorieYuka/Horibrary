using BasicTemplate.Base;
using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
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
    /// ExampleLiveChairPlot.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ExampleLiveChairPlot : UserControl
    {
        public ExampleLiveChairPlot()
        {
            InitializeComponent();
        }
    }

    class vmExampleLiveChairPlot : ObservableObject, IExample
    {
        public string ExampleName => "실시간 그래프 생성하기";
        public short ExampleNum => 1;

        public WpfPlot PlotBase { get; set; }
        public string PresetSample { get; set; }



        private bool _bTrigger;
        public bool bTrigger
        {
            get => _bTrigger;
            set
            {
                _bTrigger = value;
                OnPropertyChanged("bTrigger");
            }
        }

        private string _PresetTime;
        public string PresetTime
        {
            get => _PresetTime;
            set
            {
                _PresetTime = value;
                OnPropertyChanged("PresetTime");
            }
        }


        private BackgroundWorker bWorker;


        private ICommand _CreatePlotCmd;
        public ICommand CreatePlotCmd
        {
            get
            {
                if (_CreatePlotCmd == null)
                    _CreatePlotCmd = new BaseCommand(p => {

                        if(bTrigger == false)
                        {
                            // Check values
                            int Value = 0;
                            if (!int.TryParse(PresetSample, out Value) || !int.TryParse(PresetTime, out Value))
                            {
                                Helper.UpdateBelowStatus(null, "입력값에 오류가 있습니다.", null);
                                return;
                            }

                            PlotBase.Plot.Clear();

                            bTrigger = true;
                            bWorker.RunWorkerAsync();
                        }
                        else
                            bTrigger = false;

                    });
                return _CreatePlotCmd;
            }
        }


        private void RunLivePlot(object? sender, DoWorkEventArgs e)
        {
            List<double> Stack = new List<double>();

            Stopwatch Sw = new Stopwatch();
            RandomDataGenerator Ran = new RandomDataGenerator();

            int SampleValue = int.Parse(PresetSample);
            int TimeValue = int.Parse(PresetTime);


            Sw.Start();
            while(bTrigger && Sw.Elapsed.TotalSeconds < TimeValue)
            {
                Stack.AddRange(Ran.RandomSample(SampleValue));

                // Delete all plot without render. because clear() triggers render().
                var Plots = PlotBase.Plot.GetPlottables();
                for(int i = 0; i < Plots.Count(); i++)
                    PlotBase.Plot.RemoveAt(0);

                PlotBase.Plot.AddSignal(Stack.ToArray());

                UiInvoke((delegate { PlotBase.Render(); })) ;
                
            }
        }

        public vmExampleLiveChairPlot()
        {
            // Create plot
            PlotBase = new WpfPlot();

            // Set default value
            PresetSample = "5000";
            PresetTime = "10";

            // Set Background thread
            bWorker = new BackgroundWorker();
            bWorker.DoWork += RunLivePlot;
        }

    }
}
