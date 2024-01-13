using BasicTemplate.Base;
using System;
using System.Collections.Generic;
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

namespace BasicTemplate.Control
{
    /// <summary>
    /// TextboxWithClearBtn.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TextboxWithClearBtn : UserControl
    {
        public TextboxWithClearBtn()
        {
            InitializeComponent();
        }
    }

    class vmTextboxWithClearBtn : ObservableObject
    {
        private string MaxLimit;
        private string MinLimit;

        private string _Text;
        public string Text
        {
            get => _Text;
            set
            {

                double Dummy = 0;

                // Determine whether this value is numeric / float form.
                if (MaxLimit != null || MinLimit != null)
                {
                    if(double.TryParse(value, out Dummy))
                        _Text = value;
                    else
                        _Text = MinLimit;

                    if (!string.IsNullOrEmpty(MaxLimit))
                    {
                        if (Dummy > double.Parse(MaxLimit))
                            _Text = MaxLimit;
                    }

                    if (!string.IsNullOrEmpty(MinLimit))
                    {
                        if (Dummy < double.Parse(MinLimit))
                            _Text = MinLimit;
                    }
                }
                else
                    _Text = value;



                OnPropertyChanged("Text");
            }
        }

        private ICommand _ClearTextCmd;
        public ICommand ClearTextCmd
        {
            get
            {
                if (_ClearTextCmd == null)
                    _ClearTextCmd = new BaseCommand(p =>
                    {
                        Text = "";

                    });
                return _ClearTextCmd;
            }
        }



        public vmTextboxWithClearBtn(string _Txt, string _MaxLimit = null, string _MinLimit = null)
        {
            MaxLimit = _MaxLimit;
            MinLimit = _MinLimit;

            // Avoid error.
            double Dummy = 0;
            if (!double.TryParse(MaxLimit, out Dummy) || !double.TryParse(MinLimit, out Dummy))
            {
                MaxLimit = "";
                MinLimit = "";
            }

            Text = _Txt;
        }
    }
}
