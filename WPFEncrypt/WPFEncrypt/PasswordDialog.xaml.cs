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
using System.Windows.Shapes;

namespace PDFEncrypt
{
    /// <summary>
    /// Interaction logic for PasswordDialog.xaml
    /// </summary>
    public partial class PasswordDialog : Window
    {
        private string _header = "PDF Encrypt";
        public static RoutedCommand SecureCommand = new RoutedCommand();

        public string Password;
        public PasswordDialog()
        {
            InitializeComponent();
            SecureCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            txtPassword.Focus();
        }
        private void OnCancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnSecure(object sender, RoutedEventArgs e)
        {

            Password = txtPassword.Password;
            if (string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Password can't be empty.", _header, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.DialogResult = true;
            this.Close();
        }
    }
}
