using CombatAnalysis.Core.Models.Chat;
using System.ComponentModel;

namespace CombatAnalysis.Core.ViewModels.Chat;

public class GroupChatViewModel(GroupChatModel groupChat) : INotifyPropertyChanged
{
    private readonly GroupChatModel _groupChat = groupChat;
    private int _unreadMessages;

    public int Id => _groupChat.Id;

    public string Name => _groupChat.Name;

    public string AppUserId => _groupChat.AppUserId;

    public int UnreadMessages
    {
        get => _unreadMessages;
        set
        {
            if (_unreadMessages != value)
            {
                _unreadMessages = value;
                OnPropertyChanged(nameof(UnreadMessages));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
