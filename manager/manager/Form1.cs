using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using Ionic.Zip;

namespace manager
{
    public partial class Form1 : Form, IView
    {
        protected const int WM_NCHITTEST = 0x84;
        protected const int HTCAPTION = 2;
        protected const int HTCLIENT = 1;
        protected ContextMenu contextMenu;
        protected MenuItem menuItem1;
        protected MenuItem menuItem2;
        protected MenuItem menuItem3;
        protected MenuItem menuItem4;
        protected MenuItem menuItem5;
        protected MenuItem menuItem6;
        protected MenuItem menuItem7;
        protected MenuItem menuItem8;
        protected MenuItem menuItem9;
        protected MenuItem menuItem10;
        protected WorkWithFiles fi;
        FileSystemWatcher w;

        // events from IVIew
        public event EventHandler startDownloadForm;
        public event EventHandler encryptClicked;
        public event EventHandler decryptClicked;
        public event EventHandler asynkTaskSearchClicked;
        public event EventHandler searchFilesByUserInput;
        public event EventHandler TaskSearchClicked;
        public event EventHandler foreachSearch;
        public event EventHandler defaultSearck;
        public event EventHandler openButtonClicked;
        public event EventHandler doubleclickOpen;
        public event EventHandler selectedIndexChanged;
        public event EventHandler menu10;
        public event EventHandler menu9;
        public event EventHandler menu8;
        public event EventHandler menu7;
        public event EventHandler menu6;
        public event EventHandler menu5;
        public event EventHandler menu4;
        public event EventHandler menu3;
        public event EventHandler menu2;
        public event EventHandler menu1;



        //шаблоны архивирования
        asyncTaskZip asyncArch;
        taskZip taskArch;
        foreachZip foreachArch;
        regularZip regArch;

        //стратегия поиска по регуляркам 
        Strategy s;

        //
        Md5Hash visitor;
        crypt CesarCrypt;
        encrypt CesarEncrypt;

        //setters and getters
        public void addItemToLW2(ListViewItem i)
        {
            listView2.Items.Add(i);
        }
        public void clearListView2()
        {
            listView2.Clear();
        }
        public void AdLevelPath(string s)
        {
            fi.addLevel(s);
        }
        public ListView getsetListView { get { return listView1; } set { } }
        public string getsetFi{ get { return fi.getP(); } set { } }
        public string getsetEncryptTExtBox { get { return EncryptTextBox.Text; } set { } }
        public string getsetRichTextBox3 { get { return richTextBox3.Text; } set { } }
        public void getsetRichTextBox1(string s)
        {
            richTextBox1.Text = s;
        }
        public FileSystemWatcher getWatcher { get { return w; } set { } }

        public void renewList()
        {
            updateList();
        }

