﻿using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.DeviceCommands;
using APKInstaller.Controls;
using APKInstaller.Helpers;
using APKInstaller.ViewModel.ToolPages;
using ModernWpf;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ListView = ModernWpf.Controls.ListView;
using Page = ModernWpf.Controls.Page;

namespace APKInstaller.Pages.ToolPages
{
    /// <summary>
    /// ApplicationsPage.xaml 的交互逻辑
    /// </summary>
    public partial class ApplicationsPage : Page
    {
        private ApplicationsViewModel? Provider;

        public ApplicationsPage() => InitializeComponent();

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Provider = new ApplicationsViewModel(this);
            DataContext = Provider;
            ADBHelper.Monitor.DeviceChanged += OnDeviceChanged;
        }

        private void OnDeviceChanged(object sender, DeviceDataEventArgs e) => this.RunOnUIThread(() => Provider.GetDevices());

        private void TitleBar_BackRequested(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private async void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TitleBar.ShowProgressRing();
            int index = Provider.DeviceComboBox.SelectedIndex;
            PackageManager manager = new PackageManager(new AdvancedAdbClient(), Provider.devices[Provider.DeviceComboBox.SelectedIndex]);
            Provider.Applications = await Task.Run(() => { return Provider.CheckAPP(manager.Packages, index); });
            TitleBar.HideProgressRing();
        }

        private async void TitleBar_RefreshEvent(object sender, RoutedEventArgs e) => await Provider.Refresh();

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement Button = sender as FrameworkElement;
            switch (Button.Name)
            {
                case "More":
                    Setting Setting = Button.FindAscendant<Setting>();
                    Setting.ContextMenu.Placement = PlacementMode.Top;
                    Setting.ContextMenu.PlacementTarget = Button;
                    Setting.ContextMenu.IsOpen = (Button.Tag is bool IsOpen && IsOpen) ? false : true;
                    Button.Tag = Setting.ContextMenu.IsOpen;
                    break;
                case "Stop":
                    new AdvancedAdbClient().StopApp(Provider.devices[Provider.DeviceComboBox.SelectedIndex], Button.Tag.ToString());
                    break;
                case "Start":
                    new AdvancedAdbClient().StartApp(Provider.devices[Provider.DeviceComboBox.SelectedIndex], Button.Tag.ToString());
                    break;
                case "Uninstall":
                    break;
            }
        }

        private async void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            Provider.DeviceComboBox = sender as ComboBox;
            await Task.Run(() => this.RunOnUIThread(() => Provider.GetDevices()));
        }

        private void Setting_Loaded(object sender, RoutedEventArgs e)
        {
            Setting Setting = sender as Setting;
            ContentPresenter ContentPresenter = Setting.FindAscendant<ContentPresenter>();
            if (ContentPresenter != null)
            {
                ContentPresenter.HorizontalAlignment = HorizontalAlignment.Stretch;
            }
        }
    }
}
