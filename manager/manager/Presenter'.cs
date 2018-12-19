using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Diagnostics;

namespace manager
{
    class Presenter
    {
        private IView view;

        Md5Hash visitor;
        crypt CesarCrypt;
        encrypt CesarEncrypt;

        asyncTaskZip asyncArch;
        taskZip taskArch;
        foreachZip foreachArch;
        regularZip regArch;

        Strategy s;
        public Presenter(IView view)
        {
            this.view = view;
            view.startDownloadForm += new EventHandler(downloadForm);
            view.decryptClicked += new EventHandler(decryptClickedRealize);
            view.encryptClicked += new EventHandler(encryptClickedRealize);
            view.asynkTaskSearchClicked += new EventHandler(asyncTaskSearck);
            view.searchFilesByUserInput += new EventHandler(SearchFilesByInput);
            view.TaskSearchClicked += new EventHandler(tasksearch);
            view.foreachSearch += new EventHandler(parallelsearch);
            view.defaultSearck += new EventHandler(stansartSearch);
            view.openButtonClicked += new EventHandler(startFileButton);
            view.doubleclickOpen += new EventHandler(openFile);
            view.selectedIndexChanged += new EventHandler(changedVsyoTaki);
            view.menu10 += new EventHandler(MENU10);
            view.menu9 += new EventHandler(MENU9);
            view.menu8 += new EventHandler(MENU8);
            view.menu7 += new EventHandler(MENU7);
            view.menu6 += new EventHandler(MENU6);
            view.menu5 += new EventHandler(MENU5);
            view.menu4 += new EventHandler(MENU4);
            view.menu3 += new EventHandler(MENU3);
            view.menu2 += new EventHandler(MENU2);
            view.menu1 += new EventHandler(MENU1);

            CesarCrypt = new crypt();
            CesarEncrypt = new encrypt();
            visitor = new Md5Hash();
            asyncArch = new asyncTaskZip();
            taskArch = new taskZip();
            foreachArch = new foreachZip();
            regArch = new regularZip();
            s = new Strategy(new asynctaskSearch());
        }

        private void downloadForm(object sender, EventArgs e)
        {
            Form7 f = new Form7();
            f.ShowDialog();
        }
        private void encryptClickedRealize(object sender, EventArgs e)
        {
            if (view.getsetListView.SelectedItems.Count > 0)
            {
                if (view.getsetListView.SelectedItems[0].Tag.ToString() == "file")
                {
                    FileMethods m = new FileMethods(FileMethods.Combine(view.getsetFi, view.getsetListView.SelectedItems[0].Text));
                    CesarCrypt.setNum(int.Parse(view.getsetEncryptTExtBox));
                    m.Accept(CesarCrypt);
                }
                else
                {
                    FolderMethods m = new FolderMethods(FolderMethods.Combine(view.getsetFi, view.getsetListView.SelectedItems[0].Text));
                    CesarCrypt.setNum(int.Parse(view.getsetEncryptTExtBox));
                    m.Accept(CesarCrypt);
                }
            }
        }
        private void decryptClickedRealize(object sender, EventArgs e)
        {
            if (view.getsetListView.SelectedItems.Count > 0)
            {
                if (view.getsetListView.SelectedItems[0].Tag.ToString() == "file")
                {
                    FileMethods m = new FileMethods(FileMethods.Combine(view.getsetFi, view.getsetListView.SelectedItems[0].Text));
                    CesarEncrypt.setKey(int.Parse(view.getsetEncryptTExtBox));
                    m.Accept(CesarEncrypt);
                }
                else
                {
                    FolderMethods m = new FolderMethods(FolderMethods.Combine(view.getsetFi, view.getsetListView.SelectedItems[0].Text));
                    CesarEncrypt.setKey(int.Parse(view.getsetEncryptTExtBox));
                    m.Accept(CesarEncrypt);
                }
            }
        }
        private void asyncTaskSearck(object sender, EventArgs e)
        {
            s.setInterface(new asynctaskSearch());
            s.getRegFiles(view.getsetFi);
        }
        private void SearchFilesByInput(object sender, EventArgs e)
        {
            string pred = view.getsetRichTextBox3;

            if (pred == "")
            {
                view.renewList();
                return;
            }

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
            List<DirectoryInfo> allDir = fin(view.getsetFi);
            List<FileInfo> allFiles = finfA(view.getsetFi);

            List<DirectoryInfo> iFound = new List<DirectoryInfo>();
            List<FileInfo> iFoundFile = new List<FileInfo>();

            iFound = allDir.FindAll((item) => { return Regex.Match(item.Name, reg).Success; });
            iFoundFile = allFiles.FindAll((item) => { return Regex.Match(item.Name, reg).Success; });


            view.getsetListView.Items.Clear();


            foreach (DirectoryInfo i in iFound)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.ImageIndex = 1;
                lvi.Text = i.Name;
                lvi.Tag = "directory";
                view.getsetListView.Items.Add(lvi);
            }

