using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace BackEndPartWeb.Models
{
    public class User
    {
        public Guid Id { get;  set; }
        public string Username { get;  set; }
        public string Password { get;  set; }
        public string Email { get;  set; }      
        public int Age { get;  set; }
        public string FullName { get;  set; }
        public Image? Image { get;  set; }
        public Role Role { get; set; }      
        public List<Monster>? Monsters { get; set; }
    }
}
