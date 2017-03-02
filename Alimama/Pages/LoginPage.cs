using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alimama.Pages
{
    public class LoginPage
    {
        internal static bool IsQROpen(WebBrowser webBrowser1)
        {
            HtmlElement ele = AllPage.FindElementByClassName(webBrowser1, "qrcode-main");
            return AllPage.isElementVisible(webBrowser1, ele);
        }
    }
}
