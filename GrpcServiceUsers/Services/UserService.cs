using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcServiceUsers.model;
using GrpcServiceUsers.Protos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using User = GrpcServiceUsers.Protos.User;

namespace GrpcServiceUsers.Services
{
    public interface IUserService
    {
        Task<User> GetUser(Filter request, ServerCallContext context);
        Task<UserCollection> GetUsers(FilterActive request, ServerCallContext context);
    }

    public class UserService : UsersSecurity.UsersSecurityBase, IUserService
    {
        private readonly UserDb _userDb;
        private readonly ILogger<UserService> _logger;
       
        public UserService(ILogger<UserService> logger, UserDb userDb)
        {
            _logger = logger;
            _userDb = userDb;
        }
        [Authorize]
        public override Task<User> GetUser(Filter request, ServerCallContext context)
        {
           
            var result = _userDb.GetUser(request.Guid);
            return Task.FromResult(new User() { Guid = result.Guid.ToString(), Active = result.Active, Email = result.Email, LatName = result.LatName });
        }
        [Authorize]
        public override Task<UserCollection> GetUsers(FilterActive request, ServerCallContext context)
        {
          
            var result = _userDb.GetAllUser(request.Active);
            UserCollection collection = new UserCollection() { };
            result.ForEach(user => collection.User.Add(new User() { Guid = user.Guid.ToString(), Active = user.Active, Email = user.Email, LatName = user.LatName }));

            return Task.FromResult(collection);


        }
    }
}
