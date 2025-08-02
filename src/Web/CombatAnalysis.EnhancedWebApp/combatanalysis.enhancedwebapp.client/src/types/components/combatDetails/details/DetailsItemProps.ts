import { CombatDetailsType } from "../CombatDetailsType";
import { CombatPlayerType } from "../CombatPlayerType";

export interface DetailsItemProps {
    player: CombatPlayerType;
    details: CombatDetailsType;
    getValueShortName(value: number): string;
}