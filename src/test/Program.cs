using System;
using AirXSDKBase;
using System.Text;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Net.Http;
using AirXSDKBase.Common;
using System.Threading;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(CommonTools.ModelToQueryString(new {SecretId="123",SecretKey="456"}));
            BaseRequest b=new BaseRequest(new SignatureOption{SecretId="wnr5IXQ9UIso6hG4iBIFLVEjitWtijoiDnni",SecretKey="7b5dbb1361d005ad9c90ce295974121a"});
            while(true){
                var x= b.GET("https://staging.airdwing.com/user/check?username=18552830263").Result;
                Console.WriteLine(x);
                Thread.Sleep(1000);
                var y= b.GET("https://staging.airdwing.com/user/check?username=1855213123283263").Result;
                Console.WriteLine(y);
                Thread.Sleep(1000);
            }
        }
    }
}
