using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web;
using WwChatHttpCore.Objects;
using System.Security.Cryptography;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WwChatHttpCore.HTTP
{
    /// <summary>
    /// 微信主要业务逻辑服务类
    /// </summary>
    public class WXService
    {
        private static Dictionary<string, string> _syncKey = new Dictionary<string, string>();

        public static int UploadMediaSerialId = 0;
        public static string WeixinRouteHost = "wx2.qq.com";
        public static string DeviceID;
        /// <summary>
        /// 
        /// </summary>
        /// <returns>微信初始化url</returns>
        private string GetURLInit()
        {
            return "https://" + WeixinRouteHost + "/cgi-bin/mmwebwx-bin/webwxinit?r=";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>获取好友头像</returns>
        private string GetURLGetIcon()
        {
            return "https://" + WeixinRouteHost + "/cgi-bin/mmwebwx-bin/webwxgeticon?username=";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>获取群聊（组）头像</returns>
        private string GetURLGetHeadImg()
        {
            return "https://" + WeixinRouteHost + "/cgi-bin/mmwebwx-bin/webwxgetheadimg?username=";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>获取好友列表</returns>
        private string GetURLGetContact()
        {
            return "https://" + WeixinRouteHost + "/cgi-bin/mmwebwx-bin/webwxgetcontact";
        }
        //同步检查url
        private static string _synccheck_url = "https://webpush.weixin.qq.com/cgi-bin/mmwebwx-bin/synccheck?sid={0}&uin={1}&synckey={2}&r={3}&skey={4}&deviceid={5}";
        /// <summary>
        /// 
        /// </summary>
        /// <returns>同步url</returns>
        private string GetURLSYNC()
        {
            return "https://" + WeixinRouteHost + "/cgi-bin/mmwebwx-bin/webwxsync?sid=";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>发送消息url</returns>
        private string GetURLSendMessage()
        {
            return "https://" + WeixinRouteHost + "/cgi-bin/mmwebwx-bin/webwxsendmsg?sid=";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>发图片消息url</returns>
        private string GetURLSendMediaMessage()
        {
            return "https://" + WeixinRouteHost + "/cgi-bin/mmwebwx-bin/webwxsendmsgimg?fun=async&f=json&lang=zh_CN";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>创建群聊URL</returns>
        private string GetURLCreateChatRoom()
        {
            return "https://" + WeixinRouteHost + "/cgi-bin/mmwebwx-bin/webwxcreatechatroom";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>上传媒体</returns>
        private string GetURLUploadMedia()
        {
            return "https://file." + WeixinRouteHost + "/cgi-bin/mmwebwx-bin/webwxuploadmedia?f=json";
        }

        private JObject initData;
        /// <summary>
        /// 微信初始化
        /// </summary>
        /// <returns></returns>
        public JObject WxInit()
        {
            string init_json = "{{\"BaseRequest\":{{\"Uin\":\"{0}\",\"Sid\":\"{1}\",\"Skey\":\"{3}\",\"DeviceID\":\"{2}\"}}}}";
            Cookie sid = BaseService.GetCookie("wxsid");
            Cookie uin = BaseService.GetCookie("wxuin");

            if (sid != null && uin != null)
            {
                init_json = string.Format(init_json, uin.Value, sid.Value, GetDeviceID(), LoginService.SKey);
                byte[] bytes = BaseService.SendPostRequest(GetURLInit() + getClientMsgId() + "&pass_ticket=" + LoginService.Pass_Ticket, init_json);
                string init_str = Encoding.UTF8.GetString(bytes);

                JObject init_result = JsonConvert.DeserializeObject(init_str) as JObject;
                if (init_result["BaseResponse"]["Ret"].ToObject<int>() > 0)
                    throw new Exceptions.LoginRequiredException();

                _syncKey.Clear();
                foreach (JObject synckey in init_result["SyncKey"]["List"])  //同步键值
                {
                    _syncKey.Add(synckey["Key"].ToString(), synckey["Val"].ToString());
                }
                initData = init_result;
                return init_result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 发送图片给指定昵称
        /// </summary>
        /// <param name="nickName">收到消息者的昵称</param>
        /// <param name="imageName">图片名称，必须包含后缀</param>
        /// <param name="stream">图片数据流</param>
        public void SendImageToNickName(string nickName, string imageName, Stream stream)
        {
            SendImageToUserName(toUserName(nickName), imageName, stream);
        }

        public void SendImageToUserName(string userName, string imageName, byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
                SendImageToUserName(userName, imageName, stream);
        }

        /// <summary>
        /// 发图片
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="url">The URL.</param>
        public void SendImageToUserName(string userName, string url)
        {
            try
            {
                var arr = url.Split('/');
                var imageName = arr[arr.Length - 1];
                var data = BaseService.SendGetRequest(url);
                SendImageToUserName(userName, imageName, new MemoryStream(data));
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 发送图片给指定用户
        /// </summary>
        /// <param name="userName">指定用户名</param>
        /// <param name="imageName">图片名称，必须包含后缀</param>
        /// <param name="stream">图片数据流</param>
        private void SendImageToUserName(string userName, string imageName, Stream stream)
        {
            String from = myUserName();
            //Image image = Image.FromStream(stream);
            // 判断它的图片类型，以决定mimeType
            //String mimeType;
            //if (System.Drawing.Imaging.ImageFormat.Jpeg.Equals(image.RawFormat))
            String mimeType = MimeMapping.GetMimeMapping(imageName);
            // 获取miediaId

            string id = "WU_FILE_" + (UploadMediaSerialId++);

            Cookie wdt = BaseService.GetCookie("webwx_data_ticket");
            Cookie sid = BaseService.GetCookie("wxsid");
            Cookie uin = BaseService.GetCookie("wxuin");
            uploadMediaRequestModel uploadmediarequest = new uploadMediaRequestModel();
            uploadmediarequest.BaseRequest = new WxBaseRequestModel()
            {
                Uin = Convert.ToInt64(uin.Value),
                Sid = sid.Value,
                Skey = LoginService.SKey,
                DeviceID = GetDeviceID()
            };
            uploadmediarequest.ClientMediaId = getClientMsgId();
            uploadmediarequest.TotalLen = stream.Length;
            uploadmediarequest.StartPos = 0;
            uploadmediarequest.DataLen = stream.Length;
            uploadmediarequest.MediaType = 4;
            uploadmediarequest.UploadType = 2;
            uploadmediarequest.FromUserName = from;
            uploadmediarequest.ToUserName = userName;
            uploadmediarequest.FileMd5 = MD5(stream);
            stream.Position = 0;

            string udr = JsonConvert.SerializeObject(uploadmediarequest);
            MemoryStream buffer = new MemoryStream();
            StreamWriter writer = new StreamWriter(buffer);
            // 建立边界
            string boundary = "------" + MD5(DateTime.Now.ToString());
            addNormalTextContent(boundary, writer, id, "id");
            addNormalTextContent(boundary, writer, imageName, "name");
            addNormalTextContent(boundary, writer, mimeType, "type");
            addNormalTextContent(boundary, writer, ToGMTFormat(DateTime.Now), "lastModifiedDate");
            addNormalTextContent(boundary, writer, stream.Length.ToString(), "size");
            addNormalTextContent(boundary, writer, "pic", "mediatype");

            addNormalTextContent(boundary, writer, wdt.Value, "webwx_data_ticket");
            addNormalTextContent(boundary, writer, LoginService.Pass_Ticket, "pass_ticket");
            addNormalTextContent(boundary, writer, udr, "uploadmediarequest");
            // 文件上传
            addStreamContent(boundary, writer, buffer, stream, "filename", mimeType, imageName);
            writer.Write("--" + boundary + "--\r\n");
            writer.Flush();

            buffer.Position = 0;
            StreamContent streamContent = new StreamContent(buffer, (int)buffer.Length);
            streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data; boundary=" + boundary);

            string result = BaseService.PostAsyncAsString(GetURLUploadMedia(), streamContent).Result;
            JObject uploadResult = JsonConvert.DeserializeObject(result) as JObject;
            string mediaId = uploadResult["MediaId"].ToString();
            SendPic(mediaId, from, userName);
        }

        private void addStreamContent(string boundary, StreamWriter writer, Stream dist, Stream stream, string name, string mimeType, string fileName)
        {
            writer.Write("--");
            writer.Write(boundary);
            writer.Write('\r');
            writer.Write('\n');
            writer.Write("Content-Disposition: form-data; name=\"");
            writer.Write(name);
            writer.Write("\"; filename=\"");
            writer.Write(fileName);
            writer.Write('\"');
            writer.Write('\r');
            writer.Write('\n');
            writer.Write("Content-Type: ");
            writer.Write(mimeType);
            writer.Write('\r');
            writer.Write('\n');
            writer.Write('\r');
            writer.Write('\n');
            writer.Flush();
            stream.CopyTo(dist);
            dist.Flush();
            writer.Write('\r');
            writer.Write('\n');
        }

        private void addNormalTextContent(string boundary, StreamWriter writer, string text, string name)
        {
            writer.Write("--");
            writer.Write(boundary);
            writer.Write('\r');
            writer.Write('\n');
            writer.Write("Content-Disposition: form-data; name=\"");
            writer.Write(name);
            writer.Write('\"');
            writer.Write('\r');
            writer.Write('\n');
            writer.Write('\r');
            writer.Write('\n');
            writer.Write(text);
            writer.Write('\r');
            writer.Write('\n');
        }


        /// <summary>
        /// 发送文本消息到指定昵称
        /// </summary>
        /// <param name="nickName">收到消息者的昵称</param>
        /// <param name="message">消息内容</param>
        public void SendTextMessageToNickName(string nickName, string message)
        {
            SendTextMessageToUserName(toUserName(nickName), message);
        }

        /// <summary>
        /// 将昵称转变成userName
        /// </summary>
        /// <param name="nickName">昵称</param>
        /// <returns>userName, null 如果找不到这个昵称</returns>
        private string toUserName(string nickName)
        {
            if (contactsList == null)
                GetContact();
            foreach (JObject contact in contactsList["MemberList"])
            {
                if (contact["NickName"].ToString() == nickName)
                {
                    return contact["UserName"].ToString();
                }
            }
            return null;
        }

        /// <summary>
        /// 发送文本消息到指定用户
        /// </summary>
        /// <param name="userName">收到消息者的昵称</param>
        /// <param name="message">消息内容</param>
        public void SendTextMessageToUserName(string userName, string message)
        {
            String from = myUserName();
            SendMsg(message, from, userName, 1);
        }

        private string myUserName()
        {
            if (initData == null)
                WxInit();
            return initData["User"]["UserName"].ToString();
        }

        /// <summary>
        /// 获取好友头像
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Image GetIcon(string username)
        {
            byte[] bytes = BaseService.SendGetRequest(GetURLGetIcon() + username);
            if (bytes != null && bytes.Length > 0)
                return Image.FromStream(new MemoryStream(bytes));
            return null;
        }
        /// <summary>
        /// 获取微信讨论组头像
        /// </summary>
        /// <param name="usename"></param>
        /// <returns></returns>
        public Image GetHeadImg(string usename)
        {
            byte[] bytes = BaseService.SendGetRequest(GetURLGetHeadImg() + usename);

            return Image.FromStream(new MemoryStream(bytes));
        }
        private JObject contactsList;
        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <returns></returns>
        public JObject GetContact()
        {
            byte[] bytes = BaseService.SendGetRequest(GetURLGetContact());
            string contact_str = Encoding.UTF8.GetString(bytes);
            contactsList = JsonConvert.DeserializeObject(contact_str) as JObject;
            return contactsList;
        }
        /// <summary>
        /// 微信同步检测
        /// </summary>
        /// <returns></returns>
        public string WxSyncCheck()
        {
            string sync_key = "";
            foreach (KeyValuePair<string, string> p in _syncKey)
            {
                sync_key += p.Key + "_" + p.Value + "%7C";
            }
            sync_key = sync_key.TrimEnd('%', '7', 'C');

            Cookie sid = BaseService.GetCookie("wxsid");
            Cookie uin = BaseService.GetCookie("wxuin");

            if (sid != null && uin != null)
            {
                _synccheck_url = string.Format(_synccheck_url, sid.Value, uin.Value, sync_key, (long)(DateTime.Now.ToUniversalTime() - new System.DateTime(1970, 1, 1)).TotalMilliseconds, LoginService.SKey.Replace("@", "%40"), "e1615250492");

                byte[] bytes = BaseService.SendGetRequest(_synccheck_url + "&_=" + DateTime.Now.Ticks);
                if (bytes != null)
                {
                    return Encoding.UTF8.GetString(bytes);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 微信同步
        /// </summary>
        /// <returns></returns>
        public JObject WxSync()
        {
            string sync_json = "{{\"BaseRequest\" : {{\"DeviceID\":\"{6}\",\"Sid\":\"{1}\", \"Skey\":\"{5}\", \"Uin\":\"{0}\"}},\"SyncKey\" : {{\"Count\":{2},\"List\":[{3}]}},\"rr\" :{4}}}";
            Cookie sid = BaseService.GetCookie("wxsid");
            Cookie uin = BaseService.GetCookie("wxuin");

            string sync_keys = "";
            foreach (KeyValuePair<string, string> p in _syncKey)
            {
                sync_keys += "{\"Key\":" + p.Key + ",\"Val\":" + p.Value + "},";
            }
            sync_keys = sync_keys.TrimEnd(',');
            sync_json = string.Format(sync_json, uin.Value, sid.Value, _syncKey.Count, sync_keys, (long)(DateTime.Now.ToUniversalTime() - new System.DateTime(1970, 1, 1)).TotalMilliseconds, LoginService.SKey, GetDeviceID());

            if (sid != null && uin != null)
            {
                byte[] bytes = BaseService.SendPostRequest(GetURLSYNC() + sid.Value + "&lang=zh_CN&skey=" + LoginService.SKey + "&pass_ticket=" + LoginService.Pass_Ticket, sync_json);
                string sync_str = Encoding.UTF8.GetString(bytes);

                JObject sync_resul = JsonConvert.DeserializeObject(sync_str) as JObject;

                if (sync_resul["SyncKey"]["Count"].ToString() != "0")
                {
                    _syncKey.Clear();
                    foreach (JObject key in sync_resul["SyncKey"]["List"])
                    {
                        _syncKey.Add(key["Key"].ToString(), key["Val"].ToString());
                    }
                }
                return sync_resul;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="type"></param>
        public void SendMsg(string msg, string from, string to, int type)
        {
            string msg_json = "{{" +
            "\"BaseRequest\":{{" +
                "\"DeviceID\" : \"{10}\"," +
                "\"Sid\" : \"{0}\"," +
                "\"Skey\" : \"{6}\"," +
                "\"Uin\" : \"{1}\"" +
            "}}," +
            "\"Msg\" : {{" +
                "\"ClientMsgId\" : {8}," +
                "\"Content\" : \"{2}\"," +
                "\"FromUserName\" : \"{3}\"," +
                "\"LocalID\" : {9}," +
                "\"ToUserName\" : \"{4}\"," +
                "\"Type\" : {5}" +
            "}}," +
            "\"rr\" : {7}" +
            "}}";

            Cookie sid = BaseService.GetCookie("wxsid");
            Cookie uin = BaseService.GetCookie("wxuin");

            if (sid != null && uin != null)
            {
                msg_json = string.Format(msg_json, sid.Value, uin.Value, msg, from, to, type, LoginService.SKey, DateTime.Now.Millisecond, DateTime.Now.Millisecond, DateTime.Now.Millisecond, GetDeviceID());

                byte[] bytes = BaseService.SendPostRequest(GetURLSendMessage() + sid.Value + "&lang=zh_CN&pass_ticket=" + LoginService.Pass_Ticket, msg_json);

                string send_result = Encoding.UTF8.GetString(bytes);
            }
        }





        /// <summary>
        /// 发图片
        /// </summary>
        /// <param name="mediaId">The media identifier.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public void SendPic(string mediaId, string from, string to)
        {
            string msg_json = "{{" +
            "\"BaseRequest\":{{" +
                "\"DeviceID\" : \"{0}\"," +
                "\"Sid\" : \"{1}\"," +
                "\"Skey\" : \"{2}\"," +
                "\"Uin\" : \"{3}\"" +
            "}}," +
            "\"Scene\":0," +
            "\"Msg\" : {{" +
                "\"Type\" : {4}," +
                "\"MediaId\" : \"{5}\"," +
                "\"FromUserName\" : \"{6}\"," +
                "\"LocalID\" : {7}," +
                "\"ToUserName\" : \"{8}\"," +
                "\"ClientMsgId\" : {9}" +
            "}}" +
            "}}";

            Cookie sid = BaseService.GetCookie("wxsid");
            Cookie uin = BaseService.GetCookie("wxuin");
            
            if (sid != null && uin != null)
            {
                msg_json = string.Format(msg_json, GetDeviceID(), sid.Value, LoginService.SKey, uin.Value, 3, mediaId, from, getClientMsgId(), to, getClientMsgId());

                byte[] bytes = BaseService.SendPostRequest(GetURLSendMediaMessage() + "&pass_ticket=" + LoginService.Pass_Ticket, msg_json);

                string send_result = Encoding.UTF8.GetString(bytes);
            }
        }




        /// <summary>
        /// 群里踢人
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public void DeleteChatroom(string from, string to)
        {
            string msg_json = "{{" +
            "\"BaseRequest\":{{" +
                "\"DeviceID\" : \"e441551176\"," +
                "\"Sid\" : \"{0}\"," +
                "\"Skey\" : \"{4}\"," +
                "\"Uin\" : \"{1}\"" +
            "}}," +
            "\"ChatRoomName\" : {2}," +
            "\"DelMemberList\" : {3}," +
            "\"rr\" : {5}" +
            "}}";

            Cookie sid = BaseService.GetCookie("wxsid");
            Cookie uin = BaseService.GetCookie("wxuin");

            if (sid != null && uin != null)
            {
                msg_json = string.Format(msg_json, sid.Value, uin.Value, from, to, LoginService.SKey, DateTime.Now.Millisecond);
                long r = (long)(DateTime.Now.ToUniversalTime() - new System.DateTime(1970, 1, 1)).TotalMilliseconds;
                byte[] bytes = BaseService.SendPostRequest(GetURLCreateChatRoom() + "?lang=zh_CN&pass_ticket=" + LoginService.Pass_Ticket + "&r=" + r, msg_json);

                string send_result = Encoding.UTF8.GetString(bytes);
            }
        }

        /// <summary>
        /// 图片转为Byte字节数组
        /// </summary>
        /// <param name="FilePath">路径</param>
        /// <returns>字节数组</returns>
        private byte[] imageToByteArray(string FilePath)
        {
            using (MemoryStream ms = new MemoryStream())
            {

                using (Image imageIn = Image.FromFile(FilePath))
                {

                    using (Bitmap bmp = new Bitmap(imageIn))
                    {
                        bmp.Save(ms, imageIn.RawFormat);
                    }

                }
                return ms.ToArray();
            }
        }

        /// <summary>  
        /// 本地时间转成GMT格式的时间  
        /// </summary>  
        public static string ToGMTFormat(DateTime dt)
        {
            return dt.ToString("r") + dt.ToString("zzz").Replace(":", "");
        }

        /// <summary>
        /// 生成指定位随机数
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string CreateCheckCodeWithNum(int n)
        {
            char[] CharArray = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            string sCode = "";
            Random random = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < n; i++)
            {
                sCode += CharArray[random.Next(CharArray.Length)];
            }
            return sCode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>获取device ID 在一个生命周期 它是固定的</returns>
        public static string GetDeviceID()
        {
            if (DeviceID != null)
                return DeviceID;
            DeviceID = "e" + CreateCheckCodeWithNum(15);
            return DeviceID;
        }


        public static long getClientMsgId()
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(DateTime.Now - startTime).TotalMilliseconds;
        }

        public static string MD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] result = md5.ComputeHash(data);
            String ret = "";
            for (int i = 0; i < result.Length; i++)
                ret += result[i].ToString("x").PadLeft(2, '0');
            return ret;
        }
        public static string MD5(Stream stream)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(stream);
            String ret = "";
            for (int i = 0; i < result.Length; i++)
                ret += result[i].ToString("x").PadLeft(2, '0');
            return ret;
        }
    }
}
