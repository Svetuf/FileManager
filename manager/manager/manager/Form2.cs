using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Security.Cryptography.Xml;

namespace manager
{
    public partial class Form2 : Form
    {

        userSettings users;
        string myName = "", myPas = "";
        public Form2()
        {
            InitializeComponent();
            users = new userSettings();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            desera();
            this.FormClosing += new FormClosingEventHandler(sera2);
        }
        
        
        void desera()
        {
            BinaryFormatter binformat = new BinaryFormatter();
            try {
                Stream stream = new FileStream(@"C:\Users\Илья\Desktop\bindata.bin", FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                userSettings us = (userSettings)binformat.Deserialize(stream);
                stream.Close();
                users = us;
                foreach(string i in users.getNames())
                {
                    comboBox1.Items.Add(i);
                }
            }
            catch(Exception e) { }

          
        }


        void sera2(object sender, FormClosingEventArgs e)
        {
            sera();
        }
       void sera()
        {
            BinaryFormatter binformat = new BinaryFormatter();
            Stream stream = new FileStream(@"C:\Users\Илья\Desktop\bindata.bin", FileMode.Create, FileAccess.Write, FileShare.None);

            binformat.Serialize(stream, users);
            stream.Close();
        }

       

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            myName = comboBox1.SelectedItem.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form5 f  = new Form5();
            f.FormClosing += new FormClosingEventHandler(f2_FormClosing);
            f.ShowDialog();
        }

        void f2_FormClosing(object sender, FormClosingEventArgs e)
        {
            List<string> a = new List<string>((sender as Form5).retUser());
            primaryUser newuser = new primaryUser(a[0], a[1], @"\");
            users.Add(newuser);
            comboBox1.Items.Add(newuser.getName());
        }

        private void button2_Click(object sender, EventArgs e)
        {
             //открываем новую форму, передаем ей users и удаляем выбранного юзера
        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            myPas = richTextBox3.Text;
            
                if( users.findSome(myName, myPas))
                {
                    this.Hide();


                    //serialixa
                    sera();
                    Form1 f = new Form1(myName);
                    f.ShowDialog();
                }
            
        }
    }

    [Serializable]
    public class userSettings
    {
        public userSettings()
        {
            names = new List<string>();
            passwords = new List<string>();
            pathIGones = new List<string>();
        }
        public userSettings(string nameArg, string passwordArg, string pathIGOneArg = "")
        {
            
        }

        public void remove(string name)
        {

        }
        public void Add(primaryUser us)
        {
            names.Add(us.getName());
            passwords.Add(us.getPas());
            pathIGones.Add(us.getPathIGone());
        }

        public bool findSome(string name, string pas)
        {
           for(int i = 0; i < names.Count; i++)
            {
                if (name == names[i])
                    if (pas == passwords[i])
                        return true;
            }
            return false;
        }
        public List<string> getNames() { return names; }
        public void SetNames(List<string> me) { names = me; }

        public void SetPas(List<string> pass) { passwords = pass; }

        public List<string> getPasswords() { return passwords; }
     //   public string getPathIGone() { return pathIGone; }
        List <string> names;
        List<string> passwords;
        List<string> pathIGones;
        int kluch = 1;

        [OnSerializing]
        internal void cryptName(StreamingContext context)
        {
            for (int j = 0; j < names.Count; j++)
            {
                char[] charStr = names[j].ToCharArray();
                for (int i = 0; i < names[j].Length; i++)
                    charStr[i] = (char)(charStr[i] + kluch);
                names[j] = new string(charStr);
            }
            for (int j = 0; j < passwords.Count; j++)
            {
                char[] charStr = passwords[j].ToCharArray();
                for (int i = 0; i < passwords[j].Length; i++)
                    charStr[i] = (char)(charStr[i] + kluch);
                passwords[j] = new string(charStr);
            }

        }
        

        [OnDeserialized]
        internal void decryptNam(StreamingContext context)
        {
            for (int j = 0; j < names.Count; j++)
            {
                char[] charStr = names[j].ToCharArray();
                for (int i = 0; i < names[j].Length; i++)
                    charStr[i] = (char)(charStr[i] - kluch);
                names[j] = new string(charStr);
            }
            for (int j = 0; j < passwords.Count; j++)
            {
                char[] charStr = passwords[j].ToCharArray();
                for (int i = 0; i < passwords[j].Length; i++)
                    charStr[i] = (char)(charStr[i] - kluch);
                passwords[j] = new string(charStr);
            }
        }
        


    }

    public class primaryUser
    {
        string name;
        string password;
        string pathIGone;
        public primaryUser()
        {

        }
        public primaryUser(string nameArg, string passwordArg, string pathIGOneArg = "")
        {
            name = nameArg;
            password = passwordArg;
            pathIGone = pathIGOneArg;
        }
        public string getName() { return name; }
        public string getPas() { return password; }
        public string getPathIGone() { return pathIGone; }
    };

}
