import { CombatDetailsType } from "../CombatDetailsType";
import { CombatPlayerType } from "../CombatPlayerType";
import { PlayerDeathType } from "../PlayerDeathType";

export interface DashboardProps {
    details: CombatDetailsType;
    combatPlayers: CombatPlayerType[];
    playersDeath: PlayerDeathType[];
    getValueShortName(value: number): string;
}