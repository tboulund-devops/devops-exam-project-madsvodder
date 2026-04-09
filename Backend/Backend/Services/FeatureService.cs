using FeatureHubSDK;

namespace Backend.Services;

public class FeatureService(IFeatureHubConfig fhConfig)
{
    public async Task<bool> IsLoginEnabled()
    {
        var context = await fhConfig.NewContext().Build();
        var feature = context["Login"];
    
        Console.WriteLine($"[FeatureHub] Feature 'Login' - IsEnabled: {feature.IsEnabled}, Value: {feature.Value}, Exists: {feature.Exists}");
    
        return feature.IsEnabled;
    }
    
    public async Task<bool> IsRatingEnabled()
    {
        var context = await fhConfig.NewContext().Build();
        var feature = context["CanRate"];
    
        Console.WriteLine($"[FeatureHub] Feature 'CanRate' - IsEnabled: {feature.IsEnabled}, Value: {feature.Value}, Exists: {feature.Exists}");
    
        return feature.IsEnabled;
    }
}