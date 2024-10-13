using System.IO;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Controls;
using StarLight_Core.Downloader;
using StarLight_Core.Utilities;

namespace WpfApp2.Pages;

public partial class Kuozhan : Page
{
    public Kuozhan()
    {
        InitializeComponent();
    }

    private async void DownloadButton_OnClick(object sender, RoutedEventArgs e)
    {
        var source = new CancellationTokenSource();
        var token = source.Token;
        MultiThreadedFileDownloader downloader = new MultiThreadedFileDownloader((x =>
                Console.WriteLine(x / 8 / 1024 + "MB/s")
            ),token);
        try
        {
            await downloader.DownloadFileWithMultiThread(
                UrlTextBox.Text,
                Path.Combine(FileUtil.GetCurrentExecutingDirectory(),ExtraTextBox.Text));
        }
        catch (Exception exception)
        {
            //提示错误信息
            Console.WriteLine(exception);
            Growl.Error("下载失败。错误：" + exception);
        }
        //提示下载完成
        Growl.Success("自定义下载成功！");
    }
}