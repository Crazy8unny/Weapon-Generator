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
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Weapon_Generator
{
    public partial class Form1 : Form
    {
        Form2 pic = new Form2();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(openFile());
            label2.Visible = false;
            pictureBox1.Visible = true;
            checkIfBrowsed();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pic.pictureBox2.Image = RotateImage(new Bitmap(pictureBox1.Image), new Bitmap(pictureBox2.Image), (float)22.5);
            saveFile();
        }

        private Bitmap RotateImage(Bitmap rotateMe, Bitmap rotateMe2, float angle)
        {
            int x, y, dx, dy;
            int decr = 1;
            int size = rotateMe.Width;
            if (rotateMe.Height > size)
                size = rotateMe.Height;
            size++;
            //Now, actually rotate the image
            Bitmap ss = new Bitmap(size * 24, size * 8);
            Pen p = new Pen(Color.Green);

            using (Graphics t = Graphics.FromImage(ss))
            {
                if (checkBox2.Checked)
                {
                    Brush brush = new SolidBrush(Color.Black);
                    t.FillRectangle(brush, new System.Drawing.Rectangle(0, 0, ss.Width, ss.Height));
                }
                for (int i = 1; i < 5; i++)
                {
                    for (int j = 1; j < 11; j++)
                    {
                        using (Graphics g = Graphics.FromImage(rotateMe))
                        {
                            if (i > 2)
                            {
                                rotateMe = rotateMe2;
                                decr = 3;
                            }
                            x = (size) * j;
                            y = (size) * i;
                            dx = x + (size / 2);
                            dy = y + (size / 2);
                            t.DrawImage(RotateImg(rotateMe, (float)(22.5 * ((i - decr) * 10 + j - 1))), new Point(x, y)); //draw the image on the new bitmap
                            if (checkBox1.Checked)
                            {
                                t.DrawLine(p, new Point(x, y), new Point(x, y + size));
                                t.DrawLine(p, new Point(x, y), new Point(x + size, y));
                                t.DrawLine(p, new Point(x, y + size), new Point(x + size, y + size));
                                t.DrawLine(p, new Point(x + size, y), new Point(x + size, y + size));
                            }
                        }
                    }
                }
            }
            return ss;
        }

        //private Bitmap RotateImg(Bitmap bmp, float angle)
        //{
        //    Bitmap rotatedImage = new Bitmap(bmp.Width, bmp.Height);
        //    using (Graphics g = Graphics.FromImage(rotatedImage))
        //    {
        //        // Set the rotation point to the center in the matrix
        //        g.TranslateTransform(bmp.Width / 2, bmp.Height / 2);
        //        // Rotate
        //        g.RotateTransform(angle);
        //        // Restore rotation point in the matrix
        //        g.TranslateTransform(-bmp.Width / 2, -bmp.Height / 2);
        //        // Draw the image on the bitmap
        //        g.DrawImage(bmp, new Point(0, 0));
        //    }

        //    return rotatedImage;
        //}

        public static Bitmap RotateImg(Bitmap bmp, float angle)
        {
            int size = bmp.Width;
            if (bmp.Height > size)
                size = bmp.Height;
            size++;
            int w = bmp.Width;
            int h = bmp.Height;
            Bitmap tempImg = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(tempImg);
            g.DrawImage(bmp, new Point(0, 0));
            g.Dispose();
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, w, h));
            Matrix mtrx = new Matrix();
            mtrx.RotateAt(angle, new PointF(0, 0));
            RectangleF rct = path.GetBounds(mtrx);
            Bitmap newImg = new Bitmap(Convert.ToInt32(rct.Width), Convert.ToInt32(rct.Height));
            g = Graphics.FromImage(newImg);
            g.TranslateTransform(-rct.X, -rct.Y);
            g.RotateTransform(angle);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(tempImg, 0, 0);
            g.Dispose();
            tempImg.Dispose();
            newImg = ScaleImage(newImg, size, size);
            return newImg;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pic.pictureBox2.Image = RotateImage(new Bitmap(pictureBox1.Image), new Bitmap(pictureBox2.Image), (float)22.5);
            Image a = pic.pictureBox2.Image;
            pic = new Form2();
            pic.pictureBox2.Image = a;
            pic.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = Image.FromFile(openFile());
            label3.Visible = false;
            pictureBox2.Visible = true;
            checkIfBrowsed();
        }

        private string openFile()
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

            return filePath;
        }

        private void saveFile()
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = "c:\\";
                saveFileDialog.Filter = "Image Files(*.jpeg;*.bmp;*.png;*.jpg)|*.jpeg;*.bmp;*.png;*.jpg";
                saveFileDialog.FilterIndex = 2;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = saveFileDialog.FileName;
                    pic.pictureBox2.Image.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }

        private void checkIfBrowsed()
        {
            if (pictureBox1.Image != null && pictureBox2.Image != null)
            {
                button2.Enabled = true;
                button3.Enabled = true;
            }
        }

        private static Bitmap ScaleImage(Bitmap image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(maxWidth, maxHeight);
            using (var graphics = Graphics.FromImage(newImage))
            {
                // Calculate x and y which center the image
                int y = (maxHeight / 2) - newHeight / 2;
                int x = (maxWidth / 2) - newWidth / 2;

                // Draw image on x and y with newWidth and newHeight
                graphics.DrawImage(image, x, y, newWidth, newHeight);
            }

            return newImage;
        }
    }
}
