using System.Net.Http;
using System.Text.Json;
using WpfApp2.Enums;
using WpfApp2.Models;

namespace WpfApp2.Services;

public static class SeewoAppsMarketService
{
    private static readonly string[] MarketJsonUrls =
    [
        "https://raw.githubusercontent.com/CNwenwen/SeewoAppsMarket/main/apps.json",
        "https://gh.llkk.cc/https://raw.githubusercontent.com/CNwenwen/SeewoAppsMarket/main/apps.json"
    ];

    private static readonly HttpClient HttpClient = new();

    public static async Task<IReadOnlyList<AppDownloadItem>> GetAppsAsync(DownloadSource source, CancellationToken cancellationToken)
    {
        return source switch
        {
            DownloadSource.BuiltIn => GetBuiltInApps(),
            DownloadSource.SeewoAppsMarket => await GetAppsFromMarketAsync(cancellationToken),
            _ => GetBuiltInApps()
        };
    }

    private static IReadOnlyList<AppDownloadItem> GetBuiltInApps()
    {
        return
        [
            new AppDownloadItem
            {
                Name = "Classisland",
                Description = "一款适用于班级一体机的课程信息显示软件。",
                Url = "https://get.classisland.tech/d/ClassIsland-Ningbo-S3/classisland/disturb/1.6.0.5/ClassIsland_app_windows_x64_full_singleFile.zip",
                TargetFileName = "classisland.zip"
            },
            new AppDownloadItem
            {
                Name = "ElectronClassSchedule",
                Description = "一款电子课程表软件。",
                Url = "https://gh.llkk.cc/https://github.com/EnderWolf006/ElectronClassSchedule/releases/download/latest/Win10_Win11_ElectronClassSchedule.zip",
                TargetFileName = "Win10_Win11_ElectronClassSchedule.zip"
            },
            new AppDownloadItem
            {
                Name = "Class Widgets",
                Description = "一款全新课表软件。",
                Url = "https://gh.llkk.cc/https://github.com/RinLit-233-shiroko/Class-Widgets/releases/download/v1.1.7-b2/v1.1.7-b2_Class_Widgets.zip",
                TargetFileName = "Class_Widgets.zip"
            },
            new AppDownloadItem
            {
                Name = "ZongziTek黑板贴",
                Description = "方便的希沃小工具，使用 WinUI 3 风格。",
                Url = "https://gh.llkk.cc/https://github.com/STBBRD/ZongziTEK-Blackboard-Sticker/releases/download/latest/ZongziTEK_Blackboard_Sticker.zip",
                TargetFileName = "ZongziTek-Blackboard-Sticker.zip"
            }
        ];
    }

    private static async Task<IReadOnlyList<AppDownloadItem>> GetAppsFromMarketAsync(CancellationToken cancellationToken)
    {
        Exception? lastException = null;

        foreach (var marketJsonUrl in MarketJsonUrls)
        {
            try
            {
                return await ParseRemoteAppsAsync(marketJsonUrl, cancellationToken);
            }
            catch (Exception exception)
            {
                lastException = exception;
            }
        }

        throw new InvalidOperationException("无法读取 SeewoAppsMarket 数据源。", lastException);
    }

    private static async Task<IReadOnlyList<AppDownloadItem>> ParseRemoteAppsAsync(string marketJsonUrl, CancellationToken cancellationToken)
    {
        using var response = await HttpClient.GetAsync(marketJsonUrl, cancellationToken);
        response.EnsureSuccessStatusCode();
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

        var apps = new List<AppDownloadItem>();

        if (doc.RootElement.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("SeewoAppsMarket 数据格式不是数组。 ");
        }

        foreach (var item in doc.RootElement.EnumerateArray())
        {
            var name = GetString(item, "name", "title");
            var description = GetString(item, "description", "desc");
            var url = GetString(item, "url", "downloadUrl", "download_url");
            var fileName = GetString(item, "targetFileName", "fileName", "file_name");

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(url))
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = GuessFileNameFromUrl(url) ?? $"{name}.zip";
            }

            apps.Add(new AppDownloadItem
            {
                Name = name,
                Description = description ?? string.Empty,
                Url = url,
                TargetFileName = fileName
            });
        }

        return apps;
    }

    private static string? GetString(JsonElement element, params string[] keys)
    {
        foreach (var key in keys)
        {
            if (!element.TryGetProperty(key, out var value) || value.ValueKind != JsonValueKind.String)
            {
                continue;
            }

            return value.GetString();
        }

        return null;
    }

    private static string? GuessFileNameFromUrl(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            return null;
        }

        var fileName = Path.GetFileName(uri.LocalPath);
        return string.IsNullOrWhiteSpace(fileName) ? null : fileName;
    }
}
