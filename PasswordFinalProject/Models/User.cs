using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasswordFinalProject.Models
{
    [Table("fn_user")]
    public class User
    {
        public int Id { get; set; }
        
        public string UserName { get; set; }       
        public virtual ICollection<PasswordHistory> PasswordHistorys { get; set; }
    }
    public class UserViewModel
    {
       
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
