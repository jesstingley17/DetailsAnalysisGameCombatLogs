import { CombatPlayerType } from "../CombatPlayerType";
import { PlayerDeathType } from "../PlayerDeathType";

export interface DashboardDeathItemProps {
    playersDeath: PlayerDeathType[];
    combatPlayers: CombatPlayerType[];
}