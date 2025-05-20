export interface DamageDoneGeneralType {
    id: number;
    value: number;
    damagePerSecond: number;
    spell: string;
    critNumber: number;
    missNumber: number;
    castNumber: number;
    minValue: number;
    maxValue: number;
    averageValue: number;
    isPet: boolean;
    combatPlayerId: number;
}