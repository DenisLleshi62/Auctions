namespace WebApi.Models.Users;

public class GetUserResponse
{
    public int UserId { get; set; }
    
    public string UserName { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
}