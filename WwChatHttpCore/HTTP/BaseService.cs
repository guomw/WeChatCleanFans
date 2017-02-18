using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;

namespace WwChatHttpCore.HTTP
{
    /// <summary>
    /// 访问http服务器类
    /// </summary>
    public static class BaseService
    {
        /// <summary>
        /// 访问服务器时的cookies
        /// </summary>
        public static CookieContainer CookiesContainer;
        /// <summary>
        /// 向服务器发送get请求  返回服务器回复数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static byte[] SendGetRequest(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "get";

                if (CookiesContainer == null)
                {
                    CookiesContainer = new CookieContainer();
                }

                request.CookieContainer = CookiesContainer;  //启用cookie

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream response_stream = response.GetResponseStream();

                int count = (int)response.ContentLength;
                int offset = 0;
                byte[] buf = new byte[count];
                while (count > 0)  //读取返回数据
                {
                    int n = response_stream.Read(buf, offset, count);
                    if (n == 0) break;
                    count -= n;
                    offset += n;
                }
                return buf;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 向服务器发送post请求 返回服务器回复数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static byte[] SendPostRequest(string url, string body)
        {
            try
            {
                byte[] request_body = Encoding.UTF8.GetBytes(body);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "post";
                request.ContentLength = request_body.Length;

                Stream request_stream = request.GetRequestStream();

                request_stream.Write(request_body, 0, request_body.Length);

                if (CookiesContainer == null)
                {
                    CookiesContainer = new CookieContainer();
                }
                request.CookieContainer = CookiesContainer;  //启用cookie

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream response_stream = response.GetResponseStream();

                int count = (int)response.ContentLength;
                int offset = 0;
                byte[] buf = new byte[count];
                while (count > 0)  //读取返回数据
                {
                    int n = response_stream.Read(buf, offset, count);
                    if (n == 0) break;
                    count -= n;
                    offset += n;
                }
                return buf;
            }
            catch
            {
                return null;
            }
        }


        public static byte[] GetImageStream(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "get";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream response_stream = response.GetResponseStream();

                int count = (int)response.ContentLength;
                int offset = 0;
                byte[] buf = new byte[count];
                while (count > 0)  //读取返回数据
                {
                    int n = response_stream.Read(buf, offset, count);
                    if (n == 0) break;
                    count -= n;
                    offset += n;
                }
                return buf;
            }
            catch
            {
                return null;
            }
        }


        public static byte[] SendPostRequest(string url, byte[] request_body, WebHeaderCollection header)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "post";
                //if(header!=null)
                //{
                //    request.Headers = header;
                //}
                // 边界符
                var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
                //结尾
                byte[] foot_data = Encoding.UTF8.GetBytes("\r\n" + boundary + "--");

                request.ContentType = "multipart/form-data; boundary=" + boundary;

                //request.Headers.Add("", "");


                StringBuilder sb2 = new StringBuilder();
                foreach (var key in header.AllKeys)
                {
                    if (key != "filename")
                    {
                        sb2.Append("\r\n" + boundary + "\r\n");
                        sb2.Append("Content-Disposition: form-data; name=\"" + key + "\"\r\n\r\n\"" + header.Get(key) + "\"");
                    }
                    else
                    {
                        sb2.Append("\r\n" + boundary + "\r\n");
                        sb2.Append("Content-Disposition: form-data; name=\"" + key + "\"\r\n; filename=\"" + header.Get(key) + "\"\r\n");
                        sb2.Append("Content-Type:image/jpeg\r\n\r\n");
                    }

                }
                byte[] form_data = Encoding.UTF8.GetBytes(sb2.ToString());
                request.ContentLength = form_data.Length + request_body.Length + foot_data.Length;

                Stream request_stream = request.GetRequestStream();

                request_stream.Write(form_data, 0, form_data.Length);

