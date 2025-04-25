export interface PersonalChatMessage {
    id: number;
    username: string;
    message: string;
    time: string;
    status: number;
    type: number;
    isEdited: boolean;
    chatId: number;
    appUserId: string;
}