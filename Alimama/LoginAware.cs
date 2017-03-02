using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Alimama
{
    public interface LoginAware
    {
        /// <summary>
        /// 登录成功的回调；调用线程为主UI线程
        /// 参考 https://huobanplus.quip.com/UD25AZhAQhaG
        /// </summary>
        /// <param name="jsons">可分享的cookies</param>
        /// <returns>指示是否应当关闭登录窗体；true则关闭</returns>
        bool success(JArray jsons);
    }
}