            foreach (FileInfo i in iFoundFile)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.ImageIndex = 0;
                lvi.Text = i.Name;
                lvi.Tag = "file";
                view.getsetListView.Items.Add(lvi);
            }
            view.getsetRichTextBox1(view.getsetFi);
            view.getWatcher.Path = view.getsetFi;
            view.getWatcher.Filter = "*.*";
        }
        List<DirectoryInfo> fin(string pat)
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
            Parallel.ForEach(dMasd, (d) =>
            {
                ans.AddRange(finfA(d.FullName));
            });
            return ans;
        }
        private void tasksearch(object senser, EventArgs e)
        {
            s.setInterface(new taskSearch());
            s.getRegFiles(view.getsetFi);
        }
        private void parallelsearch(object senser, EventArgs e)
        {
            s.setInterface(new foreachSearch());
            s.getRegFiles(view.getsetFi);
        }
        private void stansartSearch(object senser, EventArgs e)
        {
            s.setInterface(new defaultSearch());
            s.getRegFiles(view.getsetFi);
        }
        private void startFileButton(object sender, EventArgs e)
        {
            try
            {
                if (view.getsetListView.SelectedItems.Count > 0 && view.getsetListView.SelectedItems[0].Tag.ToString() == "file")
                {
                    Process.Start(view.getsetFi + @"\" + view.getsetListView.SelectedItems[0].Text);
                }
            }
            catch (Exception)
            {
                view.getsetRichTextBox1("errpr");
            }
        }
        private void openFile(object sender, EventArgs e)
        {
            if (view.getsetListView.SelectedItems[0].Tag.ToString() == "file")
            {
                startFileButton(this, e);
                return;
            }
            string toAdd = view.getsetListView.SelectedItems[0].Text;
            view.AdLevelPath(toAdd);
            view.renewList();
        }
        private void changedVsyoTaki(object sender, EventArgs e)
        {
            view.clearListView2();
            if (view.getsetListView.SelectedItems.Count == 0)
                return;
            if (view.getsetListView.SelectedItems[0].Tag.ToString() == "file")
            {
                FileMethods f = new FileMethods(view.getsetFi + @"\" + view.getsetListView.SelectedItems[0].Name);
                ListViewItem lvi = new ListViewItem();
                lvi.Text = "Расширение : " + f.myType();
                view.addItemToLW2(lvi);
            }
        }
        private void MENU10(object sender, EventArgs e)
        {
            if (view.getsetListView.SelectedItems[0].Tag.ToString() == "file")
            {
                FileMethods m = new FileMethods(FileMethods.Combine(view.getsetFi, view.getsetListView.SelectedItems[0].Text));
                m.Accept(visitor);
            }
            else
            {
                FolderMethods m = new FolderMethods(FolderMethods.Combine(view.getsetFi, view.getsetListView.SelectedItems[0].Text));
                m.Accept(visitor);
            }
        }
        private void MENU9(object sender, EventArgs e)
        {
            Form8 form = new Form8(FileMethods.Combine(view.getsetFi, view.getsetListView.SelectedItems[0].Text));
            form.ShowDialog();
        }
        private void MENU8(object sender, EventArgs e)
        {
            if (view.getsetListView.SelectedItems.Count < 0)
            {
                return;
            }

            asyncArch.archive(view.getsetFi, view.getsetListView.SelectedItems[0].Text, view.getsetListView.SelectedItems[0].Tag.ToString());
        }
        private void MENU7(object sender, EventArgs e)
        {
            if (view.getsetListView.SelectedItems.Count < 0)
            {
                return;
            }

            taskArch.archive(view.getsetFi, view.getsetListView.SelectedItems[0].Text, view.getsetListView.SelectedItems[0].Tag.ToString());
        }
        private void MENU6(object sender, EventArgs e)
        {
            if (view.getsetListView.SelectedItems.Count < 0)
            {
                return;
            }
            foreachArch.archive(view.getsetFi, view.getsetListView.SelectedItems[0].Text, view.getsetListView.SelectedItems[0].Tag.ToString());
        }
        private void MENU5(object sendr, EventArgs e)
        {
            regArch.archive(view.getsetFi, view.getsetListView.SelectedItems[0].Text, view.getsetListView.SelectedItems[0].Tag.ToString());
        }

        protected void OnkoZakrito(string s, string dela)
        {
            if (dela == "copy")
            {
                try
                {
                    if (view.getsetListView.SelectedItems[0].Tag.ToString() == "file")
                    {
                        FileMethods.copy(FileMethods.Combine(view.getsetFi, view.getsetListView.SelectedItems[0].Text), FileMethods.Combine(s, view.getsetListView.SelectedItems[0].Text), true);
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
                if (view.getsetListView.SelectedItems[0].Tag.ToString() == "file")
                {
                    FileMethods.move(FileMethods.Combine(view.getsetFi, view.getsetListView.SelectedItems[0].Text), FileMethods.Combine(s, view.getsetListView.SelectedItems[0].Text));
                }
                else
                {
                    FolderMethods.move(FileMethods.Combine(view.getsetFi, view.getsetListView.SelectedItems[0].Text), s);
                }
                return;
            }

            if (dela == "rename")
            {
                if (view.getsetListView.SelectedItems[0].Tag.ToString() == "directory")
                {
                    FolderMethods.move(FileMethods.Combine(view.getsetFi, view.getsetListView.SelectedItems[0].Text),
                        FileMethods.Combine(view.getsetFi, s));
                    view.renewList();
                    return;
                }
                FileMethods.delete(FileMethods.Combine(view.getsetFi, s));
                string h = view.getsetListView.SelectedItems[0].Text;
                int ind = h.IndexOf('.');
                h = h.Substring(ind);
                FileMethods.move(FileMethods.Combine(view.getsetFi, view.getsetListView.SelectedItems[0].Text), FileMethods.Combine(view.getsetFi, s + h));
                view.renewList();
            }

        }

        private void MENU4(object sender, EventArgs e)
        {
            Form4 f = new Form4();
            f.ThrowEvent += (se, args, st, delo) => { OnkoZakrito(st, delo); };
            f.ShowDialog();
        }
        private void MENU3(object sender, EventArgs e)
        {
            Form3 f = new Form3(FileMethods.Combine(view.getsetFi, view.getsetListView.SelectedItems[0].Text), view.getsetListView.SelectedItems[0].Tag.ToString(), "replace");
            f.ThrowEvent += (senderio, args, st, delo) => { OnkoZakrito(st, delo); };
            f.ShowDialog();
        }
        private void MENU2(object sender, EventArgs e)
        {
            if (view.getsetListView.SelectedItems[0].Tag.ToString() == "file")
                FileMethods.delete(FileMethods.Combine(view.getsetFi, view.getsetListView.SelectedItems[0].Text));
            else
                FolderMethods.DeleteDirectory(FileMethods.Combine(view.getsetFi, view.getsetListView.SelectedItems[0].Text));
            view.renewList();
        }
        private void MENU1(object sender, EventArgs e)
        {
            Form3 f = new Form3(FileMethods.Combine(view.getsetFi, view.getsetListView.SelectedItems[0].Text), view.getsetListView.SelectedItems[0].Tag.ToString(), "copy");
            f.ThrowEvent += (senderio, args, st, delo) => { OnkoZakrito(st, delo); };
            f.ShowDialog();
        }
    }

    interface IView
    {
        event EventHandler startDownloadForm;
        event EventHandler encryptClicked;
        event EventHandler decryptClicked;
        event EventHandler asynkTaskSearchClicked;
        event EventHandler searchFilesByUserInput;
        event EventHandler TaskSearchClicked;
        event EventHandler foreachSearch;
        event EventHandler defaultSearck;
        event EventHandler openButtonClicked;
        event EventHandler doubleclickOpen;
        event EventHandler selectedIndexChanged;
        event EventHandler menu10;
        event EventHandler menu9;
        event EventHandler menu8;
        event EventHandler menu7;
        event EventHandler menu6;
        event EventHandler menu5;
        event EventHandler menu4;
        event EventHandler menu3;
        event EventHandler menu2;
        event EventHandler menu1;


       //getters setters
       void renewList();
       void AdLevelPath(string s);
       ListView getsetListView { get; set; }
       void clearListView2();
       void addItemToLW2(ListViewItem i);
       string getsetFi { get; set; }
       string getsetEncryptTExtBox { get; set; }
       string getsetRichTextBox3 { get; set; }
       void getsetRichTextBox1(string s);
       FileSystemWatcher getWatcher { get; set; }
    }

}
