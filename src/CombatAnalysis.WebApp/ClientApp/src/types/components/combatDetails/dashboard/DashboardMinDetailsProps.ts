import { SetStateAction } from "react";
import { CombatPlayerType } from "../CombatPlayerType";

export interface DashboardMinDetailsProps {
    name: string;
    calculation: (player: any, typeOfResource: string) => string;
    calculationValuePerTime: (player: any, typeOfResource: string) => string;
    goToCombatGeneralDetails: (playerId: number) => void;
    getDetailsValue: (player: any) => any;
    sortedPlayerData: CombatPlayerType[];
    detailsType: number;
    itemCount: number,
    setItemCount: (value: SetStateAction<number>) => void,
}