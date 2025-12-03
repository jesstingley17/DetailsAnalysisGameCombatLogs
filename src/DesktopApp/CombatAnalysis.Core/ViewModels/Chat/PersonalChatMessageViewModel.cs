using CombatAnalysis.Core.Models.Chat;
using System.ComponentModel;

namespace CombatAnalysis.Core.ViewModels.Chat;

public class PersonalChatMessageViewModel(PersonalChatMessageModel message) : INotifyPropertyChanged
{
    private readonly PersonalChatMessageModel _message = message;

    public int Id => _message.Id;

    public string Username => _message.Username;

    public string Message => _message.Message;

    public DateTimeOffset Time => _message.Time;

    public int Status
    {
        get => _message.Status;
        set
        {
            if (_message.Status != value)
            {
                _message.Status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
    }

    public int Type => _message.Type;

    public int MarkedType => _message.MarkedType;

    public bool IsEdited => _message.IsEdited;

    public int PersonalChatId => _message.PersonalChatId;

    public string AppUserId => _message.AppUserId;

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
