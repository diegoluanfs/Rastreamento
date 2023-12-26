using LocatedAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks; // Adicionado para suporte a Task

namespace LocatedAPI
{
    public interface IJWTAuthenticationManager
    {
        Task<string> AuthenticateAsync(string username, string password, int id);
    }

    public class JWTAuthenticationManager : IJWTAuthenticationManager
    {
        private readonly string tokenKey;
        private readonly Contexto contexto;

        public JWTAuthenticationManager(string tokenKey)
        {
            this.tokenKey = tokenKey;
        }

        public async Task<string> AuthenticateAsync(string username, string password, int id)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(tokenKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, id.ToString()), // Adiciona a reivindicação do ID
                    }),
                    Expires = DateTime.UtcNow.AddHours(1), // Defina o tempo de expiração conforme necessário
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                // Adicione um tratamento de erro adequado, como log ou rethrow da exceção.
                // Aqui, a exceção está sendo relançada, mas você pode personalizar conforme necessário.
                throw new Exception("Erro ao autenticar usuário.", ex);
            }
        }
    }
}
