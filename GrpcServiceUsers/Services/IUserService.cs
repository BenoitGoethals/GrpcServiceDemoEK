using System.Threading.Tasks;
using Grpc.Core;
using GrpcServiceUsers.Protos;

namespace GrpcServiceUsers.Services
{
    public interface IUserService
    {
        Task<User> GetUser(Filter request, ServerCallContext context);
        Task<UserCollection> GetUsers(FilterActive request, ServerCallContext context);
    }
}