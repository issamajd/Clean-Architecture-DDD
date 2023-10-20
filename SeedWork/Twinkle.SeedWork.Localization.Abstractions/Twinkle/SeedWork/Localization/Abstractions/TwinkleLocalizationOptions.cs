namespace Twinkle.SeedWork.Localization.Abstractions;

public class TwinkleLocalizationOptions
{
    private IList<Language> _languages;

    public TwinkleLocalizationOptions()
    {
        _languages = new List<Language>();
    }

    public void AddLanguage(string culture)
    {
        _languages.Add(new Language(culture));
    }
}