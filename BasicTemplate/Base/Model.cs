using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BasicTemplate.Base
{
    class ModelBase : ObservableObject
    {
        // Insert common  properties here.
    }

    class ModelCOM : ModelBase
    {
        public bool IsConnected { get; set; }

        public string Port { get; set; }
        public string Name { get; set; }
        public int Baudrate { get; set; }
    }

    class BelowStatusModel : ModelBase
    {
        private int? _BelowBar;
        public int? BelowBar

        {
            get => _BelowBar;
            set
            {
                _BelowBar = value;
                OnPropertyChanged("BelowBar");
            }
        }

        private string? _BelowText;
        public string? BelowText
        {
            get => _BelowText;
            set
            {
                _BelowText = value;
                OnPropertyChanged("BelowText");
            }
        }
        
        private bool? _BelowCircle;
        public bool? BelowCircle
        {
            get => _BelowCircle;
            set
            {
                _BelowCircle = value;
                OnPropertyChanged("BelowCircle");
            }
        }
    }


    #region Constant for Examples

    static class ModelConstChart1
    {
        public static readonly double MaxPlotBuffLength = 100000;
        public static readonly double MeasureTimeLimit = 60;
        public static readonly double CustomXaxisLength = 10000;
    }
    static class ModelConstChart2
    {
        public static readonly double MaxPlotBuffLength = 100000;
        public static readonly double MeasureTimeLimit = 60;
        public static readonly double CustomXaxisLength = 100000;

    }
    static class ModelConstChart3
    {
        public static readonly double MaxPlotBuffLength = 1000;
        public static readonly double MeasureTimeLimit = 60;
    }

    #endregion
}
