export interface DamageTakenType {
    id: number;
    spell: string;
    value: number;
    actualValue: number;
    time: string;
    creator: string;
    target: string;
    isPeriodicDamage: boolean;
    resisted: number;
    absorbed: number;
    realDamage: number;
    mitigated: number;
    damageTakenType: number;
    combatPlayerId: number;
}