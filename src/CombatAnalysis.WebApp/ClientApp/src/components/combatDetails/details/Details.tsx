import { memo, useEffect, useState } from "react";
import { CombatPlayerType } from "../../../types/components/combatDetails/CombatPlayerType";
import { DetailsProps } from "../../../types/components/combatDetails/details/DetailsProps";
import DetailsItem from './DetailsItem';

const Details: React.FC<DetailsProps> = ({ combatPlayers, details, getValueShortName, t }) => {
    const [filterValue, setFilterValue] = useState<number>(-1);
    const [filteredCombatPlayers, setFilteredCombatPlayers] = useState<CombatPlayerType[]>(combatPlayers);

    const detailsType: any = {
        0: "damageDone",
        1: "healDone",
        2: "damageTaken",
        3: "resourcesRecovery"
    };

    useEffect(() => {
        filter();
    }, []);

    useEffect(() => {
        filter();
    }, [filterValue]);

    const compare = (playerA: any, playerB: any) => {
        const selectedType = detailsType[filterValue];

        if (playerA[selectedType] > playerB[selectedType]) {
            return -1;
        }
        if (playerA[selectedType] < playerB[selectedType]) {
            return 1;
        }

        return 0;
    }

    const filter = () => {
        let result = new Array<CombatPlayerType>();

        if (filterValue >= 0) {
            result = [...combatPlayers].sort(compare);
        }
        else {
            result = [...combatPlayers].sort((a: CombatPlayerType, b: CombatPlayerType) => a.username.localeCompare(b.username));
        }

        setFilteredCombatPlayers(result);
    }

    const handleSelecteFilter = (e: any) => {
        setFilterValue(+e.target.value);
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