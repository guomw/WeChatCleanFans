using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alimama.Pages
{
    public class AllPage
    {
        public static HtmlElement FindElementByClassName(WebBrowser browser,string name)
        {
            IEnumerable<HtmlElement> collection = FindElementsByClassName(browser, name);
            HtmlElement element =  collection.FirstOrDefault();
            if (element == null)
            {
                Console.Error.WriteLine(name);
                StreamReader reader = new StreamReader(browser.DocumentStream);
                Console.Error.WriteLine(reader.ReadToEnd());
                //System.Console.Error.
                throw new IndexOutOfRangeException(name + " has no results");
            }
            return element;
        }

        private static IEnumerable<HtmlElement> FindElementsByClassName(WebBrowser browser, string name)
        {
            List<HtmlElement> list = new List<HtmlElement>();
            foreach(HtmlElement element in browser.Document.All)
            {
                if (element.GetAttribute("class").Split(' ').Contains(name))
                    list.Add(element);
            }
            return list;
        }

        public static bool isElementVisible(WebBrowser web, HtmlElement element)
        {

            //var element = web.Document.All[elementID];

            //if (element == null)
            //    throw new ArgumentException(elementID + " did not return an object from the webbrowser");

            // Calculate the offset of the element, all the way up through the parent nodes
            var parent = element.OffsetParent;
            int xoff = element.OffsetRectangle.X;
            int yoff = element.OffsetRectangle.Y;

            while (parent != null)
            {
                xoff += parent.OffsetRectangle.X;
                yoff += parent.OffsetRectangle.Y;
                parent = parent.OffsetParent;
            }

            // Get the scrollbar offsets
            int scrollBarYPosition = web.Document.GetElementsByTagName("HTML")[0].ScrollTop;
            int scrollBarXPosition = web.Document.GetElementsByTagName("HTML")[0].ScrollLeft;

            // Calculate the visible page space
            Rectangle visibleWindow = new Rectangle(scrollBarXPosition, scrollBarYPosition, web.Width, web.Height);

            // Calculate the visible area of the element
            Rectangle elementWindow = new Rectangle(xoff, yoff, element.ClientRectangle.Width, element.ClientRectangle.Height);

            if (visibleWindow.IntersectsWith(elementWindow))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
