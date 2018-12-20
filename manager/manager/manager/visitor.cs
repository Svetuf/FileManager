using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace manager
{
    class MD5
    {
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }

    interface IVisitor
    { 
        void Visit(FileMethods f);
        void Visit(FolderMethods f);
    }

    class Md5Hash : IVisitor
    {
        public void Visit(FileMethods f)
        {
            string hashed = MD5.CreateMD5(f.myFullPath());
            MessageBox.Show(hashed);
        }
        public void Visit(FolderMethods f)
        {
            FileMethods[] mas = FolderMethods.myfilesEntity(f.GetMyName());
            string dano = "";
            foreach(FileMethods i in mas)
            {
                dano += i.getMytTxt();
            }

            string hashed = MD5.CreateMD5(dano);
            MessageBox.Show(hashed);
        }
    }

    class crypt : IVisitor
    {
       private int NumToCript;

       public crypt()
       {
            NumToCript = 0;
       }

        public void setNum(int n)
        {
            NumToCript = n;
        }

       public void Visit(FileMethods f)
       {
           string wat = f.getMytTxt();
           wat = encrypt(wat);
           var someStream = FileMethods.create(f.myFullPath() + "_encrypted");
           byte[] info = new UTF8Encoding(true).GetBytes(wat);
           someStream.Write(info, 0, info.Length);
           someStream.Close();
           FileMethods.delete(f.myFullPath());
       }
       public void Visit(FolderMethods f)
       {
            FileMethods[] mas = FolderMethods.myfilesEntity(f.GetMyName());

            foreach (FileMethods i in mas)
            {
                Visit(i);
            }
        }

       private string encrypt(string a)
       {
            char[] mas = a.ToArray();
            string s = "";
            for(int i = 0; i < a.Length; i++)
            {
                mas[i] =  (char)(mas[i] + NumToCript);
                s += mas[i];
            }
            return s;
       }

    }

    class encrypt : IVisitor
    {
        crypt c;
        int myNum;
        public encrypt()
        {
            myNum = 0;
            c = new crypt();
        }

        public void setKey(int a)
        {
            myNum = a;
        }
        public void Visit(FileMethods f)
        {
            myNum = -myNum;
            string wat = f.getMytTxt();
            wat = encryption(wat);
            var someStream = FileMethods.create(f.myFullPath().Remove(f.myFullPath().Length - 10, 10));
            byte[] info = new UTF8Encoding(true).GetBytes(wat);
            someStream.Write(info, 0, info.Length);
            someStream.Close();
            FileMethods.delete(f.myFullPath());
        }
        public void Visit(FolderMethods f)
        {
            FileMethods[] mas = FolderMethods.myfilesEntity(f.GetMyName());

            foreach(FileMethods i in mas)
            {
                Visit(i);
                myNum = -myNum;
            }

        }

        private string encryption(string a)
        {
            char[] mas = a.ToArray();
            string s = "";
            for (int i = 0; i < a.Length; i++)
            {
                mas[i] = (char)(mas[i] + myNum);
                s += mas[i];
            }
            return s;
        }

    }

}
