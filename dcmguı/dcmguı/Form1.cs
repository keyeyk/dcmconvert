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
using Dicom.Imaging.Render;

namespace dcmguı
{
    public partial class Form1 : Form
    {
        private Point[] points = new Point[3]; // 3 noktalık bir dizi tanımlanıyor
        private int pointCount = 0; // Nokta sayısını takip eden bir değişken
        private bool measurementEnabled;
        private double X { get; set; }
        private double Y { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void InitializeComponents()
        {
            // Form ve kontrol oluşturma kodları
            this.Text = "DICOM Image Viewer";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            pictureBox1 = new PictureBox();
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            this.Controls.Add(pictureBox1);

            //pictureBox1.MouseClick += PictureBox_MouseClick;

            Button openButton = new Button();
            openButton.Text = "Dosya Seç";
            openButton.Click += OpenButton_Click;

            this.Controls.Add(openButton);
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Resim Dosyalarý|*.jpg;*.jpeg;*.png|Tüm Dosyalar|*.*";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string[] selectedFiles = openFileDialog.FileNames;
                // Seçilen dosyalarý kullanýn
                foreach (string file in selectedFiles)
                {
                    // Dosya ile ilgili iþlemleri gerçekleþtirin
                    // Örneðin: resmi bir PictureBox'a yükleme
                    pictureBox1.Image = Image.FromFile(file);
                }
            }
        }



        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (measurementEnabled)
            {
                if (points == null)
                {
                    points = new Point[3]; // points dizisini başlat
                }

                if (pointCount < 3)
                {
                    points[pointCount] = e.Location; // points dizisine noktaları ata
                    pointCount++;
                }

                ((PictureBox)sender).Invalidate();
            }
        }


        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (pointCount < 3)
            {
                points[pointCount] = e.Location;
                pointCount++;

                if (pointCount == 3)
                {
                    pictureBox1.MouseDown -= PictureBox_MouseDown;

                    // Açıyı hesapla ve mesaj olarak göster
                    double angle = CalculateAngle(points[0], points[1], points[2]);
                    MessageBox.Show("Açı: " + angle.ToString() + " derece");
                }
            }
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Kırmızı noktaları çiz
            foreach (Point point in points)
            {
                if (point != null)
                {
                    g.FillEllipse(Brushes.Red, point.X - 5, point.Y - 5, 10, 10);
                }
            }

            // Noktalar arası çizgiyi çiz
            if (pointCount >= 3)
            {
                g.DrawLine(Pens.Red, points[0], points[1]);
                g.DrawLine(Pens.Red, points[1], points[2]);
                g.DrawLine(Pens.Red, points[2], points[0]);
            }
        }


        private void SelectPointsButton_Click(object sender, EventArgs e)
        {
            pointCount = 0;
            pictureBox1.MouseDown += PictureBox_MouseDown;
            pictureBox1.Paint += PictureBox_Paint;
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            if (pointCount == 3)
            {
                double angle = CalculateAngle(points[0], points[1], points[2]);
                MessageBox.Show($"Seçilen noktalar arasındaki açı: {angle} derece");
            }
            else
            {
                MessageBox.Show("Lütfen önce 3 nokta seçin!");
            }
        }

        private void CalculateAndDrawAngle()
        {
            // Üç noktanın koordinatlarını girin
            Point point1 = new Point { X = 0, Y = 0 };
            Point point2 = new Point { X = 1, Y = 0 };
            Point point3 = new Point { X = 1, Y = 1 };

            // İki vektörün oluşturduğu açıyı hesaplayın
            double angle = CalculateAngle(point1, point2, point3);

            MessageBox.Show("Üç nokta arasındaki açı: " + angle + " derece");

        }

        private double CalculateDistance(Point p1, Point p2)
        {
            int dx = p2.X - p1.X;
            int dy = p2.Y - p1.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        private double CalculateAngle(Point p1, Point p2, Point p3)
        {
            double angleRadians = Math.Atan2(p3.Y - p2.Y, p3.X - p2.X) - Math.Atan2(p1.Y - p2.Y, p1.X - p2.X);
            double angleDegrees = angleRadians * (180.0 / Math.PI);
            return angleDegrees;
        }


        private void MeasurementButton_Click(object sender, EventArgs e)
        {
            measurementEnabled = true;
            points = null;
            pointCount = 0;
            pictureBox1.Invalidate();
        }


    }
}
