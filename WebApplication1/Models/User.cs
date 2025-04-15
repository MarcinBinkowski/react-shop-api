namespace WebApplication1.Models;

public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Department { get; set; }
    public DateTime JoinedDate { get; set; }
}