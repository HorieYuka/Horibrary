using BasicTemplate.Base;
using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
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
        private SignalPlot Sigplot;

        public string PresetSample { get; set; }

        private double[] PlotDataBuffer;

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
        int nextDataIndex = 1;

        private BackgroundWorker bWorker;
        private DispatcherTimer _updateDataTimer;
        private DispatcherTimer _renderTimer;

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


                            bTrigger = true;
                            // create a timer to modify the data
                            _updateDataTimer = new DispatcherTimer();
                            _updateDataTimer.Interval = TimeSpan.FromMilliseconds(1);
                            _updateDataTimer.Tick += UpdateData;
                            _updateDataTimer.Start();

                            // create a timer to update the GUI
                            _renderTimer = new DispatcherTimer();
                            _renderTimer.Interval = TimeSpan.FromMilliseconds(20);
                            _renderTimer.Tick += Render;
                            _renderTimer.Start();
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
            while (bTrigger && Sw.Elapsed.TotalMinutes < TimeValue)
            {
                Stack.AddRange(Ran.RandomSample(SampleValue));
                
                if(Stack.Count > Helper.MaxPlotBuffLength)
                {
                    Stack.RemoveRange(0, (Stack.Count() - Helper.MaxPlotBuffLength));
                    Array.Copy(Stack.ToArray(), PlotDataBuffer, Helper.MaxPlotBuffLength);
                }
                else
                    Array.Copy(Stack.ToArray(), PlotDataBuffer, Stack.Count());

            }
        }

        public vmExampleLiveChairPlot()
        {
            // Create plot
            PlotBase = new WpfPlot();

            // InitializePlot
            PlotDataBuffer = new double[Helper.MaxPlotBuffLength];
            Sigplot =  PlotBase.Plot.AddSignal(PlotDataBuffer);
            PlotBase.Refresh();

            // Set default value
            PresetSample = "5000";
            PresetTime = "10";

            // Set Background thread
            bWorker = new BackgroundWorker();
            bWorker.DoWork += RunLivePlot;



        }

        void Render(object sender, EventArgs e)
        {
  
            PlotBase.Refresh();
        }
        Random rand = new Random(0);
        void UpdateData(object sender, EventArgs e)
        {
            if (nextDataIndex >= PlotDataBuffer.Length)
            {
                throw new OverflowException("data array isn't long enough to accomodate new data");
                // in this situation the solution would be:
                //   1. clear the plot
                //   2. create a new larger array
                //   3. copy the old data into the start of the larger array
                //   4. plot the new (larger) array
                //   5. continue to update the new array
            }

            double randomValue = Math.Round(rand.NextDouble() - .5, 3);
            double latestValue = PlotDataBuffer[nextDataIndex - 1] + randomValue;
            PlotDataBuffer[nextDataIndex] = latestValue;
            Sigplot.MaxRenderIndex = nextDataIndex;
            nextDataIndex += 1;
        }

    }
}
