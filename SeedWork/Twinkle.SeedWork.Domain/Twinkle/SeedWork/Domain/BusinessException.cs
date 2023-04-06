namespace Twinkle.SeedWork.Domain;

public class BusinessException : Exception
{
    /// <summary>
    /// The exception Code that should be used for localization 
    /// </summary>
    public string? Code { get; }

    /// <summary>
    /// Exception details
    /// </summary>
    public string? Details { get; }
  
    public BusinessException(
        string? message = null,
        string? code = null,
        string? details = null)
        : base(message)
    {
        Code = code;
        Details = details;
    }
}