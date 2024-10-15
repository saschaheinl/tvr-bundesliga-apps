namespace TVR.Bundesliga.API.Domain.Models;

public class QrCodeDetails (string location, string qrCode)
{
    public string LocationOfImage { get; set; } = location;

    public string QrCodeAsBase64 { get; set; } = qrCode;
    
    public string PublicLink { get; set; } = string.Empty;
}
