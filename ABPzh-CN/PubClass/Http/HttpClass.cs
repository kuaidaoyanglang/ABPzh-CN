using CsharpHttpHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ABPzh_CN.PubClass.Http
{
    public static class HttpClass
    {
        public static string MD5Encrypt(string str)
        {

            byte[] buffer = Encoding.Default.GetBytes(str); //将字符串解析成字节数组，随便按照哪种解析格式都行

            MD5 md5 = MD5.Create();  //使用MD5这个抽象类的Creat()方法创建一个虚拟的MD5类的对象。

            byte[] bufferNew = md5.ComputeHash(buffer); //使用MD5实例的ComputerHash()方法处理字节数组。

            string strNew = null;

            for (int i = 0; i < bufferNew.Length; i++)

            {

                strNew += bufferNew[i].ToString("x2");  //对bufferNew字节数组中的每个元素进行十六进制转换然后拼接成strNew字符串

            }
            return strNew;
        }

        //百度开发者中心免费申请
        const string appId = "你申请到的appId";
        const string key = "你申请到的key";

        public static string GetEntoCn(string en)
        {
            string text = "";
            CsharpHttpHelper.HttpHelper httpHelper = new CsharpHttpHelper.HttpHelper();
            HttpItem httpItem = new HttpItem();
            httpItem.URL = "http://api.fanyi.baidu.com/api/trans/vip/translate";
            httpItem.Method = "POST";
            httpItem.ContentType = httpItem.postContentType;
            long salt = DateTime.Now.ToBinary();
            //appid+q+salt+密钥
            string sign = MD5Encrypt($"{appId}{en}{salt}{key}");
            httpItem.Postdata = string.Format($"from=en&to=zh&q={CsharpHttpHelper.HttpHelper.URLEncode(en, Encoding.UTF8)}&appid=20180727000189556&salt={salt}&sign={sign}");
            HttpResult html = httpHelper.GetHtml(httpItem);
            bool flag = html.StatusCode == HttpStatusCode.OK;
            if (flag)
            {
                string html2 = html.Html;
                Rootobject obj = JsonConvert.DeserializeObject<Rootobject>(html2);
                return obj.trans_result[0].dst;
            }
            return text;

        }


        public static string ToGB2312(string str)
        {
            string text = "";
            MatchCollection matchCollection = Regex.Matches(str, "\\\\u([\\w]{2})([\\w]{2})", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            byte[] array = new byte[2];
            foreach (object obj in matchCollection)
            {
                Match match = (Match)obj;
                array[0] = (byte)int.Parse(match.Groups[2].Value, NumberStyles.HexNumber);
                array[1] = (byte)int.Parse(match.Groups[1].Value, NumberStyles.HexNumber);
                text += Encoding.Unicode.GetString(array);
            }
            return text;
        }
    }

    public class Rootobject
    {
        public string from { get; set; }
        public string to { get; set; }
        public Trans_Result[] trans_result { get; set; }
    }

    public class Trans_Result
    {
        public string src { get; set; }
        public string dst { get; set; }
    }
}
