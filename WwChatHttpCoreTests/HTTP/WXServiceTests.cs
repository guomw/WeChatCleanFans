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
            LoginService._session_id = "ocpfmgvDQw==";
            LoginService.Pass_Ticket = "8OBmLQI7B1udpM7E4w08vIGUI8rJEhuuYi6GWc78mPY%3D";
            LoginService.SKey = "@crypt_e4421859_ba12173f2a00aadcd3025b44ea18fc3f";
            BaseService.CookiesContainer.Add(new System.Net.Cookie("wxuin", "185257400", "/", ".qq.com"));
            BaseService.CookiesContainer.Add(new System.Net.Cookie("wxsid", "XVSlokdJQJioplaD", "/", ".qq.com"));
            BaseService.CookiesContainer.Add(new System.Net.Cookie("wxloadtime", "1487908917", "/", ".qq.com"));
            BaseService.CookiesContainer.Add(new System.Net.Cookie("mm_lang", "zh_CN", "/", ".qq.com"));
            BaseService.CookiesContainer.Add(new System.Net.Cookie("webwxuvid", "899e8a61e831fabab1a590dbde8f1887565b604f5f501733d5f4852ab1d6394b8a1a4fcbe2d0c8b370b708b351e543c8", "/", ".qq.com"));
            BaseService.CookiesContainer.Add(new System.Net.Cookie("webwx_auth_ticket", "CIsBEJuNjMAEGoABxZZSKYCRqZMAIZN58Fr0/w7EBylhvbLffr1x7g2hOC1kKWYIEjQ1ltWIrpkhLZHmEQsEcHlscFe4Qcc7bxzyZluIRyxzAjhDsqGFtOqK9C4H1lJX/14F8JuODyBJByc5GTsJ7b2dMNkpGlyhImyd0bJRJncI8/PEQShigcjC6xY=", "/", ".qq.com"));
            BaseService.CookiesContainer.Add(new System.Net.Cookie("webwx_data_ticket", "gScvm3laP+YSOUjPxUGegUNE", "/", ".qq.com"));
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

            service.SendImageToNickName("Guo Childe","abc.jpg", new FileStream("Images/Penguins.jpg",FileMode.Open));
        }
    }
}