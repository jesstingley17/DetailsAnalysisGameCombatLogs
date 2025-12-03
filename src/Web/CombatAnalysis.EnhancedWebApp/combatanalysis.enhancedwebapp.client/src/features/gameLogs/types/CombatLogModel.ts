export type CombatLogModel = {
    id: number;
    name: string;
    date: string;
    logType: number;
    numberReadyCombats: number;
    combatsInQueue: number;
    isReady: boolean;
    appUserId: string;
}