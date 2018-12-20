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
    class ZipFile : Entity
    {
        public override void createFolder()
        {
            new ZipFolder(path).CreateFolder("new folder");
        }
        public override void createFile()
        {
            new ZipFolder(path).CreateFile("newfile.txt");
        }
        public ZipFile(string path) : base(path)
        {

        }
        public void Delete(string name)
        {
            using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(path))
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
            using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(path1))
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
            new ZipFile(DirectoryPath).OpenFile(FilePath);
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
    }
    class ZipFolder : Entity
    {
        public override void createFolder()
        {
            new ZipFolder(path).CreateFolder("new folder");
        }
        public override void createFile()
        {
            new ZipFolder(path).CreateFile("newfile.txt");
        }
        public ZipFolder(string path) : base(path)
        {

        }
        public List<string> GetAllFiles()
        {
            using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(path))
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
        public void CreateFolder(string directoryname)
        {

            int index = path.LastIndexOf('\\');
            string name = path.Remove(index, path.Length - index);

            var dir = Directory.CreateDirectory(name + '\\' + directoryname);


            //  zp.AddDirectory(dir.FullName);
            using (Ionic.Zip.ZipFile zp = Ionic.Zip.ZipFile.Read(path))
            {
                zp.AddItem(dir.FullName, directoryname);
                zp.Save();
            }
            dir.Delete();
        }
        public void CreateFile(string filename)
        {

            MemoryStream memory = new MemoryStream();
            var bytes = ReadBytes(filename);
            memory.Write(bytes, 0, bytes.Length);

            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(path))

            {
                ZipEntry e = zip.AddEntry(filename, memory);
                // zip.AddFile(filename);

                zip.Save();

            }

        }
        public static Byte[] ReadBytes(string fileName)
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
        public List<string> GetLevelFiles(int Slesh, string name)
        {
            using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(path))
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
