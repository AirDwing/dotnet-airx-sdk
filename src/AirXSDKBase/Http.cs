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
    public class BaseRequest
    {
        HttpClient hc;
        public SignatureOption SignatureOption { get; set; }
        public BaseRequest(SignatureOption SignatureOption)
        {
            this.SignatureOption = SignatureOption;
            hc = new HttpClient();
        }
        public async Task<string> GET(string url)
        {
            Uri uri=new Uri(url);
            StringBuilder sb=new StringBuilder();
            var query= HttpUtility.ParseQueryString(uri.Query);
            Array.Sort(query.AllKeys);
            var request = CommonTools.ModelToQueryString(new CommonParameterModel
            {
                Nonce = CommonTools.Nonce(5),
                SecretId = this.SignatureOption.SecretId,
                SignatureMethod = this.SignatureOption.SignatureMethod.ToString(),
                Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString()
            });
            sb.Append("GET");
            sb.Append(uri.Host);
            sb.Append(uri.LocalPath);
            sb.Append("?");
            sb.Append(request);
            foreach(var i in query.AllKeys){
                sb.Append($"&{i}={query.GetValues(i)[0]}");
            }
            Console.WriteLine("############");
            Signature s=new Signature(SignatureOption);
            var Signature= s.Compute(sb.ToString());
            Console.WriteLine($"{url}&{request}&Signature={Signature}");
            var result = await hc.GetAsync($"{url}&{request}&Signature={Signature}");
            return await result.Content.ReadAsStringAsync();
        }

    }
}