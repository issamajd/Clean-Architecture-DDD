using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace Twinkle.SeedWork.Localization.AspNetCore;

public class JsonFileLocalizationProvider
{
    private readonly IFileProvider _fileProvider;

    public JsonFileLocalizationProvider(IFileProvider fileProvider, IStringLocalizer<>)
    {
        if (fileProvider is not PhysicalFileProvider physicalFileProvider)
            throw new ArgumentException("Only physical provider is supported at this moment");
        
        _fileProvider = fileProvider;
    }

    public async Task<Dictionary<string, string>> ExtractInfoFromJsonFile(string path)
    {
        var fileInfo = _fileProvider.GetFileInfo(path);
        if (!fileInfo.Exists)
            throw new InvalidOperationException("Json file doesn't exist: {path}");
        
        using var stream = new StreamReader(fileInfo.CreateReadStream());
        var jsonFile = await stream.ReadToEndAsync();
        var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile);
        return dictionary ?? new Dictionary<string, string>();
    }
}