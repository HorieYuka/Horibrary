using ScottPlot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }

    class NumericValidationRule : ValidationRule
    {
        public Type ValidationType { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string strValue = Convert.ToString(value);

            if (string.IsNullOrEmpty(strValue))
                return new ValidationResult(false, $"Value cannot be coverted to string.");
            bool canConvert = false;
            switch (ValidationType.Name)
            {
                case "Boolean":
                    bool boolVal = false;
                    canConvert = bool.TryParse(strValue, out boolVal);
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of boolean");
                case "Int32":
                    int intVal = 0;
                    canConvert = int.TryParse(strValue, out intVal);
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of Int32");
                case "Double":
                    double doubleVal = 0;
                    canConvert = double.TryParse(strValue, out doubleVal);
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of Double");
                case "Int64":
                    long longVal = 0;
                    canConvert = long.TryParse(strValue, out longVal);
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of Int64");
                default:
                    throw new InvalidCastException($"{ValidationType.Name} is not supported");
            }
        }
    }


}
