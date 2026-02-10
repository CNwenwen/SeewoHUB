using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Controls;
using StarLight_Core.Downloader;
using StarLight_Core.Models.Downloader;
using StarLight_Core.Utilities;
using WpfApp2.Enums;
using WpfApp2.Models;
using WpfApp2.Services;

namespace WpfApp2.Pages;

public partial class DownloadsPage
{
    private readonly ObservableCollection<AppDownloadItem> _apps = [];
    private DownloadSource _currentSource = DownloadSource.BuiltIn;

    public DownloadsPage()
    {
        InitializeComponent();
        AppsListView.ItemsSource = _apps;
        Loaded += async (_, _) => await LoadAppsAsync();
    }

    private async void RefreshButton_OnClick(object sender, RoutedEventArgs e)
    {
        await LoadAppsAsync();
    }

    private async void SourceComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SourceComboBox.SelectedItem is not ComboBoxItem comboBoxItem)
        {
            return;
        }

        if (!Enum.TryParse(comboBoxItem.Tag?.ToString(), out DownloadSource source))
        {
            return;
        }

        _currentSource = source;
        await LoadAppsAsync();
    }

    private async Task LoadAppsAsync()
    {
        try
        {
            var items = await SeewoAppsMarketService.GetAppsAsync(_currentSource, CancellationToken.None);
            ApplyApps(items);
            Growl.Success($"已加载 {_apps.Count} 个应用（{_currentSource}）。");
        }
        catch (Exception exception)
        {
            if (_currentSource == DownloadSource.SeewoAppsMarket)
            {
                var fallbackItems = await SeewoAppsMarketService.GetAppsAsync(DownloadSource.BuiltIn, CancellationToken.None);
                ApplyApps(fallbackItems);
                Growl.Warning($"SeewoAppsMarket 加载失败，已回退到内置源：{exception.Message}");
                return;
            }

            Growl.Error($"加载应用列表失败：{exception.Message}");
        }
    }

    private void ApplyApps(IEnumerable<AppDownloadItem> items)
    {
        _apps.Clear();
        foreach (var item in items)
        {
            _apps.Add(item);
        }
    }

    private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var selectedApps = _apps.Where(x => x.IsSelected).ToList();
        if (selectedApps.Count == 0)
        {
            Growl.Warning("请至少勾选一个应用。");
            return;
        }

        var downloadPath = SettingsPage.Settings.Path ?? FileUtil.GetCurrentExecutingDirectory();
        Directory.CreateDirectory(downloadPath);

        DownloaderConfig.MaxThreads = SettingsPage.Settings.Process;
        DownloaderConfig.VerificationFile = true;
        DownloaderConfig.UserAgent = "SeewoHUB/1.1.0";

        using var source = new CancellationTokenSource();
        var downloader = new MultiThreadedFileDownloader(null, source.Token);

        foreach (var app in selectedApps)
        {
            try
            {
                await downloader.DownloadFileWithMultiThread(app.Url, Path.Combine(downloadPath, app.TargetFileName));
                Growl.Success($"{app.Name} 下载成功！");
            }
            catch (Exception exception)
            {
                Growl.Error($"{app.Name} 下载失败：{exception.Message}");
            }
        }
    }
}
