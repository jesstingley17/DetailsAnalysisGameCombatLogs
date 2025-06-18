using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Extensions;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Chat;
using CombatAnalysis.Core.Models.User;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Commands;
using System;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.ViewModels.Chat;

public class ChatViewModel : ParentTemplate
{
    private readonly IHttpClientHelper _httpClientHelper;
    private readonly ILogger _logger;
    private readonly IMemoryCache _memoryCache;

    private bool _isChatSelected;
    private IImprovedMvxViewModel? _personalChatMessagesTemplate;
    private IImprovedMvxViewModel? _groupChatMessagesTemplate;
    private ObservableCollection<GroupChatViewModel>? _myGroupChats;
    private ObservableCollection<PersonalChatViewModel>? _personalChats;
    private ObservableCollection<AppUserModel>? _users;
    private List<AppUserModel>? _allUsers;
    private string? _inputedUsername;
    private int _selectedUsersIndex = -1;
    private GroupChatViewModel? _selectedMyGroupChat;
    private PersonalChatViewModel? _selectedPersonalChat;
    private AppUserModel? _myAccount;
    private LoadingStatus _groupChatLoadingResponse;
    private LoadingStatus _personalChatLoadingResponse;
    private IChatHubHelper _personalChatHubConnection;
    private IChatHubHelper _groupChatHubConnection;

