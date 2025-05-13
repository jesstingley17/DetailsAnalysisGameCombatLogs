export interface PlayerDeathType {
    id: number,
    username: string,
    lastHitSpellOrItem: string,
    lastHitValue: number,
    time: string,
    combatPlayerId: number,
}