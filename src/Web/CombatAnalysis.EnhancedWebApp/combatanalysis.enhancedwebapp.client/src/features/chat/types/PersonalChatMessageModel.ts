
export type PersonalChatMessageModel = {
    id: number;
    username: string;
    message: string;
    time: Date;
    status: number;
    type: number;
    markedType: number;
    isEdited: boolean;
    personalChatId: number;
    appUserId: string;
}