using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alimama.Pages;

namespace Alimama
{
    public partial class MockWeb : Form
    {
        private bool currentPageLoaded;
    
        public MockWeb()
        {
            InitializeComponent();
        }

        public bool IsCurrentPageLoaded()
        {
            return currentPageLoaded;
        }

        public async void Login(string username, string password)
        {
            currentPageLoaded = false;
            try
            {
                Invoke((MethodInvoker)delegate {
                    currentPageLoaded = false;
                    webBrowser1.Url = new Uri("https://login.taobao.com/member/login.jhtml?style=mini&newMini2=true&css_style=alimama&from=alimama&redirectURL=http://www.alimama.com&full_redirect=true&disableQuickLogin=true");
                });
                await Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(500);
                        if (IsCurrentPageLoaded())
                        {
                            break;
                        }
                    }
                    Console.WriteLine("loaded!");
                    // 此处进入UI
                    Invoke((MethodInvoker)delegate
                    {
                        HtmlElement loginBox = AllPage.FindElementByClassName(webBrowser1, "login-switch");
                        if (LoginPage.IsQROpen(webBrowser1))
                        {
                            loginBox.InvokeMember("click");
                        }
                    });
                });
            }
            catch(InvalidOperationException)
            {
                Thread.Sleep(500);
                Login(username, password);
            }
            
        }

        public void ElementChangeWithoutPageLoad(Delegate func)
        {
            Invoke(func);
        }

        public void ElementChangeWithPageLoad(Delegate func)
        {
            currentPageLoaded = false;
            ElementChangeWithoutPageLoad(func);
        }
        
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //
            Console.WriteLine(e);
            currentPageLoaded = true;
        }

        private void webBrowser1_FileDownload(object sender, EventArgs e)
        {
            // File Download
            Console.WriteLine("File Downloaded:"+ e);
        }

        //public MockWeb init()
        //{
        //    InitializeComponent();
        //    return this;
        //}
    }
}
