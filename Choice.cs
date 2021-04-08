using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuoridorProject
{
    public partial class Choice : Form
    {
        public Choice()
        {
            InitializeComponent();
        }

        private void Choice_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) // 1 VS 1
        {
            Open open = new Open(2);
            open.Show();
            this.Hide();
            
        }

        private void button2_Click(object sender, EventArgs e) // 1 VS AI
        {
            Open open = new Open(1);
            open.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e) // AI VS AI
        {
            Open open = new Open(3);
            open.Show();
            this.Hide();
        }
    }
}
