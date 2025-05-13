import { CombatDetailsType } from "../CombatDetailsType";
import { CombatPlayerType } from "../CombatPlayerType";

export interface DashboardItemProps {
    name: string;
    details: CombatDetailsType;
    combatPlayers: CombatPlayerType[];
    detailsType: number;
    getValueShortName: (value: number) => string;
}