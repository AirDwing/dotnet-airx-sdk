using Dwing.AirXSDKBase.Common;
using Dwing.AirXSDKBase.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Dwing.AirXSDKBase
{
    public class AirXBaseOption
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

        public int Port { get; set; } = -1;
    }

    public class AirXBase
    {
        public AirXBaseOption SDKOption { get; set; }
        private Signature _signature;
        private HttpClient _httpclient;
        public SignatureOption SignatureOption { get; set; }

        public AirXBase(AirXBaseOption SDKOption)
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
            var result = await _httpclient.PostAsync(_uri.Uri, new FormUrlEncodedContent(bodylist));
            return await result.Content.ReadAsStringAsync();
        }
    }
}