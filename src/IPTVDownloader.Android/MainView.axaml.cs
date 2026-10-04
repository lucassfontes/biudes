using Android.App;
using Android.Content;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using IPTVDownloader.Models;
using IPTVDownloader.ViewModels;

namespace IPTVDownloader;

public partial class MainView : UserControl
{
    private readonly MainWindowViewModel _vm;
    private ScrollViewer? _contentScroll;
    private bool _restoringScroll;
    private bool _initialized;

    public MainView(MainWindowViewModel vm)
    {
        _vm = vm;
        DataContext = vm;
        InitializeComponent();
        _contentScroll = this.FindControl<ScrollViewer>("ContentScroll");
        AttachedToVisualTree += OnAttached;
        DetachedFromVisualTree += OnDetached;
        _vm.ScrollRestoreRequested += RestoreScroll;
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    private async void OnAttached(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (_initialized) return;
        _initialized = true;
        await _vm.InitializeAsync();
    }

    private async void OnDetached(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (_contentScroll is not null)
            _vm.UpdateScrollOffset(_contentScroll.Offset.Y);
        await _vm.SaveStateAsync();
    }

    private void OnOpenDownloadFolder(object? sender, RoutedEventArgs e)
    {
        try
        {
            var activity = ActivityProvider.Current;
            if (activity is null)
            {
                _vm.Status = $"Downloads: {_vm.DownloadRoot}";
                return;
            }

            var intent = new Intent(Intent.ActionOpenDocumentTree);
            intent.AddFlags(ActivityFlags.NewTask | ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission);
            activity.StartActivity(intent);
            _vm.Status = $"Pasta de downloads do app: {_vm.DownloadRoot}";
        }
        catch (Exception ex)
        {
            _vm.Status = $"Downloads: {_vm.DownloadRoot} • {ex.Message}";
        }
    }

    private void OnSearchKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && _vm.SearchCommand.CanExecute(null))
        {
            _vm.SearchCommand.Execute(null);
            e.Handled = true;
        }
    }

    private void OnContentScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        if (_restoringScroll || _contentScroll is null) return;
        _vm.UpdateScrollOffset(_contentScroll.Offset.Y);
    }

    private async void RestoreScroll(double offset)
    {
        if (_contentScroll is null) return;
        _restoringScroll = true;
        try
        {
            foreach (var delay in new[] { 30, 120, 300, 650 })
            {
                await Task.Delay(delay);
                await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                {
                    if (_contentScroll is null) return;
                    var max = Math.Max(0, _contentScroll.Extent.Height - _contentScroll.Viewport.Height);
                    _contentScroll.Offset = new Vector(0, Math.Clamp(offset, 0, max));
                });
            }
        }
        finally
        {
            _restoringScroll = false;
        }
    }

    private void OnAlphaClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is string letter && _vm.AlphaCommand.CanExecute(letter))
            _vm.AlphaCommand.Execute(letter);
    }

    private void OnCardPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not Border border || border.DataContext is not MediaItem item || !item.CanOpenFromCard)
            return;

        if (e.Source is Visual source)
        {
            Visual? current = source;
            while (current is not null && !ReferenceEquals(current, border))
            {
                if (current is Button) return;
                current = current.GetVisualParent();
            }
        }

        if (item.OpenCommand?.CanExecute(null) == true)
            item.OpenCommand.Execute(null);
    }
}
