using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Security.Claims;
using System.Configuration;

namespace telegramBot_BL.Helpers
{
    public static class JWTs
    {
        static string Create(object payload, string key)
        {
            var handler = new JwtSecurityTokenHandler();
            
           
            //create jwt
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Audience = ConfigurationManager.AppSettings["Audience"],
                Expires = DateTime.UtcNow.AddMinutes(10),
                Subject = new ClaimsIdentity(ObjectToClaims(payload)),
                Issuer = ConfigurationManager.AppSettings["Issuer"],
                SigningCredentials = new SigningCredentials(getKey(key), SecurityAlgorithms.RsaSha256, SecurityAlgorithms.Sha512Digest)

            };
            var token = handler.CreateToken(descriptor);

            return "";
        }

        private static RsaSecurityKey getKey(string keyString)
        {
            var keyBytes = Convert.FromBase64String(keyString); // your key here
            AsymmetricKeyParameter asymmetricKeyParameter = PublicKeyFactory.CreateKey(keyBytes);
            RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)asymmetricKeyParameter;
            RSAParameters rsaParams = new RSAParameters
            {
                Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned(),
                Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned()
            };
            return new RsaSecurityKey(rsaParams);
        }

        private static List<Claim> ObjectToClaims(object obj)
        {
            List<Claim> lst = new List<Claim>();
            var props = obj.GetType().GetProperties();
            foreach(var prop in props)
            {
                lst.Add(new Claim(prop.Name, prop.GetValue(obj).ToString()));
            }
            return lst;
        }
    }
}
