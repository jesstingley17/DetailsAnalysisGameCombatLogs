import DamageDoneGeneralHelper from '../components/helpers/DamageDoneGeneralHelper';
import DamageTakenGeneralHelper from '../components/helpers/DamageTakenGeneralHelper';
import HealDoneGeneralHelper from '../components/helpers/HealDoneGeneralHelper';
import ResourceRecoveryGeneralHelper from '../components/helpers/ResourceRecoveryGeneralHelper';
import { useLazyGetDamageDoneGeneralByCombatPlayerIdQuery } from '../store/api/combatParser/DamageDone.api';
import { useLazyGetDamageTakenGeneralByCombatPlayerIdQuery } from '../store/api/combatParser/DamageTaken.api';
import { useLazyGetHealDoneGeneralByCombatPlayerIdQuery } from '../store/api/combatParser/HealDone.api';
import { useLazyGetResourceRecoveryGeneralByCombatPlayerIdQuery } from '../store/api/combatParser/ResourcesRecovery.api';

const useCombatGeneralData = (combatPlayer, detailsType) => {
    const fixedNumberUntil = 2;

    const [getDamageDoneGeneralByCombatPlayerIdAsync] = useLazyGetDamageDoneGeneralByCombatPlayerIdQuery();
    const [getDamageTakenGeneralByCombatPlayerIdAsync] = useLazyGetDamageTakenGeneralByCombatPlayerIdQuery();
    const [getHealDoneGeneralByCombatPlayerIdAsync] = useLazyGetHealDoneGeneralByCombatPlayerIdQuery();
    const [getResourceRecoveryGeneralByCombatPlayerIdAsync] = useLazyGetResourceRecoveryGeneralByCombatPlayerIdQuery();

    const getProcentage = (firstValue, secondValue) => {
        const number = firstValue / secondValue;
        const procentage = number * 100;
        const round = procentage.toFixed(2);

        return round;
    }

    const getSpellValueProcentage = (item, targetValue) => {
        const procentage = (item.value / targetValue) * 100;

        return procentage.toFixed(fixedNumberUntil);
    }

    const getValueShortName = (value) => {
        const thousands = value / 1000;
        const millions = value / 1000000;

        if (millions >= 1) {
            return `${millions.toFixed(fixedNumberUntil)} M`;
        }
        else if (thousands >= 1) {
            return `${thousands.toFixed(fixedNumberUntil)} K`;
        }

        return value;
    }

    const getGeneralListAsync = async () => {
        const data = await getPlayerGeneralDetailsAsync();

        switch (+detailsType) {
            case 0:
                return <DamageDoneGeneralHelper
                    generalData={data}
                    getProcentage={getProcentage}
                    combatPlayer={combatPlayer}
                    getValueShortName={getValueShortName}
                    getSpellValueProcentage={getSpellValueProcentage}
                />
            case 1:
                return <HealDoneGeneralHelper
                    generalData={data}
                    getProcentage={getProcentage}
                    combatPlayer={combatPlayer}
                    getValueShortName={getValueShortName}
                    getSpellValueProcentage={getSpellValueProcentage}
                />
            case 2:
                return <DamageTakenGeneralHelper
                    generalData={data}
                    getProcentage={getProcentage}
                    combatPlayer={combatPlayer}
                    getValueShortName={getValueShortName}
                    getSpellValueProcentage={getSpellValueProcentage}
                />
            case 3:
                return <ResourceRecoveryGeneralHelper
                    generalData={data}
                    combatPlayer={combatPlayer}
                    getValueShortName={getValueShortName}
                    getSpellValueProcentage={getSpellValueProcentage}
                />
            default:
                return <DamageDoneGeneralHelper
                    generalData={data}
                    getProcentage={getProcentage}
                    combatPlayer={combatPlayer}
                    getValueShortName={getValueShortName}
                    getSpellValueProcentage={getSpellValueProcentage}
                />
        }
    }

    const getPlayerGeneralDetailsAsync = async () => {
        let detailsResult = null;
        switch (+detailsType) {
            case 0:
                detailsResult = await getDamageDoneGeneralByCombatPlayerIdAsync(combatPlayer.id);
                break;
            case 1:
                detailsResult = await getHealDoneGeneralByCombatPlayerIdAsync(combatPlayer.id);
                break;
            case 2:
                detailsResult = await getDamageTakenGeneralByCombatPlayerIdAsync(combatPlayer.id);
                break;
            case 3:
                detailsResult = await getResourceRecoveryGeneralByCombatPlayerIdAsync(combatPlayer.id);
                break;
            default:
                detailsResult = await getDamageDoneGeneralByCombatPlayerIdAsync(combatPlayer.id);
                break;
        }

        if (detailsResult.data !== undefined) {
            return detailsResult.data;
        }

        return null;
    }

    return [getGeneralListAsync, getPlayerGeneralDetailsAsync];
}

export default useCombatGeneralData;