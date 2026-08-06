using System;
using System.Collections.Generic;
using System.Text;

namespace MusicServiceCRUD.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime RegistrationDate { get; set; }
        public bool IsPremium { get; set; }
    }
}
