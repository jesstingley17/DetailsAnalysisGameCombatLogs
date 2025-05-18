import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { CombatPlayerPropertyType } from '../../types/components/combatDetails/CombatPlayerPropertyType';
import { DetailsSpecificalCombatChartProps } from '../../types/components/combatDetails/DetailsSpecificalCombatChartProps';
import DetailsPieChart from './DetailsPieChart';

const DetailsSpecificalCombatChart: React.FC<DetailsSpecificalCombatChartProps> = ({ combatPlayers }) => {
    const { t } = useTranslation("combatDetails/detailsSpecificalCombat");

    const [damageDonePieChart, setDamageDonePieChart] = useState({});
    const [healDonePieChart, setHealDonePieChart] = useState({});
    const [damageTakenPieChart, setDamageTakenPieChart] = useState({});

    useEffect(() => {
        let data = createPieChardData();

        setDamageDonePieChart({
            title: t("Damage"),
            color: "#83B4FF",
            data: data.damageDone
        });
        setHealDonePieChart({
            title: t("Healing"),
            color: "#83B4FF",
            data: data.healDone
        });
        setDamageTakenPieChart({
            title: t("DamageTaken"),
            color: "#83B4FF",
            data: data.damageTaken
        });
    }, []);

    const compare = (a: CombatPlayerPropertyType, b: CombatPlayerPropertyType) => {
        if (a.value > b.value) {
            return -1;
        }
        if (a.value < b.value) {
            return 1;
        }

        return 0;
    }

    const createPieChardData = () => {
        const healDone = new Array<CombatPlayerPropertyType>(combatPlayers.length);
        const damageTaken = new Array<CombatPlayerPropertyType>(combatPlayers.length);
        const damageDone = new Array<CombatPlayerPropertyType>(combatPlayers.length);

        for (let i = 0; i < combatPlayers.length; i++) {
            const realmNameIndex = combatPlayers[i].username.indexOf('-');
            const username = combatPlayers[i].username.substr(0, realmNameIndex);

            damageDone[i] = { name: "", value: 0 };
            damageDone[i].name = username
            damageDone[i].value = combatPlayers[i].damageDone;

            healDone[i] = { name: "", value: 0 };
            healDone[i].name = username
            healDone[i].value = combatPlayers[i].healDone;

            damageTaken[i] = { name: "", value: 0 };
            damageTaken[i].name = username
            damageTaken[i].value = combatPlayers[i].damageTaken;
        }

        return {
            damageDone: damageDone.sort(compare),
            healDone: healDone.sort(compare),
            damageTaken: damageTaken.sort(compare)
        };
    }

    return (
        <div className="details-specifical-combat__container_general-details-charts">
            <DetailsPieChart
                payload={damageDonePieChart}
            />
            <DetailsPieChart
                payload={healDonePieChart}
            />
            <DetailsPieChart
                payload={damageTakenPieChart}
            />
        </div>
    );
}

export default DetailsSpecificalCombatChart;