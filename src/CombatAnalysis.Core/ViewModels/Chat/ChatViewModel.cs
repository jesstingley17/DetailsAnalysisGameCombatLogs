using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Exceptions;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Services;
using CombatAnalysis.Core.Models.Chat;
using CombatAnalysis.Core.Models.User;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels.Chat;

public class ChatViewModel : ParentTemplate
{
    private readonly ILogger<ChatViewModel> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly IGroupChatService _groupChatService;
    private readonly IPersonalChatService _personalChatService;
    private readonly IUserService _userService;
    private readonly IChatHubHelper _personalChatHubConnection;
    private readonly IChatHubHelper _groupChatHubConnection;

    private bool _isChatSelected;
    private IImprovedMvxViewModel? _personalChatMessagesTemplate;
    private IImprovedMvxViewModel? _groupChatMessagesTemplate;
    private MvxObservableCollection<GroupChatViewModel> _myGroupChats = [];
    private ObservableCollection<PersonalChatViewModel> _personalChats = [];
    private MvxObservableCollection<AppUserModel>? _users;
    private List<AppUserModel>? _allUsers;
    private string? _inputedUsername;
    private int _selectedUsersIndex = -1;
    private GroupChatViewModel? _selectedMyGroupChat;
    private PersonalChatViewModel? _selectedPersonalChat;
    private AppUserModel? _myAccount;
    private LoadingStatus _groupChatLoadingResponse;
    private LoadingStatus _personalChatLoadingResponse;

