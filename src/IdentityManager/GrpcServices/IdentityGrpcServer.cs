using Google.Protobuf.Collections;
using Grpc.Core;
using IdentityManager.Interface;
using Northstar.Message;
using System.Security.Claims;

namespace IdentityManager.GrpcServices
{
    public class IdentityGrpcServer : IdentityGrpcService.IdentityGrpcServiceBase
    {
        private readonly IUserStoreService _userStoreService;

        public IdentityGrpcServer(IUserStoreService userStoreService)
        {
            _userStoreService = userStoreService;
        }

        public override async Task<LoginResponsMessage> Create(LoginMessage request, ServerCallContext context)
        {
            List<Claim> claims = new List<Claim>();

            foreach (var claim in request.Claims)
            {
                claims.Add(new Claim(claim.Type, claim.Value));
            }

            var resp = await _userStoreService.AddUserAsync(request.Email, request.Password, claims);
            if (resp.Error == null)
            {
                LoginCreateSuccessMessage success = new LoginCreateSuccessMessage() { Id = resp.Id };
                return new LoginResponsMessage() { Success = success };
            }
            else
            {
                IdentityErrorMessage error = new IdentityErrorMessage();
                error.Message.Add(resp.Error.Message);

                return new LoginResponsMessage() { Error = error };
            }
        }
    }
}
