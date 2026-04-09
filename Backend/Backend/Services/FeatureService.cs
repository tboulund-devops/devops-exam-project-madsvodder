using FeatureHubSDK;

namespace Backend.Services;

public class FeatureService(IFeatureHubConfig fhConfig)
{
    public async Task<bool> IsLoginEnabled()
    {
        var context = await fhConfig.NewContext().Build();
        return context["Login"].IsEnabled;
    }
}