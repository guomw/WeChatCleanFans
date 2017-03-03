using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using System.Windows.Forms;
using Alimama;
using System.Threading;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace AlimamaTests
{
    class SimpleAware : LoginAware
    {
        bool LoginAware.success(JArray jsons)
        {
            Console.WriteLine(JsonConvert.SerializeObject(jsons));
            return true;
        }
    }
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {   
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MockWeb());
            LoginWindow web = new LoginWindow(new SimpleAware());
            
            //Thread thread = new Thread(() =>
            //{
            //    web.Login("tb8138055", "yangzhenhao3");
            //});
            //thread.Start();

            //WebBrowser browser = web.workingBrowser();
            //// HRESULT E_FAIL
            //browser.Url = new Uri("https://login.taobao.com/member/login.jhtml?style=mini&newMini2=true&css_style=alimama&from=alimama&redirectURL=http://www.alimama.com&full_redirect=true&disableQuickLogin=true");
            Application.Run(web);
            
            //Console.WriteLine(browser.DocumentTitle);
            //Console.WriteLine(browser.Document);
        }
    }

    
}
