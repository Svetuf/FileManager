using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace manager
{
    public partial class Form8 : Form
    {

        private string GLOBALpath;
        public Form8()
        {
            InitializeComponent();
        }

        public Form8(string path)
        {
            InitializeComponent();
            GLOBALpath = path;
        }

        void statistic(string path, int dlina)
        {
            var topWordsLength = dlina;
            string result = "";
            var lines = 0;

            string filePath = path;

            byte[] b = File.ReadAllBytes(filePath);

            using (var reader = File.OpenText(filePath))
            {
                while (reader.ReadLine() != null)
                    lines++;
            }
            result += "Число строк: " + lines + "\n";
   


            string textToAnalyse = Encoding.Default.GetString(b).ToLower().Replace(",", "").Replace(".", "").Replace("(", "").Replace(")", "").Replace("-", "");
            string[] arrayOfWords = textToAnalyse.Split();
            
            result += "Всего слов: " + arrayOfWords.Length + "\n";

            var presortedList = arrayOfWords.GroupBy(s => s).Where(g => g.Count() > 1).OrderByDescending(g => g.Count()).Select(g => g.Key).ToList();
            presortedList.Remove("");
            var sortedList = (from word in presortedList where word.Length > topWordsLength select word);

            var topTenWords = sortedList.Take(10);

            result += "Топ 10 слов длиной > " + topWordsLength + ":\n";
            int i = 1;
            foreach (var word in topTenWords)
            {
                result += i + ") " + word + "\n";
                i++;
            }

            richTextBox1.Text = result;   
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                return;
            int num = int.Parse(textBox1.Text);
            statistic(GLOBALpath, num);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                int num = int.Parse(textBox1.Text);
                statistic(GLOBALpath, num);
            }

        }
    }
}
