using System.IO;
using System.Windows;
using HandyControl.Controls;
using StarLight_Core.Downloader;
using StarLight_Core.Utilities;

namespace WpfApp2.Pages;

public partial class DownloadsPage
{
    public static string Dizhi { get; set; }
    public DownloadsPage()
    {
        InitializeComponent();
        Growl.Warning("由于inkcanvasforclass作者在多重方面的原因下，删除了此软件和存储库。但我们还将保留inkcanvasforclass的页面以表示我们对开发者的致敬。在此，致敬！");
    }
    
    private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Dizhi = SettingsPage.Settings.Dizhi ?? FileUtil.GetCurrentExecutingDirectory();
        if (Dizhi == "" || Dizhi == null)
        {
            Dizhi = FileUtil.GetCurrentExecutingDirectory();
        }
        //定义一个多线程下载器
        var source = new CancellationTokenSource();
        var token = source.Token;
        MultiThreadedFileDownloader downloader = new MultiThreadedFileDownloader((x =>
                Console.WriteLine(x / 8 / 1024 + "MB/s")
            ),token);
        //如果Classisland的CheckBox是选择的，那么：
        if (Classisland.IsChecked == true)
        {
            //尝试下载文件，抓取错误
            try
            {
                await downloader.DownloadFileWithMultiThread(
                    "https://gh.llkk.cc/https://github.com/ClassIsland/ClassIsland/releases/download/1.5.0.2/ClassIsland.zip",
                    Path.Combine(Dizhi,"classisland.zip"));
            }
            catch (Exception exception)
            {
                //提示错误信息
                Console.WriteLine(exception);
                Growl.Error("下载失败。错误：" + exception);
                throw;
            }
            //提示下载完成
            Growl.Success("Classisland下载成功！");
        }
        //如果ZongziTek黑板贴的CheckBox是选择的，那么：
        if (ZongziTekSticker.IsChecked == true)
        {
            //尝试下载文件，抓取错误
            try
            {
                await downloader.DownloadFileWithMultiThread(
                    "https://gh.llkk.cc/https://github.com/STBBRD/ZongziTEK-Blackboard-Sticker/releases/download/v3.2.4/ZongziTEK_Blackboard_Sticker.zip",
                    Path.Combine(Dizhi,"ZongziTek-Blackboard-Sticker.zip"));
            }
            catch (Exception exception)
            {
                //提示错误信息
                Console.WriteLine(exception);
                Growl.Error("下载失败。错误：" + exception);
                throw;
            }
            //提示下载完成
            Growl.Success("ZongziTek黑板贴下载成功！");
        }
        if (Ecs.IsChecked == true)
        {
            //尝试下载文件，抓取错误
            try
            {
                await downloader.DownloadFileWithMultiThread(
                    "https://gh.llkk.cc/https://github.com/EnderWolf006/ElectronClassSchedule/releases/download/latest/Win10_Win11_ElectronClassSchedule.zip",
                    Path.Combine(Dizhi,"Win10_Win11_ElectronClassSchedule.zip"));
            }
            catch (Exception exception)
            {
                //提示错误信息
                Console.WriteLine(exception);
                Growl.Error("下载失败。错误：" + exception);
                throw;
            }
            //提示下载完成
            Growl.Success("ElectronClassSchedule下载成功！");
        }
        if (Cw.IsChecked == true)
        {
            //尝试下载文件，抓取错误
            try
            {
                await downloader.DownloadFileWithMultiThread(
                    "https://gh.llkk.cc/https://github.com/RinLit-233-shiroko/Class-Widgets/releases/download/v1.1.7-b2/v1.1.7-b2_Class_Widgets.zip",
                    Path.Combine(Dizhi,"Class_Widgets.zip"));
            }
            catch (Exception exception)
            {
                //提示错误信息
                Console.WriteLine(exception);
                Growl.Error("下载失败。错误：" + exception);
                throw;
            }
            //提示下载完成
            Growl.Success("Class Widgets下载成功！");
        }
    }
}