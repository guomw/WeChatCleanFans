using Microsoft.VisualStudio.TestTools.UnitTesting;
using WwChatHttpCore.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Net;

namespace WwChatHttpCore.HTTP.Tests
{
    [TestClass()]
    public class LoginServiceTests
    {
        private LoginService service = new LoginService();

        [TestMethod()]
        public void GetQRCodeTest()
        {
            Image image = service.GetQRCode();
            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".png";
            image.Save(fileName);
            Process.Start("explorer.exe", fileName);
            // 一直等待到50s
            int count = 50;
            while (count-- > 0)
            {
                Thread.Sleep(1000);
                object result  = service.LoginCheck(); 
                if(result is string)
                {
                    service.GetSidUid((string)result);
                    // 显示当前的所有数据
                    Console.WriteLine("LoginService._session_id = \"" + LoginService._session_id + "\";");
                    Console.WriteLine("LoginService.Pass_Ticket = \"" + LoginService.Pass_Ticket + "\";");
                    Console.WriteLine("LoginService.SKey = \"" + LoginService.SKey + "\";");
                    // 所有的cookie
                    // name value path domain
                    foreach (Cookie c in BaseService.GetAllCookies(BaseService.CookiesContainer))
                    {   
                        Console.WriteLine("BaseService.CookiesContainer.Add(new System.Net.Cookie(\""+c.Name+"\",\""+c.Value+"\",\""+c.Path+"\",\""+c.Domain+"\"));");
                    }
                    //BaseService.CookiesContainer.Add(new System.Net.Cookie())
                    return;
                }
            }
            
        }

        //[TestMethod()]
        public void GetSidUidTest()
        {

        }
    }
}