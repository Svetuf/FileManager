using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Linq;
using System.Net;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace manager
{
    public partial class Form7 : Form
    {
        CancellationTokenSource cancelTokenSource;

        CancellationToken token;

        List<string> urls = new List<string>();

        IProgress<int> progress;


        public Form7()
        {
            InitializeComponent();
            cancelTokenSource = new CancellationTokenSource();
            token =  cancelTokenSource.Token;
            


            urls.Add("https://www.yandex.ru/");
            urls.Add("https://www.google.ru/");
            urls.Add("https://www.apple.com/ru/");
            urls.Add("https://www.microsoft.com/ru-ru/");
            urls.Add("https://www.oracle.com/ru/index.html");
            urls.Add("https://digdes.ru");
            urls.Add("https://mail.ru");
            urls.Add("https://www.yahoo.com/");
            urls.Add("https://github.com/");
            urls.Add("https://ru.aliexpress.com/");
            urls.Add("https://www.youtube.com/");
            urls.Add("https://www.amazon.com/");
            urls.Add("https://www.youtube.com/");
            urls.Add("https://www.youtube.com/");
            urls.Add("https://www.youtube.com/");
            urls.Add("https://www.youtube.com/");
            urls.Add("https://www.youtube.com/");
            urls.Add("https://www.youtube.com/");
            urls.Add("https://www.youtube.com/");
            urls.Add("https://www.youtube.com/");
            urls.Add("https://www.youtube.com/");
            urls.Add("https://www.youtube.com/");
            urls.Add("https://www.youtube.com/");
            urls.Add("https://www.youtube.com/");


            progressBar1.Maximum = urls.Count + 1;
            progressBar1.Value = 0;

        }

        public static async Task<string> downSite(string path)
        {
            WebClient client = new WebClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
           
            return  await client.DownloadStringTaskAsync(path);
        }

        public  async Task<List<string>> RunDownloadAsync(IProgress<int>progress, CancellationToken token, List<string> murls)
        {
            List<string> websites = murls;
            List<string> output = new List<string>();
            int report = 0;

            foreach (string site in websites)
            {
                
                //token.ThrowIfCancellationRequested();
                if (token.IsCancellationRequested)
                    return null;
                output.Add(await downSite(site));
                report++;
                progress.Report(report);
                // update statusbar

               // this.Invoke((MethodInvoker)(() =>
              //  {
              //      this.progressBar1.Increment(1);
               // }));



            }

           // var res = await Task.WhenAll(output);
            return  output;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            cancelTokenSource.Cancel();
           // this.Close();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            //  try {
                
              //  Thread myThread = new Thread(new ThreadStart( async () => {
                    List<string> x = new List<string>();
                    Progress<int> myProgress = new Progress<int>();
                    myProgress.ProgressChanged += reportProgress;


                    x = await RunDownloadAsync(myProgress, token, urls);
                    if (token.IsCancellationRequested)
                    {
                        progressBar1.Value = progressBar1.Maximum;
                        return;
                    }
                    Stream stream = new FileStream(@"C:\Users\Илья\Desktop\web.txt", FileMode.Create, FileAccess.Write, FileShare.None);
                    if (x != null)
                        foreach (string st in x)
                        {
                            if (token.IsCancellationRequested)
                                break;
                            stream.Write(Encoding.Default.GetBytes(st), 0, st.Length);
                        }
                    
                    stream.Close();
            progressBar1.Value = urls.Count + 1; ;

            //    }));
            //    myThread.Start();

                
                
          //  }
         //   catch(Exception e2)
          //  {
         //       int a = 4;
          //  }



        }

        private void reportProgress(object sender, int e)
        {
            this.Invoke((MethodInvoker)(() =>
            {
                progressBar1.Value = e;
            }));
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private async void button3_Click(object sender, EventArgs e)
        {
            List<Task<string>> output = new List<Task<string>>();
            foreach(string site in urls)
            {
                output.Add(downSite(site));
                progressBar1.Increment(1);
                if (token.IsCancellationRequested)
                    break;
            }
            var res = await Task.WhenAll(output);


            Stream stream = new FileStream(@"C:\Users\Илья\Desktop\web.txt", FileMode.Create, FileAccess.Write, FileShare.None);
            if (res != null)
                foreach (string st in res)
                {
                    if (token.IsCancellationRequested)
                        break;
                    stream.Write(Encoding.Default.GetBytes(st), 0, st.Length);
                }
            progressBar1.Value = progressBar1.Maximum;

            stream.Close();

        }
    }
}
