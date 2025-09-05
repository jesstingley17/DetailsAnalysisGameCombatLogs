using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Helpers;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Services;
using CombatAnalysis.Core.Models.Chat;
using CombatAnalysis.Core.Models.User;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels.Chat;

public class PersonalChatMessagesVewModel : MvxViewModel, IImprovedMvxViewModel
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<PersonalChatMessagesVewModel> _logger;
    private readonly IPersonalChatService _groupChatService;

    private ObservableCollection<PersonalChatMessageViewModel>? _messages;
    private List<PersonalChatMessageViewModel>? _allMessages;
    private PersonalChatViewModel? _selectedChat;
    private string? _selectedChatName;
    private string? _message;
    private AppUserModel? _myAccount;
    private IChatHubHelper? _hubConnection;

    public PersonalChatMessagesVewModel(IMemoryCache memoryCache, ILogger<PersonalChatMessagesVewModel> logger, IPersonalChatService groupChatService)
    {
        Handler = new VMHandler<PersonalChatMessagesVewModel>();
        Parent = this;
        SavedViewModel = this;

        _memoryCache = memoryCache;
        _logger = logger;
        _groupChatService = groupChatService;

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

    #region Command Actions

    private async Task SendMessageAsync()
    {
        try
        {
            ArgumentNullException.ThrowIfNull(Message, nameof(Message));
            ArgumentNullException.ThrowIfNull(SelectedChat, nameof(SelectedChat));
            ArgumentNullException.ThrowIfNull(MyAccount, nameof(MyAccount));
            ArgumentNullException.ThrowIfNull(_hubConnection, nameof(_hubConnection));

            await _hubConnection.SendMessageAsync(Message, SelectedChat.Id, MyAccount.Id, MyAccount.Username);

            Message = string.Empty;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to send message: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    private async Task SendMessageHasBeenReadAsync(PersonalChatMessageViewModel? message)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(message, nameof(message));
            ArgumentNullException.ThrowIfNull(_hubConnection, nameof(_hubConnection));
            ArgumentNullException.ThrowIfNull(MyAccount, nameof(MyAccount));

            if (message.AppUserId == MyAccount.Id)
            {
                return;
            }

            await _hubConnection.SubscribeMessageHasBeenReadAsync(message.Id, MyAccount.Id);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to send message has been read: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    private async Task SendMessageKeyDownAsync(string? message)
    {
        Message = message;

        await SendMessageAsync();
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
            ArgumentNullException.ThrowIfNull(hubConnection, nameof(hubConnection));
            ArgumentNullException.ThrowIfNull(SelectedChat, nameof(SelectedChat));
            ArgumentNullException.ThrowIfNull(MyAccount, nameof(MyAccount));
            ArgumentNullException.ThrowIfNull(Messages, nameof(Messages));

            _hubConnection = hubConnection;

            await hubConnection.ConnectToChatHubAsync($"{Hubs.Server}{Hubs.PersonalChatMessagesAddress}");
            await hubConnection.JoinChatRoomAsync(SelectedChat.Id);

            hubConnection.SubscribeMessagesUpdated<PersonalChatMessageModel>(SelectedChat.Id, MyAccount.Id, async (message) =>
            {
                await InvokeOnMainThreadAsync(() =>
                {
                    Messages.Insert(0, new PersonalChatMessageViewModel(message));
                });
            });

            hubConnection.SubscribeReceiveMessageHasBeenRead<int>(async (messageId) =>
            {
                await InvokeOnMainThreadAsync(() =>
                {
                    var message = Messages.FirstOrDefault(x => x.Id == messageId);
                    ArgumentNullException.ThrowIfNull(message, nameof(message));

                    message.Status = 2;
                });
            });
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to init Signal connection: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    private async Task FillAsync()
    {
        await InvokeOnMainThreadAsync(() =>
        {
            ArgumentNullException.ThrowIfNull(_allMessages, nameof(_allMessages));
            ArgumentNullException.ThrowIfNull(Messages, nameof(Messages));

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

    private async Task LoadMessagesForSelectedChatAsync()
    {
        ArgumentNullException.ThrowIfNull(Messages, nameof(Messages));

        await InvokeOnMainThreadAsync(() =>
        {
            Messages.Clear();
        });

        await LoadMessagesAsync();
    }

    private async Task LoadMessagesAsync()
    {
        try
        {
            ArgumentNullException.ThrowIfNull(SelectedChat, nameof(SelectedChat));

            var messages = await _groupChatService.LoadMessagesAsync(SelectedChat.Id);

            _allMessages = [];
            foreach (var message in messages)
            {
                _allMessages.Add(new PersonalChatMessageViewModel(message));
            }

            await FillAsync();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to load personal chat messages: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    private void GetMyAccount()
    {
        MyAccount = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User)) ?? new AppUserModel();
    }
}
