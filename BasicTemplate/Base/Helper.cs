using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace BasicTemplate.Base
{
    class Helper
    {

        public static EventHandler EvtBelowStatus;

        public static void UpdateBelowStatus(bool? _BelowCircle = null, string? _BelowText = null, int? _Belowbar = null)
        {
            BelowStatusModel Md = new BelowStatusModel();

            Md.BelowCircle = _BelowCircle;
            Md.BelowText = _BelowText;
            Md.BelowBar = _Belowbar;

            if (EvtBelowStatus != null)
                EvtBelowStatus.Invoke(Md, new EventArgs());
        }

        public static MessageBoxResult ClaimMessBox(string Header, string Comment,
            MessageBoxButton Btn, MessageBoxImage Img)
              => MessageBox.Show(Comment, Header, Btn, Img);

        public static MessageBoxResult ClaimWaitMessBox(string Header, string Comment,
            MessageBoxButton Btn, MessageBoxImage Img)
        {
            MessageBoxResult Result = new MessageBoxResult();

            Task.Run(() =>
            Result = ClaimMessBox(Header, Comment,
            MessageBoxButton.OK, MessageBoxImage.Question));

            return Result;
        }

    }

    [ValueConversion(typeof(bool), typeof(bool))]
    class InverseBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }

    static class Constants
    {
        public const double  MaxPlotBuffLength = 100000;
        public const double MeasureTimeLimit = 60;
        public const double CustomXaxisLength = 10000;
    }
}