    public ChatViewModel(IHttpClientHelper httpClientHelper, IMemoryCache memoryCache, ILogger logger, 
        IChatHubHelper personalChatHubConnection, IChatHubHelper groupChatHubConnection)
    {
        _httpClientHelper = httpClientHelper;
        _memoryCache = memoryCache;
        _logger = logger;
        _personalChatHubConnection = personalChatHubConnection;
        _groupChatHubConnection = groupChatHubConnection;

        RefreshGroupChatsCommand = new MvxAsyncCommand(LoadGroupChatsAsync);
        RefreshPersonalChatsCommand = new MvxAsyncCommand(LoadPersonalChatsAsync);
        CreatePersonalChatCommand = new MvxAsyncCommand(CreateNewPersonalChatAsync);

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

    public ObservableCollection<AppUserModel>? Users
    {
        get { return _users; }
        set
        {
            SetProperty(ref _users, value);
        }
    }

    public ObservableCollection<GroupChatViewModel>? MyGroupChats
    {
        get { return _myGroupChats; }
        set
        {
            SetProperty(ref _myGroupChats, value);
        }
    }

    public ObservableCollection<PersonalChatViewModel>? MyPersonalChats
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

                var groupChatMessagesVewModel = GroupChatMessagesTemplate as GroupChatMessagesViewModel;
                if (groupChatMessagesVewModel != null)
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
                
                var personalChatMessagesVewModel = PersonalChatMessagesTemplate as PersonalChatMessagesVewModel;
                if (personalChatMessagesVewModel != null)
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

            RaisePropertyChanged(() => IsShowEmptyMyGroupChat);
            RaisePropertyChanged(() => IsLoadMyGroupChatList);
        }
    }

    public LoadingStatus PersonalChatLoadingResponse
    {
        get { return _personalChatLoadingResponse; }
        set
        {
            SetProperty(ref _personalChatLoadingResponse, value);

            RaisePropertyChanged(() => IsShowEmptyPersonalChat);
            RaisePropertyChanged(() => IsLoadPersonalChatList);
        }
    }

    public bool IsShowEmptyMyGroupChat
    {
        get
        {
            return (int)GroupChatLoadingResponse > 0 && (int)GroupChatLoadingResponse < 3
                        && MyGroupChats?.Count == 0;
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

    public bool IsShowEmptyPersonalChat
    {
        get
        {
            return (int)PersonalChatLoadingResponse > 0 && (int)PersonalChatLoadingResponse < 3
                        && MyPersonalChats?.Count == 0;
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

    private async Task CreateNewPersonalChatAsync()
    {
        try
        {
            if (Users == null)
            {
                throw new ArgumentNullException(nameof(Users));
            }
            else if (MyAccount == null)
            {
                throw new ArgumentNullException(nameof(MyAccount));
            }

            var targetUser = Users[SelectedUsersIndex];
            var personalChat = new PersonalChatModel
            {
                InitiatorId = MyAccount.Id,
                CompanionId = targetUser.Id,
            };

            InputedUsername = string.Empty;

            var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            var response = await _httpClientHelper.PostAsync("PersonalChat", JsonContent.Create(personalChat), refreshToken, API.ChatApi);
            response.EnsureSuccessStatusCode();

            await LoadPersonalChatsAsync();
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

    private async Task LoadGroupChatsAsync()
    {
        GroupChatLoadingResponse = LoadingStatus.Pending;

        IsChatSelected = false;

        try
        {
            var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            var response = await _httpClientHelper.GetAsync($"GroupChatUser/findByUserId/{MyAccount?.Id}", refreshToken, API.ChatApi);
            response.EnsureSuccessStatusCode();

            var myGroupChatUsers = await response.Content.ReadFromJsonAsync<IEnumerable<GroupChatUserModel>>();
            if (myGroupChatUsers == null)
            {
                throw new ArgumentNullException(nameof(myGroupChatUsers));
            }

            await GetMyGroupChatsByChatUserIdAsync(myGroupChatUsers, refreshToken);

            GroupChatLoadingResponse = LoadingStatus.Successful;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            GroupChatLoadingResponse = LoadingStatus.Failed;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);

            GroupChatLoadingResponse = LoadingStatus.Failed;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            GroupChatLoadingResponse = LoadingStatus.Failed;
        }
    }

    private async Task LoadPersonalChatsAsync()
    {
        PersonalChatLoadingResponse = LoadingStatus.Pending;

        IsChatSelected = false;

        try
        {
            var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
            if (string.IsNullOrEmpty(refreshToken))
            {
                return;
            }

            var response = await _httpClientHelper.GetAsync("PersonalChat", refreshToken, API.ChatApi);
            response.EnsureSuccessStatusCode();

            var personalChats = await response.Content.ReadFromJsonAsync<IEnumerable<PersonalChatModel>>();
            var myPersonalChats = personalChats?
                .Where(x => x.InitiatorId == MyAccount?.Id || x.CompanionId == MyAccount?.Id)
                .ToList();
            if (myPersonalChats == null)
            {
                throw new ArgumentNullException(nameof(myPersonalChats));
            }

            await MakePersonalChatContainerAsync(myPersonalChats, refreshToken);
            await LoadUsersAsync();

            PersonalChatLoadingResponse = LoadingStatus.Successful;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            PersonalChatLoadingResponse = LoadingStatus.Failed;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);

            PersonalChatLoadingResponse = LoadingStatus.Failed;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            PersonalChatLoadingResponse = LoadingStatus.Failed;
        }
    }

    private async Task GetMyGroupChatsByChatUserIdAsync(IEnumerable<GroupChatUserModel> myGroupChatUsers, string refreshToken)
    {
        MyGroupChats = [];

        foreach (var groupChatUser in myGroupChatUsers)
        {
            var response = await _httpClientHelper.GetAsync($"GroupChat/{groupChatUser.ChatId}", refreshToken, API.ChatApi);
            response.EnsureSuccessStatusCode();

            var groupChat = await response.Content.ReadFromJsonAsync<GroupChatModel>();
            if (groupChat == null)
            {
                throw new ArgumentNullException(nameof(groupChat));
            }

            if (_groupChatHubConnection == null)
            {
                throw new ArgumentNullException(nameof(_groupChatHubConnection));
            }

            await _groupChatHubConnection.JoinUnreadMessageRoomAsync(groupChatUser.ChatId);
            _groupChatHubConnection.SubscribeUnreadMessagesUpdated(async (chatId, meInChatId, count) =>
            {
                if (meInChatId == groupChatUser.Id)
                {
                    await UpdateGroupChatUnreadMessagesAsync(chatId, meInChatId, count);
                }
            });

            await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
            {
                MyGroupChats?.Add(new GroupChatViewModel(groupChat) { UnreadMessages = groupChatUser.UnreadMessages });
            });
        }
    }

    private async Task MakePersonalChatContainerAsync(IEnumerable<PersonalChatModel> myPersonalChats, string refreshToken)
    {
        MyPersonalChats = [];

        foreach (var chat in myPersonalChats)
        {
            await GetPersonalChatCompanionAsync(chat, refreshToken);

            if (_personalChatHubConnection == null)
            {
                throw new ArgumentNullException(nameof(_personalChatHubConnection));
            }
            else if (MyAccount == null)
            {
                throw new ArgumentNullException(nameof(MyAccount));
            }

            await _personalChatHubConnection.JoinUnreadMessageRoomAsync(chat.Id);
            _personalChatHubConnection.SubscribeUnreadMessagesUpdated(async (chatId, meInChatId, count) =>
            {
                if (MyAccount.Id == meInChatId)
                {
                    await UpdatePersonalChatUnreadMessagesAsync(chatId, meInChatId, count);
                }
            });

            await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
            {
                MyPersonalChats.Add(new PersonalChatViewModel(chat));
            });
        }
    }

    private async Task UpdatePersonalChatUnreadMessagesAsync(int chatId, string meInChatId, int count)
    {
        try
        {
            if (MyPersonalChats == null)
            {
                throw new ArgumentNullException(nameof(MyPersonalChats));
            }

            var chat = MyPersonalChats.FirstOrDefault(x => x.Id == chatId);
            if (chat == null)
            {
                throw new ArgumentNullException(nameof(chat));
            }

            var index = MyPersonalChats.IndexOf(chat);
            await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
            {
                MyPersonalChats[index].CurrentUnreadMessages = count;
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

    private async Task UpdateGroupChatUnreadMessagesAsync(int chatId, string meInChatId, int count)
    {
        try
        {
            if (MyGroupChats == null)
            {
                throw new ArgumentNullException(nameof(MyGroupChats));
            }

            var chat = MyGroupChats.FirstOrDefault(x => x.Id == chatId);
            if (chat == null)
            {
                throw new ArgumentNullException(nameof(chat));
            }

            var index = MyGroupChats.IndexOf(chat);
            await AsyncDispatcher.ExecuteOnMainThreadAsync(() =>
            {
                MyGroupChats[index].UnreadMessages = count;
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

    private async Task GetPersonalChatCompanionAsync(PersonalChatModel personalChat, string refreshToken)
    {
        var companionId = personalChat.CompanionId == MyAccount?.Id ? personalChat.InitiatorId : personalChat.CompanionId;
        var response = await _httpClientHelper.GetAsync($"Account/{companionId}", refreshToken, API.UserApi);
        response.EnsureSuccessStatusCode();

        var companion = await response.Content.ReadFromJsonAsync<AppUserModel>();
        if (companion == null)
        {
            throw new ArgumentNullException(nameof(companion));
        }

        personalChat.Username = companion?.Username ?? string.Empty;
    }

    private async Task LoadUsersAsync()
    {
        try
        {
            var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
            if (string.IsNullOrEmpty(refreshToken))
            {
                return;
            }

            var response = await _httpClientHelper.GetAsync("Account", refreshToken, API.UserApi);
            response.EnsureSuccessStatusCode();

            var users = await response.Content.ReadFromJsonAsync<List<AppUserModel>>();
            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }

            _allUsers = users;

            var freeUsers = ExcludeUsersThatAlreadyHasChat();

            Users = new ObservableCollection<AppUserModel>(freeUsers);
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

    private List<AppUserModel> ExcludeUsersThatAlreadyHasChat()
    {
        if (MyPersonalChats == null)
        {
            return new List<AppUserModel>();
        }

        var freeUsers = _allUsers?
            .Where(user => !MyPersonalChats.Any(chat => chat.InitiatorId == user.Id || chat.CompanionId == user.Id))
            .Where(user => user.Id != MyAccount?.Id)
            .ToList();

        return freeUsers ?? new List<AppUserModel>();
    }

    private void LoadAppUserUsernameByStartChars(string username)
    {
        if (_allUsers == null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(username))
        {
            var usernameByStartChars = _allUsers.Where(x => x.Username.StartsWith(username));
            if (usernameByStartChars == null)
            {
                return;
            }

            Users = new ObservableCollection<AppUserModel>(usernameByStartChars.ToList());
        }
        else
        {
            Users = new ObservableCollection<AppUserModel>(_allUsers);
        }
    }

    private void GetMyAccount()
    {
        MyAccount = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
    }

    private async Task InitChatSignalRAsync()
    {
        try
        {
            if (MyAccount == null)
            {
                throw new ArgumentNullException(nameof(MyAccount));
            }

            await _personalChatHubConnection.ConnectToUnreadMessageHubAsync($"{Hubs.Server}{Hubs.PersonalChatUnreadMessageAddress}");
            await _groupChatHubConnection.ConnectToUnreadMessageHubAsync($"{Hubs.Server}{Hubs.GroupChatUnreadMessageAddress}");
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
}
