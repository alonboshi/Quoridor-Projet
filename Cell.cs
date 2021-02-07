using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuoridorProject
{
    class Cell
    {
        private Image newImage;
        public Cell()
        {
            string dir = Directory.GetCurrentDirectory();
            string filename = Path.Combine(dir, @"NewFolder1\grey_square.jpg");
            this.newImage = Image.FromFile(filename);
        }
        public Image GetImage()
        {
            return this.newImage;
        }
        public void DeleteImage()
        {
            string dir = Directory.GetCurrentDirectory();
            string filename = Path.Combine(dir, @"NewFolder1\grey_square.jpg");
            this.newImage = Image.FromFile(filename);
        }
        public void Change(int num) {
            switch (num)
            {
                case 0:
                    Black();
                    break;
                case 1:
                    Red();
                    break;
            }
        }
        public void Change_pm(int num)
        {
            switch (num)
            {
                case 0:
                    Black_pm();
                    break;
                case 1:
                    Red_pm();
                    break;
            }
        }

        public void Red()
        {
            string dir = Directory.GetCurrentDirectory();
            string filename = Path.Combine(dir, @"NewFolder1\red_square.jpg");
            this.newImage = Image.FromFile(filename);
        }
        public void Red_pm()
        {
            string dir = Directory.GetCurrentDirectory();
            string filename = Path.Combine(dir, @"NewFolder1\red_square_pm.png");
            this.newImage = Image.FromFile(filename);
        }
        public void Black()
        {
            string dir = Directory.GetCurrentDirectory();
            string filename = Path.Combine(dir, @"NewFolder1\black_square.jpeg");
            this.newImage = Image.FromFile(filename);
        }
        public void Black_pm()
        {
            string dir = Directory.GetCurrentDirectory();
            string filename = Path.Combine(dir, @"NewFolder1\black_square_pm.jpg");
            this.newImage = Image.FromFile(filename);
        }
        public void Blue()
        {

        }
        public void White()
        {

        }
        public void PaintCell(Graphics g,int x,int y)
        {
            Point p = new Point(x,y);
            g.DrawImage(this.newImage, p);
        }
    }
}
