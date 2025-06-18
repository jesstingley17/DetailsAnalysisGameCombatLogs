using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Extensions;
using CombatAnalysis.Core.Helpers;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Chat;
using CombatAnalysis.Core.Models.User;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.ViewModels.Chat;

public class PersonalChatMessagesVewModel : MvxViewModel, IImprovedMvxViewModel
{
    private readonly IHttpClientHelper _httpClientHelper;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger _logger;

    private ObservableCollection<PersonalChatMessageViewModel>? _messages;
    private List<PersonalChatMessageViewModel>? _allMessages;
    private PersonalChatViewModel? _selectedChat;
    private string? _selectedChatName;
    private string? _message;
    private AppUserModel? _myAccount;
    private IChatHubHelper? _hubConnection;

    public PersonalChatMessagesVewModel(IHttpClientHelper httpClientHelper, IMemoryCache memoryCache, ILogger logger)
    {
        Handler = new VMHandler<PersonalChatMessagesVewModel>();
        Parent = this;
        SavedViewModel = this;

        _httpClientHelper = httpClientHelper;
        _memoryCache = memoryCache;
        _logger = logger;

        SendMessageCommand = new MvxAsyncCommand(SendMessageAsync);
        MessageHasBeenReadCommand = new MvxAsyncCommand<PersonalChatMessageViewModel>(SendMessageHasBeenReadAsync);
        SendMessageKeyDownCommand = new MvxAsyncCommand<string>(SendMessageKeyDownAsync);

        Messages = [];
        _allMessages = [];

        GetMyAccount();
    }

    public IVMHandler Handler { get; set; }

    public IMvxViewModel Parent { get; set; }

    public IMvxViewModel SavedViewModel { get; set; }

    #region Commands

    public IMvxAsyncCommand SendMessageCommand { get; set; }

    public IMvxAsyncCommand<PersonalChatMessageViewModel> MessageHasBeenReadCommand { get; set; }

    public IMvxAsyncCommand<string> SendMessageKeyDownCommand { get; set; }

    #endregion

    #region View model properties

    public ObservableCollection<PersonalChatMessageViewModel>? Messages
    {
        get { return _messages; }
        set
        {
            SetProperty(ref _messages, value);
        }
    }

    public PersonalChatViewModel? SelectedChat
    {
        get { return _selectedChat; }
        set
        {
            SetProperty(ref _selectedChat, value);

            if (value != null)
            {
                Task.Run(LoadMessagesForSelectedChatAsync);
            }
        }
    }

    public string? SelectedChatName
    {
        get { return _selectedChatName; }
        set
        {
            SetProperty(ref _selectedChatName, value);
        }
    }

    public string? Message
    {
        get { return _message; }
        set
        {
            SetProperty(ref _message, value);
        }
    }

    public AppUserModel? MyAccount
    {
        get { return _myAccount; }
        set
        {
            SetProperty(ref _myAccount, value);
        }
    }

    #endregion

    public override void ViewDestroy(bool viewFinishing = true)
    {
        if (_hubConnection != null)
        {
            Task.Run(async () => await _hubConnection.LeaveFromChatRoomAsync(SelectedChat?.Id ?? 0));
        }

        base.ViewDestroy(viewFinishing);
    }

    public async Task InitChatSignalRAsync(IChatHubHelper hubConnection)
    {
        try
        {
            _hubConnection = hubConnection;

            if (SelectedChat == null)
            {
                throw new ArgumentNullException(nameof(SelectedChat));
            }
            else if (MyAccount == null)
            {
                throw new ArgumentNullException(nameof(MyAccount));
            }

            await hubConnection.ConnectToChatHubAsync($"{Hubs.Server}{Hubs.PersonalChatMessagesAddress}");
            await hubConnection.JoinChatRoomAsync(SelectedChat.Id);

            hubConnection.SubscribeMessagesUpdated<PersonalChatMessageModel>(SelectedChat.Id, MyAccount.Id, async (message) =>
            {
                await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
                {
                    Messages?.Insert(0, new PersonalChatMessageViewModel(message));
                });
            });

            hubConnection?.SubscribeReceiveMessageHasBeenRead<int>(async (messageId) =>
            {
                await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
                {
                    var message = Messages?.FirstOrDefault(x => x.Id == messageId);
                    message.Status = 2;
                });
            });
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private async Task FillAsync()
    {
        await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
        {
            if (_allMessages == null || Messages == null)
            {
                return;
            }

            foreach (var item in _allMessages)
            {
                if (item.ChatId == SelectedChat?.Id
                    && !Messages.Any(x => x.Id == item.Id))
                {
                    Messages.Add(item);
                }
            }
        });
    }

    private async Task SendMessageHasBeenReadAsync(PersonalChatMessageViewModel? message)
    {
        try
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            else if (_hubConnection == null)
            {
                throw new ArgumentNullException(nameof(_hubConnection));
            }
            else if (MyAccount == null)
            {
                throw new ArgumentNullException(nameof(MyAccount));
            }

            if (message.AppUserId == MyAccount.Id)
            {
                return;
            }

            await _hubConnection.SubscribeMessageHasBeenReadAsync(message.Id, MyAccount.Id);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private async Task SendMessageKeyDownAsync(string? message)
    {
        Message = message;

        await SendMessageAsync();
    }

    private async Task SendMessageAsync()
    {
        try
        {
            if (Message == null)
            {
                throw new ArgumentNullException(nameof(Message));
            }
            else if (SelectedChat == null)
            {
                throw new ArgumentNullException(nameof(SelectedChat));
            }
            else if (MyAccount == null)
            {
                throw new ArgumentNullException(nameof(MyAccount));
            }
            else if (_hubConnection == null)
            {
                throw new ArgumentNullException(nameof(_hubConnection));
            }

            await _hubConnection.SendMessageAsync(Message, SelectedChat.Id, MyAccount.Id, MyAccount.Username);

            Message = string.Empty;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private async Task LoadMessagesForSelectedChatAsync()
    {
        await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
        {
            Messages?.Clear();
        });

        await LoadMessagesAsync();
    }

    private async Task LoadMessagesAsync()
    {
        try
        {
            var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            var response = await _httpClientHelper.GetAsync($"PersonalChatMessage/getByChatId?chatId={SelectedChat?.Id}&pageSize=20", refreshToken, API.ChatApi);
            response.EnsureSuccessStatusCode();

            var messages = await response.Content.ReadFromJsonAsync<IEnumerable<PersonalChatMessageModel>>();
            if (messages == null)
            {
                throw new ArgumentNullException(nameof(_allMessages));
            }

            _allMessages = [];

            foreach (var message in messages)
            {
                _allMessages.Add(new PersonalChatMessageViewModel(message));
            }

            await FillAsync();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void GetMyAccount()
    {
        MyAccount = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User)) ?? new AppUserModel();
    }
}
