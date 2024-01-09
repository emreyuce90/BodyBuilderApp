using BodyBuilder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Utilities.JWT {
    public interface ITokenCreate {
        AccessToken CreateToken(User user);

    }
}