        ///
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT)
                m.Result = (IntPtr)HTCAPTION;
        }

        protected void menu1_click(object sender, EventArgs e) // copy file/directory
        {
            menu1(sender, e);
        }

        protected void menu2_click(object sender, EventArgs e) // delete
        {
            menu2(sender, e);
        }

        protected void menu3_click(object sender, EventArgs e) // replase
        {
            menu3(sender, e);
        }

        protected void menu4_click(object sender, EventArgs e) // rename
        {
            menu4(sender, e);
        }

        protected void menu5_click(object sender, EventArgs e,
            string somePath = "getMefromSelectedItem", string sometag = "plzTagme", string werewer = "nigde")
        {
            menu5(sender, e);
        }

        protected void menu5_click(object sender, EventArgs e)
        {
            menu5_click(sender, e, "getMefromSelectedItem", "plzTagme", "nigde");
        }

        protected void menu6_click(object sender, EventArgs e)
        {
            menu6(sender, e);
        }

        protected void menu7_click(object sender, EventArgs e)
        {
            menu7(sender, e);
        }
        

        protected void menu8_click(object sender, EventArgs e)
        {
            menu8(sender, e);
        }

        protected void menu9_click(object sender, EventArgs e)
        {
            menu9(sender, e);
        }

        protected void menu10_click(object sender, EventArgs e)
        {
            menu10(sender, e);
        }


        public Form1(string name) : this()
        {
            richTextBox2.Text = name;

        }
        public Form1()
        {
            w = new FileSystemWatcher();
            InitializeComponent();

            Presenter pres = new Presenter(this);


            visitor = new Md5Hash();
            CesarCrypt = new crypt();
            CesarEncrypt = new encrypt();

            asyncArch = new asyncTaskZip();
            taskArch = new taskZip();
            foreachArch = new foreachZip();
            regArch = new regularZip();

            s = new Strategy(new defaultSearch());

            fi = new WorkWithFiles();
            ImageList imageListSmall = new ImageList();
            imageListSmall.Dispose();
            imageListSmall.Images.Add(Bitmap.FromFile("C:\\file.ico"));
            imageListSmall.Images.Add(Bitmap.FromFile("C:\\papka.ico"));
            imageListSmall.Images.Add(Bitmap.FromFile("C:\\intro-external-drive.png"));
            imageListSmall.ImageSize = new Size(32, 32);
            listView1.LargeImageList = imageListSmall;
            var drives = FolderMethods.getDrInfo();
            foreach (var i in drives)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.ImageIndex = 2;
                lvi.Text = i.Name;
                lvi.Tag = "directory";
                listView1.Items.Add(lvi);
            }
            contextMenu = new System.Windows.Forms.ContextMenu();
            menuItem1 = new MenuItem("&Copy", new EventHandler(menu1_click));
            menuItem2 = new MenuItem("&Cut", new EventHandler(menu3_click));
            menuItem3 = new MenuItem("&Delete", new EventHandler(menu2_click));
            menuItem4 = new MenuItem("&Rename", new EventHandler(menu4_click));
            menuItem5 = new MenuItem("&Archive", new EventHandler(menu5_click));
            menuItem6 = new MenuItem("&Archive parralelfreach", new EventHandler(menu6_click));
            menuItem7 = new MenuItem("&Archive parralelTask", new EventHandler(menu7_click));
            menuItem8 = new MenuItem("&Archive Task async", new EventHandler(menu8_click));
            menuItem9 = new MenuItem("&Statictic", new EventHandler(menu9_click));
            menuItem10 = new MenuItem("&MD5 hash", new EventHandler(menu10_click));
            contextMenu.MenuItems.Add(menuItem1);
            contextMenu.MenuItems.Add(menuItem2);
            contextMenu.MenuItems.Add(menuItem3);
            contextMenu.MenuItems.Add(menuItem4);
            contextMenu.MenuItems.Add(menuItem5);
            contextMenu.MenuItems.Add(menuItem6);
            contextMenu.MenuItems.Add(menuItem7);
            contextMenu.MenuItems.Add(menuItem8);
            contextMenu.MenuItems.Add(menuItem9);
            contextMenu.MenuItems.Add(menuItem10);
            richTextBox2.Text = "eat";
        }

        protected void Form1_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
        }

        protected void Form1_Load(object sender, EventArgs e)
        {
            w.SynchronizingObject = this;
            w.Changed += new FileSystemEventHandler((s, e1) => updateList());
            w.Created += new FileSystemEventHandler((s, e1) => updateList());
            w.Deleted += new FileSystemEventHandler((s, e1) => updateList());
            try
            {
                w.EnableRaisingEvents = true;
            }
            catch (Exception we) { }
        }

        protected void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        protected void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedIndexChanged(sender, e);
        }

        protected void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           doubleclickOpen(sender,e);
        }

        protected void button2_Click_1(object sender, EventArgs e)
        {
            fi.fuckGoBack();
            updateList();
        }

        public void updateList()
        {
            if (fi.getP() == "" || fi.getP().Length <= 2)
            {
                listView1.Items.Clear();
                var drives = FolderMethods.getDrInfo();
                foreach (var i in drives)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.ImageIndex = 2;
                    lvi.Text = i.Name;
                    lvi.Tag = "directory";
                    listView1.Items.Add(lvi);
                }
                fi.obnull();
                richTextBox1.Text = "DISKS";
                w.Path = @"\";
                w.Filter = "*.*";
                return;
            }
            try
            {
                listView1.Items.Clear();

                FolderMethods.UpdateDirectories(listView1.Items, fi.getP());
                FileMethods.UpdateFiles(listView1.Items, fi.getP());

                richTextBox1.Text = fi.getP();
                w.Path = fi.getP();
                w.Filter = "*.*";
            }
            catch (Exception) { }
        }

        protected void button4_Click(object sender, EventArgs e)
        {
            openButtonClicked(sender,e);
        }

        protected void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenu.Show(listView1, e.Location);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            updateList();
        }


        private void button3_Click_1(object sender, EventArgs e)
        {
            defaultSearck(sender, e);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreachSearch(sender,e);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            startDownloadForm(sender, e);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TaskSearchClicked(sender, e);
        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            searchFilesByUserInput(sender, e);
        }
   
        private void button8_Click(object sender, EventArgs e)
        {
            asynkTaskSearchClicked(sender, e);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            encryptClicked(sender, e);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            decryptClicked(sender, e);
        }
    }

    public class WorkWithFiles
    {
        string myPath;

        public WorkWithFiles()
        {

            myPath = "";
        }

        public void obnull()
        {
            myPath = "";
        }

        public string getP()
        {
            if (false)
            {
                //показать диски
            }
            return myPath;
        }

        public string setP(string n)
        {
            myPath = n;
            return myPath;
        }

        public void fuckGoBack()
        {
            if (myPath.Length == 0 || myPath[myPath.Length - 1] == 92)
            {
                obnull();
                return;
            }
            while (myPath[myPath.Length - 1] != 92)
            {
                myPath = myPath.Remove(myPath.Length - 1);
            }
            while (myPath.Length > 0 && myPath[myPath.Length - 1] == 92)
                myPath = myPath.Remove(myPath.Length - 1);
            if (myPath.Length == 2)
                myPath += @"\";
        }

        public void goAntiBack()
        {

        }

        public void addLevel(string name)
        {
            if (myPath.Length == 0)
            {
                myPath += name;
                return;
            }
            //myPath += @"\" + name;
            myPath = FileMethods.Combine(myPath, name);
        }
    }
}