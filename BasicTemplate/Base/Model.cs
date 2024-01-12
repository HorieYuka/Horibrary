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

    class ModelPlot : ModelBase
    {
        public bool bIsCustomXEnable { get; set; }

        private string _PresetSample;
        public string PresetSample

        {
            get => _PresetSample;
            set
            {
                _PresetSample = value;
                OnPropertyChanged("PresetSample");
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

        private string _CustomX;
        public string CustomX

        {
            get => _CustomX;
            set
            {
                _CustomX = value;
                OnPropertyChanged("CustomX");
            }
        }

        public bool ValidateValue()
        {
            int Dummy = 0;

            if (int.TryParse(PresetSample, out Dummy))
                if (Dummy < 0 && Dummy > Constants.MaxPlotBuffLength)
                    return false;

            if (int.TryParse(PresetTime, out Dummy))
                if (Dummy < 0 && Dummy > Constants.MeasureTimeLimit)
                    return false;

            if (int.TryParse(CustomX, out Dummy))
                if (Dummy < 0 && Dummy > Constants.CustomXaxisLength)
                    return false;

            return true;
        }

        public ModelPlot(string _PresetSample = "100", string _PresetTime = "10", string _CustomX = " 100")
        {
            PresetSample = _PresetSample;
            PresetTime = _PresetTime;
            CustomX = _CustomX;
        }
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
}
