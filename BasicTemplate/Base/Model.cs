using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BasicTemplate.Base
{
    class ModelBase : ObservableObject
    {
        // Insert common  properties here.
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

    static class ModelConstDevice1
    {
        public static readonly string[] BaudrateList =
            ["2400", "4800", "9600", "14400", "19200", " 28800", "38400", "57600", "76800", "115200", "230400"];
    }

    #endregion
}
