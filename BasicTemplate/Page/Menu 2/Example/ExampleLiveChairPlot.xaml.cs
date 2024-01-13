using BasicTemplate.Base;
using BasicTemplate.Control;
using ScottPlot;
using ScottPlot.Plottable;
using ScottPlot.SnapLogic;
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
using System.Windows.Media.Media3D;
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
        public string ExampleName => "실시간 그래프, 크로스 헤어 생성하기";
        public short ExampleNum => 2;

        public WpfPlot PlotBase { get; set; }
        public bool bIsCustomXEnable { get; set; }

        private SignalPlot Sigplot;
        private BackgroundWorker bWorker;
        private double[] PlotDataBuffer;

        private vmTextboxWithClearBtn _TextPresetSample;
        public vmTextboxWithClearBtn TextPresetSample
        {
            get => _TextPresetSample;
            set
            {
                _TextPresetSample = value;
                OnPropertyChanged("TextPresetSample");
            }
        }

        private vmTextboxWithClearBtn _TextPresetTime;
        public vmTextboxWithClearBtn TextPresetTime
        {
            get => _TextPresetTime;
            set
            {
                _TextPresetTime = value;
                OnPropertyChanged("TextPresetTime");
            }
        }

        private vmTextboxWithClearBtn _TextCustomX;
        public vmTextboxWithClearBtn TextCustomX
        {
            get => _TextCustomX;
            set
            {
                _TextCustomX = value;
                OnPropertyChanged("TextCustomX");
            }
        }

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
                            if (!ValidateValue())
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

            int SampleValue = int.Parse(TextPresetSample.Text);
            int TimeValue = int.Parse(TextPresetTime.Text);

            Sw.Start();
            while (bTrigger && Sw.Elapsed.TotalMinutes < TimeValue)
            {

                Stack.AddRange(Ran.RandomSample(SampleValue));

                if (Stack.Count > Constants.MaxPlotBuffLength)
                {
                    Stack.RemoveRange(0, (Stack.Count() - (int)Constants.MaxPlotBuffLength));
                    Array.Copy(Stack.ToArray(), PlotDataBuffer, (int)Constants.MaxPlotBuffLength);
                }
                else
                    Array.Copy(Stack.ToArray(), PlotDataBuffer, Stack.Count());

                Sigplot.MaxRenderIndex = Stack.Count() - 1;

                if (!bIsCustomXEnable)
                    PlotBase.Plot.AxisAuto();
                else
                {
                    PlotBase.Plot.AxisAutoY();
                    PlotBase.Plot.SetAxisLimitsX(
                        Math.Max(0, Sigplot.MaxRenderIndex - double.Parse(TextCustomX.Text)), 
                        Math.Max(1, Sigplot.MaxRenderIndex));
                }

                UiInvoke(delegate { PlotBase.Refresh(); });
                Thread.Sleep(50);
            }

        }

        public bool ValidateValue()
        {
            int Dummy = 0;

            if (int.TryParse(TextPresetSample.Text, out Dummy))
                if (Dummy < 0 && Dummy > Constants.MaxPlotBuffLength)
                    return false;

            if (int.TryParse(TextPresetTime.Text, out Dummy))
                if (Dummy < 0 && Dummy > Constants.MeasureTimeLimit)
                    return false;

            if (int.TryParse(TextCustomX.Text, out Dummy))
                if (Dummy < 0 && Dummy > Constants.CustomXaxisLength)
                    return false;

            return true;
        }


        public vmExampleLiveChairPlot()
        {
            // Create plot
            PlotBase = new WpfPlot();
            PlotBase.Configuration.LockHorizontalAxis = true;
            PlotBase.Configuration.LockVerticalAxis = true;

            // InitializePlot
            PlotDataBuffer = new double[(int)Constants.MaxPlotBuffLength];
            Sigplot = PlotBase.Plot.AddSignal(PlotDataBuffer);
            Sigplot.MaxRenderIndex = 0;
            PlotBase.Refresh();

            // Create Controls
            TextPresetSample = new vmTextboxWithClearBtn(
                _MaxLimit: Constants.MaxPlotBuffLength.ToString(),
                _MinLimit: "1",
                _Txt: "100");
            TextPresetTime = new vmTextboxWithClearBtn(
                _MaxLimit: Constants.MeasureTimeLimit.ToString(),
                _MinLimit: "1",
                _Txt: "10");
            TextCustomX = new vmTextboxWithClearBtn(
                _MaxLimit: Constants.CustomXaxisLength.ToString(),
                _MinLimit: "1",
                _Txt: "100");

            // Set Background thread
            bWorker = new BackgroundWorker();
            bWorker.DoWork += RunLivePlot;
        }

    }



}
