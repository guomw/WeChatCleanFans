using Microsoft.VisualStudio.TestTools.UnitTesting;
using WwChatHttpCore.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;

namespace WwChatHttpCore.HTTP.Tests
{
    [TestClass()]
    public class WXServiceTests
    {
        public void init()
        {
//        测试名称: GetQRCodeTest
//测试结果:	已通过
//结果 的标准输出:
            LoginService._session_id = "oZ_iwtDZlQ==";
            LoginService.Pass_Ticket = "zy3%2FJSr7prn%2BxfTzJxr7wmJ%2FAzjzbSwVAxDKQfhjZLE%3D";
            LoginService.SKey = "@crypt_e4421859_b7a8a8bdc5f9a0fc33ca01bb807ac706";
            BaseService.CookiesContainer.Add(new System.Net.Cookie("wxuin", "185257400", "/", ".qq.com"));
            BaseService.CookiesContainer.Add(new System.Net.Cookie("wxsid", "XxZWuU8lre1tvyw5", "/", ".qq.com"));
            BaseService.CookiesContainer.Add(new System.Net.Cookie("wxloadtime", "1487911709", "/", ".qq.com"));
            BaseService.CookiesContainer.Add(new System.Net.Cookie("mm_lang", "zh_CN", "/", ".qq.com"));
            BaseService.CookiesContainer.Add(new System.Net.Cookie("webwxuvid", "899e8a61e831fabab1a590dbde8f188779e7509abba289b55a0ae8366fb7fc230ca5dba885e28521b09ea703bed612b7", "/", ".qq.com"));
            BaseService.CookiesContainer.Add(new System.Net.Cookie("webwx_auth_ticket", "CIsBELGdrM0CGoABlp0W2btzIw3zw5+OsXBTXQ7EBylhvbLffr1x7g2hOC07CH01Kb5vEpPzhazJJWGWubC9ZmhXVpeDkuxeZ31FaIC/v+KAyq3N/DQV4r14r723repKie/lANtbIaV5D+3G+TYHXhrQvACcEfpfChdcubJRJncI8/PEQShigcjC6xY=", "/", ".qq.com"));
            BaseService.CookiesContainer.Add(new System.Net.Cookie("webwx_data_ticket", "gScTLL1S5mnaSkV73jvSX3Wd", "/", ".qq.com"));
            WXService.WeixinRouteHost = "wx.qq.com";


        }
        private readonly WXService service = new WXService();
        [TestMethod()]
        public void SendMsgTest()
        {
            init();
            //new WXService().SendMsg("haha")
            //JObject list = service.GetContact();
            //Console.WriteLine(list);

            //JObject info = service.WxInit();
            //string myName = info["User"]["UserName"].ToString();

            //foreach (JObject contact in list["MemberList"])
            //{
            //    if (contact["NickName"].ToString()== "罗国华"){
            //        service.SendMsg("哈哈", myName, contact["UserName"].ToString(), 1);
            //        return;
            //    }
            //}
            //service.SendTextMessageToNickName("Guo Childe", "如果说呢");
            service.SendTextMessageToNickName("Guo Childe", "中文，！" + DateTime.Now);

            service.SendImageToNickName("Guo Childe","well.png", new FileStream("Images/test.png",FileMode.Open));
        }
    }
}