export interface PersonalChat {
    id: number;
    initiatorId: string;
    initiatorUnreadMessages: number;
    companionId: string;
    companionUnreadMessages: number;
}