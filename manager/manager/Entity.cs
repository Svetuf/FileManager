using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace manager
{
    abstract class Entity
    {

        protected string path;

        public Entity(string path)
        {
            this.path = path;
        }
        public static string getExstention(string a)
        {
            return Path.GetExtension(a);
        }
        public abstract void createFolder();
        public abstract void createFile();
        public static GZipStream GetGZipStream(FileStream targetStream)
        {
            GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress);
            return compressionStream;
        }

    }

    class FileMethods : Entity
    {

        public override void createFolder()
        {
            FolderMethods.CreateDirectory(path + "\\newfolder");
        }
        public override void createFile()
        {
             File.Create(path + "\\new_file.txt");
        }

        public string getMytTxt()
        {
            return File.ReadAllText(path);
        }
        public FileMethods(string path) : base(path)
        {

        }
        
        public static StreamReader getSR(string p)
        {
            return new StreamReader(p);
        }
        public static string Combine(string a, string b)
        {
            return Path.Combine(a, b);
        }
        public static string Name(string path)
        {
            return Path.GetFileName(path);
        }

        public string myFullPath()
        {
            return Path.GetFullPath(path);
        }

        public string MyName()
        {
            return Path.GetFileName(path);
        }

        public static FileInfo[] GetFileInfos(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] files = di.GetFiles();
            return files;
        }

        public static FileStream GetFileStream(string pathfile)
        {
            FileStream fileStream = new FileStream(pathfile, FileMode.OpenOrCreate);
            return fileStream;
        }
        
        public static FileStream GetTargetStream(string compressfile)
        {
            FileStream targetStream = File.Create(compressfile);
            return targetStream;
        }
        
        public static void UpdateFiles(ListView.ListViewItemCollection list, string pat)
        {
            DirectoryInfo di = new DirectoryInfo(pat);
            FileInfo[] files = di.GetFiles();

            foreach (FileInfo info in files)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.ImageIndex = 0;
                lvi.Text = info.Name;
                lvi.Tag = "file";
                list.Add(lvi);
            }
        }

        public static bool IfExist(string path)
        {
            return File.Exists(path);
        }

        public static byte[] ReadBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        public static void copy(string from, string to, bool overwr)
        {
            File.Copy(from, to, overwr);
        }

        public static void move(string from, string to)
        {
            File.Move(from, to);
        }

        public static void delete(string pa)
        {
            File.Delete(pa);
        }

        public static void archiveFile(string fullNamefrom, string fullNameto)
        {
            using (FileStream myFile = new FileStream(fullNamefrom, FileMode.OpenOrCreate))
            {
                using (FileStream kd = File.Create(fullNameto + ".zip"))
                {
                    using (GZipStream zipka = new GZipStream(kd, CompressionMode.Compress))
                    {
                        myFile.CopyTo(zipka);
                    }
                }
            }
        }

        public string myType(string pat)
        {
            return Path.GetExtension(pat);
        }

        public string myType()
        {
            return Path.GetExtension(path);
        }

        public static string fullPath(string h)
        {
            return Path.GetFullPath(h);
        }

        public void Accept(IVisitor i)
        {
            i.Visit(this);
        }

        public static FileStream create(string pas)
        {
           return File.Create(pas);
        }

    }
    class FolderMethods : Entity
    {
        public override void createFolder()
        {
            Directory.CreateDirectory(path + "\\newdir");
        }
        public override void createFile()
        {
            FileMethods.create(path + "\\newFile.txt");
        }
        ArchiveClass archivator = new regularZip();
        public FolderMethods(string path) : base(path)
        {
            archivator = new regularZip();
        }

        public static DriveInfo[] getDrInfo()
        {
            return DriveInfo.GetDrives();
        }
        public static string GetName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        public string GetNameWithoutPath()
        {
            return Path.GetDirectoryName(path);
        }

        public string GetMyName()
        {
            return Path.GetFullPath(path);
            
        }

        public static string Combine(string a, string b)
        {
            return Path.Combine(a, b);
        }

        public static void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public static void DeleteDirectory(string path)
        {
            Directory.Delete(path, true);
        }

        public void CreateZipFrom(string pathfile, string compressfile)
        {
            archivator.archive(pathfile,path, "directory");
        }


        public static DirectoryInfo[] GetDirectoryInfos(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            DirectoryInfo[] directories = di.GetDirectories();
            return directories;
        }

        public static FolderMethods[] getrecursiveDirs(string p)
        {
            DirectoryInfo di = new DirectoryInfo(p);
            string[] directories = Directory.GetDirectories(p, "*.*", SearchOption.AllDirectories);
            List<FolderMethods> ans = new List<FolderMethods>();
            foreach( string i in directories)
            {
                ans.Add(new FolderMethods(i));
            }
            return ans.ToArray();
        }

        public static DirectoryInfo GetDirectoryInfo(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            return di;
        }

        public static void UpdateDirectories(ListView.ListViewItemCollection list, string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            DirectoryInfo[] directories = di.GetDirectories();

            foreach (DirectoryInfo info in directories)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.ImageIndex = 1;
                lvi.Text = info.Name;
                lvi.Tag = "directory";
                list.Add(lvi);
            }
        }

        public static bool IfExist(string path)
        {
            return Directory.Exists(path);
        }

        public static string[] myfiles(string pas)
        {
            return Directory.GetFiles(pas, "*.*",SearchOption.AllDirectories);
        }

        public static FileMethods[] myfilesEntity(string pas)
        {
            string [] somenew = Directory.GetFiles(pas, "*.*", SearchOption.AllDirectories);
            List<FileMethods> LoL = new List<FileMethods>();

            foreach(string a in somenew)
            {
                LoL.Add(new FileMethods(a));
            }
            return LoL.ToArray();
        }

        public static void move(string from, string to)
        {
            Directory.Move(from, to);
        } 

        public void Accept(IVisitor i)
        {
            i.Visit(this);
        }

        public string[] Get_Files_In_Selected_Folder()
        {
            string[] files = Directory.GetFiles(path);
            return files;
        }

        public static string[] GetDirectories(string path)
        {
            return Directory.GetDirectories(path);
        }
        public static void CopyDir(string FromDir, string ToDir)
        {
            FolderMethods.CreateDirectory(ToDir);

            foreach (string s1 in new FolderMethods(FromDir).Get_Files_In_Selected_Folder())
            {
                string s2 = ToDir + "\\" + new FolderMethods(s1.ToString()).GetNameWithoutPath();
                FileMethods.copy(s1,s2, true);
            }
            foreach (string s in FolderMethods.GetDirectories(FromDir))
            {
                CopyDir(s, ToDir + "\\" + new FolderMethods(s.ToString()).GetNameWithoutPath());
            }
        }
    }
    class ZippedFile : Entity
    {
        public override void createFolder()
        {
            new ZippedFolder(path).CreateFolder("new folder");
        }
        public override void createFile()
        {
            new ZippedFolder(path).CreateFile("newfile.txt");
        }
        public ZippedFile(string path) : base(path)
        {

        }
        public ZipFile CreateZip()
        {
            ZipFile zf = new ZipFile(path);
            return zf;
        }
        public void Close()
        {
            ZipFile zf = new ZipFile(path);
            zf.Save();
        }
        public string GetName()
        {
            string name = path.Remove(path.Length - 12, 12);
            return name;
        }
        public bool Existing()
        {
            if (ZipFile.IsZipFile(path) == true)
            { return true; }
            else { return false; }

        }
        public string GetFullName(string name)
        {
            return name;
        }
        public void Delete(string name)
        {
            //int ZipPlace = path.IndexOf(".zip\\");

            //string path1 = path.Substring(0, ZipPlace + 4);
            using (ZipFile zip = ZipFile.Read(path))
            {
                List<string> fullist = zip.EntryFileNames.ToList();
                if (name[name.Length - 1] == '/')
                    name = name.Remove(name.Length - 1, 1) + "\\";
                zip.RemoveEntry(name.Replace('\\', '/'));

                zip.Save();
            }
        }
        public void OpenFile(string name)
        {
            int ZipPlace = path.IndexOf(".zip");
            string path1 = path.Substring(0, ZipPlace + 4);
            string ArchiveWay = path.Substring(ZipPlace + 4);
            using (ZipFile zip = ZipFile.Read(path1))
            {
                while (path1[path1.Length - 1] != '\\')
                {
                    path1 = path1.Remove(path1.Length - 1, 1);
                }
                foreach (ZipEntry e in zip)
                {
                    if (e.FileName == ArchiveWay.Replace('\\', '/') + name)
                        e.Extract(path1, ExtractExistingFileAction.DoNotOverwrite);
                }

            }
        }
        public static void OpenZipFile(string DirectoryPath, string FilePath)
        {
            new ZippedFile(DirectoryPath).OpenFile(FilePath);
            int ZipPlace = DirectoryPath.IndexOf(".zip\\");
            string name = FilePath;
            string path1 = DirectoryPath.Substring(0, ZipPlace + 4);
            string ArchiveWay = DirectoryPath.Substring(ZipPlace + 5);
            while (path1[path1.Length - 1] != '\\')
            {
                path1 = path1.Remove(path1.Length - 1, 1);
            }

            while (DirectoryPath[DirectoryPath.Length - 1] != '\\')
            {
                DirectoryPath = DirectoryPath.Remove(DirectoryPath.Length - 1, 1);
            }
            Process.Start(path1 + name);
        }
        public void DeleteZipFile()
        {
            ZippedFile file = new ZippedFile(path);

        }
        public int method()
        {
            int ZipPlace = path.IndexOf(".zip\\");
            return ZipPlace;
        }
        public ZipFile CreateZipFile()
        {
            ZipFile zf = new ZipFile(path + "archive.zip", Encoding.Default);

            return zf;
        }
        public void AddFileZip(string f, ZipFile zf)
        {
            zf.AddFile(f);
        }
        public void Save()
        {
            ZipFile zf = ZipFile.Read(path + "archive.zip");
            zf.Save();
        }
    }
    class ZippedFolder : Entity
    {
        public override void createFolder()
        {
            new ZippedFolder(path).CreateFolder("new folder");
        }
        public override void createFile()
        {
            new ZippedFolder(path).CreateFile("newfile.txt");
        }
        public ZippedFolder(string path) : base(path)
        {

        }
        MemoryStream data = new MemoryStream();
        public  bool Existing()
        {
            if (ZipFile.IsZipFile(path) == true)
            { return true; }
            else { return false; }
        }

        public  string GetName()
        {
            string name = path.Remove(path.Length - 12, 12);
            return name;
        }

        public  string GetFullName(string name)
        {
            return name;
        }

        public List<string> GetAllFiles()
        {
            using (ZipFile zip = ZipFile.Read(path))
            {
                zip.AlternateEncoding = Encoding.Default;
                List<string> fulList = zip.EntryFileNames.ToList();
                List<string> shortList = new List<string>();
                int i = 0;
                foreach (string elem in fulList)
                {
                    i = elem.IndexOf("/");
                    if (i != -1)
                        shortList.Add(elem.Substring(0, i) + "/");
                    else
                        shortList.Add(elem);
                }
                //List<string> gi = shortList.Distinct().ToList();

                return shortList.Distinct().ToList();
            }
        }

        public void InsertZipToZip(string newWay)
        {
            int ZipPlace = path.IndexOf(".archive.zip\\");
            string ArchiveWay = path.Substring(ZipPlace + 13);
            string path1 = path.Substring(0, ZipPlace + 12);
            try
            {
                FolderMethods.DeleteDirectory("ExtractData");
            }
            catch { }
            FolderMethods.CreateDirectory("ExtractData");
            int newZipPlace = newWay.IndexOf(".archive.zip");
            string newArchiveWay = newWay.Substring(newZipPlace);
            string newpath = newWay.Substring(0, newZipPlace + 12);
            if (ArchiveWay[ArchiveWay.Length - 1] == '/')
            {

                ArchiveWay = ArchiveWay.Remove(ArchiveWay.Length - 1, 1) + "\\";
                if (newArchiveWay != "")
                { newArchiveWay = newArchiveWay.Remove(newArchiveWay.Length - 1, 1) + "\\"; }
                else { newArchiveWay = "\\"; }
                using (ZipFile zip = ZipFile.Read(path1))
                {
                    foreach (ZipEntry e in zip)
                    {
                        if (e.FileName.Contains(ArchiveWay.Replace('\\', '/')) && e.FileName.IndexOf(ArchiveWay.Replace('\\', '/')) == 0)
                        {
                            e.Extract("ExtractData", ExtractExistingFileAction.DoNotOverwrite);
                        }
                    }
                }

            }
            else
            {
                using (ZipFile zip = ZipFile.Read(path))
                {

                    foreach (ZipEntry e in zip)
                    {
                        if (e.FileName == ArchiveWay.Replace('\\', '/'))
                            e.Extract("ExtractData", ExtractExistingFileAction.DoNotOverwrite);
                    }
                }
            }

            while (newArchiveWay[newArchiveWay.Length - 1] != '\\')
            {
                newArchiveWay = newArchiveWay.Remove(newArchiveWay.Length - 1, 1);
            }
            using (ZipFile newzip = ZipFile.Read(newpath))
            {
                newzip.AddItem("ExtractData\\" + ArchiveWay, ArchiveWay.Replace('\\', '/'));
                newzip.Save();
            }
            FolderMethods.DeleteDirectory("ExtractData");
        }

        public void CreateFolder(string directoryname)
        {

            int index = path.LastIndexOf('\\');
            string name = path.Remove(index, path.Length - index);

            var dir = Directory.CreateDirectory(name + '\\' + directoryname);


            //  zp.AddDirectory(dir.FullName);
            using (ZipFile zp = ZipFile.Read(path))
            {
                zp.AddItem(dir.FullName, directoryname);
                zp.Save();
            }
            dir.Delete();
        }

        public void CreateFile(string filename)
        {

            MemoryStream memory = new MemoryStream();
            var bytes = ReadMMFAllBytes(filename);
            memory.Write(bytes, 0, bytes.Length);

            using (ZipFile zip = new ZipFile(path))

            {
                ZipEntry e = zip.AddEntry(filename, memory);
                // zip.AddFile(filename);

                zip.Save();

            }

        }

        public static Byte[] ReadMMFAllBytes(string fileName)
        {
            using (var mmf = MemoryMappedFile.CreateFromFile("C:\\Users\\Илья\\Desktop\\Новая папка\\arch.zip"))
            {
                using (var stream = mmf.CreateViewStream())
                {
                    using (BinaryReader binReader = new BinaryReader(stream))
                    {
                        return binReader.ReadBytes((int)stream.Length);
                    }
                }
            }
        }
        public List<string> OpenFolder(string name)
        {

            using (ZipFile zip = ZipFile.Read(path))
            {
                zip.AlternateEncoding = Encoding.GetEncoding(1251);


                List<string> fulList = zip.EntryFileNames.ToList();
                List<string> shortList = new List<string>();
                int i = 0;

                foreach (string elem in fulList)
                {
                    i = elem.IndexOf("/");
                    if (i != -1)
                        shortList.Add(elem.Substring(0, i) + "/");
                    else
                        shortList.Add(elem);
                }

                return shortList.Distinct().ToList();
            }

            //return (names);
        }
        public void Delete(string name)
        {

            using (ZipFile zip = ZipFile.Read(path))
            {

                zip.RemoveEntry(name);

                zip.Save();
            }
        }
        public void InsertZipToDir(string newway)
        {
            try
            {
                FolderMethods.DeleteDirectory("ExtractData");
            }
            catch { }
            FolderMethods.CreateDirectory("ExtractData");

            int ZipPlace = path.IndexOf(".archive.zip\\");
            string ArchiveWay = path.Substring(ZipPlace + 13);
            string path1 = path.Substring(0, ZipPlace + 12);
            using (ZipFile zip = ZipFile.Read(path1))
            {
                if (path[path.Length - 1] == '/')
                {
                    ArchiveWay = ArchiveWay.Remove(ArchiveWay.Length - 1, 1) + "\\";
                    foreach (ZipEntry e in zip)
                    {
                        if (e.FileName.Contains(ArchiveWay.Replace('\\', '/')) && e.FileName.IndexOf(ArchiveWay.Replace('\\', '/')) == 0)
                        {
                            e.Extract("ExtractData", ExtractExistingFileAction.DoNotOverwrite);
                        }
                    }
                    FolderMethods.CopyDir("ExtractData\\" + ArchiveWay, newway + '\\' + ArchiveWay);
                   
                    FolderMethods.DeleteDirectory("ExtractData");
                }
                else
                {
                    foreach (ZipEntry e in zip)
                    {
                        if (e.FileName == ArchiveWay.Replace('\\', '/'))
                            e.Extract("ExtractData", ExtractExistingFileAction.DoNotOverwrite);
                    }
        
                    FileMethods.copy("ExtractData\\" + ArchiveWay,newway, false);
                    FolderMethods.DeleteDirectory("ExtractData");
                }
            }
        }
        public void InsertDirToZip(bool isfile, string newway)
        {
            int ZipPlace = newway.IndexOf(".archive.zip\\");
            string ArchiveWay;
            string path1;
            if (ZipPlace <= 0)
            {
                ZipPlace = newway.IndexOf(".archive.zip");
                int index = newway.LastIndexOf("\\");
                ArchiveWay = newway.Substring(index); ;
                path1 = newway.Substring(0, ZipPlace + 12);
            }
            else
            {
                ArchiveWay = newway.Substring(ZipPlace + 13);
                path1 = newway.Substring(0, ZipPlace + 12);
            }


            using (ZipFile zip = ZipFile.Read(path1))
            {
                if (isfile)
                {

                    while (ArchiveWay != "" && ArchiveWay[ArchiveWay.Length - 1] != '\\')
                    {
                        ArchiveWay = ArchiveWay.Remove(ArchiveWay.Length - 1, 1);

                    }
                    zip.AddItem(path, ArchiveWay.Replace('\\', '/'));
                }

                else
                    zip.AddDirectory(path + "\\", ArchiveWay.Replace('\\', '/').Replace(".archive.zip", ""));
                zip.Save();
            }
        }

        public List<string> OpenFolderInZip()
        {

            int ZipPlace = path.IndexOf(".archive.zip\\");
            string path1 = path.Substring(0, ZipPlace + 12);
            string ArchiveWay = path.Substring(ZipPlace);
            string Way = path.Substring(ZipPlace + 13);

            int SleshCount = (ArchiveWay.Length - ArchiveWay.Replace("\\", "").Length) - 1;
            List<string> l = new ZippedFolder(path1).GetLevelFiles(SleshCount, Way.Replace('\\', '/'));

            return l;

        }
        public List<string> GetLevelFiles(int Slesh, string name)
        {
            using (ZipFile zip = ZipFile.Read(path))
            {
                int ZipPlace1 = name.IndexOf("//");
                if (ZipPlace1 > 0)
                { name = name.Remove(ZipPlace1, 1); }
                List<string> fulList = zip.EntryFileNames.ToList();
                string[] shortList = new string[fulList.Count];
                string[] newList = new string[fulList.Count];
                List<string> finalList = new List<string>();


                shortList = fulList.ToArray();
                int k = 0;
                for (int i = 0; i < fulList.Count; i++)
                {
                    if (shortList[i].Contains(name) && shortList[i].IndexOf(name) == 0)
                    {
                        newList[k] = shortList[i];
                        k++;
                    }

                }
                k = 0;
                int count = 0;
                while (k < newList.Length && newList[k] != null)
                {
                    count++;
                    k++;
                }


                string[] s = new string[count];
                k = 0;
                while (k < newList.Length && newList[k] != null)
                {
                    s[k] = newList[k];
                    k++;
                }

                for (int i = 1; i <= Slesh; i++)
                {
                    for (int j = 0; j < count; j++)
                    {
                        k = s[j].IndexOf("/");
                        s[j] = s[j].Substring(k + 1);
                    }
                }

                foreach (string elem in s)
                {
                    k = elem.IndexOf("/");
                    if (k != -1)
                        if (elem.Substring(0, k) != "")
                            finalList.Add(elem.Substring(0, k) + "/");
                        else { }
                    else
                       if (elem != "")
                        finalList.Add(elem);
                }
                return finalList.Distinct().ToList();
            }
        }

    }

}