    public ChatViewModel(IMemoryCache memoryCache, ILogger<ChatViewModel> logger, IChatHubHelper personalChatHubConnection,
        IChatHubHelper groupChatHubConnection, IGroupChatService groupCatService, IUserService userService, 
        IPersonalChatService personalChatService)
    {
        _memoryCache = memoryCache;
        _logger = logger;
        _personalChatHubConnection = personalChatHubConnection;
        _groupChatHubConnection = groupChatHubConnection;
        _groupChatService = groupCatService;
        _userService = userService;
        _personalChatService = personalChatService;

        RefreshGroupChatsCommand = new MvxAsyncCommand(LoadGroupChatsAsync);
        RefreshPersonalChatsCommand = new MvxAsyncCommand(LoadPersonalChatsAsync);
        CreatePersonalChatCommand = new MvxAsyncCommand(CreatePersonalChatAsync);

        Basic.Parent = this;
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), -2);

        GetMyAccount();
    }

    #region Commands

    public IMvxAsyncCommand RefreshGroupChatsCommand { get; set; }

    public IMvxAsyncCommand CreatePersonalChatCommand { get; set; }

    public IMvxAsyncCommand RefreshPersonalChatsCommand { get; set; }

    #endregion

    #region View model properties

    public bool IsChatSelected
    {
        get { return _isChatSelected; }

        set
        {
            SetProperty(ref _isChatSelected, value);
        }
    }

    public IImprovedMvxViewModel? PersonalChatMessagesTemplate
    {
        get { return _personalChatMessagesTemplate; }

        set
        {
            SetProperty(ref _personalChatMessagesTemplate, value);
        }
    }

    public IImprovedMvxViewModel? GroupChatMessagesTemplate
    {
        get { return _groupChatMessagesTemplate; }

        set
        {
            SetProperty(ref _groupChatMessagesTemplate, value);
        }
    }

    public MvxObservableCollection<AppUserModel>? Users
    {
        get { return _users; }
        set
        {
            SetProperty(ref _users, value);
        }
    }

    public MvxObservableCollection<GroupChatViewModel> MyGroupChats
    {
        get { return _myGroupChats; }
        set
        {
            SetProperty(ref _myGroupChats, value);
        }
    }

    public ObservableCollection<PersonalChatViewModel> MyPersonalChats
    {
        get { return _personalChats; }
        set
        {
            SetProperty(ref _personalChats, value);
        }
    }

    public int SelectedUsersIndex
    {
        get { return _selectedUsersIndex; }
        set
        {
            SetProperty(ref _selectedUsersIndex, value);
        }
    }

    public string? InputedUsername
    {
        get { return _inputedUsername; }
        set
        {
            SetProperty(ref _inputedUsername, value);
            if (!string.IsNullOrEmpty(value))
            {
                LoadAppUserUsernameByStartChars(value);
            }
        }
    }

    public GroupChatViewModel? SelectedMyGroupChat
    {
        get { return _selectedMyGroupChat; }
        set
        {
            SetProperty(ref _selectedMyGroupChat, value);

            if (value != null)
            {
                IsChatSelected = true;
                SelectedPersonalChat = null;

                PersonalChatMessagesTemplate?.ViewDestroy();
                PersonalChatMessagesTemplate = null;

                GroupChatMessagesTemplate?.ViewDestroy();

                GroupChatMessagesTemplate = Mvx.IoCProvider?.IoCConstruct<GroupChatMessagesViewModel>();
                GroupChatMessagesTemplate?.Handler.PropertyUpdate<GroupChatMessagesViewModel>(GroupChatMessagesTemplate, nameof(GroupChatMessagesViewModel.SelectedChat), value);

                if (GroupChatMessagesTemplate is GroupChatMessagesViewModel groupChatMessagesVewModel)
                {
                    Task.Run(async () => await groupChatMessagesVewModel.InitChatSignalRAsync(_groupChatHubConnection));
                }
            }
        }
    }

    public PersonalChatViewModel? SelectedPersonalChat
    {
        get { return _selectedPersonalChat; }
        set
        {
            SetProperty(ref _selectedPersonalChat, value);

            if (value != null)
            {
                IsChatSelected = true;
                SelectedMyGroupChat = null;

                GroupChatMessagesTemplate?.ViewDestroy();
                GroupChatMessagesTemplate = null;

                PersonalChatMessagesTemplate?.ViewDestroy();

                PersonalChatMessagesTemplate = Mvx.IoCProvider?.IoCConstruct<PersonalChatMessagesVewModel>();
                PersonalChatMessagesTemplate?.Handler.PropertyUpdate<PersonalChatMessagesVewModel>(PersonalChatMessagesTemplate, nameof(PersonalChatMessagesVewModel.SelectedChat), value);

                if (PersonalChatMessagesTemplate is PersonalChatMessagesVewModel personalChatMessagesVewModel)
                {
                    Task.Run(async () => await personalChatMessagesVewModel.InitChatSignalRAsync(_personalChatHubConnection));
                }
            }
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

    public LoadingStatus GroupChatLoadingResponse
    {
        get { return _groupChatLoadingResponse; }
        set
        {
            SetProperty(ref _groupChatLoadingResponse, value);

            RaisePropertyChanged(() => IsLoadMyGroupChatList);
        }
    }

    public LoadingStatus PersonalChatLoadingResponse
    {
        get { return _personalChatLoadingResponse; }
        set
        {
            SetProperty(ref _personalChatLoadingResponse, value);

            RaisePropertyChanged(() => IsLoadPersonalChatList);
        }
    }

    public bool IsLoadMyGroupChatList
    {
        get
        {
            return (int)GroupChatLoadingResponse > 0 && (int)GroupChatLoadingResponse < 3
                        && MyGroupChats?.Count > 0;
        }
    }

    public bool IsLoadPersonalChatList
    {
        get
        {
            return (int)PersonalChatLoadingResponse > 0 && (int)PersonalChatLoadingResponse < 3
                        && MyPersonalChats?.Count > 0;
        }
    }

    #endregion

    #region Command Actions

    private async Task LoadGroupChatsAsync()
    {
        try
        {
            ArgumentNullException.ThrowIfNull(MyAccount, nameof(MyAccount));

            GroupChatLoadingResponse = LoadingStatus.Pending;

            IsChatSelected = false;

            var groupChatUsers = await _groupChatService.LoadChatUsersByUserIdAsync(MyAccount.Id);
            foreach (var user in groupChatUsers)
            {
                await SubscribeToGroupChatUnreadMessagesAsync(user.Id, user.ChatId);
            }

            var groupChats = await _groupChatService.LoadChatsAsync(groupChatUsers);

            await InvokeOnMainThreadAsync(() =>
            {
                MyGroupChats.Clear();
            });

            foreach (var groupChat in groupChats)
            {
                await InvokeOnMainThreadAsync(() =>
                {
                    MyGroupChats.Add(new GroupChatViewModel(groupChat));
                });
            }

            GroupChatLoadingResponse = LoadingStatus.Successful;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to load group chats: Parameter '{ParamName}' was null.", ex.ParamName);

            GroupChatLoadingResponse = LoadingStatus.Failed;
        }
        catch (ChatServiceException)
        {
            GroupChatLoadingResponse = LoadingStatus.Failed;
        }
        catch (Exception)
        {
            GroupChatLoadingResponse = LoadingStatus.Failed;

            throw;
        }
    }

    private async Task LoadPersonalChatsAsync()
    {
        try
        {
            ArgumentNullException.ThrowIfNull(MyAccount, nameof(MyAccount));

            PersonalChatLoadingResponse = LoadingStatus.Pending;

            IsChatSelected = false;

            var personalChats = await _personalChatService.LoadPersonalChatsAsync(MyAccount.Id);
            foreach (var chat in personalChats)
            {
                await SubscribeToPersonalChatUnreadMessagesAsync(chat.Id);
            }

            await InvokeOnMainThreadAsync(() =>
            {
                MyPersonalChats.Clear();
            });

            foreach (var personalChat in personalChats)
            {
                await InvokeOnMainThreadAsync(() =>
                {
                    MyPersonalChats.Add(new PersonalChatViewModel(personalChat));
                });
            }

            await LoadUsersAsync();

            PersonalChatLoadingResponse = LoadingStatus.Successful;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to load personal chats: Parameter '{ParamName}' was null.", ex.ParamName);

            PersonalChatLoadingResponse = LoadingStatus.Failed;
        }
        catch (ChatServiceException)
        {
            GroupChatLoadingResponse = LoadingStatus.Failed;
        }
        catch (Exception)
        {
            PersonalChatLoadingResponse = LoadingStatus.Failed;

            throw;
        }
    }

    private async Task CreatePersonalChatAsync()
    {
        try
        {
            ArgumentNullException.ThrowIfNull(Users, nameof(Users));
            ArgumentNullException.ThrowIfNull(MyAccount, nameof(MyAccount));

            InputedUsername = string.Empty;

            var targetUser = Users[SelectedUsersIndex];

            await _personalChatService.CreateNewPersonalChatAsync(MyAccount.Id, targetUser.Id);

            await LoadPersonalChatsAsync();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to create a new personal chat: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    #endregion

    public override void Prepare()
    {
        base.Prepare();

        Task.Run(async () => {
            await InitChatSignalRAsync();
            await LoadGroupChatsAsync();
            await LoadPersonalChatsAsync();
        });
    }

    public override void ViewDestroy(bool viewFinishing = true)
    {
        if (_personalChatHubConnection != null)
        {
            Task.Run(_personalChatHubConnection.StopAsync);
        }

        if (_groupChatHubConnection != null)
        {
            Task.Run(_groupChatHubConnection.StopAsync);
        }

        base.ViewDestroy(viewFinishing);
    }

    private async Task SubscribeToGroupChatUnreadMessagesAsync(string groupChatUserId, int chatId)
    {
        ArgumentNullException.ThrowIfNull(_groupChatHubConnection, nameof(_groupChatHubConnection));

        await _groupChatHubConnection.JoinUnreadMessagesRoomAsync(chatId);
        _groupChatHubConnection.SubscribeUnreadMessagesUpdated(async (chatId, meInChatId, count) =>
        {
            if (meInChatId == groupChatUserId)
            {
                await UpdateGroupChatUnreadMessagesAsync(chatId, count);
            }
        });
    }

    private async Task SubscribeToPersonalChatUnreadMessagesAsync(int chatId)
    {
        ArgumentNullException.ThrowIfNull(MyAccount, nameof(MyAccount));
        ArgumentNullException.ThrowIfNull(_personalChatHubConnection, nameof(_personalChatHubConnection));

        await _personalChatHubConnection.JoinUnreadMessagesRoomAsync(chatId);
        _personalChatHubConnection.SubscribeUnreadMessagesUpdated(async (chatId, meInChatId, count) =>
        {
            if (MyAccount.Id == meInChatId)
            {
                await UpdatePersonalChatUnreadMessagesAsync(chatId, count);
            }
        });
    }

    private async Task UpdatePersonalChatUnreadMessagesAsync(int chatId, int count)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(MyPersonalChats, nameof(MyPersonalChats));

            var chat = MyPersonalChats.FirstOrDefault(x => x.Id == chatId);
            ArgumentNullException.ThrowIfNull(chat, nameof(chat));

            var index = MyPersonalChats.IndexOf(chat);
            await InvokeOnMainThreadAsync(() =>
            {
                MyPersonalChats[index].CurrentUnreadMessages = count;
            });
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to update personal chat unread messages: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    private async Task UpdateGroupChatUnreadMessagesAsync(int chatId, int count)
    {
        ArgumentNullException.ThrowIfNull(MyGroupChats, nameof(MyGroupChats));

        var chat = MyGroupChats.FirstOrDefault(x => x.Id == chatId);
        ArgumentNullException.ThrowIfNull(chat, nameof(chat));

        var index = MyGroupChats.IndexOf(chat);
        await InvokeOnMainThreadAsync(() =>
        {
            MyGroupChats[index].UnreadMessages = count;
        });
    }

    private async Task LoadUsersAsync()
    {
        try
        {
            ArgumentNullException.ThrowIfNull(MyPersonalChats, nameof(MyPersonalChats));

            var users = await _userService.LoadUsersAsync();

            _allUsers = [.. users];

            var freeUsers = _allUsers?
                .Where(user => !MyPersonalChats.Any(chat => chat.InitiatorId == user.Id || chat.CompanionId == user.Id))
                .Where(user => user.Id != MyAccount?.Id)
                .ToList();
            ArgumentNullException.ThrowIfNull(freeUsers, nameof(freeUsers));

            Users = new MvxObservableCollection<AppUserModel>(freeUsers);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to load users: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    private void LoadAppUserUsernameByStartChars(string username)
    {
        ArgumentNullException.ThrowIfNull(_allUsers, nameof(_allUsers));

        if (string.IsNullOrEmpty(username))
        {
            Users = new MvxObservableCollection<AppUserModel>(_allUsers);

            return;
        }

        var usernameByStartChars = _allUsers.Where(x => x.Username.StartsWith(username));
        if (usernameByStartChars == null)
        {
            return;
        }

        Users = new MvxObservableCollection<AppUserModel>([.. usernameByStartChars]);
    }

    private void GetMyAccount()
    {
        MyAccount = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
    }

    private async Task InitChatSignalRAsync()
    {
        try
        {
            ArgumentNullException.ThrowIfNull(MyAccount, nameof(MyAccount));

            await _personalChatHubConnection.ConnectToChatHubAsync($"{Hubs.Server}{Hubs.PersonalChatAddress}");
            await _personalChatHubConnection.JoinChatRoomAsync(MyAccount.Id);
            await _groupChatHubConnection.ConnectToUnreadMessagesHubAsync($"{Hubs.Server}{Hubs.PersonalChatUnreadMessageAddress}");
            _personalChatHubConnection.SubscribeToChat<PersonalChatModel>("ReceivePersonalChat", async (chat) =>
            {
                await _personalChatService.UpdatePersonalChatAsync(chat, MyAccount.Id);
                await InvokeOnMainThreadAsync(() =>
                {
                    MyPersonalChats.Add(new PersonalChatViewModel(chat));
                });
            });

            await _groupChatHubConnection.ConnectToChatHubAsync($"{Hubs.Server}{Hubs.GroupChatAddress}");
            await _groupChatHubConnection.JoinChatRoomAsync(MyAccount.Id);
            await _groupChatHubConnection.ConnectToUnreadMessagesHubAsync($"{Hubs.Server}{Hubs.GroupChatUnreadMessageAddress}");
            _groupChatHubConnection.SubscribeToChat<GroupChatUserModel>("ReceiveJoinedUser", async (user) =>
            {
                var groupChat = await _groupChatService.LoadChatAsync(user);
                await InvokeOnMainThreadAsync(() =>
                {
                    MyGroupChats.Add(new GroupChatViewModel(groupChat));
                });
            });
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Failed to init group and personal chat signals: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }
}
