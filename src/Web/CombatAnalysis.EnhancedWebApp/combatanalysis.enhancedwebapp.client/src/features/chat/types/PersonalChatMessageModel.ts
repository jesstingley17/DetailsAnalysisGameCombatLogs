
export type PersonalChatMessageModel = {
    id: number;
    username: string;
    message: string;
    time: string;
    status: number;
    type: number;
    markedType: number;
    isEdited: boolean;
    isRead: boolean;
    chatId: number;
    appUserId: string;
}