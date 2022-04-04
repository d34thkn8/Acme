using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ACME.ENCUESTAS.API.Utils
{
    public class GenerateToken
    {
        public GenerateToken(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public string CrearToken(List<Claim> claims)
        {
            //creamos el header
            var _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var _signingCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var _header = new JwtHeader(_signingCredentials);

            //creara el payload
            var _payLoad = new JwtPayload(
                issuer: Configuration["Jwt:Issuer"],
                audience: Configuration["Jwt:Audience"],
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(30));

            var _token = new JwtSecurityToken(
                _header,
                _payLoad);
            return new JwtSecurityTokenHandler().WriteToken(_token);
        }
    }
}
