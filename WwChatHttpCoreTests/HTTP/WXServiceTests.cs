using Microsoft.VisualStudio.TestTools.UnitTesting;
using WwChatHttpCore.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

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
            LoginService._session_id = "QdrMLKl8Wg==";
            LoginService.Pass_Ticket = "OsD07ynIzcYgPqjM9a68rucmpd0fAZigB4UOQNfPQl4%3D";
            LoginService.SKey = "@crypt_e4421859_efa52fd7e12f9602590a3ce77c304fcd";
            BaseService.CookiesContainer.Add(new System.Net.Cookie("wxuin", "185257400", "/", "wx.qq.com"));
            BaseService.CookiesContainer.Add(new System.Net.Cookie("wxsid", "rtmZey+R/5/4K7DN", "/", "wx.qq.com"));
            BaseService.CookiesContainer.Add(new System.Net.Cookie("wxloadtime", "1487843432", "/", "wx.qq.com"));
            BaseService.CookiesContainer.Add(new System.Net.Cookie("mm_lang", "zh_CN", "/", "wx.qq.com"));
            BaseService.CookiesContainer.Add(new System.Net.Cookie("webwxuvid", "899e8a61e831fabab1a590dbde8f188748d0aa0b3b603249fbd579e28fb32a8caf1fc3b5f287792ee020df8893644c13", "/", "wx.qq.com"));
            BaseService.CookiesContainer.Add(new System.Net.Cookie("webwx_auth_ticket", "CIsBEIabkIYFGoABkOag2OgJ7cX34rE2nJDBCg7EBylhvbLffr1x7g2hOC1+M5Ff9VRR48EyQjN3rYyOFTBvzZ3+gGE9GlkEydxuGkUdrQ/oiHGs4Tts7ioKNHN7qS1SG507f8eMkO5888ndc6o1+xXhDmVi4Ed1OzdD7rJRJncI8/PEQShigcjC6xY=", "/", "wx.qq.com"));
            BaseService.CookiesContainer.Add(new System.Net.Cookie("webwx_data_ticket", "gScxU5X+DISs5bjlJIx9BW4c", "/", ".qq.com"));


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
            
        }
    }
}