import { CombatDetailsType } from "../CombatDetailsType";
import { CombatPlayerType } from "../CombatPlayerType";

export interface DetailsProps {
    combatPlayers: CombatPlayerType[];
    details: CombatDetailsType;
    getValueShortName(value: number): string;
    t(key: string): string;
}