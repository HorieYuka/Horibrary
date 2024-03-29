﻿using System;
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

    class PaletteDataModel : ObservableObject
    {
        public event EventHandler UpdateOptEvt;

        private short _Alpha;
        public short Alpha
        {
            get => _Alpha;
            set
            {
                _Alpha = value;
                TellValueChanged();
                OnPropertyChanged("Alpha");
            }
        }


        private short _ColorWidth;
        public short ColorWidth
        {
            get => _ColorWidth;
            set
            {
                _ColorWidth = value;
                TellValueChanged();
                OnPropertyChanged("ColorWidth");
            }
        }

        public PaletteDataModel()
        {
            Alpha = 100;
            ColorWidth = 25;
        }

        private void TellValueChanged()
            => UpdateOptEvt?.Invoke(null, new EventArgs());

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
        public static readonly int GetBufferLength = 4096;
    }

    static class ModelConstDevice2
    {
        public static readonly int IO_OUT_BUFLEN = 128;
    }

    #endregion
}
