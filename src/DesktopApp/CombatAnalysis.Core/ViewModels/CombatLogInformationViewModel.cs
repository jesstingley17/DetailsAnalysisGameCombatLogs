using AutoMapper;
using CombatAnalysis.CombatParser.Details;
using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Observers;
using CombatAnalysis.Core.Models.GameLogs;
using CombatAnalysis.Core.Models.User;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels;

public class CombatLogInformationViewModel : ParentTemplate, IAuthObserver
{
    private readonly IMvxNavigationService _mvvmNavigation;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;
    private readonly ICombatParserService _parser;
    private readonly ICombatParserAPIService _combatParserAPIService;
    private readonly IMemoryCache _memoryCache;

    private ObservableCollection<string> _combatLogNames = [];
    private string? _dungeonName;
    private string? _combatName;
    private ObservableCollection<string> _combatLogPaths = [];
    private bool _isNeedSave;
    private ObservableCollection<CombatLogModel> _combatLogs = [];
    private ObservableCollection<CombatLogModel> _combatLogsForTargetUser = [];
    private bool _isAllowSaveLogs = true;
    private CancellationTokenSource _cancellationTokenSource = new();

    private bool _fileIsCorrect = true;
    private bool _openUploadedLogs;
    private bool _isParsing;
    private bool _combatLogUploadingFailed;
    private int _combatListSelectedIndex;
    private int _selectedCombatLogTypeTabItem;
    private bool _isAuth;
    private LogType _logType;
    private LoadingStatus _combatLogLoadingStatus;
    private bool _removingInProgress;
    private bool _uploadingLogs;
    private bool _noCombatsUploaded;
    private bool _processAborted;
    private bool _showConnectMore;

