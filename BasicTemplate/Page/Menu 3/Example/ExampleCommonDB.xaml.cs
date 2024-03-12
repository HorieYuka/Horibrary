using BasicTemplate.Base;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// ExampleCommonDB.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ExampleCommonDB : UserControl
    {
        public ExampleCommonDB()
        {
            InitializeComponent();
        }

        private void PwordBoxChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password; }
        }
    }

    class vmExampleCommonDB : ObservableObject, IExample
    {
        public string ExampleName => "기본적인 DBMS 동작";
        public short ExampleNum => 0;

        public string Id { private get; set; }
        // Set "private get" for hiding Password and this way should not break MVVM rules.
        public string Password { private get; set; }

        public string BasePath { private get; set; }

        private BackgroundWorker LoginWorker;

        public vmExampleCommonDB()
        {
            LoginWorker = new BackgroundWorker();
            LoginWorker.DoWork += LoginAttempt;
        }

        private void LoginAttempt(object? sender, DoWorkEventArgs e)
        {
            DBMS.Connect(Id, Password);
        }

        private ICommand _UploadFileCmd;
        public ICommand UploadFileCmd
        {
            get
            {
                if (_UploadFileCmd == null)
                    _UploadFileCmd = new BaseCommand(p =>
                    {
                    });
                return _UploadFileCmd;
            }
        }

        private ICommand _DeleteFileCmd;
        public ICommand DeleteFileCmd
        {
            get
            {
                if (_DeleteFileCmd == null)
                    _DeleteFileCmd = new BaseCommand(p =>
                    {

                    });
                return _DeleteFileCmd;
            }
        }

        private ICommand _CreateDirCmd;
        public ICommand CreateDirCmd
        {
            get
            {
                if (_CreateDirCmd == null)
                    _CreateDirCmd = new BaseCommand(p =>
                    {

                    });
                return _CreateDirCmd;
            }
        }

        private ICommand _DeleteDirCmd;
        public ICommand DeleteDirCmd
        {
            get
            {
                if (_DeleteDirCmd == null)
                    _DeleteDirCmd = new BaseCommand(p =>
                    {

                    });
                return _DeleteDirCmd;
            }
        }
    }
}
