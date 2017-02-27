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
using IniParser.Model;

namespace WwChatHttpCore.HTTP.Tests
{
    [TestClass()]
    public class LoginServiceTests
    {
        public static string INIFile()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.WXTest.ini";
            //return Path.GetTempPath() + "/.WXTest.ini"; 
        }
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
                    new WXService().WxSyncCheck();

                    IniData data = new IniData();
                    SectionData commonData = new SectionData("Common");
                    commonData.Keys["_session_id"] = LoginService._session_id;
                    commonData.Keys["Pass_Ticket"] = LoginService.Pass_Ticket;
                    commonData.Keys["SKey"] = LoginService.SKey;
                    
                    //Path.
                    // 显示当前的所有数据
                    Console.WriteLine("LoginService._session_id = \"" + LoginService._session_id + "\";");
                    Console.WriteLine("LoginService.Pass_Ticket = \"" + LoginService.Pass_Ticket + "\";");
                    Console.WriteLine("LoginService.SKey = \"" + LoginService.SKey + "\";");
                    // 所有的cookie
                    // name value path domain
                    foreach (Cookie c in BaseService.GetAllCookies(BaseService.CookiesContainer))
                    {
                        SectionData cookiesData = new SectionData("Cookie_" + c.Name);
                        cookiesData.Keys.AddKey("Name", c.Name);
                        cookiesData.Keys.AddKey("Value", c.Value);
                        cookiesData.Keys.AddKey("Path", c.Path);
                        cookiesData.Keys.AddKey("Domain", c.Domain);

                        data.Sections.Add(cookiesData);
                        
                        Console.WriteLine("BaseService.CookiesContainer.Add(new System.Net.Cookie(\""+c.Name+"\",\""+c.Value+"\",\""+c.Path+"\",\".qq.com\"));");
                        
                    }
                    //BaseService.CookiesContainer.Add(new System.Net.Cookie())
                    Console.WriteLine("WXService.WeixinRouteHost = \""+WXService.WeixinRouteHost+"\";");
                    commonData.Keys["WeixinRouteHost"] = WXService.WeixinRouteHost;

                    data.Sections.Add(commonData);
                    
                    new IniParser.FileIniDataParser().WriteFile(INIFile(), data);
                    //Console.WriteLine(iniFile);
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