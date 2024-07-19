namespace TVR.Bundesliga.API.Models;

public class Guest
{
    public Guest(string name, string emailAddress)
    {
        Name = name;
        EmailAddress = emailAddress;
    }
    
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string EmailAddress { get; set; }
}
