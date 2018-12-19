using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace manager
{
    class Strategy
    {
        whatiDo myInter;

        public Strategy(whatiDo a)
        {
            myInter = a;
        }

        public void setInterface(whatiDo param)
        {
            myInter = param;
        }
        public void getRegFiles(string dire)
        {
            string[] FilesName = Directory.GetFiles(dire, "*.*", SearchOption.AllDirectories);

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

            myInter.whatiFound(dire, FilesName.ToList(), regular_str);
        }

    }

    interface whatiDo
    {
        void whatiFound(string dir, List<string> FilesName, List<string> regular_str);
    }

    class defaultSearch : whatiDo
    {
        public void whatiFound(string dir, List<string> FilesName, List<string> regular_str)
        {
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



            StreamWriter sw = new StreamWriter(Path.Combine(dir, "foundreg.txt"), false, System.Text.Encoding.Default);

            foreach (string line in Itog)
            {
                sw.WriteLine(line);
            }
            sw.Close();
        }
    }

    class foreachSearch : whatiDo
    {
        public void whatiFound(string dir, List<string> FilesName, List<string> regular_str)
        {
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



            StreamWriter sw = new StreamWriter(Path.Combine(dir, "foundreg.txt"), false, System.Text.Encoding.Default);

            foreach (string line in Itog)
            {
                sw.WriteLine(line);
            }
            sw.Close();
        }
    }

    class taskSearch : whatiDo
    {
        public void whatiFound(string dir, List<string> FilesName, List<string> regular_str)
        {
            List<string> Itog = new List<string>();

            Task[] u = new Task[FilesName.Count];

            for (int j = 0; j < FilesName.Count; j++)
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

            StreamWriter sw = new StreamWriter(Path.Combine(dir, "foundreg.txt"), false, System.Text.Encoding.Default);



            foreach (string line in Itog)
            {
                sw.WriteLine(line);
            }

            sw.Close();
        }
    }

    class asynctaskSearch : whatiDo
    {
        string asd(string a, List<string> b)
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
                            return (a + "   " + s + "   " + Regex.Match(line, s));
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
        async Task<string> dodo(string path, List<string> regular_str)
        {
            return await Task.Factory.StartNew(() => asd(path, regular_str));
        }

        public async Task<List<string>> dosmth(string a)
        {
            List<Task<string>> output = new List<Task<string>>();

            string[] FilesName = Directory.GetFiles(a, "*.*", SearchOption.AllDirectories);

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

            foreach (string st in FilesName)
            {
                output.Add(dodo(st, regular_str));
            }
            var res = await Task.WhenAll(output);
            return new List<string>(res);
        }

        public void whatiFound(string dir, List<string> FilesName, List<string> regular_str)
        {
            List<string> x = new List<string>();

            Thread myThread = new Thread(new ThreadStart(async () => {
                //  x = new List<string>();
                x = await dosmth(dir);

                StreamWriter sw = new StreamWriter(Path.Combine(dir, "foundreg.txt"), false, System.Text.Encoding.Default);

                foreach (string line in x)
                {
                    sw.WriteLine(line);
                }

                sw.Close();

            }));
            myThread.Start();
        }
    }


}
