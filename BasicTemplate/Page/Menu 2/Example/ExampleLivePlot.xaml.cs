using BasicTemplate.Base;
using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

        public string ExampleName => "실시간 그래프 생성하기";
        public short ExampleNum => 1;

        public WpfPlot PlotBase { get; set; }
        public ModelPlot Model { get; set; }

        private SignalPlot Sigplot;

        private double[] PlotDataBuffer;
        private BackgroundWorker bWorker;

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


        private void RunLivePlot(object? sender, DoWorkEventArgs e)
        {

        }

        public vmExampleLivePlot()
        {

        }


    }
}
