namespace DDD.Customers;

public class RegisterCustomerAccountDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    
    public int? Age { get; set; }
}