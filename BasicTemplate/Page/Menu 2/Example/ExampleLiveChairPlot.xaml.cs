using BasicTemplate.Base;
using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
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
        public string PresetTime { get; set; }
        public string CustomX { get; set; }
        public bool bIsCustomXEnable { get; set; }



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




        private BackgroundWorker bWorker;

        private ICommand _CreatePlotCmd;
        public ICommand CreatePlotCmd
        {
            get
            {
                if (_CreatePlotCmd == null)
                    _CreatePlotCmd = new BaseCommand(p =>
                    {

                        if (bTrigger == false)
                        {
                            // Check values
                            int Value = 0;
                            if (!int.TryParse(PresetSample, out Value) || !int.TryParse(PresetTime, out Value))
                            {
                                Helper.UpdateBelowStatus(null, "입력값에 오류가 있습니다.", null);
                                return;
                            }

                            bTrigger = true;
                            PlotBase.Refresh();

                            // create a timer to update the GUI
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
            while (bTrigger && Sw.Elapsed.TotalMinutes < TimeValue)
            {

                Stack.AddRange(Ran.RandomSample(SampleValue));

                if (Stack.Count > Helper.MaxPlotBuffLength)
                {
                    Stack.RemoveRange(0, (Stack.Count() - (int) Helper.MaxPlotBuffLength));
                    Array.Copy(Stack.ToArray(), PlotDataBuffer, (int)Helper.MaxPlotBuffLength);
                }
                else
                    Array.Copy(Stack.ToArray(), PlotDataBuffer, Stack.Count());

                Sigplot.MaxRenderIndex = Stack.Count() - 1;

                if(!bIsCustomXEnable)
                    PlotBase.Plot.AxisAuto();
                else
                {
                    PlotBase.Plot.AxisAutoY();
                    PlotBase.Plot.SetAxisLimitsX(Math.Max(0, Sigplot.MaxRenderIndex - double.Parse(CustomX)), Math.Max(1, Sigplot.MaxRenderIndex));
                }


                UiInvoke(delegate { PlotBase.Refresh(); });
                Thread.Sleep(50);
            }

        }


        public vmExampleLiveChairPlot()
        {
            // Create plot
            PlotBase = new WpfPlot();

            // InitializePlot
            PlotDataBuffer = new double[(int)Helper.MaxPlotBuffLength];
            Sigplot = PlotBase.Plot.AddSignal(PlotDataBuffer);
            Sigplot.MaxRenderIndex = 0;
            PlotBase.Refresh();

            // Set default value
            PresetSample = "1";
            PresetTime = "10";
            CustomX = "100";

            // Set Background thread
            bWorker = new BackgroundWorker();
            bWorker.DoWork += RunLivePlot;


        }

    }



}
