using Microsoft.Extensions.Localization;

namespace Twinkle.SeedWork.Localization.AspNetCore;

public class JsonStringLocalizer<T> : IStringLocalizer<T>
{
    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        throw new NotImplementedException();
    }

    public LocalizedString this[string name] => throw new NotImplementedException();

    public LocalizedString this[string name, params object[] arguments] => throw new NotImplementedException();
}