using System.Globalization;

namespace Twinkle.SeedWork.Localization.Abstractions;

public class Language
{
    private CultureInfo _cultureInfo;

    public Language(string culture)
    {
        _cultureInfo = new CultureInfo(culture);
    }
}