export interface DamageDoneType {
    id: number;
    spell: string;
    value: number;
    time: string;
    creator: string;
    target: string;
    damageType: number;
    isPeriodicDamage: boolean;
    isPet: boolean;
    combatPlayerId: number;
}