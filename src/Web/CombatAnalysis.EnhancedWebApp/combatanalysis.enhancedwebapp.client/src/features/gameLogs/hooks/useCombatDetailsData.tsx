import type { JSX } from 'react';
import DamageDoneHelper from '../components/helpers/DamageDoneHelper';
import DamageTakenHelper from '../components/helpers/DamageTakenHelper';
import HealDoneHelper from '../components/helpers/HealDoneHelper';
import ResourceRecoveryHelper from '../components/helpers/ResourceRecoveryHelper';

type CombatDetailsData = {
    getComponentByDetailsTypeAsync: () => Promise<JSX.Element>;
}

const useCombatDetailsData = (combatPlayerId: number, pageSize: number, detailsType: string, t: (key: string) => string): CombatDetailsData => {
    const helpersComponent = {
        "DamageDone": DamageDoneHelper,
        "HealDone": HealDoneHelper,
        "DamageTaken": DamageTakenHelper,
        "ResourceRecovery": ResourceRecoveryHelper
    };

    const getComponentByDetailsTypeAsync = async (): Promise<JSX.Element> => {
        const HelperComponent = helpersComponent[detailsType as keyof typeof helpersComponent] || DamageDoneHelper;

        return (
            <HelperComponent
                combatPlayerId={combatPlayerId}
                pageSize={pageSize}
                t={t}
                getUserNameWithoutRealm={getUserNameWithoutRealm}
            />
        );
    }

    const getUserNameWithoutRealm = (username: string): string => {
        if (!username.includes('-')) {
            return username;
        }

        const realmNameIndex = username.indexOf('-');
        const userNameWithoutRealm = username.substr(0, realmNameIndex);

        return userNameWithoutRealm;
    }

    return { getComponentByDetailsTypeAsync };
}

export default useCombatDetailsData;