    public CombatLogInformationViewModel(IMapper mapper, IMvxNavigationService mvvmNavigation, ICombatParserService parser,
        IMemoryCache memoryCache, ICacheService cacheService, ICombatParserAPIService combatParserAPIService)
    {
        _mapper = mapper;
        _mvvmNavigation = mvvmNavigation;
        _parser = parser;
        _memoryCache = memoryCache;
        _cacheService = cacheService;
        _combatParserAPIService = combatParserAPIService;

        OpenUploadedLogsCommand = new MvxCommand(() => OpenUploadedLogs = !OpenUploadedLogs);
        OpenPlayerAnalysisCommand = new MvxAsyncCommand(OpenPlayerAnalysisAsync);
        LoadCombatsCommand = new MvxAsyncCommand(() => LoadCombatsAsync(CombatLogs));
        LoadCombatsByUserCommand = new MvxAsyncCommand(() => LoadCombatsAsync(CombatLogsForTargetUser));
        ReloadCombatsCommand = new MvxAsyncCommand(LoadCombatLogsAsync);
        DeleteCombatCommand = new MvxAsyncCommand(DeleteAsync);
        CancelParsingCommand = new MvxCommand(CancelParsing);

        GetLogTypeCommand = new MvxCommand<int>(GetLogType);

        Basic.Parent = this;
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), 0);
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.LogPanelStatusIsVisibly), true);

        var authObservable = Basic as IAuthObservable;
        authObservable?.AddObserver(this);

        IsAuth = false;
    }

    #region Commands

    public IMvxCommand OpenUploadedLogsCommand { get; private set; }

    public IMvxAsyncCommand LoadCombatsCommand { get; private set; }

    public IMvxAsyncCommand LoadCombatsByUserCommand { get; private set; }

    public IMvxAsyncCommand ReloadCombatsCommand { get; private set; }

    public IMvxAsyncCommand DeleteCombatCommand { get; private set; }

    public IMvxAsyncCommand OpenPlayerAnalysisCommand { get; private set; }

    public IMvxCommand<int> GetLogTypeCommand { get; private set; }

    public IMvxCommand CancelParsingCommand { get; private set; }

    #endregion

    #region View model properties

    public bool OpenUploadedLogs
    {
        get { return _openUploadedLogs; }
        set
        {
            SetProperty(ref _openUploadedLogs, value);
        }
    }

    public bool NoCombatsUploaded
    {
        get { return _noCombatsUploaded; }
        set
        {
            SetProperty(ref _noCombatsUploaded, value);
        }
    }

    public bool UploadingLogs
    {
        get { return _uploadingLogs; }
        set
        {
            SetProperty(ref _uploadingLogs, value);
        }
    }

    public ObservableCollection<CombatLogModel> CombatLogs
    {
        get { return _combatLogs; }
        set
        {
            SetProperty(ref _combatLogs, value);
        }
    }

    public ObservableCollection<CombatLogModel> CombatLogsForTargetUser
    {
        get { return _combatLogsForTargetUser; }
        set
        {
            SetProperty(ref _combatLogsForTargetUser, value);
        }
    }

    public ObservableCollection<string> CombatLogNames
    {
        get { return _combatLogNames; }
        set
        {
            SetProperty(ref _combatLogNames, value);
        }
    }

    public ObservableCollection<string> CombatLogPaths
    {
        get { return _combatLogPaths; }
        set
        {
            SetProperty(ref _combatLogPaths, value);

        }
    }

    public bool IsParsing
    {
        get { return _isParsing; }
        set
        {
            SetProperty(ref _isParsing, value);
            if (value)
            {
                OpenUploadedLogs = false;
            }
        }
    }

    public bool CombatLogUploadingFailed
    {
        get { return _combatLogUploadingFailed; }
        set
        {
            SetProperty(ref _combatLogUploadingFailed, value);
        }
    }

    public bool FileIsCorrect
    {
        get { return _fileIsCorrect; }
        set
        {
            SetProperty(ref _fileIsCorrect, value);
        }
    }

    public bool IsNeedSave
    {
        get { return _isNeedSave; }
        set
        {
            SetProperty(ref _isNeedSave, value);
        }
    }

    public string? DungeonName
    {
        get { return _dungeonName; }
        set
        {
            SetProperty(ref _dungeonName, value);
        }
    }

    public string? CombatName
    {
        get { return _combatName; }
        set
        {
            SetProperty(ref _combatName, value);
        }
    }

    public int CombatListSelectedIndex
    {
        get { return _combatListSelectedIndex; }
        set
        {
            SetProperty(ref _combatListSelectedIndex, value);
        }
    }

    public int SelectedCombatLogTypeTabItem
    {
        get { return _selectedCombatLogTypeTabItem; }
        set
        {
            SetProperty(ref _selectedCombatLogTypeTabItem, value);
        }
    }

    public bool IsAllowSaveLogs
    {
        get { return _isAllowSaveLogs; }
        set
        {
            SetProperty(ref _isAllowSaveLogs, value);
        }
    }

    public bool IsAuth
    {
        get { return _isAuth; }
        set
        {
            SetProperty(ref _isAuth, value);
        }
    }

    public LogType LogType
    {
        get { return _logType; }
        set
        {
            SetProperty(ref _logType, value);
            Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.LogType), value);
        }
    }

    public LoadingStatus CombatLogLoadingStatus
    {
        get { return _combatLogLoadingStatus; }
        set
        {
            SetProperty(ref _combatLogLoadingStatus, value);
        }
    }

    public bool RemovingInProgress
    {
        get { return _removingInProgress; }
        set
        {
            SetProperty(ref _removingInProgress, value);
        }
    }

    public bool ShowConnectMore
    {
        get { return _showConnectMore; }
        set
        {
            SetProperty(ref _showConnectMore, value);
        }
    }

    #endregion

    public void AuthUpdate(bool isAuth)
    {
        IsAuth = isAuth;
        if (!isAuth)
        {
            LogType = LogType.Public;
            SelectedCombatLogTypeTabItem = 0;
        }
    }

    #region Ovveride methods

    public override void Prepare()
    {
        base.Prepare();

        CombatLogPaths = [.. AppStaticData.SelectedCombatLogFilePaths];
        CombatLogPaths.CollectionChanged += CombatLogPaths_CollectionChanged;

        ShowConnectMore = CombatLogPaths.Count > 0;
        GetCombatLogNames();
    }

    public override void ViewAppeared()
    {
        IsParsing = false;

        CombatLogs?.Clear();
        CheckAuth();
    }

    public override void ViewDestroy(bool viewFinishing = true)
    {
        base.ViewDestroy(viewFinishing);
    }

    public override async Task Initialize()
    {
        await base.Initialize();

        var token = ((BasicTemplateViewModel)Basic).RequestCancelationToken();
        await LoadCombatLogsAsync(token);
    }

    #endregion

    private void CombatLogPaths_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        GetCombatLogNames();

        ShowConnectMore = CombatLogPaths.Count > 0;
    }

    private void GetLogType(int logType)
    {
        LogType = (LogType)logType;
    }

    private void GetCombatLogNames()
    {
        if (CombatLogPaths.Count == 0)
        {
            return;
        }

        CombatLogNames.Clear();

        foreach (var item in CombatLogPaths)
        {
            var split = item.Split(@"\");
            CombatLogNames.Add(split[^1]);
        }
    }

    private async Task OpenPlayerAnalysisAsync()
    {
        CombatLogUploadingFailed = false;
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.None);

        await CombatLogFileValidateAsync(CombatLogPaths.ToList() ?? []);
    }

    private async Task LoadCombatsAsync(ObservableCollection<CombatLogModel> combatCollection)
    {
        NoCombatsUploaded = false;

        var combatLog = combatCollection[CombatListSelectedIndex];
        if (combatLog.NumberReadyCombats == 0)
        {
            NoCombatsUploaded = true;

            return;
        }

        UploadingLogs = true;

        var token = ((BasicTemplateViewModel)Basic).RequestCancelationToken();
        var loadedCombats = await _combatParserAPIService.LoadCombatsAsync(combatLog.Id, token);
        if (loadedCombats == null)
        {
            return;
        }

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.AllowStep), 1);

        var dataForGeneralAnalysis = Tuple.Create(loadedCombats.ToList(), LogType);
        await _mvvmNavigation.Navigate<CombatsViewModel, Tuple<List<CombatModel>, LogType>>(dataForGeneralAnalysis);

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.CombatLog), combatLog);
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Combats), loadedCombats.ToList());
    }

    private async Task DeleteAsync()
    {
        if (CombatListSelectedIndex < 0)
        {
            return;
        }

        DungeonName = string.Empty;
        CombatName = string.Empty;
        RemovingInProgress = true;

        var token = ((BasicTemplateViewModel)Basic).RequestCancelationToken();
        var selectedCombatLogByUser = _combatLogs.FirstOrDefault(x => x.Id == CombatLogsForTargetUser[CombatListSelectedIndex].Id);
        if (selectedCombatLogByUser != null)
        {
            await _combatParserAPIService.DeleteCombatLogByUserAsync(selectedCombatLogByUser.Id, token);
        }

        await LoadCombatLogsAsync(token);

        RemovingInProgress = false;
    }

    private void CancelParsing()
    {
        _processAborted = true;
        _cancellationTokenSource?.Cancel();
    }

    private void CheckAuth()
    {
        var user = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
        IsAuth = user != null;
    }

    private async Task CombatLogFileValidateAsync(List<string> combatLogPaths)
    {
        foreach (var item in combatLogPaths)
        {
            FileIsCorrect = await _parser.FileCheckAsync(item);
            if (!FileIsCorrect) return;
        }

        IsParsing = true;

        await PrepareCombatDataAsync(combatLogPaths);

        IsParsing = false;
    }

    private async Task PrepareCombatDataAsync(List<string> combatLogPaths)
    {
        _cancellationTokenSource = new CancellationTokenSource();

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Combats), new List<CombatModel>());
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.PetsId), new Dictionary<string, List<string>>());
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.AllowStep), 0);

        CombatParser.Consts.API.CombatParserApi = API.CombatParserApi;
        await _parser.ParseAsync(combatLogPaths, _cancellationTokenSource.Token);

        var combatsList = _mapper.Map<List<CombatModel>>(_parser.Combats);

        await _combatParserAPIService.GetBossAsync(combatsList, _cancellationTokenSource.Token);

        ClearCache();

        AppStaticData.PreparedCombatsCount = _parser.Combats.Count;

        CreateCache(_parser.CombatDetails);

        _parser.Clear();

        var dataForGeneralAnalysis = Tuple.Create(combatsList, LogType);

        if (_processAborted)
        {
            _processAborted = false;

            return;
        }

        if (!IsNeedSave)
        {
            Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.AllowStep), 1);
            Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.None);

            await _mvvmNavigation.Navigate<CombatsViewModel, Tuple<List<CombatModel>, LogType>>(dataForGeneralAnalysis);
            Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Combats), combatsList);

            return;
        }

        await UploadingCombatLogAsync(combatsList, dataForGeneralAnalysis);
    }

    private void ClearCache()
    {
        for (var i = 0; i < AppStaticData.PreparedCombatsCount; i++)
        {
            _cacheService.Remove($"{AppCacheKeys.CombatDetails_DamageDone}_{i}");
            _cacheService.Remove($"{AppCacheKeys.CombatDetails_DamageDoneGeneral}_{i}");
            _cacheService.Remove($"{AppCacheKeys.CombatDetails_HealDone}_{i}");
            _cacheService.Remove($"{AppCacheKeys.CombatDetails_HealDoneGeneral}_{i}");
            _cacheService.Remove($"{AppCacheKeys.CombatDetails_DamageTaken}_{i}");
            _cacheService.Remove($"{AppCacheKeys.CombatDetails_DamageTakenGeneral}_{i}");
            _cacheService.Remove($"{AppCacheKeys.CombatDetails_ResourcesRecovery}_{i}");
            _cacheService.Remove($"{AppCacheKeys.CombatDetails_ResourcesRecoveryGeneral}_{i}");
        }
    }

    private void CreateCache(List<CombatDetails> combatDetails)
    {
        for (var i = 0; i < combatDetails.Count; i++)
        {
            var combat = combatDetails[i];

            _cacheService.Add($"{AppCacheKeys.CombatDetails_Positions}_{i}", combat.Positions.AsReadOnly());

            _cacheService.Add($"{AppCacheKeys.CombatDetails_DamageDone}_{i}", combat.DamageDone.AsReadOnly());
            _cacheService.Add($"{AppCacheKeys.CombatDetails_DamageDoneGeneral}_{i}", combat.DamageDoneGeneral.AsReadOnly());
            _cacheService.Add($"{AppCacheKeys.CombatDetails_HealDone}_{i}", combat.HealDone.AsReadOnly());
            _cacheService.Add($"{AppCacheKeys.CombatDetails_HealDoneGeneral}_{i}", combat.HealDoneGeneral.AsReadOnly());
            _cacheService.Add($"{AppCacheKeys.CombatDetails_DamageTaken}_{i}", combat.DamageTaken.AsReadOnly());
            _cacheService.Add($"{AppCacheKeys.CombatDetails_DamageTakenGeneral}_{i}", combat.DamageTakenGeneral.AsReadOnly());
            _cacheService.Add($"{AppCacheKeys.CombatDetails_ResourcesRecovery}_{i}", combat.ResourcesRecovery.AsReadOnly());
            _cacheService.Add($"{AppCacheKeys.CombatDetails_ResourcesRecoveryGeneral}_{i}", combat.ResourcesRecoveryGeneral.AsReadOnly());
        }
    }

    private async Task UploadingCombatLogAsync(List<CombatModel> combatList, Tuple<List<CombatModel>, LogType> dataForGeneralAnalysis)
    {
        var createdCombatLog = await _combatParserAPIService.SaveCombatLogAsync(combatList, LogType, CancellationToken.None);
        if (createdCombatLog.AppUserId == null)
        {
            Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.ResponseStatus), LoadingStatus.Failed);

            CombatLogUploadingFailed = true;

            return;
        }

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Combats), combatList);
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.CombatLog), createdCombatLog);

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.AllowStep), 1);

        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.IsCombatLogsMustSave), true);

        await _mvvmNavigation.Navigate<CombatsViewModel, Tuple<List<CombatModel>, LogType>>(dataForGeneralAnalysis);
    }

    private async Task LoadCombatLogsAsync(CancellationToken cancellationToken)
    {
        NoCombatsUploaded = false;

        CombatLogLoadingStatus = LoadingStatus.Pending;

        var combatLogsData = await _combatParserAPIService.LoadCombatLogsAsync(cancellationToken);
        if (combatLogsData == null)
        {
            CombatLogLoadingStatus = LoadingStatus.Failed;
            CombatLogs = [];

            return;
        }

        var readyCombatLogData = new List<CombatLogModel>();

        foreach (var item in combatLogsData)
        {
            if (item.IsReady)
            {
                readyCombatLogData.Add(item);
            }
        }

        var publicLogs = readyCombatLogData.Where(x => x.LogType == (int)LogType.Public).ToList();
        CombatLogs = new ObservableCollection<CombatLogModel>(publicLogs);

        CombatLogLoadingStatus = LoadingStatus.Successful;

        LoadCombatLogsForTargetUser(readyCombatLogData);
    }

    private void LoadCombatLogsForTargetUser(List<CombatLogModel> combatLogs)
    {
        var user = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
        if (user == null)
        {
            CombatLogsForTargetUser = [];

            return;
        }

        var combatLogsForTargetUser = combatLogs.Where(x => x.AppUserId == user.Id).ToList();
        CombatLogsForTargetUser = new ObservableCollection<CombatLogModel>(combatLogsForTargetUser);
    }
}
