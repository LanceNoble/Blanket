using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
            //Size = new Size(500, 500);
            //Location = new System.Drawing.Point(100, 0);
            Size = Screen.PrimaryScreen.Bounds.Size;


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
