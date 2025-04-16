using System.IO;
using System.Windows;
using HandyControl.Controls;
using StarLight_Core.Downloader;
using StarLight_Core.Models.Downloader;
using StarLight_Core.Utilities;

namespace WpfApp2.Pages;

public partial class DownloadsPage
{
    public static string DownloadPath { get; set; }
    public DownloadsPage()
    {
        InitializeComponent();
      
    }
    
    private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        DownloadPath = SettingsPage.Settings.Path ?? FileUtil.GetCurrentExecutingDirectory();
        DownloaderConfig.MaxThreads = SettingsPage.Settings.Process;
        DownloaderConfig.VerificationFile = true;
        DownloaderConfig.UserAgent = "SeewoHUB/1.1.0";
        var source = new CancellationTokenSource();
        var token = source.Token;
        MultiThreadedFileDownloader downloader = new MultiThreadedFileDownloader(null,token);
        if (Classisland.IsChecked == true)
        {
            try
            {
                await downloader.DownloadFileWithMultiThread(
                    "https://get.classisland.tech/d/ClassIsland-Ningbo-S3/classisland/disturb/1.6.0.5/ClassIsland_app_windows_x64_full_singleFile.zip",
                    Path.Combine(DownloadPath,"classisland.zip"));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Growl.Error("下载失败。错误：" + exception);
                return;
            }
            Growl.Success("Classisland下载成功！");
        }
        if (ZongziTekSticker.IsChecked == true)
        {
            try
            {
                await downloader.DownloadFileWithMultiThread(
                    "https://gh.llkk.cc/https://github.com/STBBRD/ZongziTEK-Blackboard-Sticker/releases/download/latest/ZongziTEK_Blackboard_Sticker.zip",
                    Path.Combine(DownloadPath,"ZongziTek-Blackboard-Sticker.zip"));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Growl.Error("下载失败。错误：" + exception);
                return;
            }
            Growl.Success("ZongziTek黑板贴下载成功！");
        }
        if (Ecs.IsChecked == true)
        {
            try
            {
                await downloader.DownloadFileWithMultiThread(
                    "https://gh.llkk.cc/https://github.com/EnderWolf006/ElectronClassSchedule/releases/download/latest/Win10_Win11_ElectronClassSchedule.zip",
                    Path.Combine(DownloadPath,"Win10_Win11_ElectronClassSchedule.zip"));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Growl.Error("下载失败。错误：" + exception);
                return;
            }
            Growl.Success("ElectronClassSchedule下载成功！");
        }
        if (Cw.IsChecked == true)
        {
            try
            {
                await downloader.DownloadFileWithMultiThread(
                    "https://gh.llkk.cc/https://github.com/RinLit-233-shiroko/Class-Widgets/releases/download/v1.1.7-b2/v1.1.7-b2_Class_Widgets.zip",
                    Path.Combine(DownloadPath,"Class_Widgets.zip"));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Growl.Error("下载失败。错误：" + exception);
                return;
            }
            Growl.Success("Class Widgets下载成功！");
        }
    }
}