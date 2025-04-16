using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2.Pages;

public partial class SettingsPage : Page
{
    public static class Settings
    {
        public static int Process = 64;
        public static string? Path { get; set; }
    }
    public SettingsPage()
    {
        InitializeComponent();
    }

    private void ProcessSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        Settings.Process = (int)ProcessSlider.Value;
        Console.WriteLine((int)ProcessSlider.Value);
    }

    private void PathTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        Settings.Path = PathTextBox.Text;
    }
}