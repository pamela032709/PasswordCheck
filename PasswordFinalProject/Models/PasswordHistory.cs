using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasswordFinalProject.Models
{
    [Table("fn_password_history")]
    public class PasswordHistory
    {
        public int Id { get; set; }

        public string Password { get; set; }
        
        public User User { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
