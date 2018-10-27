using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Weapon_Generator
{
    public partial class Form1 : Form
    {
        int rotateTimes = 1;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Image Files(*.jpeg;*.bmp;*.png;*.jpg)|*.jpeg;*.bmp;*.png;*.jpg";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            pictureBox1.Image = Image.FromFile(filePath);
            pictureBox1.Visible = true;
            pictureBox2.Visible = true;
            button1.Visible = false;
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
                pictureBox2.Image = RotateImage(new Bitmap(pictureBox1.Image), 18 * rotateTimes);
                rotateTimes++;
        }

        private Bitmap RotateImage(Bitmap rotateMe, float angle)
        {
            //First, re-center the image in a larger image that has a margin/frame
            //to compensate for the rotated image's increased size

            //var bmp = new Bitmap(rotateMe.Width + (rotateMe.Width / 2), rotateMe.Height + (rotateMe.Height / 2));

            //using (Graphics g = Graphics.FromImage(bmp))
            //    g.DrawImageUnscaled(rotateMe, (rotateMe.Width / 4), (rotateMe.Height / 4), bmp.Width, bmp.Height);

            //bmp.Save("moved.png");
            //rotateMe = bmp;

            //Now, actually rotate the image
            Bitmap rotatedImage = new Bitmap(rotateMe.Width, rotateMe.Height);
            Bitmap ss = new Bitmap(rotateMe.Width * 40, rotateMe.Height * 40);
            //Bitmap backup = rotatedImage;

            using (Graphics t = Graphics.FromImage(ss))
            {
                for (int i = 1; i < 5; i++)
                {
                    for (int j = 1; j < 11; j++)
                    {
                        using (Graphics g = Graphics.FromImage(rotatedImage))
                        {
                            t.DrawImage(Rotate(rotateMe, 18 * (i * j)), new Point(rotateMe.Height * j, rotateMe.Width * i)); //draw the image on the new bitmap
                            //rotatedImage = backup;
                        }
                    }
                }
            }
            ss.Save("rotated.png");
            return ss;
        }

        private Bitmap Rotate(Bitmap rotateMe, float angle)
        {
            //First, re-center the image in a larger image that has a margin/frame
            //to compensate for the rotated image's increased size

            //var bmp = new Bitmap(rotateMe.Width + (rotateMe.Width / 2), rotateMe.Height + (rotateMe.Height / 2));

            //using (Graphics g = Graphics.FromImage(bmp))
            //    g.DrawImageUnscaled(rotateMe, (rotateMe.Width / 4), (rotateMe.Height / 4), bmp.Width, bmp.Height);

            //bmp.Save("moved.png");
            //rotateMe = bmp;

            //Now, actually rotate the image
            Bitmap rotatedImage = new Bitmap(rotateMe.Width, rotateMe.Height);

            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                g.TranslateTransform(rotateMe.Width / 2, rotateMe.Height / 2);   //set the rotation point as the center into the matrix
                g.RotateTransform(angle);                                        //rotate
                g.TranslateTransform(-rotateMe.Width / 2, -rotateMe.Height / 2); //restore rotation point into the matrix
                g.DrawImage(rotateMe, new Point(0, 0));                          //draw the image on the new bitmap
            }

            rotatedImage.Save("rotated.png");
            return rotatedImage;
        }
    }
}
