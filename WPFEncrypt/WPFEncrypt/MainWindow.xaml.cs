using iText.Kernel.Pdf;

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

namespace WPFEncrypt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Encrypt();
        }

        public void Encrypt()
        {
            string sourcePdf = @"E:\Temp\boardingPass.pdf";
            //Passowrd the pwd for PDF security                 
            string destPdf = @"E:\Temp\securedboardingPass.pdf";
            if (File.Exists(destPdf))
                File.Delete(destPdf);

            using (Stream input = new FileStream(sourcePdf, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                PdfReader reader = new PdfReader(input);
                //PdfDocument pdfDocTemp = new PdfDocument(reader);
                //var permissions = (int)reader.GetPermissions();
                // sourcePdf  unsecured PDF file
                //destPdf secured PDF file

                using (Stream output = new FileStream(destPdf, FileMode.Create, FileAccess.Write, FileShare.None))
                {



                    string Password = "abc@123";

                    //var properties = new EncryptionProperties();

                    var userPassword = Encoding.ASCII.GetBytes(Password);

                    WriterProperties props = new WriterProperties().SetStandardEncryption(userPassword, null, EncryptionConstants.ALLOW_PRINTING, EncryptionConstants.STANDARD_ENCRYPTION_128);// | EncryptionConstants.DO_NOT_ENCRYPT_METADATA
                    PdfWriter writer = new PdfWriter(output, props);
                    PdfDocument pdfDoc = new PdfDocument(reader, writer);
                    pdfDoc.Close();

                    //properties.SetStandardEncryption(userPassword, null, permissions, (int)EncryptionConstants.STANDARD_ENCRYPTION_128);


                    //PdfEncryptor.Encrypt(reader, output, properties, null);
                    //PdfEncryptor.Encrypt(reader, output, true, Password, Password, PdfWriter.ALLOW_PRINTING);
                }
            }
        }
    }

    
}
