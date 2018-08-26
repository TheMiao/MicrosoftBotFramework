using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace NewElmeAPI
{

    public class WinHttp
    {
        private ComObject HttpObj;
        private string contentType;
        private int contentLength;
        private bool active;
        private ArrayList PostDataList;//提交的数据字段 
        private bool isutf8;
        public WinHttp()
        {
            //构建WinHttp对象  
            HttpObj = new ComObject("WinHttp.WinHttpRequest.5.1");
            contentType = "application/x-www-form-urlencoded";
            contentLength = 0;
            PostDataList = new System.Collections.ArrayList();
        }

        /// <summary>
        /// HttpPost请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="dictHead"></param>
        /// <returns></returns>
        public static string HttpPost(string url, string postData, Dictionary<string, string> dictHead)
        {
            WinHttp winHttp = new WinHttp();
            winHttp.ContentType = "Content-Type: application/json";
            winHttp.Open("POST", url, false);

            if (dictHead != null && dictHead.Count > 0)
            {
                foreach (string key in dictHead.Keys)
                {
                    winHttp.SetRequestHeader(key, dictHead[key]);
                }
            }

            string ret = winHttp.Send(postData);
            winHttp.ClearPostData();
            return utf8_gb2312(winHttp.ResponseBody);
        }


        public static string OAuthHeader(string apiKey, string nonce, string timeStamp, string sig, string accessToken, )
        {
            //构造OAuth头部 
            StringBuilder oauthHeader = new StringBuilder();
            oauthHeader.AppendFormat("OAuth realm=\"\", oauth_consumer_key={0}, ", apiKey);
            oauthHeader.AppendFormat("oauth_nonce={0}, ", nonce);
            oauthHeader.AppendFormat("oauth_timestamp={0}, ", timeStamp);
            oauthHeader.AppendFormat("oauth_signature_method={0}, ", "HMAC-SHA1");
            oauthHeader.AppendFormat("oauth_version={0}, ", "1.0");
            oauthHeader.AppendFormat("oauth_signature={0}, ", sig);
            oauthHeader.AppendFormat("oauth_token={0}", accessToken);

            //构造请求 
            StringBuilder requestBody = new StringBuilder("");
            Encoding encoding = Encoding.GetEncoding("utf-8");
            byte[] data = encoding.GetBytes(requestBody.ToString());

            // Http Request的设置 
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Headers.Set("Authorization", oauthHeader.ToString());
            //request.Headers.Add("Authorization", authorization); 
            request.ContentType = "application/atom+xml";
            request.Method = "GET";
            return request;
        }

        /// <summary>
        /// UTF8转换成GB2312
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string utf8_gb2312(string text)
        {
            //声明字符集   
            Encoding utf8, gb2312;
            //utf8   
            utf8 = Encoding.GetEncoding("utf-8");
            //gb2312   
            gb2312 = Encoding.GetEncoding("gb2312");
            byte[] utf;
            utf = utf8.GetBytes(text);
            utf = Encoding.Convert(utf8, gb2312, utf);
            //返回转换后的字符   
            return gb2312.GetString(utf);
        }
        /// <summary>  
        /// 获取当前时间戳  
        /// </summary>  
        /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.</param>  
        /// <returns></returns>  
        public static string GetTimeStamp(bool bflag = true)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string ret = string.Empty;
            if (bflag)
                ret = Convert.ToInt64(ts.TotalSeconds).ToString();
            else
                ret = Convert.ToInt64(ts.TotalMilliseconds).ToString();

            return ret;
        }

        /// <summary>
        /// Base64编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64(string str)
        {
            System.Text.Encoding encode = System.Text.Encoding.ASCII;
            byte[] bytedata = encode.GetBytes(str);
            string strPath = Convert.ToBase64String(bytedata, 0, bytedata.Length);
            return strPath;
        }

        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string DeBase64(string input)
        {
            byte[] bpath = Convert.FromBase64String(input);
            string ret = System.Text.ASCIIEncoding.Default.GetString(bpath);
            return ret;
        }

        /// <summary>
        /// 取MD5
        /// </summary>
        /// <param name="sDataIn"></param>
        /// <returns></returns>
        public static string GetMD5(string sDataIn)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytValue, bytHash;
            bytValue = System.Text.Encoding.UTF8.GetBytes(sDataIn);
            bytHash = md5.ComputeHash(bytValue);
            md5.Clear();
            string sTemp = "";
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
            }
            return sTemp.ToLower();
        }

        //提交参数信息的个数  
        public int PostDataCount
        {
            get { return PostDataList.Count; }
        }

        //设置Content-Type属性  
        public string ContentType
        {
            get { return contentType; }
            set
            {
                if (!active) contentType = value;
                else if (contentType.CompareTo(value) != 0)
                {
                    contentType = value;
                    SetRequestHeader("Content-Type", contentType);
                }
            }
        }

        //对象是否是打开状态  
        public bool Active
        {
            get { return active; }
        }

        //设置Send数据的长度   
        public int ContentLength
        {
            get { return contentLength; }
            set
            {
                if (!active) contentLength = value;
                else if (contentLength != value)
                {
                    contentLength = value;
                    HttpObj.DoMethod("SetRequestHeader", new object[2] { "Content-Length", value });
                }
            }
        }

        //执行之后返回的结果  
        public string ResponseBody
        {
            get
            {
                if (active)
                {
                    ComObject AdoStream = new ComObject("Adodb.Stream");
                    AdoStream["Type"] = 1;
                    AdoStream["Mode"] = 3;
                    AdoStream.DoMethod("Open", new object[] { });
                    AdoStream.DoMethod("Write", new object[1] { HttpObj["ResponseBody"] });
                    AdoStream["Position"] = 0;
                    AdoStream["Type"] = 2;
                    AdoStream["Charset"] = "UTF-8";
                    return AdoStream["ReadText"].ToString();
                }
                else return "";
            }
        }

        //设定请求头  
        public string SetRequestHeader(string Header, object Value)
        {
            object obj;
            obj = HttpObj.DoMethod("SetRequestHeader", new object[] { Header, Value });
            if (obj != null) return obj.ToString();
            else return "True";
        }

        //打开URL执行OpenMethod方法,Async指定是否采用异步方式调用,异步方式不会阻塞  
        public string Open(string OpenMethod, string URL, bool Async)
        {
            object obj;
            obj = HttpObj.DoMethod("Open", new object[] { OpenMethod, URL, Async });
            if (obj != null)
            {
                active = false;
                return obj.ToString();
            }
            else
            {
                SetRequestHeader("Content-Type", contentType);
                SetRequestHeader("User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)");
                if (contentLength != 0) SetRequestHeader("Content-Length", contentLength);
                active = true;
                return "True";
            }
        }

        //发送数据  
        public string Send(string body)
        {
            if (!active) return "False";
            object obj;
            obj = HttpObj.DoMethod("Send", new object[1] { body });
            if (obj != null) return obj.ToString();
            else return "True";
        }

        //清空提交信息  
        public void ClearPostData()
        {
            this.PostDataList.Clear();
        }

        //增加提交数据信息  
        public void AddPostField(string FieldName, object Value)
        {
            this.PostDataList.Add(FieldName + "=" + Value.ToString());
        }

        //通过参数指定提交  
        public string Post()
        {
            if (!active)
            {
                return "False";
            }
            string st = "";
            for (int i = 0; i < this.PostDataList.Count; i++)
            {
                if (st != "") st = st + "&" + PostDataList[i].ToString();
                else st = PostDataList[i].ToString();
            }
            this.ContentLength = st.Length;
            return Send(st);
        }


        //设置等待超时等  
        public string SetTimeouts(long ResolveTimeout, long ConnectTimeout, long SendTimeout, long ReceiveTimeout)
        {
            object obj;
            obj = HttpObj.DoMethod("SetTimeouts", new object[4] { ResolveTimeout, ConnectTimeout, SendTimeout, ReceiveTimeout });
            if (obj != null) return obj.ToString();
            else return "True";
        }
        //等待数据提交完成  
        public string WaitForResponse(object Timeout, out bool Succeeded)
        {
            if (!active) { Succeeded = false; return ""; }
            object obj;
            bool succ;
            succ = false;
            System.Reflection.ParameterModifier[] ParamesM;
            ParamesM = new System.Reflection.ParameterModifier[1];
            ParamesM[0] = new System.Reflection.ParameterModifier(2); // 初始化为接口参数的个数  
            ParamesM[0][1] = true; // 设置第二个参数为返回参数  

            //ParamesM[1] = true;  
            object[] ParamArray = new object[2] { Timeout, succ };
            obj = HttpObj.DoMethod("WaitForResponse", ParamArray, ParamesM);

            Succeeded = bool.Parse(ParamArray[1].ToString());
            //Succeeded = bool.Parse(ParamArray[1].ToString);  
            if (obj != null) { return obj.ToString(); }
            else return "True";
        }

        public string GetResponseHeader(string Header, ref string Value)
        {
            if (!active) { Value = ""; return ""; }
            object obj;
            /*string str; 
            str = ""; 
            System.Reflection.ParameterModifier[] Parames; 
            Parames = new System.Reflection.ParameterModifier[1]; 
            Parames[0] = new System.Reflection.ParameterModifier (2); // 初始化为接口参数的个数 
            Parames[0][1] = true; */
            // 设置第二个参数为返回参数  
            obj = HttpObj.DoMethod("GetResponseHeader", new object[2] { Header, Value });
            //Value = str;  
            if (obj != null) { return obj.ToString(); }
            else return "True";
        }

        public string GetAllResponseHeaders()
        {
            object obj;
            obj = HttpObj["GetAllResponseHeaders"];
            if (obj != null) { return obj.ToString(); }
            else return "True";
        }

    }

    /// <summary>  
    /// COM对象的后期绑定调用类库    
    /// </summary>  
    public class ComObject
    {
        private System.Type _ObjType;
        private object ComInstance;
        /*public DxComObject() 
        { 
            throw new 
        }*/
        public ComObject(string ComName)
        {
            //根据COM对象的名称创建COM对象  
            _ObjType = System.Type.GetTypeFromProgID(ComName);
            if (_ObjType == null)
                throw new Exception("指定的COM对象名称无效");
            ComInstance = System.Activator.CreateInstance(_ObjType);
        }

        public System.Type ComType
        {
            get { return _ObjType; }
        }

        //执行的函数  
        public object DoMethod(string MethodName, object[] args)
        {
            return ComType.InvokeMember(MethodName, System.Reflection.BindingFlags.InvokeMethod, null, ComInstance, args);
        }

        public object DoMethod(string MethodName, object[] args, System.Reflection.ParameterModifier[] ParamMods)
        {
            return ComType.InvokeMember(MethodName, System.Reflection.BindingFlags.InvokeMethod, null, ComInstance, args, ParamMods, null, null);
        }
        //获得属性与设置属性  
        public object this[string propName]
        {
            get
            {
                return _ObjType.InvokeMember(propName, System.Reflection.BindingFlags.GetProperty, null, ComInstance, null);
            }
            set
            {
                _ObjType.InvokeMember(propName, System.Reflection.BindingFlags.SetProperty, null, ComInstance, new object[] { value });
            }
        }
    }

}
