using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System.IO.Ports;
using AForge.Vision;
using AForge.Vision.Motion;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection VideoCapTureDevices;
        private VideoCaptureDevice Finalvideo;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //comboBox2.DataSource = SerialPort.GetPortNames();
            VideoCapTureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo VideoCaptureDevice in VideoCapTureDevices)
            {

                comboBox1.Items.Add(VideoCaptureDevice.Name);

            }

            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Finalvideo = new VideoCaptureDevice(VideoCapTureDevices[comboBox1.SelectedIndex].MonikerString);
            Finalvideo.NewFrame += new NewFrameEventHandler(Finalvideo_NewFrame);
            Finalvideo.Start();
        }

        void Finalvideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            Bitmap image = (Bitmap)eventArgs.Frame.Clone();
            // create filter
            Mirror filter = new Mirror(false, true);
            // apply the filter
            filter.ApplyInPlace(image);

            ResizeBilinear filterr = new ResizeBilinear(450, 360);
            // apply the filter

            Bitmap newimage = filterr.Apply(image);

            pictureBox1.Image = newimage;

            //image1 için işlemler

            Bitmap image1 = (Bitmap)eventArgs.Frame.Clone();
            Mirror filterrrr = new Mirror(false, true);
            // apply the filter
            filterrrr.ApplyInPlace(image1);

            ResizeBilinear filterrr = new ResizeBilinear(450, 360);
            // apply the filter

            Bitmap sonimage = filterrr.Apply(image1);


            //
            // El Tanıma Eklenecek
            //
            ColorFiltering Cfilter = new ColorFiltering();
            // set color ranges to keep
            Cfilter.Red = new IntRange(150, 255);
            Cfilter.Green = new IntRange(50, 150);
            Cfilter.Blue = new IntRange(50, 150);
            // apply the filter
            Cfilter.ApplyInPlace(sonimage);
            // create motion detector


            // ring alarm or do somethng else
            BlobCounter bc = new BlobCounter();
                // process binary image
                bc.ProcessImage(sonimage);
                Rectangle[] rects = bc.GetObjectsRectangles();
                // process blobs
                foreach (Rectangle rect in rects)
                {
                    // ...
                    if (rects.Length > 1)
                    {
                        Rectangle objectRect = rects[0];
                        //Graphics g = Graphics.FromImage(image);
                        Graphics g = pictureBox1.CreateGraphics();
                        using (Pen pen = new Pen(Color.FromArgb(252, 3, 26), 2))
                        {
                            g.DrawRectangle(pen, objectRect);
                        }
                        //Cizdirilen Dikdörtgenin Koordinatlari aliniyor.
                        int objectX = objectRect.X + (objectRect.Width / 2);
                        int objectY = objectRect.Y + (objectRect.Height / 2);
                        //  g.DrawString(objectX.ToString() + "X" + objectY.ToString(), new Font("Arial", 12), Brushes.Red, new System.Drawing.Point(250, 1));

                        g.Dispose();

                        if (objectX <= 150 && objectY <= 120)
                        {
                            serialPort1.Write("6");
                        }

                        else if (objectX > 300 && objectX <= 450 && objectY <= 120)
                        {
                            serialPort1.Write("3");
                        }
                        else if (objectX <= 150 && objectY > 120 && objectY <= 240)
                        {
                            serialPort1.Write("5");
                        }

                        else if (objectX > 300 && objectX <= 450 && objectY > 120 && objectY <= 240)
                        {
                            serialPort1.Write("2");
                        }
                        else if (objectX <= 150 && objectY > 240 && objectY <= 360)
                        {
                            serialPort1.Write("4");
                        }

                        else if (objectX > 300 && objectX <= 450 && objectY > 240 && objectY <= 360)
                        {
                            serialPort1.Write("1");
                        }

                    }
                    

                }
            

            pictureBox2.Image = sonimage;

            // create an instance of blob counter algorithm



        }

        private void button2_Click(object sender, EventArgs e)
        {
            serialPort1.BaudRate = 9600;
            serialPort1.PortName = comboBox2.SelectedItem.ToString();
            serialPort1.Open();
            if (serialPort1.IsOpen == true)
            {
                Altyazı.Text = serialPort1.PortName + " portuna bağlandı.";


                button2.Enabled = false;


            }
            else
            {
                Altyazı.Text = "Porta bağlamadı. Kontrol et!!";
            }
        }
    }
}
