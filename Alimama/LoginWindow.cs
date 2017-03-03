using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alimama
{
    /// <summary>
    /// 可以使用<see cref="LoginAware"/>该接口作为结果回调获取者
    /// </summary>
    public partial class LoginWindow : Form
    {
        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetCookieEx(
        string url,
        string cookieName,
        StringBuilder cookieData,
        ref int size,
        Int32 dwFlags,
        IntPtr lpReserved);
        private const Int32 InternetCookieHttponly = 0x2000;
        /// <summary>
        /// Gets the URI cookie container.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static CookieContainer GetUriCookieContainer(Uri uri)
        {
            CookieContainer cookies = null;
            // Determine the size of the cookie
            int datasize = 8192 * 16;
            StringBuilder cookieData = new StringBuilder(datasize);
            if (!InternetGetCookieEx(uri.ToString(), null, cookieData, ref datasize, InternetCookieHttponly, IntPtr.Zero))
            {
                if (datasize < 0)
                    return null;
                // Allocate stringbuilder large enough to hold the cookie
                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookieEx(
                    uri.ToString(),
                    null, cookieData,
                    ref datasize,
                    InternetCookieHttponly,
                    IntPtr.Zero))
                    return null;
            }
            if (cookieData.Length > 0)
            {
                cookies = new CookieContainer();
                cookies.SetCookies(uri, cookieData.ToString().Replace(';', ','));
            }
            return cookies;
        }

        private readonly LoginAware aware;

        public LoginWindow(LoginAware aware)
        {
            this.aware = aware;
            InitializeComponent();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //webBrowser1.Script
            //Console.WriteLine(webBrowser1.DocumentTitle);
            if (webBrowser1.DocumentTitle.StartsWith("淘宝联盟"))
            {
                button1.Enabled = true;
            }else
            {
                button1.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CookieContainer cookies = GetUriCookieContainer(webBrowser1.Url);
            JArray jsons = new JArray();
            foreach(Cookie cookie in cookies.GetCookies(webBrowser1.Url))
            {
                //Console.WriteLine(cookie.Name+":"+cookie.Value);
                JObject json = new JObject();
                json["name"] = cookie.Name;
                json["path"] = cookie.Path;
                json["domain"] = cookie.Domain;
                json["value"] = cookie.Value;
                jsons.Add(json);
            }

            if (aware.success(jsons))
            {
                this.Close();
            }
        }
    }
}
