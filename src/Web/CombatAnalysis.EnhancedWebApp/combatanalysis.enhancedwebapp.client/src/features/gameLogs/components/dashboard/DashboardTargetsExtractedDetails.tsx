import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import type { CombatTargetModel } from '../../types/CombatTargetModel';

export interface DashboardTargetsExtractedDetailsProps {
    name: string;
    calculation(combatTargetPlayerSum: number): string;
    calculationDamagePerTimeByTarget(damage: number): string;
    goToCombatGeneralDetails(playerId: number): void;
    getValueShortName(value: number): string;
    targets: CombatTargetModel[];
    itemCount: number;
    setItemCount(value: SetStateAction<number>): void;
}

const DashboardTargetsExtractedDetails: React.FC<DashboardTargetsExtractedDetailsProps> = ({ name, calculation, calculationDamagePerTimeByTarget, goToCombatGeneralDetails, getValueShortName, targets, itemCount, setItemCount }) => {
    const minCount = 4;

    const { t } = useTranslation("helpers/combatDetailsHelper");

    return (
        <div className="min-details">
            <div className="exit">
                <FontAwesomeIcon
                    icon={faXmark}
                    onClick={() => setItemCount(minCount)}
                />
            </div>
            <div className="title">
                <div>{name}</div>
            </div>
            <ul>
                {targets.slice(0, itemCount).map((target: CombatTargetModel) => (
                    <li key={target.id}>
                        <div className="min-details__title">
                            <div className="username">{target.username.split('-')[0]}</div>
                            <div className="min-details__values">
                                <div className="actual-value">{getValueShortName(target.sum)}</div>
                                <div className="value-per-time">{calculationDamagePerTimeByTarget(target.sum)} {t("DPS")}</div>
                            </div>
                        </div>
                        <div className="player-progress" onClick={() => goToCombatGeneralDetails(target.id)}>
                            <div className="progress">
                                <div className="progress-bar" role="progressbar" style={{ width: calculation(target.sum) + '%' }}></div>
                            </div>
                        </div>
                        <div className="procentage">
                            <div className="player-contribution">{calculation(target.sum)}%</div>
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default memo(DashboardTargetsExtractedDetails);