import { faCalendarDay, faDeleteLeft, faSitemap } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useLazyGetCombatPlayerByIdQuery } from '../../store/api/core/CombatParser.api';
import { CombatDetailsType } from '../../types/components/combatDetails/CombatDetailsType';
import { CombatPlayerType } from '../../types/components/combatDetails/CombatPlayerType';
import CombatGeneralDetails from './CombatGeneralDetails';
import CombatMoreDetails from './CombatMoreDetails';

import "../../styles/combatGeneralDetails.scss";

const CombatDetails: React.FC = () => {
    const { t } = useTranslation("combatDetails/combatGeneralDetails");

    const navigate = useNavigate();

    const [combatPlayer, setCombatPlayer] = useState<CombatPlayerType | null>(null);
    const [tabIndex, setTabIndex] = useState<number>(0);
    const [details, setDetails] = useState<CombatDetailsType>({
        id: 0,
        combatId: 0,
        detailsType: '',
        combatLogId: 0,
        name: '',
        tab: 0,
        number: 0,
        isWin: false
    });

    const [getCombatPlayerById] = useLazyGetCombatPlayerByIdQuery();

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);

        const id: number = parseInt(queryParams.get("id") || '0');
        const combatId: number = parseInt(queryParams.get("combatId") || '0');
        const detailsType: string = queryParams.get("detailsType") || '';
        const combatLogId: number = parseInt(queryParams.get("combatLogId") || '0');
        const name: string = queryParams.get("name") || '';
        const tab: number = parseInt(queryParams.get("tab") || '0');
        const number: number = parseInt(queryParams.get("number") || '0');
        const isWin: boolean = queryParams.get("isWin") === 'true';

        setDetails({
            id,
            combatId,
            detailsType,
            combatLogId,
            name,
            tab,
            number,
            isWin,
        });
    }, []);

    useEffect(() => {
        if (details.id <= 0) {
            return;
        }

        const getGeneralDetails = async () => {
            await getCombatPlayerByIdAsync(details.id);
        }

        getGeneralDetails();
    }, [details.id]);

    const getCombatPlayerByIdAsync = async (id: number) => {
        const combatPlayer = await getCombatPlayerById(id);
        if (combatPlayer.data !== undefined) {
            setCombatPlayer(combatPlayer.data);
        }
    }

    const getDetailsTypeName = () => {
        switch (details.detailsType) {
            case "DamageDone":
                return t("Damage");
            case "HealDone":
                return t("Healing");
            case "DamageTaken":
                return t("DamageTaken");
            case "ResourceRecovery":
                return t("ResourcesRecovery");
            default:
                return "";
        }
    }

    if (details.id <= 0 || !combatPlayer) {
        return <div>Loading...</div>;
    }

    return (
        <div className="general-details__container">
            <div className="general-details__navigate">
                <div className="player">
                    <div className="btn-shadow select-another-player"
                        onClick={() => navigate(`/details-specifical-combat?id=${details.combatId}&combatLogId=${details.combatLogId}&name=${details.name}&tab=${details.tab}&number=${details.number}&isWin=${details.isWin}`)}>
                        <FontAwesomeIcon
                            icon={faDeleteLeft}
                        />
                        <div>{t("SelectPlayer")}</div>
                    </div>
                    <div className="btn-shadow username">
                        <div>{combatPlayer?.username}</div>
                    </div>
                </div>
                <div className="boss">
                    <div>{details.name}</div>
                    <div className={`combat-number ${details.isWin ? 'win' : 'lose'}`}>{details.number}</div>
                </div>
                <div className="details-type">{getDetailsTypeName()}</div>
                <ul className="types">
                    <li className="nav-item">
                        <div className={`btn-shadow ${tabIndex === 0 ? "active" : ""}`} onClick={() => setTabIndex(0)}>
                            <FontAwesomeIcon
                                icon={faSitemap}
                            />
                            <div>{t("CommonInform")}</div>
                        </div>
                    </li>
                    <li className="nav-item">
                        <div className={`btn-shadow ${tabIndex === 1 ? "active" : ""}`} onClick={() => setTabIndex(1)}>
                            <FontAwesomeIcon
                                icon={faCalendarDay}
                            />
                            <div>{t("DetailsInform")}</div>
                        </div>
                    </li>
                </ul>
            </div>
            {tabIndex === 0
                ? <CombatGeneralDetails
                    combatPlayer={combatPlayer}
                    detailsType={details.detailsType}
                />
                : <CombatMoreDetails
                    combatPlayerId={details.id}
                    detailsType={details.detailsType}
                />
            }
        </div>
    );
}

export default CombatDetails;