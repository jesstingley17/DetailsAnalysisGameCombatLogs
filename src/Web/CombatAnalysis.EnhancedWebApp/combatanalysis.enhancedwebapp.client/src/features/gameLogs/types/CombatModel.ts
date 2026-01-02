import type { BossModel } from "./BossModel";

export type CombatModel = {
    id: number;
    dungeonName: string;
    resourcesRecovery: number;
    damageDone: number;
    healDone: number;
    damageTaken: number;
    isWin: boolean;
    startDate: string;
    finishDate: string;
    duration: string;
    isReady: boolean;
    combatLogId: number;
    boss: BossModel;
}