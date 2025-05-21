import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo } from "react";
import { useTranslation } from 'react-i18next';
import { CombatTargetType } from '../../../types/components/combatDetails/dashboard/CombatTargetType';
import { DashboardTargetsExtractedDetailsProps } from '../../../types/components/combatDetails/dashboard/DashboardTargetsExtractedDetailsProps';

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
                {targets.slice(0, itemCount).map((target: CombatTargetType) => (
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