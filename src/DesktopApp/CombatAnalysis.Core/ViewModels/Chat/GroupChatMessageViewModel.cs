using CombatAnalysis.Core.Models.Chat;
using System.ComponentModel;

namespace CombatAnalysis.Core.ViewModels.Chat;

public class GroupChatMessageViewModel(GroupChatMessageModel message) : INotifyPropertyChanged
{
    private readonly GroupChatMessageModel _message = message;
    private bool _isRead;

    public int Id => _message.Id;

    public string Username => _message.Username;

    public string Message => _message.Message;

    public DateTimeOffset Time => _message.Time;

    public int Status => _message.Status;

    public int Type => _message.Type;

    public int MarkedType => _message.MarkedType;

    public bool IsEdited => _message.IsEdited;

    public bool IsRead
    {
        get => _isRead;
        set
        {
            if (_isRead != value)
            {
                _isRead = value;
                OnPropertyChanged(nameof(IsRead));
            }
        }
    }

    public int GroupChatId => _message.GroupChatId;

    public string GroupChatUserId => _message.GroupChatUserId;

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

