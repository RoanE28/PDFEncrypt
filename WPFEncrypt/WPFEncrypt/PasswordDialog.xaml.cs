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
        public string Password;
        public PasswordDialog()
        {
            InitializeComponent();
            txtPassword.Focus();
        }
        private void OnCancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnSecure(object sender, RoutedEventArgs e)
        {
            Password = txtPassword.Password;
            this.DialogResult = true;
            this.Close();
        }
    }
}
