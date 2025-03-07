﻿using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Telegram.Td.Api;
using Unigram.Collections;
using Unigram.Common;
using Unigram.Controls;
using Unigram.Services;
using Unigram.Views.Popups;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Unigram.ViewModels.Settings
{
    public class SettingsBlockedChatsViewModel : TLViewModelBase
    {
        public SettingsBlockedChatsViewModel(IProtoService protoService, ICacheService cacheService, ISettingsService settingsService, IEventAggregator aggregator)
            : base(protoService, cacheService, settingsService, aggregator)
        {
            Items = new ItemsCollection(protoService);

            BlockCommand = new RelayCommand(BlockExecute);
            UnblockCommand = new RelayCommand<MessageSender>(UnblockExecute);
        }

        public ObservableCollection<MessageSender> Items { get; private set; }

        public RelayCommand BlockCommand { get; }
        private async void BlockExecute()
        {
            var selected = await SharePopup.PickChatAsync(Strings.Resources.BlockUser);
            if (selected == null)
            {
                return;
            }

            if (selected.Type is ChatTypePrivate privata)
            {
                Items.Insert(0, new MessageSenderUser(privata.UserId));
                ProtoService.Send(new ToggleMessageSenderIsBlocked(new MessageSenderUser(privata.UserId), true));
            }
            else
            {
                Items.Insert(0, new MessageSenderChat(selected.Id));
                ProtoService.Send(new ToggleMessageSenderIsBlocked(new MessageSenderChat(selected.Id), true));
            }
        }

        public RelayCommand<MessageSender> UnblockCommand { get; }
        private async void UnblockExecute(MessageSender sender)
        {
            var confirm = await MessagePopup.ShowAsync(Strings.Resources.AreYouSureUnblockContact, Strings.Resources.AppName, Strings.Resources.OK, Strings.Resources.Cancel);
            if (confirm == ContentDialogResult.Primary)
            {
                Items.Remove(sender);
                ProtoService.Send(new ToggleMessageSenderIsBlocked(sender, false));
            }
        }

        public class ItemsCollection : MvxObservableCollection<MessageSender>, ISupportIncrementalLoading
        {
            private readonly IProtoService _protoService;

            public ItemsCollection(IProtoService protoService)
            {
                _protoService = protoService;
            }

            public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
            {
                return AsyncInfo.Run(async task =>
                {
                    var response = await _protoService.SendAsync(new GetBlockedMessageSenders(Count, 20));
                    if (response is MessageSenders chats)
                    {
                        foreach (var sender in chats.Senders)
                        {
                            Add(sender);
                        }

                        return new LoadMoreItemsResult { Count = (uint)chats.Senders.Count };
                    }

                    return new LoadMoreItemsResult();
                });
            }

            public bool HasMoreItems => true;
        }
    }
}