                request_stream.Write(request_body, 0, request_body.Length);
                ////结尾
                request_stream.Write(foot_data, 0, foot_data.Length);

                if (CookiesContainer == null)
                {
                    CookiesContainer = new CookieContainer();
                }
                request.CookieContainer = CookiesContainer;  //启用cookie

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream response_stream = response.GetResponseStream();

                StreamReader sr = new StreamReader(response_stream, Encoding.UTF8);
                string responseContent = sr.ReadToEnd().Trim();

                int count = (int)response.ContentLength;
                int offset = 0;
                byte[] buf = new byte[count];
                while (count > 0)  //读取返回数据
                {
                    int n = response_stream.Read(buf, offset, count);
                    if (n == 0) break;
                    count -= n;
                    offset += n;
                }
                return buf;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static byte[] MediaUpload(string url, string fileName, byte[] buffer, string contentType)
        {
            try
            {
                //BinaryReader br = new BinaryReader(fileStream);
                //                byte[] buffer = br.ReadBytes(Convert.ToInt32(fileStream.Length));

                byte[] request_body = Encoding.UTF8.GetBytes(contentType);

                var webRequest = WebRequest.Create(url);

                // 边界符
                var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
                // 最后的结束符
                //结尾
                byte[] foot_data = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                // 设置属性
                webRequest.Method = "POST";
                webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
                StringBuilder sb = new StringBuilder();
                sb.Append("--" + boundary + "\r\n");
                sb.Append("Content-Disposition: form-data; name=\"filename\"; filename=\"" + fileName + "\"; filelength=\"" + buffer.Length + "\"");
                sb.Append("\r\n");
                sb.Append("Content-Type:image/jpeg");
                sb.Append("\r\n\r\n");
                string head = sb.ToString();
                byte[] form_data = Encoding.UTF8.GetBytes(head);

                //post总长度
                long length = request_body.Length + form_data.Length + buffer.Length + foot_data.Length;

                webRequest.ContentLength = length;

                var requestStream = webRequest.GetRequestStream();
                //这里要注意一下发送顺序，先发送form_data > buffer > foot_data
                //发送表单参数
                requestStream.Write(request_body, 0, request_body.Length);
                requestStream.Write(form_data, 0, form_data.Length);
                //发送文件内容
                requestStream.Write(buffer, 0, buffer.Length);
                //结尾
                requestStream.Write(foot_data, 0, foot_data.Length);

                requestStream.Close();
                requestStream.Dispose();

                //fileStream.Close();
                //fileStream.Dispose();
                //br.Close();
                //br.Dispose();
                WebResponse pos = webRequest.GetResponse();

                //Stream response_stream = pos.GetResponseStream();
                //int count = (int)pos.ContentLength;
                //int offset = 0;
                //byte[] buf = new byte[count];
                //while (count > 0)  //读取返回数据
                //{
                //    int n = response_stream.Read(buf, offset, count);
                //    if (n == 0) break;
                //    count -= n;
                //    offset += n;
                //}
                //return buf;
                StreamReader sr = new StreamReader(pos.GetResponseStream(), Encoding.UTF8);
                string responseContent = sr.ReadToEnd().Trim();
                sr.Close();
                sr.Dispose();
                if (pos != null)
                {
                    pos.Close();
                    pos = null;
                }
                if (webRequest != null)
                    webRequest = null;

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// 获取指定cookie
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Cookie GetCookie(string name)
        {
            List<Cookie> cookies = GetAllCookies(CookiesContainer);
            foreach (Cookie c in cookies)
            {
                if (c.Name == name)
                {
                    return c;
                }
            }
            return null;
        }

        private static List<Cookie> GetAllCookies(CookieContainer cc)
        {
            List<Cookie> lstCookies = new List<Cookie>();

            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.Instance, null, cc, new object[] { });

            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField
                    | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c in colCookies) lstCookies.Add(c);
            }
            return lstCookies;
        }
    }
}
