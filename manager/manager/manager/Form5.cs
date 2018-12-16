using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace manager
{
    public partial class Form5 : Form
    {
        private string name;
        private string password;
        private string pasConfirm;

        public Form5()
        {
            InitializeComponent();
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (password == pasConfirm)
                this.Close();
            
        }

        public List<string>retUser()
        {
            List<string> a = new List<string>();
            a.Add(name);
            a.Add(password);
            return a;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            name = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            password = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            pasConfirm = textBox3.Text;
        }
    }
}
