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
        public Cell(int i,int j,int num)
        {
            DeleteImage(i, j, num);
        }
        public Image GetImage()
        {
            return this.newImage;
        }
        public void DeleteImage(int x,int y,int num)
        {
            switch (num)
            {

                case 2:
                    if (x == 0 || x == 8)
                    {
                        string dir = Directory.GetCurrentDirectory();
                        string filename = Path.Combine(dir, @"NewFolder1\border.jpg");
                        this.newImage = Image.FromFile(filename);
                    }
                    else
                    {
                        string dir = Directory.GetCurrentDirectory();
                        string filename = Path.Combine(dir, @"NewFolder1\grey_square.jpg");
                        this.newImage = Image.FromFile(filename);
                    }
                    break;
                case 3:
                    if (x == 0 || x == 8 || y == 0)
                    {
                        string dir = Directory.GetCurrentDirectory();
                        string filename = Path.Combine(dir, @"NewFolder1\border.jpg");
                        this.newImage = Image.FromFile(filename);
                    }
                    else
                    {
                        string dir = Directory.GetCurrentDirectory();
                        string filename = Path.Combine(dir, @"NewFolder1\grey_square.jpg");
                        this.newImage = Image.FromFile(filename);
                    }
                    break;
                case 4:
                    if (x == 0 || x == 8 || y == 8 )
                    {
                        string dir = Directory.GetCurrentDirectory();
                        string filename = Path.Combine(dir, @"NewFolder1\border.jpg");
                        this.newImage = Image.FromFile(filename);
                    }
                    else
                    {
                        string dir = Directory.GetCurrentDirectory();
                        string filename = Path.Combine(dir, @"NewFolder1\grey_square.jpg");
                        this.newImage = Image.FromFile(filename);
                    }
                    break;
            }
            
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
            string filename = Path.Combine(dir, @"NewFolder1\red_square_pm.jpg");
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
        public void PaintCell(Graphics g, int x, int y)
        {
            Point p = new Point(x, y);
            g.DrawImage(this.newImage, p);
        }
        public void PaintHorizontal(Graphics g, int x, int y)
        {
            Point p = new Point(x, y);
            string dir = Directory.GetCurrentDirectory();
            string filename = Path.Combine(dir, @"NewFolder1\wall_horizontal.jpg");
            g.DrawImage(Image.FromFile(filename), p);
        }
        public void PaintVertical(Graphics g, int x, int y)
        {
            Point p = new Point(x, y);
            string dir = Directory.GetCurrentDirectory();
            string filename = Path.Combine(dir, @"NewFolder1\wall_vertical.jpg");
            g.DrawImage(Image.FromFile(filename), p);
        }
    }
}
