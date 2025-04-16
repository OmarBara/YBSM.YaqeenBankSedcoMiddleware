using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Core.Domain.Extensions
{
    public class JWT
    {
        public static string Generate(string key, List<Claim> claims,DateTime expire,string audience, string issuer)
        {
     

            // Else we generate JSON Web Token
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: expire,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256));

            // var tokenKey = Encoding.UTF8.GetBytes(key);
            // var tokenDescriptor = new SecurityTokenDescriptor
            // {
            //     Subject = new ClaimsIdentity(claims),
            //     Expires = expire,
            //     SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),SecurityAlgorithms.HmacSha256Signature)
            // };
            // var token = tokenHandler.CreateToken(tokenDescriptor);

            return  tokenHandler.WriteToken(token);
        }
    }
}