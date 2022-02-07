﻿using APKInstaller.Pages.SettingsPages;
using APKInstaller.ViewModel;
using ModernWpf.Controls;
using System;
using System.IO;
using System.Windows;
using System.Windows.Navigation;

namespace APKInstaller.Pages
{
    /// <summary>
    /// InstallPage.xaml 的交互逻辑
    /// </summary>
    public partial class InstallPage : Page
    {
        internal InstallViewModel? Provider;

        public InstallPage() => InitializeComponent();

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string _path = string.Empty;
            string[] arguments = Environment.GetCommandLineArgs();

            if (arguments.GetLength(0) > 1)
            {
                if (arguments[1].EndsWith(".apk"))
                {
                    string filePathFormMainArgs = arguments[1];
                    if (File.Exists(filePathFormMainArgs))
                    {
                        _path = filePathFormMainArgs;
                    }
                }
                else if (arguments[1] == "-f" && arguments[2].EndsWith(".apk"))
                {
                    string filePathFormMainArgs = arguments[2];
                    if (File.Exists(filePathFormMainArgs))
                    {
                        _path = filePathFormMainArgs;
                    }
                }
            }

            //ModernWpf.MessageBox.Show(string.Join('\n',arguments), "Arguments", MessageBoxButton.OK);

            Provider = new InstallViewModel(_path, this);
            DataContext = Provider;
            await Provider.Refresh();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Provider?.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as FrameworkElement).Name)
            {
                case "ActionButton":
                    Provider?.InstallAPP();
                    break;
                case "DownloadButton":
                    Provider?.LoadNetAPK();
                    break;
                case "FileSelectButton":
                    Provider?.OpenAPK();
                    break;
                case "DeviceSelectButton":
                    _ = Frame.Navigate(typeof(SettingsPage));
                    break;
                case "SecondaryActionButton":
                    Provider?.OpenAPP();
                    break;
                case "CancelOperationButton":
                    Application.Current.Shutdown();
                    break;
            }
        }
    }
}
