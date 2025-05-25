using System.ComponentModel.DataAnnotations.Schema;
namespace EspnBackend.Security{

    [Table("UserLoginHistory")]
    public class UserLoginHistory
    {
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime LoginTime { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
}   
}