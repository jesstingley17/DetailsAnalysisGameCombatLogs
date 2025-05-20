import { CombatTargetType } from "./CombatTargetType";

export interface TopPlayersByTargetProps {
    calculation: (combatTargetPlayerSum: number) => string;
    calculationDamagePerTimeByTarget: (damage: number) => string;
    goToCombatGeneralDetails: (playerId: number) => void;
    getValueShortName: (value: number) => string;
    targetTopPlayers: CombatTargetType[];
}