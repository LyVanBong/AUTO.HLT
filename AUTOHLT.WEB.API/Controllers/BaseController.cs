using AUTOHLT.WEB.API.Database;
using JWT.Algorithms;
using JWT.Builder;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AUTOHLT.WEB.API.Controllers
{
    public class BaseController : ApiController
    {
        /// <summary>
        /// khóa bí mật
        /// </summary>
        private const string _secret = "uLOx1EFyahJ65S7Lp4oAMK0Yyer6DbtkKvWFC79qGofgzKFm6dMqv8ou3luxhOulg2gjTXJk1MlbgXLU0K6qFsHSRAG9BpXIgfluzLdeRMxQvkIBfoh3BV0WaWcsle3e2E0qpTgT5OBbSTOgP9m4FWJskYOBo9Fq5Yr6kTz0pXPItMT35mwtxCAJ5HBSz8j7PC3DiNZx06eAqvElYuE7dn2nBrZbimY81VBcF1iuBNZ5dUiurU2z4VsEB4Obymhw";

        /// <summary>
        /// database autohlt
        /// </summary>
        protected bsoft_autohltEntities DatabaseAutohlt { get; private set; }

        public BaseController()
        {
            DatabaseAutohlt = new bsoft_autohltEntities();
        }

        /// <summary>
        /// xac thuc jwt
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected async Task<VerifyingModel> Verifying(HttpRequestMessage request)
        {
            try
            {
                return await Task.FromResult(JsonConvert.DeserializeObject<VerifyingModel>(new JwtBuilder()
                    .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                    .WithSecret(_secret)
                    .MustVerifySignature()
                    .Decode(request?.Headers?.Authorization?.ToString())));
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Tạo jwt
        /// </summary>
        /// <param name="user"></param>
        /// <param name="idUser"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        protected async Task<string> Signing(string user, Guid idUser, int? role)
        {
            try
            {
                return await Task.FromResult(new JwtBuilder()
                    .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                    .WithSecret(_secret)
                    .AddClaim("Exp", DateTimeOffset.UtcNow.AddDays(7).ToUnixTimeSeconds())
                    .AddClaim("Iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                    .AddClaim("Iss", "autohlt.com")
                    .AddClaim("IdUser", idUser)
                    .AddClaim("UserName", user)
                    .AddClaim("Role", role)
                    .Encode());
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    public class VerifyingModel
    {
        public long Exp { get; set; }
        public long Iat { get; set; }
        public string Iss { get; set; }
        public Guid IdUser { get; set; }
        public string UserName { get; set; }
        public int? Role { get; set; }
    }
}