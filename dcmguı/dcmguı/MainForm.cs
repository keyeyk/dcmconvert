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

namespace dcmguı
{
    public partial class MainForm : Form
    {
        private string tempFolderPath = Path.Combine(Path.GetTempPath(), "TempDCMFiles");

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Geçici klasörü oluştur
            Directory.CreateDirectory(tempFolderPath);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Geçici klasörü sil
            Directory.Delete(tempFolderPath, true);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "DICOM Files (*.dcm)|*.dcm|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string dcmFilePath = openFileDialog.FileName;
                ConvertDCMtoPNGAndShow(dcmFilePath);
            }
        }

        private void ConvertDCMtoPNGAndShow(string dcmFilePath)
        {
            try
            {
                using (Image image = Image.FromFile(dcmFilePath))
                {
                    // Convert DICOM to PNG
                    string pngFilePath = dcmFilePath + ".png";
                    image.Save(pngFilePath, ImageFormat.Png);

                    // Show PNG in PictureBox
                    pictureBox1.Image = Image.FromFile(pngFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
