namespace TVR.Bundesliga.API.Domain.Models;

public class Guest(string name, string emailAddress)
{
    public int Id { get; set; }
    
    public string Name { get; set; } = name;

    public string EmailAddress { get; set; } = emailAddress;
}
