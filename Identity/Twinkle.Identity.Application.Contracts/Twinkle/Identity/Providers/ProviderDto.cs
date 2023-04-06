namespace Twinkle.Identity.Providers;

public class ProviderDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string? BusinessName { get; set; }
}