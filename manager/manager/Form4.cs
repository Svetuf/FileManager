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
    public partial class Form4 : Form
    {

        public delegate void EventHandler(object sender, EventArgs args, string pa, string watido);
        public event EventHandler ThrowEvent = delegate { };

        public Form4()
        {
            InitializeComponent();
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                ThrowEvent(this, new EventArgs(), richTextBox1.Text, "rename");
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ThrowEvent(this, new EventArgs(), richTextBox1.Text, "rename");
            this.Close();
        }
    }
}
