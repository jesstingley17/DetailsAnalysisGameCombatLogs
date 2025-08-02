export interface CombatType {
    id: number;
    dungeonName: string;
    name: string;
    energyRecovery: number;
    damageDone: number;
    healDone: number;
    damageTaken: number;
    isWin: boolean;
    startDate: string;
    finishDate: string;
    duration: string;
    isReady: boolean;
    combatLogId: number;
}