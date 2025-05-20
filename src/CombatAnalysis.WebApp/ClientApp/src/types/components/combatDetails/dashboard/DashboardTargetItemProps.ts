import { CombatDetailsType } from "../CombatDetailsType";
import { CombatPlayerType } from "../CombatPlayerType";
import { CombatTargetType } from "./CombatTargetType";

export interface DashboardTargetItemProps {
    combatTarget: CombatTargetType[];
    details: CombatDetailsType;
    duration: number;
    combatPlayers: CombatPlayerType[];
    detailsType: number;
    getValueShortName: (value: number) => string;
}