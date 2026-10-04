using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;

namespace IPTVDownloader;

[Activity(
    Label = "IPTV Downloader — v_2.0.24",
    Theme = "@style/AppTheme",
    Icon = "@drawable/iptv_downloader",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode | ConfigChanges.KeyboardHidden,
    ScreenOrientation = ScreenOrientation.Unspecified)]
public sealed class MainActivity : AvaloniaMainActivity<App>
{
    protected override void OnResume()
    {
        base.OnResume();
        ActivityProvider.Current = this;
    }

    protected override void OnPause()
    {
        if (ReferenceEquals(ActivityProvider.Current, this)) ActivityProvider.Current = null;
        base.OnPause();
    }

    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder) =>
        base.CustomizeAppBuilder(builder)
            .WithInterFont()
            .LogToTrace();
}
