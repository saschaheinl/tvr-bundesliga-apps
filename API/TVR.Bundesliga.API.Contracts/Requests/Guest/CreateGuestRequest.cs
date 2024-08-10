namespace TVR.Bundesliga.API.Contracts.Requests.Guest;

/// <summary>
/// Request contract for creating guests.
/// </summary>
/// <param name="name">Name of the guest</param>
/// <param name="emailAddress">Email address of the guest</param>
public class CreateGuestRequest(string name, string emailAddress)
{
    /// <summary>
    /// Name of the guest to create
    /// </summary>
    /// <example>1. BC Beuel</example>
    public string Name { get; set; } = name;

    /// <summary>
    /// Email address of the Guest
    /// </summary>
    /// <example>vorstand@bcbeuel.de</example>
    public string EmailAddress { get; set; } = emailAddress;
}
