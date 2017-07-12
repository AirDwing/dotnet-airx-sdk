using System;
using System.Security.Cryptography;
using System.Text;

namespace Dwing.AirXSDKBase
{
    public class Signature
    {
        #region 构造函数

        public SignatureOption SignatureOption { get; set; }

        public Signature(string SecretKey) : this(
            new SignatureOption
            {
                SecretKey = SecretKey
            })
        { }

        public Signature(string SecretKey, SignatureOption.SignatureType SignatureMethod) : this(
            new SignatureOption
            {
                SecretKey = SecretKey,
                SignatureMethod = SignatureMethod
            })
        { }

        public Signature(SignatureOption SignatureOption)
        {
            this.SignatureOption = SignatureOption;
        }

        #endregion 构造函数

        public string Compute(string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str);

            switch (this.SignatureOption.SignatureMethod)
            {
                case SignatureOption.SignatureType.HmacSHA1:
                    return Convert.ToBase64String(ComputeHMACSHA1(buffer));

                case SignatureOption.SignatureType.HmacSHA256:
                    return Convert.ToBase64String(ComputeHMACSHA256(buffer));

                default:
                    return Convert.ToBase64String(ComputeHMACSHA1(buffer));
            }
        }

        private byte[] ComputeHMACSHA1(byte[] buffer)
        {
            HMACSHA1 h = new HMACSHA1(Encoding.UTF8.GetBytes(this.SignatureOption.SecretKey));
            return h.ComputeHash(buffer);
        }

        private byte[] ComputeHMACSHA256(byte[] buffer)
        {
            HMACSHA256 h = new HMACSHA256(Encoding.UTF8.GetBytes(this.SignatureOption.SecretKey));
            return h.ComputeHash(buffer);
        }
    }

    public class SignatureOption
    {
        public string SecretId { get; set; }
        public string SecretKey { get; set; }

        public enum SignatureType
        {
            HmacSHA1, HmacSHA256
        }

        public SignatureType SignatureMethod { get; set; } = SignatureType.HmacSHA1;
    }
}