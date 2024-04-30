namespace QAgencyAppointment.Business.Options;

public class JwtOptions
{
    public const string OptionsName = nameof(JwtOptions);
    
    public string SecretKey { get; init; }
    
    public string Issuer { get; init; }
    
    public string Audience { get; init; }
    
    public int DurationInMinutes { get; init; }
}