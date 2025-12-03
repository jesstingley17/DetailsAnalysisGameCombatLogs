import { memo, useEffect, useState, type ChangeEvent } from 'react';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';
import type { CombatDetailsModel } from '../../types/dashboard/CombatDetailsModel';
import DetailsItem from './DetailsItem';

interface DetailsProps {
    combatPlayers: CombatPlayerModel[];
    details: CombatDetailsModel;
    getValueShortName(value: number): string;
    t(key: string): string;
}

const Details: React.FC<DetailsProps> = ({ combatPlayers, details, getValueShortName, t }) => {
    const [filterValue, setFilterValue] = useState<number>(-1);
    const [filteredCombatPlayers, setFilteredCombatPlayers] = useState<CombatPlayerModel[]>(combatPlayers);

    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const detailsType: any = {
        0: "damageDone",
        1: "healDone",
        2: "damageTaken",
        3: "resourcesRecovery"
    };

    useEffect(() => {
        filter();
    }, [filterValue]);

    const compare = (playerA: CombatPlayerModel, playerB: CombatPlayerModel) => {
        const keys: (keyof CombatPlayerModel)[] = ['damageDone', 'healDone', 'damageTaken', 'resourcesRecovery'];
        const key = keys[detailsType];

        if (playerA[key] > playerB[key]) {
            return -1;
        }
        if (playerA[key] < playerB[key]) {
            return 1;
        }

        return 0;
    }

    const filter = () => {
        let result = new Array<CombatPlayerModel>();

        if (filterValue >= 0) {
            result = [...combatPlayers].sort(compare);
        }
        else {
            result = [...combatPlayers].sort((a: CombatPlayerModel, b: CombatPlayerModel) => a.username.localeCompare(b.username));
        }

        setFilteredCombatPlayers(result);
    }

    const handleSelecteFilter = (e: ChangeEvent<HTMLSelectElement>) => {
        setFilterValue(parseInt(e.target.value || "0"));
    }

    return (
        <div className="details">
            <div className="details__filter">
                <div>{t("Filter")}:</div>
                <span>
                    <select className="form-control" value={filterValue} onChange={handleSelecteFilter}>
                        <option value="-1">{t("Username")}</option>
                        <option value="0">{t("Damage")}</option>
                        <option value="1">{t("Healing")}</option>
                        <option value="2">{t("DamageTaken")}</option>
                        <option value="3">{t("ResourcesRecovery")}</option>
                    </select>
                </span>
            </div>
            <ul>
                {filteredCombatPlayers?.map((player) => (
                    <li key={player.id} className="card">
                        <div className="card-body">
                            <h5 className="card-title">{player.username}</h5>
                        </div>
                        <DetailsItem
                            player={player}
                            details={details}
                            getValueShortName={getValueShortName}
                        />
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default memo(Details);