using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Exceptions;
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

public class GroupChatMessagesViewModel : MvxViewModel, IImprovedMvxViewModel
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<GroupChatMessagesViewModel> _logger;
    private readonly IGroupChatService _groupChatService;
    private readonly IUserService _userService;

    private ObservableCollection<GroupChatMessageViewModel>? _messages;
    private List<GroupChatMessageViewModel>? _allMessages;
    private ObservableCollection<AppUserModel>? _usersToInviteToChat;
    private ObservableCollection<AppUserModel>? _users;
    private List<AppUserModel>? freeUsersToInvite;
    private GroupChatViewModel? _selectedChat;
    private string _meInChatId = string.Empty;
    private GroupChatMessageModel? _selectedMessage;
    private int _selectedMessageIndex = -1;
    private string? _selectedChatName;
    private string? _message;
    private bool _chatMenuIsVisibly;
    private AppUserModel? _myAccount;
    private LoadingStatus _addUserToChatResponse;
    private int _selectedUsersForInviteToGroupChatIndex = -1;
    private string? _inputedUserEmailForInviteToChat;
    private bool _inviteToChatIsVisibly;
    private bool _isEditMode;
    private bool _isRemoveMode;
    private IChatHubHelper? _hubConnection;

    public GroupChatMessagesViewModel(IMemoryCache memoryCache, ILogger<GroupChatMessagesViewModel> logger, IGroupChatService groupChatService, 
        IUserService userService)
    {
        Handler = new VMHandler<GroupChatMessagesViewModel>();
        Parent = this;
        SavedViewModel = this;

        _memoryCache = memoryCache;
        _logger = logger;
        _groupChatService = groupChatService;
        _userService = userService;

        SendMessageCommand = new MvxAsyncCommand(SendMessageAsync);
        MessageHasBeenReadCommand = new MvxAsyncCommand<GroupChatMessageViewModel>(SendMessageHasBeenReadAsync);
        SendMessageKeyDownCommand = new MvxAsyncCommand<string>(SendMessageKeyDownAsync);

        ShowChatMenuCommand = new MvxCommand(() => ChatMenuIsVisibly = !ChatMenuIsVisibly);
        OpenInviteToChatCommand = new MvxAsyncCommand(OpenInviteToChatAsync);
        InviteToChatCommand = new MvxAsyncCommand(InviteToChatAsync);
        CloseInviteToChatCommand = new MvxCommand(SwitchInviteToGroupChat);
        TurnOnEditModeCommand = new MvxCommand(() => IsEditMode = !IsEditMode);
        EditMessageCommand = new MvxAsyncCommand(EditMessageAsync);
        RemoveMessageCommand = new MvxAsyncCommand(RemoveMessageAsync);

        Messages = [];
        _allMessages = [];

        Task.Run(LoadUsersAsync);
    }

    public IVMHandler Handler { get; set; }

    public IMvxViewModel Parent { get; set; }

    public IMvxViewModel SavedViewModel { get; set; }

    #region Commands

    public IMvxAsyncCommand SendMessageCommand { get; set; }

    public IMvxAsyncCommand<GroupChatMessageViewModel> MessageHasBeenReadCommand { get; set; }

    public IMvxAsyncCommand<string> SendMessageKeyDownCommand { get; set; }

    public IMvxCommand ShowChatMenuCommand { get; set; }

    public IMvxAsyncCommand OpenInviteToChatCommand { get; set; }

    public IMvxAsyncCommand InviteToChatCommand { get; set; }

    public IMvxCommand CloseInviteToChatCommand { get; set; }

    public IMvxCommand TurnOnEditModeCommand { get; set; }

    public IMvxAsyncCommand EditMessageCommand { get; set; }

    public IMvxAsyncCommand RemoveMessageCommand { get; set; }

    #endregion

    #region View model properties

    public string MeInChatId
    {
        get { return _meInChatId; }
        set
        {
            SetProperty(ref _meInChatId, value);
        }
    }

    public ObservableCollection<AppUserModel>? Users
    {
        get { return _users; }
        set
        {
            SetProperty(ref _users, value);
        }
    }

    public ObservableCollection<GroupChatMessageViewModel>? Messages
    {
        get { return _messages; }
        set
        {
            SetProperty(ref _messages, value);
        }
    }

    public GroupChatViewModel? SelectedChat
    {
        get { return _selectedChat; }
        set
        {
            SetProperty(ref _selectedChat, value);
        }
    }

    public GroupChatMessageModel? SelectedMessage
    {
        get { return _selectedMessage; }
        set
        {
            SetProperty(ref _selectedMessage, value);
        }
    }

    public int SelectedMessageIndex
    {
        get { return _selectedMessageIndex; }
        set
        {
            SetProperty(ref _selectedMessageIndex, value);
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

    public bool ChatMenuIsVisibly
    {
        get { return _chatMenuIsVisibly; }
        set
        {
            SetProperty(ref _chatMenuIsVisibly, value);
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

    public LoadingStatus AddUserToChatResponse
    {
        get { return _addUserToChatResponse; }
        set
        {
            SetProperty(ref _addUserToChatResponse, value);
        }
    }

    public ObservableCollection<AppUserModel>? UsersToInviteToChat
    {
        get { return _usersToInviteToChat; }
        set
        {
            SetProperty(ref _usersToInviteToChat, value);
        }
    }

    public bool InviteToChatIsVisibly
    {
        get { return _inviteToChatIsVisibly; }
        set
        {
            SetProperty(ref _inviteToChatIsVisibly, value);
        }
    }

    public int SelectedUsersForInviteToGroupChatIndex
    {
        get { return _selectedUsersForInviteToGroupChatIndex; }
        set
        {
            SetProperty(ref _selectedUsersForInviteToGroupChatIndex, value);
        }
    }

    public string? InputedUserEmailForInviteToChat
    {
        get { return _inputedUserEmailForInviteToChat; }
        set
        {
            SetProperty(ref _inputedUserEmailForInviteToChat, value);
            if (value != null)
            {
                LoadUsernamesForInviteByStartChars(value);
            }
        }
    }

    public bool IsEditMode
    {
        get { return _isEditMode; }
        set
        {
            SetProperty(ref _isEditMode, value);
        }
    }

    public bool IsRemoveMode
    {
        get { return _isRemoveMode; }
        set
        {
            SetProperty(ref _isRemoveMode, value);
        }
    }

    #endregion

    #region Command Actions

    private async Task OpenInviteToChatAsync()
    {
        AddUserToChatResponse = LoadingStatus.None;

        try
        {
            ArgumentNullException.ThrowIfNull(Users, nameof(Users));

            var freeUsers = await _groupChatService.GetFreeUsersToInviteAsync([.. Users]);

            freeUsersToInvite = [.. freeUsers];

            UsersToInviteToChat = new ObservableCollection<AppUserModel>(freeUsersToInvite);

            SwitchInviteToGroupChat();

            AddUserToChatResponse = LoadingStatus.Successful;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to open invite to group chat: Parameter '{ParamName}' was null.", ex.ParamName);

            AddUserToChatResponse = LoadingStatus.Failed;
        }
        catch (ChatServiceException)
        {
            AddUserToChatResponse = LoadingStatus.Failed;
        }
        catch (Exception)
        {
            AddUserToChatResponse = LoadingStatus.Failed;

            throw;
        }
    }

    private async Task InviteToChatAsync()
    {
        try
        {
            ArgumentNullException.ThrowIfNull(SelectedChat, nameof(SelectedChat));
            ArgumentNullException.ThrowIfNull(UsersToInviteToChat, nameof(UsersToInviteToChat));

            AddUserToChatResponse = LoadingStatus.Pending;

            InputedUserEmailForInviteToChat = string.Empty;

            var userId = UsersToInviteToChat[SelectedUsersForInviteToGroupChatIndex].Id;

            await _groupChatService.InviteToChatAsync(SelectedChat.Id, userId);

            SwitchInviteToGroupChat();

            AddUserToChatResponse = LoadingStatus.Successful;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to invite to group chat: Parameter '{ParamName}' was null.", ex.ParamName);

            AddUserToChatResponse = LoadingStatus.Failed;
        }
        catch (ChatServiceException)
        {
            AddUserToChatResponse = LoadingStatus.Failed;
        }
        catch (Exception)
        {
            AddUserToChatResponse = LoadingStatus.Failed;

            throw;
        }
    }

    private void SwitchInviteToGroupChat()
    {
        InviteToChatIsVisibly = !InviteToChatIsVisibly;
    }

    private async Task EditMessageAsync()
    {
        try
        {
            ArgumentNullException.ThrowIfNull(SelectedMessage, nameof(SelectedMessage));

            await _groupChatService.EditChatMessageAsync(SelectedMessage);

            IsEditMode = false;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to edit group chat message: Parameter '{ParamName}' was null.", ex.ParamName);

            AddUserToChatResponse = LoadingStatus.Failed;
        }
    }

    private async Task RemoveMessageAsync()
    {
        try
        {
            ArgumentNullException.ThrowIfNull(SelectedMessage, nameof(SelectedMessage));
            ArgumentNullException.ThrowIfNull(Messages, nameof(Messages));

            await _groupChatService.RemoveMessageAsync(SelectedMessage.Id);

            await InvokeOnMainThreadAsync(() =>
            {
                Messages.Remove(Messages[SelectedMessageIndex]);
                SelectedMessage = null;
            });
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to remove group chat message: Parameter '{ParamName}' was null.", ex.ParamName);
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
            ArgumentNullException.ThrowIfNull(hubConnection, nameof(hubConnection));
            ArgumentNullException.ThrowIfNull(Messages, nameof(Messages));

            await GetUserInGroupChatAsync();
            await LoadMessagesForSelectedChatAsync(SelectedChat?.Name ?? string.Empty);

            _hubConnection = hubConnection;

            ArgumentNullException.ThrowIfNullOrEmpty(MeInChatId, nameof(MeInChatId));
            ArgumentNullException.ThrowIfNull(SelectedChat, nameof(SelectedChat));

            await hubConnection.ConnectToChatMessagesHubAsync($"{Hubs.Server}{Hubs.GroupChatMessagesAddress}");
            await hubConnection.JoinChatMessagesRoomAsync(SelectedChat.Id);

            hubConnection.SubscribeMessagesUpdated<GroupChatMessageModel>(SelectedChat.Id, MeInChatId, async (message) =>
            {
                await InvokeOnMainThreadAsync(() =>
                {
                    Messages.Insert(0, new GroupChatMessageViewModel(message));
                });
            });

            hubConnection.SubscribeReceiveMessageHasBeenRead<int>(async (messageId) =>
            {
                await InvokeOnMainThreadAsync(async () =>
                {
                    await _groupChatService.LoadUnreadMessagesAsync(messageId);
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

    private async Task SendMessageHasBeenReadAsync(GroupChatMessageViewModel? message)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(message, nameof(message));
            ArgumentNullException.ThrowIfNull(_hubConnection, nameof(_hubConnection));
            ArgumentNullException.ThrowIfNullOrEmpty(MeInChatId, nameof(MeInChatId));

            if (message.GroupChatUserId == MeInChatId || message.IsRead)
            {
                return;
            }

            await _hubConnection.SubscribeMessageHasBeenReadAsync(message.Id, MeInChatId);
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

    private async Task SendMessageAsync()
    {
        try
        {
            ArgumentNullException.ThrowIfNull(Message, nameof(Message));
            ArgumentNullException.ThrowIfNull(SelectedChat, nameof(SelectedChat));
            ArgumentNullException.ThrowIfNull(MyAccount, nameof(MyAccount));
            ArgumentNullException.ThrowIfNull(_hubConnection, nameof(_hubConnection));
            ArgumentNullException.ThrowIfNullOrEmpty(MeInChatId, nameof(MeInChatId));

            await _hubConnection.SendMessageAsync(Message, SelectedChat.Id, MeInChatId, MyAccount.Username, 0);

            Message = string.Empty;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to send message: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    private async Task LoadMessagesForSelectedChatAsync(string chatName)
    {
        await InvokeOnMainThreadAsync(() =>
        {
            Messages?.Clear();
        });

        SelectedChatName = chatName;

        await LoadMessagesAsync();
    }

    private async Task LoadMessagesAsync()
    {
        try
        {
            ArgumentNullException.ThrowIfNull(SelectedChat, nameof(SelectedChat));
            ArgumentNullException.ThrowIfNullOrEmpty(MeInChatId, nameof(MeInChatId));

            var messages = await _groupChatService.LoadMessagesAsync(SelectedChat.Id, MeInChatId);

            _allMessages = [];
            foreach (var item in messages)
            {
                _allMessages.Add(new GroupChatMessageViewModel(item));
            }

            await LoadUnreadMessagesAsync();
            await FillAsync();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to load group chat messages: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    private async Task LoadUnreadMessagesAsync()
    {
        ArgumentNullException.ThrowIfNull(_allMessages, nameof(_allMessages));

        foreach (var message in _allMessages)
        {
            var unreadGroupChatMessages = await _groupChatService.LoadUnreadMessagesAsync(message.Id);
            if (!unreadGroupChatMessages.Any())
            {
                message.IsRead = true;
            }
        }
    }

    private async Task LoadUsersAsync()
    {
        var users = await _userService.LoadUsersAsync();

        Users = new ObservableCollection<AppUserModel>([.. users.Where(x => x.Id != MyAccount?.Id)]);
    }

    private void LoadUsernamesForInviteByStartChars(string startChars)
    {
        if (string.IsNullOrEmpty(startChars))
        {
            UsersToInviteToChat = new ObservableCollection<AppUserModel>(freeUsersToInvite ?? []);

            return;
        }

        var usersEmailByStartChars = Users?.Where(x => x.Username.StartsWith(startChars));
        ArgumentNullException.ThrowIfNull(usersEmailByStartChars, nameof(usersEmailByStartChars));

        UsersToInviteToChat = new ObservableCollection<AppUserModel>(usersEmailByStartChars);
    }

    private async Task GetUserInGroupChatAsync()
    {
        try
        {
            MyAccount = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
            ArgumentNullException.ThrowIfNull(MyAccount, nameof(MyAccount));
            ArgumentNullException.ThrowIfNull(SelectedChat, nameof(SelectedChat));

            var userInChat = await _groupChatService.GetUserInGroupChatAsync(SelectedChat.Id, MyAccount.Id);

            MeInChatId = userInChat.Id;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to get user in chat: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }
}
