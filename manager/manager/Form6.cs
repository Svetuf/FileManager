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
    public partial class Form6 : Form
    {
        string us;
        string pas;
        userSettings etiuseri;
        int myPos = -1;

        public Form6(userSettings uset)
        {
            etiuseri = uset;
        }
        public Form6()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if(etiuseri.getPasswords()[myPos] == richTextBox1.Text)
            {
               //удаляем и возвращаем исправленный usersettings
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            us = comboBox1.Text;
            myPos = etiuseri.getNames().IndexOf(us);
        }
    }
}
