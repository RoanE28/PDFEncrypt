using iText.Kernel.Pdf;

using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.IO;
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

namespace PDFEncrypt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedCommand SecureCommand = new RoutedCommand();

        private string _header = "PDF Encrypt";
        public string _securedFileLocation = "";
        public string _securedFileName = "";
        private FileInfo _originalFileInfo;
        public FileInfo OriginalFileInfo
        {
            get { return _originalFileInfo; }
            set
            {
                _originalFileInfo = value;
                if (_originalFileInfo != null)
                {
                    txtOriginalFile.Text = _originalFileInfo.FullName;
                    txtOriginalFile.CaretIndex = txtOriginalFile.Text.Length;

                    _securedFileLocation = $@"{_originalFileInfo.Directory}\Secuired\";
                    _securedFileName = _originalFileInfo.Name;
                    txtSecuredFile.Text = System.IO.Path.Combine(_securedFileLocation, _securedFileName);
                    txtSecuredFile.CaretIndex = txtSecuredFile.Text.Length;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            SecureCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
        }

        public string OrifinalFileName
        {
            get
            {
                return _originalFileInfo?.FullName ?? "";
            }
        }

        private void OnOpenfile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "pdf files (*.pdf)|*.pdf";
            if (ofd.ShowDialog().GetValueOrDefault())
            {
                OriginalFileInfo = new FileInfo(ofd.FileName);
            }
        }

        private void OnSavefile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "pdf files (*.pdf)|*.pdf";
            sfd.DefaultExt = _securedFileLocation;
            if (sfd.ShowDialog().GetValueOrDefault())
            {
                _securedFileLocation = System.IO.Path.GetDirectoryName(sfd.FileName);//sfd.FileName.Replace(sfd.SafeFileName, "");
                _securedFileName = sfd.SafeFileName;
                txtSecuredFile.Text = System.IO.Path.Combine(_securedFileLocation, _securedFileName);
                txtSecuredFile.CaretIndex = txtSecuredFile.Text.Length;
            }
        }

        public void OnSecureFile(object sender, RoutedEventArgs e)
        {
            if (OriginalFileInfo == null)
            {
                MessageBox.Show("No file selected.", _header, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var dialog = new PasswordDialog();
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                SecureFile(dialog.Password);
            }
        }

        private void SecureFile(string password)
        {
            try
            {
                string sourcePdf = _originalFileInfo.FullName;
                string destPdf = System.IO.Path.Combine(_securedFileLocation, _securedFileName);
                if (!Directory.Exists(_securedFileLocation))
                    Directory.CreateDirectory(_securedFileLocation);

                if (File.Exists(destPdf))
                {
                    if (MessageBox.Show("Secured Output file already exists. Do you want to replace this file?", _header, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                        return;

                    File.Delete(destPdf);
                }

                var permissions = 0;

                PdfReader readerProperties = new PdfReader(sourcePdf);
                PdfDocument pdfDocTemp = new PdfDocument(readerProperties);
                permissions = (int)readerProperties.GetPermissions();

                using (Stream input = new FileStream(sourcePdf, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    PdfReader reader = new PdfReader(input);

                    using (Stream output = new FileStream(destPdf, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        var userPassword = Encoding.ASCII.GetBytes(password);

                        WriterProperties props = new WriterProperties().SetStandardEncryption(userPassword, null, EncryptionConstants.ALLOW_PRINTING, EncryptionConstants.STANDARD_ENCRYPTION_128);
                        PdfWriter writer = new PdfWriter(output, props);
                        PdfDocument pdfDoc = new PdfDocument(reader, writer);
                        pdfDoc.Close();

                    }
                }

                ClearCurrentFileInfo();

                if (MessageBox.Show("The PDF is now password protected. Would you like to open it?", _header, MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    System.Diagnostics.Process.Start(destPdf);
            }
            catch (iText.Kernel.Crypto.BadPasswordException pex)
            {
                MessageBox.Show("The PDF you are trying to secure is already password protected.", _header, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _header, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void OnExit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void OnAbout(object sender, RoutedEventArgs e)
        {
        }

        private void ClearCurrentFileInfo()
        {
            txtOriginalFile.Text = "";
            txtOriginalFile.CaretIndex = 0;

            _originalFileInfo = null;

            _securedFileLocation = "";
            _securedFileName = "";
            txtSecuredFile.Text = "";
            txtSecuredFile.CaretIndex = 0;
        }

    }


}
