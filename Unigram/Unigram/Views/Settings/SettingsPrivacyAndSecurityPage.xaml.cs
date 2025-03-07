﻿using Unigram.Common;
using Unigram.ViewModels.Settings;
using Unigram.Views.Settings.Privacy;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Unigram.Views.Settings
{
    public sealed partial class SettingsPrivacyAndSecurityPage : HostedPage
    {
        public SettingsPrivacyAndSecurityViewModel ViewModel => DataContext as SettingsPrivacyAndSecurityViewModel;

        public SettingsPrivacyAndSecurityPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (ApiInfo.IsPackagedRelease && ViewModel.CacheService.Options.CanIgnoreSensitiveContentRestrictions)
            {
                FindName(nameof(SensitiveContent));
            }
        }

        private void Sessions_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsSessionsPage), null, new DrillInNavigationTransitionInfo());
        }

        private void WebSessions_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsWebSessionsPage));
        }

        private void BlockedUsers_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsBlockedChatsPage));
        }

        private void ShowPhone_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPrivacyShowPhonePage));
        }

        private void StatusTimestamp_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPrivacyShowStatusPage));
        }

        private void ProfilePhoto_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPrivacyShowPhotoPage));
        }

        private void Forwards_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPrivacyShowForwardedPage));
        }

        private void PhoneCall_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPrivacyAllowCallsPage));
        }

        private void ChatInvite_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPrivacyAllowChatInvitesPage));
        }

        #region Binding

        private string ConvertOnOff(bool value)
        {
            return value ? Strings.Resources.NotificationsOn : Strings.Resources.NotificationsOff;
        }

        private string ConvertSync(bool sync)
        {
            return sync ? Strings.Resources.SyncContactsInfoOn : Strings.Resources.SyncContactsInfoOff;
        }

        private string ConvertP2P(int mode)
        {
            switch (mode)
            {
                case 0:
                default:
                    return Strings.Resources.LastSeenEverybody;
                case 1:
                    return Strings.Resources.LastSeenContacts;
                case 2:
                    return Strings.Resources.LastSeenNobody;
            }
        }

        private string ConvertTtl(int days)
        {
            if (days >= 365)
            {
                return Locale.Declension("Years", days / 365);
            }

            return Locale.Declension("Months", days / 30);
        }

        #endregion
    }
}
