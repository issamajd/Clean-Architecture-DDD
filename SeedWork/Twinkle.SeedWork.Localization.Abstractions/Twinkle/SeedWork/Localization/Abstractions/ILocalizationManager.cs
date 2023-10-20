namespace Twinkle.SeedWork.Localization.Abstractions;

public interface ILocalizationManager
{
    protected ILocalizationStore LocalizationStore { get; set; }
}