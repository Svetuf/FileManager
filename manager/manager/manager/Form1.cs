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

namespace manager
{
    public partial class Form1 : Form
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
        protected WorkWithFiles fi;
        FileSystemWatcher w;


        //шаблоны архивирования
        asyncTaskZip asyncArch;
        taskZip taskArch;
        foreachZip foreachArch;
        regularZip regArch;

        //стратегия поиска по регуляркам 
        Strategy s;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT)
                m.Result = (IntPtr)HTCAPTION;
        }

        protected void OnkoZakrito(string s, string dela)
        {
            if (dela == "copy")
            {
                try
                {
                    if (listView1.SelectedItems[0].Tag.ToString() == "file")
                    {
                        FileMethods.copy(Path.Combine(fi.getP(), listView1.SelectedItems[0].Text), Path.Combine(s, listView1.SelectedItems[0].Text), true);
                       // File.Copy(Path.Combine(fi.getP(), listView1.SelectedItems[0].Text), Path.Combine(s, listView1.SelectedItems[0].Text), true);
                    }
                    else
                    {
                        // directory
                    }
                }
                catch (Exception e)
                {

                }
                return;
            }
            if (dela == "replace")
            {
                if (listView1.SelectedItems[0].Tag.ToString() == "file")
                {
                    FileMethods.move(Path.Combine(fi.getP(), listView1.SelectedItems[0].Text), Path.Combine(s, listView1.SelectedItems[0].Text));
                }
                else
                {
                    FolderMethods.move(Path.Combine(fi.getP(), listView1.SelectedItems[0].Text), s);
                }
                return;
            }

            if (dela == "rename")
            {
                if (listView1.SelectedItems[0].Tag.ToString() == "directory")
                {
                    FolderMethods.move(Path.Combine(fi.getP(), listView1.SelectedItems[0].Text),
                        Path.Combine(fi.getP(), s));
                    updateList();
                    return;
                }
                FileMethods.delete(Path.Combine(fi.getP(), s));
                string h = listView1.SelectedItems[0].Text;
                int ind = h.IndexOf('.');
                h = h.Substring(ind);
                FileMethods.move(Path.Combine(fi.getP(), listView1.SelectedItems[0].Text), Path.Combine(fi.getP(), s + h));
                updateList();
            }

        }

        protected void menu1_click(object sender, EventArgs e) // copy file/directory
        {
            Form3 f = new Form3(Path.Combine(fi.getP(), listView1.SelectedItems[0].Text), listView1.SelectedItems[0].Tag.ToString(), "copy");
            f.ThrowEvent += (senderio, args, st, delo) => { OnkoZakrito(st, delo); };
            f.ShowDialog();
        }

        protected void menu2_click(object sender, EventArgs e) // delete
        {
            if (listView1.SelectedItems[0].Tag.ToString() == "file")
                FileMethods.delete(Path.Combine(fi.getP(), listView1.SelectedItems[0].Text));
            else
                FolderMethods.DeleteDirectory(Path.Combine(fi.getP(), listView1.SelectedItems[0].Text));
            updateList();
        }

        protected void menu3_click(object sender, EventArgs e) // replase
        {
            Form3 f = new Form3(Path.Combine(fi.getP(), listView1.SelectedItems[0].Text), listView1.SelectedItems[0].Tag.ToString(), "replace");
            f.ThrowEvent += (senderio, args, st, delo) => { OnkoZakrito(st, delo); };
            f.ShowDialog();
        }

        protected void menu4_click(object sender, EventArgs e) // rename
        {
            Form4 f = new Form4();
            f.ThrowEvent += (se, args, st, delo) => { OnkoZakrito(st, delo); };
            f.ShowDialog();
        }

        protected void menu5_click(object sender, EventArgs e,
            string somePath = "getMefromSelectedItem", string sometag = "plzTagme", string werewer = "nigde")
        {



            regArch.archive(fi.getP(), listView1.SelectedItems[0].Text, listView1.SelectedItems[0].Tag.ToString());

            /*
            if (somePath == "getMefromSelectedItem")
                somePath = listView1.SelectedItems[0].Text;
            if (sometag == "plzTagme")
                sometag = listView1.SelectedItems[0].Tag.ToString();
            if (werewer == "nigde")
                werewer = fi.getP();


            if (sometag == "directory")
            {
                Directory.CreateDirectory(werewer + "\\" + somePath + ".archivated");

                DirectoryInfo dInf = new DirectoryInfo(Path.Combine(werewer, somePath));
                FileInfo[] files = dInf.GetFiles("*", SearchOption.AllDirectories);

                foreach (FileInfo curFile in files)
                {
                    archiveFile(curFile.FullName, werewer + "\\" + somePath + ".archivated" +
                    "\\" + curFile.Name + ".zip");
                }

            }
            else
            {
                archiveFile(Path.Combine(werewer, somePath),
                    Path.Combine(werewer, somePath) + ".zip");
            }

            */
        }

        protected void menu5_click(object sender, EventArgs e)
        {
            menu5_click(sender, e, "getMefromSelectedItem", "plzTagme", "nigde");
        }

        protected void menu6_click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 0)
            {
                return;
            }


            foreachArch.archive(fi.getP(), listView1.SelectedItems[0].Text, listView1.SelectedItems[0].Tag.ToString());

            /*
            string sometag = listView1.SelectedItems[0].Tag.ToString();
            string somePath = listView1.SelectedItems[0].Text;
            string werewer = fi.getP();

            if (sometag == "directory")
            {
                Directory.CreateDirectory(werewer + "\\" + somePath + ".archivated");

                DirectoryInfo dInf = new DirectoryInfo(Path.Combine(werewer, somePath));
                FileInfo[] files = dInf.GetFiles("*", SearchOption.AllDirectories);

                Parallel.ForEach(files, (currentFile) =>
                {
                    archiveFile(currentFile.FullName, werewer + "\\" + somePath + ".archivated" +
                    "\\" + currentFile.Name + ".zip");
                });

            }
            else
            {
                archiveFile(Path.Combine(werewer, somePath),
                    Path.Combine(werewer, somePath) + ".zip");
            }
            */

        }

        protected void menu7_click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 0)
            {
                return;
            }
            
            taskArch.archive(fi.getP(), listView1.SelectedItems[0].Text, listView1.SelectedItems[0].Tag.ToString());

            /*
            string sometag = listView1.SelectedItems[0].Tag.ToString();
            string somePath = listView1.SelectedItems[0].Text;
            string werewer = fi.getP();

            if (sometag == "directory")
            {
                Directory.CreateDirectory(werewer + "\\" + somePath + ".archivated");

                DirectoryInfo dInf = new DirectoryInfo(Path.Combine(werewer, somePath));
                FileInfo[] files = dInf.GetFiles("*", SearchOption.AllDirectories);

                Task[] tsk = new Task[files.Length];
                for (int i = 0; i < files.Length; i++)
                    tsk[i] = Task.Factory.StartNew((currentFile) =>
                {
                    archiveFile(((FileInfo)currentFile).FullName, werewer + "\\" + somePath + ".archivated" +
                    "\\" + ((FileInfo)currentFile).Name + ".zip");
                }, files[i]);
                Task.WaitAll(tsk);
            }
            else
            {
                archiveFile(Path.Combine(werewer, somePath),
                    Path.Combine(werewer, somePath) + ".zip");
            }
            */
        }

        protected void menu8_click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 0)
            {
                return;
            }
            
            asyncArch.archive(fi.getP(), listView1.SelectedItems[0].Text, listView1.SelectedItems[0].Tag.ToString());

            /*
            string sometag = listView1.SelectedItems[0].Tag.ToString();
            string somePath = listView1.SelectedItems[0].Text;
            string werewer = fi.getP();

            if (sometag == "directory")
            {
                Directory.CreateDirectory(werewer + "\\" + somePath + ".archivated");

                DirectoryInfo dInf = new DirectoryInfo(Path.Combine(werewer, somePath));
                FileInfo[] files = dInf.GetFiles("*", SearchOption.AllDirectories);

                List<Task> tsk = new List<Task>();
                for (int i = 0; i < files.Length; i++)
                    tsk.Add(Task.Factory.StartNew((currentFile) =>
                    {
                        archiveFile(((FileInfo)currentFile).FullName, werewer + "\\" + somePath + ".archivated" +
                        "\\" + ((FileInfo)currentFile).Name + ".zip");
                    }, files[i]));
                await Task.WhenAll(tsk);
            }
            else
            {
                archiveFile(Path.Combine(werewer, somePath),
                    Path.Combine(werewer, somePath) + ".zip");
            }
            */
        }

        protected void menu9_click(object sender, EventArgs e)
        {
            Form8 form = new Form8(Path.Combine(fi.getP(), listView1.SelectedItems[0].Text)) ;
            form.ShowDialog();
        }


        public Form1(string name) : this()
        {
            richTextBox2.Text = name;

        }
        public Form1()
        {
            w = new FileSystemWatcher();
            InitializeComponent();

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
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo i in drives)
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
            contextMenu.MenuItems.Add(menuItem1);
            contextMenu.MenuItems.Add(menuItem2);
            contextMenu.MenuItems.Add(menuItem3);
            contextMenu.MenuItems.Add(menuItem4);
            contextMenu.MenuItems.Add(menuItem5);
            contextMenu.MenuItems.Add(menuItem6);
            contextMenu.MenuItems.Add(menuItem7);
            contextMenu.MenuItems.Add(menuItem8);
            contextMenu.MenuItems.Add(menuItem9);
            richTextBox2.Text = "eat";
        }

        protected void Form1_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
        }

        protected void menuStrip1_DragEnter(object sender, DragEventArgs e)
        {
        }

        void fileEvent(object sender, FileSystemEventArgs e)
        {
            updateList();
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
            listView2.Items.Clear();
            if (listView1.SelectedItems.Count == 0)
                return;
            if (listView1.SelectedItems[0].Tag.ToString() == "file")
            {
                FileMethods f = new FileMethods(fi.getP() + @"\" + listView1.SelectedItems[0].Name);
                ListViewItem lvi = new ListViewItem();
                lvi.Text = "Расширение : " + f.myType();
                listView2.Items.Add(lvi);
            }
        }

        protected void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems[0].Tag.ToString() == "file")
            {
                button4_Click(this, e);
                return;
            }
            string toAdd = listView1.SelectedItems[0].Text;
            fi.addLevel(toAdd);
            updateList();
        }

        protected void button2_Click_1(object sender, EventArgs e)
        {
            fi.fuckGoBack();
            updateList();
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
                w.Path = @"\";
                w.Filter = "*.*";
                return;
            }
            try
            {
                listView1.Items.Clear();

                FolderMethods.UpdateDirectories(listView1.Items, fi.getP());
                FileMethods.UpdateFiles(listView1.Items, fi.getP());

                /*
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
                */
                richTextBox1.Text = fi.getP();
                w.Path = fi.getP();
                w.Filter = "*.*";
            }
            catch (Exception) { }
        }

        protected void button3_Click(object sender, EventArgs e)
        {
            fi.goAntiBack();
            updateList();
        }

        protected void button4_Click(object sender, EventArgs e)
        {
            try
            {
                //File.Open(fi.getP() + @"\" + listView1.SelectedItems[0].Text, FileMode.Open);
                if (listView1.SelectedItems.Count > 0 && listView1.SelectedItems[0].Tag.ToString() == "file")
                {
                    Process.Start(fi.getP() + @"\" + listView1.SelectedItems[0].Text);
                }
            }
            catch (Exception)
            {
                richTextBox1.Text = "errpr";
            }
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
            s.setInterface(new defaultSearch());

            s.getRegFiles(fi.getP());

            /*
            string[] FilesName = Directory.GetFiles(fi.getP(), "*.*", SearchOption.AllDirectories);

            int NumberFiles = FilesName.Length;
            List<string> regular_str = new List<string>();
            List<string> types = new List<string>();
            regular_str.Add(@"(((8|\+7)[\- ]?)(\(?\d{3}\)?[\- ]?)?[\d\- ]{7})");
            regular_str.Add(@"(\d{4}\s\d{6})");
            regular_str.Add(@"(\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,6})");
            regular_str.Add(@"(/^\d{10}|\d{12}$/)");
            regular_str.Add(@"(https?://([a-z1-9]+.)?[a-z1-9\-]+(\.[a-z]+){1,}/?)");
            regular_str.Add(@"([A - Za - zА - Яа - яЁё]{ 3,})");
            regular_str.Add(@"([1-31]{1,2}).([1-12]{1,2}).([1950-2050]{4,4})");


            System.Diagnostics.Stopwatch swr = new Stopwatch();
            swr.Start();



            List<string> Itog = new List<string>();

            foreach (string file in FilesName)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(file))
                    {
                        String line = sr.ReadToEnd();

                        foreach (string s in regular_str)
                        {

                            if (Regex.Match(line, s).Success)
                            {
                                Itog.Add(file + "   " + s + "   " + Regex.Match(line, s));
                            }
                        }
                    }
                }

                catch (Exception e2)
                {
                    MessageBox.Show(e2.Message);
                }

            }



            StreamWriter sw = new StreamWriter(Path.Combine(fi.getP(), "foundreg.txt"), false, System.Text.Encoding.Default);

            foreach (string line in Itog)
            {
                sw.WriteLine(line);
            }
            sw.Close();
            swr.Stop();
            MessageBox.Show((swr.ElapsedMilliseconds / 100.0).ToString());
            */
        }

        private void button5_Click(object sender, EventArgs e)
        {
            s.setInterface(new foreachSearch());
            s.getRegFiles(fi.getP());

            /*
            string[] FilesName = Directory.GetFiles(fi.getP(), "*.*", SearchOption.AllDirectories);

            int NumberFiles = FilesName.Length;
            List<string> regular_str = new List<string>();
            List<string> types = new List<string>();
            regular_str.Add(@"(((8|\+7)[\- ]?)(\(?\d{3}\)?[\- ]?)?[\d\- ]{7})");
            regular_str.Add(@"(\d{4}\s\d{6})");
            regular_str.Add(@"(\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,6})");
            regular_str.Add(@"(/^\d{10}|\d{12}$/)");
            regular_str.Add(@"(https?://([a-z1-9]+.)?[a-z1-9\-]+(\.[a-z]+){1,}/?)");
            regular_str.Add(@"([A - Za - zА - Яа - яЁё]{ 3,})");
            regular_str.Add(@"([1-31]{1,2}).([1-12]{1,2}).([1950-2050]{4,4})");

            System.Diagnostics.Stopwatch swr = new Stopwatch();
            swr.Start();


            List<string> Itog = new List<string>();

            Parallel.ForEach(FilesName, (currentFile) =>
            {
                try
                {
                    using (StreamReader sr = new StreamReader(currentFile))
                    {
                        String line = sr.ReadToEnd();

                        foreach (string s in regular_str)
                        {

                            if (Regex.Match(line, s).Success)
                            {
                                Itog.Add(currentFile + "   " + s + "   " + Regex.Match(line, s));
                            }
                        }
                    }
                }

                catch (Exception e2)
                {
                    MessageBox.Show(e2.Message);
                }

            });



            StreamWriter sw = new StreamWriter(Path.Combine(fi.getP(), "foundreg.txt"), false, System.Text.Encoding.Default);

            foreach (string line in Itog)
            {
                sw.WriteLine(line);
            }
            swr.Stop();
            MessageBox.Show((swr.ElapsedMilliseconds / 100.0).ToString());
            sw.Close();
            */
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // this.Hide();
            Form7 f = new Form7();
            f.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            s.setInterface(new taskSearch());
            s.getRegFiles(fi.getP());

            /*
            string[] FilesName = Directory.GetFiles(fi.getP(), "*.*", SearchOption.AllDirectories);

            int NumberFiles = FilesName.Length;
            List<string> regular_str = new List<string>();
            List<string> types = new List<string>();
            regular_str.Add(@"(((8|\+7)[\- ]?)(\(?\d{3}\)?[\- ]?)?[\d\- ]{7})");
            regular_str.Add(@"(\d{4}\s\d{6})");
            regular_str.Add(@"(\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,6})");
            regular_str.Add(@"(/^\d{10}|\d{12}$/)");
            regular_str.Add(@"(https?://([a-z1-9]+.)?[a-z1-9\-]+(\.[a-z]+){1,}/?)");
            regular_str.Add(@"([A - Za - zА - Яа - яЁё]{ 3,})");
            regular_str.Add(@"([1-31]{1,2}).([1-12]{1,2}).([1950-2050]{4,4})");

            List<string> Itog = new List<string>();

            Task[] u = new Task[FilesName.Length];

            for (int j = 0; j < FilesName.Length; j++)
            {
                u[j] = Task.Factory.StartNew((i) =>
                {
                    try
                    {
                        using (StreamReader sr = new StreamReader((string)i))
                        {
                            String line = sr.ReadToEnd();

                            foreach (string s in regular_str)
                            {

                                if (Regex.Match(line, s).Success)
                                {
                                    Itog.Add((string)i + "   " + s + "   " + Regex.Match(line, s));
                                }
                            }
                        }
                    }

                    catch (Exception e2)
                    {
                        MessageBox.Show(e2.Message);
                    }
                }, FilesName[j]);
            }

            Task.WaitAll(u);

            StreamWriter sw = new StreamWriter(Path.Combine(fi.getP(), "foundreg.txt"), false, System.Text.Encoding.Default);



            foreach (string line in Itog)
            {
                sw.WriteLine(line);
            }

            sw.Close();
            */
        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            string pred = richTextBox3.Text;

            string reg = "";
            for (int i = 0; i < pred.Length; i++)
            {
                if (pred[i] == '*')
                {
                    reg += "(.*)*";
                    continue;
                }
                if (pred[i] == '?')
                {
                    reg += "?";
                    continue;
                }
                reg += pred[i];

            }

            //  DirectoryInfo dInf = new DirectoryInfo(fi.getP());
            //  DirectoryInfo[] dMas = dInf.GetDirectories();
            // FileInfo[] fMas = dInf.GetFiles();

            List<DirectoryInfo> allDir = /*FolderMethods.getrecursiveDirs(fi.getP()).ToList();*/ fin(fi.getP());
            List<FileInfo> allFiles = /*FolderMethods.myfiles(fi.getP()).ToList();*/ finfA(fi.getP());

            List<DirectoryInfo> iFound = new List<DirectoryInfo>();
            List<FileInfo> iFoundFile = new List<FileInfo>();

            iFound = allDir.FindAll((item) => { return Regex.Match(item.Name, reg).Success; });
            iFoundFile = allFiles.FindAll((item) => { return Regex.Match(item.Name, reg).Success; });


            listView1.Items.Clear();


            foreach (DirectoryInfo i in iFound)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.ImageIndex = 1;
                lvi.Text = i.Name;
                lvi.Tag = "directory";
                listView1.Items.Add(lvi);
            }

            foreach (FileInfo i in iFoundFile)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.ImageIndex = 0;
                lvi.Text = i.Name;
                lvi.Tag = "file";
                listView1.Items.Add(lvi);
            }
            richTextBox1.Text = fi.getP();
            w.Path = fi.getP();
            w.Filter = "*.*";



        }

        List<DirectoryInfo>fin(string pat)
        {
            DirectoryInfo dInf = new DirectoryInfo(pat);
            DirectoryInfo[] dMas = dInf.GetDirectories();
            List<DirectoryInfo> ans = new List<DirectoryInfo>();
            ans.AddRange(dMas);
            Parallel.ForEach(dMas, (d) =>
            {
                ans.AddRange(fin(d.FullName));
            });
            return ans;
         }
       
        List<FileInfo> finfA(string pat)
        {
            DirectoryInfo dInf = new DirectoryInfo(pat);
            DirectoryInfo[] dMasd = dInf.GetDirectories();
            FileInfo[] dMas = dInf.GetFiles();
            List<FileInfo> ans = new List<FileInfo>();
            ans.AddRange(dMas);
            Parallel.ForEach(dMasd, (d)=>
            {
                ans.AddRange(finfA(d.FullName));
            });
            return ans;
        }
        
        public async Task<List<string>> dosmth(string a)
        {
            List<Task<string>> output = new List<Task<string>>();

            string[] FilesName = FolderMethods.myfiles(a);

            int NumberFiles = FilesName.Length;
            List<string> regular_str = new List<string>();
            List<string> types = new List<string>();
            regular_str.Add(@"(((8|\+7)[\- ]?)(\(?\d{3}\)?[\- ]?)?[\d\- ]{7})");
            regular_str.Add(@"(\d{4}\s\d{6})");
            regular_str.Add(@"(\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,6})");
            regular_str.Add(@"(/^\d{10}|\d{12}$/)");
            regular_str.Add(@"(https?://([a-z1-9]+.)?[a-z1-9\-]+(\.[a-z]+){1,}/?)");
            regular_str.Add(@"([A - Za - zА - Яа - яЁё]{ 3,})");
            regular_str.Add(@"([1-31]{1,2}).([1-12]{1,2}).([1950-2050]{4,4})");

            List<string> Itog = new List<string>();

            foreach(string st in FilesName)
            {
                output.Add(dodo(st,regular_str));
            }
            var res = await Task.WhenAll(output);
            return new List<string>(res);
        }
        
        string asd(string a, List<string>b)
        {
            try
            {
                using (StreamReader sr = new StreamReader(a))
                {
                    String line = sr.ReadToEnd();

                    foreach (string s in b)
                    {

                        if (Regex.Match(line, s).Success)
                        {
                            return(a + "   " + s + "   " + Regex.Match(line, s));
                        }
                    }
                }
            }

            catch (Exception e2)
            {
                MessageBox.Show(e2.Message);
            }
            return "";
        }
        async Task<string>dodo(string path, List<string> regular_str)
        {
           return await Task.Factory.StartNew(() => asd(path, regular_str));
        }
        private void button8_Click(object sender, EventArgs e)
        {
            s.setInterface(new asynctaskSearch());
            s.getRegFiles(fi.getP());

            /*
            List<string> x = new List<string>();

            Thread myThread = new Thread(new ThreadStart(async () => {
               //  x = new List<string>();
                x = await dosmth(fi.getP());

                StreamWriter sw = new StreamWriter(Path.Combine(fi.getP(), "foundreg.txt"), false, System.Text.Encoding.Default);

                foreach (string line in x)
                {
                    sw.WriteLine(line);
                }

                sw.Close();

            }));
            myThread.Start();
            */
            
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
            myPath = Path.Combine(myPath, name);
        }
    }
}