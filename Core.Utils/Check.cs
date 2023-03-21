namespace DDD.Core.Utils;

public static class Check
{
    /// <param name="value">the value being checked</param>
    /// <param name="parameterName">name of parameter</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>the value after being checked</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static T NotNull<T>(T value, string parameterName)
    {
        if (value == null)
        {
            throw new ArgumentNullException(parameterName);
        }

        return value;
    }
    
    /// <param name="value">the value being checked</param>
    /// <param name="parameterName">name of parameter</param>
    /// <param name="message"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>the value after being checked</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static T NotNull<T>(T value, string parameterName, string message)
    {
        if (value == null)
        {
            throw new ArgumentNullException(parameterName, message);
        }

        return value;
    }
    
    /// <param name="value">the value being checked</param>
    /// <param name="parameterName">name of parameter</param>
    /// <param name="minLength">minimum accepted length of the value</param>
    /// <param name="maxLength">maximum accepted length of the value</param>
    /// <returns>the value after being checked</returns>
    /// <exception cref="ArgumentException"></exception>
    public static string NotNullOrEmpty(string? value, string parameterName,
        int minLength = 0, int maxLength = Int32.MaxValue)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException($"{parameterName} can not be null or empty!", parameterName);
        }

        if (value.Length > maxLength)
        {
            throw new ArgumentException($"{parameterName} length must be equal to or lower than {maxLength}!",
                parameterName);
        }

        if (value.Length < minLength)
        {
            throw new ArgumentException($"{parameterName} length must be equal to or bigger than {minLength}!",
                parameterName);
        }

        return value;
    }
}