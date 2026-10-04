using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using IPTVDownloader.Services;
using IPTVDownloader.ViewModels;

namespace IPTVDownloader;

public partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is ISingleViewApplicationLifetime singleView)
        {
            var store = new SettingsStore();
            var xtream = new XtreamClient();
            var downloader = new DownloadService(xtream);
            var images = new ImageCacheService(xtream.HttpClient, store.Paths);
            var vm = new MainWindowViewModel(store, xtream, downloader, images);
            singleView.MainView = new MainView(vm);
        }

        base.OnFrameworkInitializationCompleted();
    }
}
