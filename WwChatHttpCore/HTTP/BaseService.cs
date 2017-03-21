using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

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
        public static readonly CookieContainer CookiesContainer = new CookieContainer();
        private static readonly HttpClientHandler clientHandler = new HttpClientHandler();
        /// <summary>
        /// HTTP请求发送者
        /// </summary>
        private static readonly HttpClient client = new HttpClient(clientHandler);

        static BaseService()
        {
            clientHandler.CookieContainer = CookiesContainer;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(ProductHeaderValue.Parse("Mozilla/5.0")));//  (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36
        }

        /// <summary>
        /// 向服务器发送get请求  返回服务器回复数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static byte[] SendGetRequest(string url)
        {
            try
            {
                return client.GetByteArrayAsync(url).Result;
            }
            catch(Exception ex)
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
                ByteArrayContent content = new ByteArrayContent(request_body);
                return client.PostAsync(url, content).Result.Content.ReadAsByteArrayAsync().Result;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// 以异步方式Post一个正文到指定 URL，并且将其结果串化返回
        /// </summary>
        /// <param name="url">POST目标</param>
        /// <param name="content">请求正文</param>
        /// <returns>串化结果</returns>
        public static Task<string> PostAsyncAsString(string url, HttpContent content)
        {
            HttpClient postClient = new HttpClient();
            postClient.DefaultRequestHeaders.Add("Origin", "https://" + WXService.WeixinRouteHost);
            postClient.DefaultRequestHeaders.Referrer = new Uri("https://" + WXService.WeixinRouteHost + "/");
            return postClient.PostAsync(url, content).Result.Content.ReadAsStringAsync();
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

        public static List<Cookie> GetAllCookies(CookieContainer cc)
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
