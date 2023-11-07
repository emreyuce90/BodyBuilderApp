using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Utilities.JWT {
    public static class JwtExtensions {
        public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier) {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));
        }

        public static void AddEMail(this ICollection<Claim> claims, string email) {
            claims.Add(new Claim(ClaimTypes.Email, email));
        }

        public static void AddName(this ICollection<Claim> claims, string name) {
            claims.Add(new Claim(ClaimTypes.Name, name));
        }

        public static void AddRoles(this ICollection<Claim> claims, string[] roles) {
            foreach (var role in roles.ToList()) {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            //roles.ToList().ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));
        }

        public static void AddRole(this ICollection<Claim> claims, string roleId) {
           
                claims.Add(new Claim(ClaimTypes.Role, roleId));
            
        }

        public static void AddCompany(this ICollection<Claim> claims, string company) {
            claims.Add(new Claim(ClaimTypes.Anonymous, company));
        }
    }
}
