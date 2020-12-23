using System;

namespace GrpcServiceUsers.model
{
    public class User
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string ForName { get; set; }
        public string LatName { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }

    }
}