using System.Windows;

namespace ZFPimImporter
{
    public partial class ModalWarning : Window
    {
        public static string WarningMessage;
        public ModalWarning()
        {
            InitializeComponent();
            txtSomeBox.Text = WarningMessage;
        }

        private void BtnOkay_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}