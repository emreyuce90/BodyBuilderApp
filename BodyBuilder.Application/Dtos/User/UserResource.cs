﻿using BodyBuilder.Application.Utilities.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Dtos.User {
    public class UserResource {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public AccessToken Token { get; set; }
        public string RefreshToken { get; set; }
        public string RoleName { get; set; }
    }
}
