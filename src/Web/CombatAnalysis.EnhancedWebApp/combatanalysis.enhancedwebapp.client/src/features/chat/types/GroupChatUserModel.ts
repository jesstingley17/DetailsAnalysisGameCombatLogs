export type GroupChatUserModel = {
    id?: string;
    username: string;
    unreadMessages: number;
    groupChatId: number;
    appUserId: string;
}