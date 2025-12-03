
const SystemChatMessage: React.FC<{ message: string }> = ({ message }) => {
    return (
        <div className="chat-messages__content">
            <div className="system-message">
                <div className="system-message__message">{message}</div>
            </div>
        </div>
    );
}

export default SystemChatMessage;