using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dicom;
using Dicom.Imaging;


namespace dcmguı
{
    public partial class MainForm : Form
    {
        private string tempFolderPath = Path.Combine(Path.GetTempPath(), "TempDCMFiles");
        private object listViewFiles;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Geçici klasörü sil
            Directory.Delete(tempFolderPath, true);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Görüntü Dosyaları|*.dcm;*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.tif;*.tiff|Tüm Dosyalar|*.*";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in openFileDialog.FileNames)
                {
                    ShowImage(filePath);
                }
            }
        }

        private void ShowImage(string dcmFilePath)
        {
            try
            {
                DicomFile dicomFile = DicomFile.Open(dcmFilePath);
                DicomImage dicomImage = new DicomImage(dicomFile.Dataset);

                Image image = dicomImage.RenderImage().As<Image>();

                pictureBox1.Image = image;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ConvertDCMtoPNGAndShow(string dcmFilePath)
        {
            try
            {
                DicomFile dicomFile = DicomFile.Open(dcmFilePath);
                DicomImage dicomImage = new DicomImage(dicomFile.Dataset);

                // Create a bitmap from the DICOM image
                Bitmap bitmap = dicomImage.RenderImage().As<Bitmap>();

                // Show the bitmap in PictureBox
                pictureBox1.Image = bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



    }
}
