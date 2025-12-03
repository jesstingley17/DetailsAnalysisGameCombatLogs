import type { CombatAuraModel } from '../../types/CombatAuraModel';

interface CombatAuraTargetsProps {
    combatAuras: CombatAuraModel[];
}

const CombatAuraTargets: React.FC<CombatAuraTargetsProps> = ({ combatAuras }) => {
    const uniqueTargets = [...new Set(combatAuras.map(aura => aura.target))];

    return (
        <ul className="targets">
            {uniqueTargets.map((target, index) => (
                <div key={index}>{target}</div>
            ))}
        </ul>
    );
}

export default CombatAuraTargets;