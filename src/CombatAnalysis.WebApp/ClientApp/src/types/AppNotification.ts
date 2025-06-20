export type AppNotification = {
    id: number;
    title: string;
    message: string;
    createdAt: string;
    readAt: string;
    type: number;
    status: number;
    initiatorId: string;
    targetName: string;
}