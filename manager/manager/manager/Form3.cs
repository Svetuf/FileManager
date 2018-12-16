using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace manager
{
    public partial class Form3 : Form
    {

        protected WorkWithFiles fi;
        public string iDo;

        public delegate void EventHandler(object sender, EventArgs args, string pa, string watido);
        public event EventHandler ThrowEvent = delegate { };


        public Form3()
        {
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            InitializeComponent();
            fi = new WorkWithFiles();
            ImageList imageListSmall = new ImageList();
            imageListSmall.Images.Add(Bitmap.FromFile("C:\\file.ico"));
            imageListSmall.Images.Add(Bitmap.FromFile("C:\\papka.ico"));
            imageListSmall.Images.Add(Bitmap.FromFile("C:\\Hard-Drive.png"));
            imageListSmall.ImageSize = new Size(32, 32);
            listView1.LargeImageList = imageListSmall;
        }

        public Form3(string ar, string tag, string watido)
        {
            iDo = watido;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            InitializeComponent();
            fi = new WorkWithFiles();
            ImageList imageListSmall = new ImageList();
            imageListSmall.Images.Add(Bitmap.FromFile("C:\\file.ico"));
            imageListSmall.Images.Add(Bitmap.FromFile("C:\\papka.ico"));
            imageListSmall.Images.Add(Bitmap.FromFile("C:\\Hard-Drive.png"));
            imageListSmall.ImageSize = new Size(32, 32);
            listView1.LargeImageList = imageListSmall;
            fi = new WorkWithFiles();
            if (tag == "file")
                fi.setP(Path.GetDirectoryName(ar));
            else
                fi.obnull();
            updateList();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            
        }

        protected void updateList()
        {
            if (fi.getP() == "" || fi.getP().Length <= 2)
            {
                listView1.Items.Clear();
                DriveInfo[] drives = DriveInfo.GetDrives();
                foreach (DriveInfo i in drives)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.ImageIndex = 2;
                    lvi.Text = i.Name;
                    lvi.Tag = "directory";
                    listView1.Items.Add(lvi);
                }
                fi.obnull();
                richTextBox1.Text = "DISKS";
                return;
            }
            try
            {
                listView1.Items.Clear();

                DirectoryInfo dInf = new DirectoryInfo(fi.getP());
                DirectoryInfo[] dMas = dInf.GetDirectories();
                FileInfo[] fMas = dInf.GetFiles();

                foreach (DirectoryInfo i in dMas)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.ImageIndex = 1;
                    lvi.Text = i.Name;
                    lvi.Tag = "directory";
                    listView1.Items.Add(lvi);
                }

                foreach (FileInfo i in fMas)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.ImageIndex = 0;
                    lvi.Text = i.Name;
                    lvi.Tag = "file";
                    listView1.Items.Add(lvi);
                }
                richTextBox1.Text = fi.getP();
            }
            catch (Exception) { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ThrowEvent(this, new EventArgs(), fi.getP(), iDo);
            this.Close();
        }

        protected void listView1_SelectedIndexChanged(object sender, EventArgs e)
        { }
 
        private void listView1_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            string toAdd = listView1.SelectedItems[0].Text;
            fi.addLevel(toAdd);
            updateList();
            
        }

        public void button2_Click(object sender, EventArgs e)
        {
            fi.fuckGoBack();
            updateList();
        }
    }
}
