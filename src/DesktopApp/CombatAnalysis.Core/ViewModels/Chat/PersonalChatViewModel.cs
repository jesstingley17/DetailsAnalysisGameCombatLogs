using CombatAnalysis.Core.Models.Chat;
using System.ComponentModel;

namespace CombatAnalysis.Core.ViewModels.Chat;

public class PersonalChatViewModel(PersonalChatModel personalChat) : INotifyPropertyChanged
{
    private readonly PersonalChatModel _personalChat = personalChat;
    private int _currentUnreadMessages;

    public int Id => _personalChat.Id;

    public string Username => _personalChat.Username;

    public string InitiatorId => _personalChat.InitiatorId;

    public int InitiatorUnreadMessages => _personalChat.InitiatorUnreadMessages;

    public string CompanionId => _personalChat.CompanionId;

    public int CompanionUnreadMessages => _personalChat.CompanionUnreadMessages;

    public int CurrentUnreadMessages
    {
        get => _currentUnreadMessages;
        set
        {
            if (_currentUnreadMessages != value)
            {
                _currentUnreadMessages = value;
                OnPropertyChanged(nameof(CurrentUnreadMessages));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
