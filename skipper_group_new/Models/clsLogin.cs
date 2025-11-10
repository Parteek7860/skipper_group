using System.ComponentModel.DataAnnotations;

namespace skipper_group_new.Models
{
    public class clsLogin
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string message { get; set; }
       
    }
}
