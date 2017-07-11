using System;
using System.Net.Http;
using System.Web;
using System.Threading.Tasks;
using AirXSDKBase.Common;
using AirXSDKBase.Model;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace AirXSDKBase
{
    public class SDKOption
    {
        public string SecretId { get; set; }
        public string SecretKey { get; set; }
        /// <summary>
        /// 访问的域名
        /// </summary>
        /// <returns></returns>
        public string Domain { get; set; }
        /// <summary>
        /// 是否启用https
        /// </summary>
        /// <returns></returns>
        public bool Secure { get; set; }
        public int Port { get; set; }=-1;
    }
    public class BaseRequest
    {
        public SDKOption SDKOption { get; set; }
        Signature _signature;
        HttpClient _httpclient;
        public SignatureOption SignatureOption { get; set; }
        public BaseRequest(SDKOption SDKOption)
        {
            this.SignatureOption = new SignatureOption { SecretId = SDKOption.SecretId, SecretKey = SDKOption.SecretKey };
            _httpclient = new HttpClient();
            _signature = new Signature(SignatureOption);
            this.SDKOption = SDKOption;
        }
        private UriBuilder CreateUriBuilder(string path)
        {
            var result = new UriBuilder()
            {
                Scheme = SDKOption.Secure ? "https" : "http",
                Host = SDKOption.Domain,
                Path = path
            };
            if (SDKOption.Port != -1)
            {
                result.Port = SDKOption.Port;
            }
            return result;
        }
        public async Task<string> GET(string path, object obj)
        {
            var _uri = CreateUriBuilder(path);
            StringBuilder sb = new StringBuilder();
            sb.Append("GET");
            sb.Append(_uri.Host);
            sb.Append(_uri.Path);
            sb.Append("?");
            string querystring = $@"{CommonTools.ModelToQueryString(new CommonParameterModel
            {
                Nonce = CommonTools.Nonce(),
                SecretId = this.SignatureOption.SecretId,
                SignatureMethod = this.SignatureOption.SignatureMethod.ToString(),
                Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString()
            })}&{CommonTools.ModelToQueryStringSort(obj)}";
            sb.Append(querystring);
            var query = HttpUtility.ParseQueryString(querystring);
            query["Signature"] = _signature.Compute(sb.ToString());
            _uri.Query = query.ToString();
            Console.WriteLine(_uri.Uri.ToString());
            var result = await _httpclient.GetAsync(_uri.Uri);
            return await result.Content.ReadAsStringAsync();
        }
        public async Task<string> POST(string path, object obj)
        {
            var _uri = CreateUriBuilder(path);
            StringBuilder sb = new StringBuilder();
            sb.Append("POST");
            sb.Append(_uri.Host);
            sb.Append(_uri.Path);
            sb.Append("?");
            Console.WriteLine(_uri.Uri.ToString());
            var smodel = new CommonParameterModel
            {
                Nonce = CommonTools.Nonce(),
                SecretId = this.SignatureOption.SecretId,
                SignatureMethod = this.SignatureOption.SignatureMethod.ToString(),
                Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString()
            };
            string querystring = $@"{CommonTools.ModelToQueryString(smodel)}&{CommonTools.ModelToQueryStringSort(obj)}";
            sb.Append(querystring);
            var Signature = _signature.Compute(sb.ToString());
            var bodylist = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Nonce",smodel.Nonce),
                new KeyValuePair<string, string>("SecretId",smodel.SecretId),
                new KeyValuePair<string, string>("SignatureMethod",smodel.SignatureMethod),
                new KeyValuePair<string, string>("Timestamp",smodel.Timestamp),
                new KeyValuePair<string, string>("Signature",Signature)
            };
            bodylist.AddRange(CommonTools.ModelToKeyValueSort(obj));
            Console.WriteLine(new FormUrlEncodedContent(bodylist));
            var result = await _httpclient.PostAsync(_uri.Uri, new FormUrlEncodedContent(bodylist));
            return await result.Content.ReadAsStringAsync();
        }
    }
}