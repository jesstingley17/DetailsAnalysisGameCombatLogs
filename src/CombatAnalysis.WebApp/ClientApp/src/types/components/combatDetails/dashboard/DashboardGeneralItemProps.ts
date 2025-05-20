import { CombatDetailsType } from "../CombatDetailsType";
import { CombatPlayerType } from "../CombatPlayerType";

export interface DashboardGeneralItemProps {
    name: string;
    details: CombatDetailsType;
    duration: number;
    combatPlayers: CombatPlayerType[];
    detailsType: number;
    getValueShortName: (value: number) => string;
}