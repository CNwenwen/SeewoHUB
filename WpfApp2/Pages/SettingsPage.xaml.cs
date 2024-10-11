using System.Windows;
using System.Windows.Controls;

namespace WpfApp2.Pages;

public partial class SettingsPage : Page
{
    public static class Settings
    {
        public static int Process { get; set; }
    }
    public SettingsPage()
    {
        InitializeComponent();
    }

    private void ProcessSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        var temp = Convert.ToInt32(ProcessSlider.Value) * 8;
        Settings.Process = temp;
        Console.WriteLine(temp);
    }
}