using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
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
        public static GZipStream GetGZipStream(FileStream targetStream)
        {
            GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress);
            return compressionStream;
        }

    }

    class FileMethods : Entity
    {

        public FileMethods(string path) : base(path)
        {

        }
        
        public static string Name(string path)
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
        
        public static void CopyToZippedFolder(string pathfile)
        {
            File.Copy(pathfile, pathfile + "_ZIP" + Path.DirectorySeparatorChar + Path.GetFileName(pathfile));
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

    }

    // Folder methods.
    class FolderMethods : Entity
    {

        ArchiveClass archivator = new regularZip();
        public FolderMethods(string path) : base(path)
        {
            archivator = new regularZip();
        }

        // Returns the path of the folder.
        public static string GetName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        // Create directory.
        public static void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        // Deletes directory
        public static void DeleteDirectory(string path)
        {
            Directory.Delete(path, true);
        }

        // Creates a zip archive of a folder.
        public void CreateZipFrom(string pathfile, string compressfile)
        {
            archivator.archive(pathfile,path, "directory");
        }

        // Returns the list of directories from path.
        public static DirectoryInfo[] GetDirectoryInfos(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            DirectoryInfo[] directories = di.GetDirectories();
            return directories;
        }

        public static DirectoryInfo[] getrecursiveDirs(string p)
        {
            DirectoryInfo di = new DirectoryInfo(p);
            DirectoryInfo[] directories = di.GetDirectories("",SearchOption.AllDirectories);
            return directories;
        }

        // Returns the directory info.
        public static DirectoryInfo GetDirectoryInfo(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            return di;
        }

        // Creates an unzipped folder near the zip.
        public static void Unzip(string pathToUzip, string whereToUnzip)
        {
            //ipFile.ExtractToDirectory(pathToUzip, whereToUnzip);
        }

        // Updates Directory list.
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

        // If the directory exists.
        public static bool IfExist(string path)
        {
            return Directory.Exists(path);
        }

        public static string[] myfiles(string pas)
        {
            return Directory.GetFiles(pas, "*.*",SearchOption.AllDirectories);
        }

        public static void move(string from, string to)
        {
            Directory.Move(from, to);
        } 

     }
}
