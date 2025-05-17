import { CombatPlayerType } from "../CombatPlayerType";

export interface TopPlayersProps {
    calculation: (player: any, typeOfResource: string) => string;
    calculationValuePerTime: (player: any, typeOfResource: string) => string;
    getDetailsValue: (player: any) => any;
    sortedPlayerData: CombatPlayerType[];
    detailsType: number;
}