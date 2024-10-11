using System.IO;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Controls;
using StarLight_Core.Downloader;
using StarLight_Core.Utilities;

namespace WpfApp2.Pages;

public partial class DownloadsPage : Page
{
    public DownloadsPage()
    {
        InitializeComponent();
    }
    
    private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        //定义一个多线程下载器
        var source = new CancellationTokenSource();
        var token = source.Token;
        MultiThreadedFileDownloader downloader = new MultiThreadedFileDownloader((Action<double>)(x =>
                Console.WriteLine(x)
            ),token);
        //如果Classisland的CheckBox是选择的，那么：
        if (Classisland.IsChecked == true)
        {
            //尝试下载文件，抓取错误
            try
            {
                await downloader.DownloadFileWithMultiThread(
                    "https://gh.llkk.cc/https://github.com/ClassIsland/ClassIsland/releases/download/1.5.0.2/ClassIsland.zip",
                    Path.Combine(FileUtil.GetCurrentExecutingDirectory(),"classisland.zip"));
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
                    Path.Combine(FileUtil.GetCurrentExecutingDirectory(),"ZongziTek-Blackboard-Sticker.zip"));
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
    }
}