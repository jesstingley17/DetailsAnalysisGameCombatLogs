export type AppNotification = {
    id: number;
    type: number;
    status: number;
    initiatorId: string;
    initiatorName?: string;
    recipientId: string;
    createdAt: string;
}