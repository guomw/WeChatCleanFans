using Microsoft.VisualStudio.TestTools.UnitTesting;
using WwChatHttpCore.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;
using IniParser.Model;

namespace WwChatHttpCore.HTTP.Tests
{
    [TestClass()]
    public class WXServiceTests
    {
        public void init()
        {
            IniData data  = new IniParser.FileIniDataParser().ReadFile(LoginServiceTests.INIFile());

            SectionData commonData = data.Sections.GetSectionData("Common");
            LoginService._session_id = commonData.Keys["_session_id"];
            LoginService.Pass_Ticket = commonData.Keys["Pass_Ticket"];
            LoginService.SKey = commonData.Keys["SKey"];
            WXService.WeixinRouteHost = commonData.Keys["WeixinRouteHost"];

            foreach(SectionData session in data.Sections){
                if (!session.SectionName.StartsWith("Cookie"))
                    continue;
                BaseService.CookiesContainer.Add(new System.Net.Cookie(session.Keys["Name"], session.Keys["Value"], session.Keys["Path"], session.Keys["Domain"]));
            }

            service.WxSyncCheck();

            WXService.UploadMediaSerialId = 10;
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

            //service.SendTextMessageToNickName("Guo Childe", "中文，！" + DateTime.Now);
            using (Stream str = new FileStream("Images/test.png", FileMode.Open))
            {
                service.SendImageToNickName("Guo Childe", "well.png",str);
            }

            using (Stream str = new FileStream("Images/test.png", FileMode.Open))
            {
                service.SendImageToNickName("Guo Childe", "well.png", str);
            }

            using (Stream str = new FileStream("Images/test.png", FileMode.Open))
            {
                service.SendImageToNickName("Guo Childe", "well.png", str);
            }

            using (Stream str = new FileStream("Images/test.png", FileMode.Open))
            {
                service.SendImageToNickName("Guo Childe", "well.png", str);
            }

            using (Stream str = new FileStream("Images/test.png", FileMode.Open))
            {
                service.SendImageToNickName("Guo Childe", "well.png", str);
            }
        }
    }
}