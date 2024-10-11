using System.Windows;
using HandyControl.Controls;
using StarLight_Core.Downloader;
using StarLight_Core.Models.Downloader;
using StarLight_Core.Utilities;
using WpfApp2.Pages;
using Path = System.IO.Path;

namespace WpfApp2;
//https://gh.llkk.cc/https://github.com/STBBRD/ZongziTEK-Blackboard-Sticker/releases/download/v3.2.4/ZongziTEK_Blackboard_Sticker.zip
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        DownloaderConfig.MaxThreads = 64;
    }
}