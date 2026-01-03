import type { PlayerModel } from "./PlayerModel";

export type CombatPlayerModel = {
    id: number;
    averageItemLevel: number;
    resourcesRecovery: number;
    damageDone: number;
    healDone: number;
    damageTaken: number;
    player: PlayerModel;
    combatId: number;
}