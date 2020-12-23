using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using GrpcServiceUsers.model;
using GrpcServiceUsers.Protos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using User = GrpcServiceUsers.Protos.User;

namespace GrpcServiceUsers.Services
{
    public class UserService : UsersSecurity.UsersSecurityBase, IUserService
    {
        private readonly UserDb _userDb;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        public UserService(ILogger<UserService> logger, UserDb userDb, IMapper mapper)
        {
            _logger = logger;
            _userDb = userDb;
            _mapper = mapper;
        }
        [Authorize]
        public override Task<User> GetUser(Filter request, ServerCallContext context)
        {
           
            var result = _userDb.GetUser(request.Guid);
      
            return Task.FromResult(_mapper.Map<Protos.User>(result));
        }
        [Authorize]
        public override Task<UserCollection> GetUsers(FilterActive request, ServerCallContext context)
        {
          
            var result = _userDb.GetAllUser(request.Active);
            UserCollection collection = new UserCollection() { };
            result.ForEach(user => collection.User.Add((_mapper.Map<Protos.User>(user))));

            return Task.FromResult(collection);


        }
    }
}
