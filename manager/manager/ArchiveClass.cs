using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manager
{
    abstract class ArchiveClass
    {
        public string were;
        public string somePath;
        public void archive(string werewear, string nameofFile_Dir, string type)
        {
            were = werewear;
            somePath = nameofFile_Dir;
            if (type == "directory")
            {
                List<FileInfo> pathsToArchiveFiles;
                Directory.CreateDirectory(werewear + "\\" + nameofFile_Dir + ".archivated");

                DirectoryInfo dInf = new DirectoryInfo(Path.Combine(werewear, nameofFile_Dir));
                FileInfo[] files = dInf.GetFiles("*", SearchOption.AllDirectories);
                pathsToArchiveFiles = files.ToList();
                archiveDir(pathsToArchiveFiles);
            }
            else
                arcOneFile(Path.Combine(werewear,nameofFile_Dir), Path.Combine(werewear, nameofFile_Dir));
        }

        public abstract void archiveDir(List<FileInfo> a);
        
        protected void arcOneFile(string fullNamefrom, string fullNameto)
        {
            FileMethods.archiveFile(fullNamefrom, fullNameto);
        }

    }

    class regularZip : ArchiveClass
    {
        public override void archiveDir(List<FileInfo>a)
        {
            foreach (FileInfo curFile in a)
            {
                arcOneFile(curFile.FullName, were + "\\" + somePath + ".archivated" +
                    "\\" + curFile.Name + ".zip");
            }
        }
    }

    class foreachZip : ArchiveClass
    {
        public override void archiveDir(List<FileInfo> a)
        {
            Parallel.ForEach(a, (currentFile) =>
            {
                arcOneFile(currentFile.FullName, were + "\\" + somePath + ".archivated" +
                    "\\" + currentFile.Name + ".zip");
            });
        }
    }

    class taskZip : ArchiveClass
    {
        public override void archiveDir(List<FileInfo> a)
        {
            Task[] tsk = new Task[a.Count];
            for (int i = 0; i < a.Count; i++)
                tsk[i] = Task.Factory.StartNew((currentFile) =>
                {
                    arcOneFile( ((FileInfo)currentFile).FullName, were + "\\" + somePath + ".archivated" +
                    "\\" + ((FileInfo)currentFile).Name + ".zip");
                }, a[i]);
            Task.WaitAll(tsk);
        }
    }

    class asyncTaskZip : ArchiveClass
    {
        public override async void archiveDir(List<FileInfo> a)
        {
            List<Task> tsk = new List<Task>();
            for (int i = 0; i < a.Count; i++)
                tsk.Add(Task.Factory.StartNew((currentFile) =>
                {
                    arcOneFile(((FileInfo)currentFile).FullName, were + "\\" + somePath + ".archivated" +
                   "\\" + ((FileInfo)currentFile).Name + ".zip");
                }, a[i]));
            await Task.WhenAll(tsk);
        }
    }


}
