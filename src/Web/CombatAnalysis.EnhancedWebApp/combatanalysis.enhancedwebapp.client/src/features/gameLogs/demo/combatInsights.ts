/** Static demo snapshot shape (matches public/demo/combat-demo.json). */
export type DemoSnapshot = {
    encounterName: string;
    durationSeconds: number;
    roleLabel: string;
    playerDisplayName: string;
    specLabel: string;
    damageDone: number;
    healDone: number;
    damageTaken: number;
    deathCount: number;
    damageScore: number;
    healScore: number;
};

export type InsightPart = {
    key: 'insightDeaths' | 'insightNoDeaths' | 'insightDamageTakenHigh' | 'insightDamageTakenOk' | 'insightScoreStrong' | 'insightScoreGrow';
    params?: Record<string, string | number>;
};

/**
 * Picks three coaching-style lines from the same heuristics the full app can grow into.
 * Order: survivability → damage taken context → score interpretation (DPS vs heal focus).
 */
export function getInsightParts(snapshot: DemoSnapshot): InsightPart[] {
    const parts: InsightPart[] = [];

    if (snapshot.deathCount > 0) {
        parts.push({ key: 'insightDeaths', params: { count: snapshot.deathCount } });
    } else {
        parts.push({ key: 'insightNoDeaths' });
    }

    const throughput = Math.max(snapshot.damageDone + snapshot.healDone, 1);
    const takenRatio = snapshot.damageTaken / throughput;
    parts.push(takenRatio > 0.55 ? { key: 'insightDamageTakenHigh' } : { key: 'insightDamageTakenOk' });

    const primaryIsDamage = snapshot.damageDone >= snapshot.healDone;
    const primaryScore = primaryIsDamage ? snapshot.damageScore : snapshot.healScore;
    parts.push(primaryScore >= 75 ? { key: 'insightScoreStrong' } : { key: 'insightScoreGrow' });

    return parts;
}
