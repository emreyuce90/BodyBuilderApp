using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Domain.Entities {
    public class User :BaseEntity{

        public string Email { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public bool MailConfirm { get; set; }
        public string MailConfirmValue { get; set; }
        public DateTime MailConfirmDate { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public List<Role> Roles { get; set; }
        public Guid RoleId { get; set; }

    }
}
