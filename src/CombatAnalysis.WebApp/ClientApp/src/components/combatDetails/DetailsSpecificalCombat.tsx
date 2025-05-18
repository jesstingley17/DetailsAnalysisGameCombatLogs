import { faDeleteLeft, faMagnifyingGlassMinus, faMagnifyingGlassPlus, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useLazyGetCombatPlayersByCombatIdQuery, useLazyGetPlayersDeathByPlayerIdQuery } from '../../store/api/core/CombatParser.api';
import { CombatDetailsType } from '../../types/components/combatDetails/CombatDetailsType';
import { CombatPlayerType } from '../../types/components/combatDetails/CombatPlayerType';
import { PlayerDeathType } from '../../types/components/combatDetails/PlayerDeathType';
import Details from './details/Details';
import PersonalTabs from '../common/PersonalTabs';
import DetailsSpecificalCombatChart from './DetailsSpecificalCombatChart';
import Dashboard from './dashboard/Dashboard';

import "../../styles/detailsSpecificalCombat.scss";

const DetailsSpecificalCombat: React.FC = () => {
    const fixedNumberUntil = 2;

    const { t } = useTranslation("combatDetails/detailsSpecificalCombat");

    const navigate = useNavigate();

    const [details, setDetails] = useState<CombatDetailsType>({
        id: 0,
        detailsType: '',
        combatLogId: 0,
        name: '',
        tab: 0,
        number: 0,
        isWin: false
    });
    const [combatPlayers, setCombatPlayers] = useState<CombatPlayerType[]>([]);
    const [playersDeath, setPlayersDeath] = useState<PlayerDeathType[] | null>(null);
    const [selectedPlayers, setSelectedPlayers] = useState<CombatPlayerType[]>([]);
    const [showCommonStatistics, setShowCommonStatistics] = useState(false);
    const [showSearch, setShowSearch] = useState(false);

    const maxWidth = 425;
    const screenSize = {
        width: window.innerWidth,
        height: window.innerHeight
    };

    const [getCombatPlayersByCombatIdAsync] = useLazyGetCombatPlayersByCombatIdQuery();
    const [getPlayersDeathByCombatIdAsync] = useLazyGetPlayersDeathByPlayerIdQuery();

    const filterContent = useRef<HTMLInputElement>(null);

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);

        const id: number = parseInt(queryParams.get("id") || '0');
        const detailsType: string = queryParams.get("detailsType") || '';
        const combatLogId: number = parseInt(queryParams.get("combatLogId") || '0');
        const name: string = queryParams.get("name") || '';
        const tab: number = parseInt(queryParams.get("tab") || '0');
        const number: number = parseInt(queryParams.get("number") || '0');
        const isWin: boolean = queryParams.get("isWin") === 'true';

        setDetails({
            id,
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

        const fetchData = async () => {
            const combatPlayersData = await getCombatPlayersAsync();
            await getPlayersDeathAsync(combatPlayersData);
        }

        fetchData();
    }, [details.id]);

    const handlerSearch = (e: any) => {
        const foundPlayers = combatPlayers.filter((item) => item.username.toLowerCase().startsWith(e.target.value.toLowerCase()));
        setSelectedPlayers(foundPlayers);
    }

    const getCombatPlayersAsync = async () => {
        const combatPlayersResult = await getCombatPlayersByCombatIdAsync(details.id);
        if (combatPlayersResult.data !== undefined) {
            setCombatPlayers(combatPlayersResult.data);
            setSelectedPlayers(combatPlayersResult.data);

            return combatPlayersResult.data;
        }

        return [];
    }

    const getPlayersDeathAsync = async (players: CombatPlayerType[]) => {
        const deathsPromises = players.map(player => getPlayersDeathByCombatIdAsync(player.id));
        const deathsResults: any[] = await Promise.all(deathsPromises);
        const deaths: PlayerDeathType[] = deathsResults.filter(result => result.data && result.data.length > 0).map(result => result.data[0]);

        setPlayersDeath(deaths);
    }

    const cleanSearch = () => {
        if (filterContent.current) {
            filterContent.current.value = "";
        }

        setSelectedPlayers(combatPlayers);
    }

    const getValueShortName = (value: number): string => {
        const thousands = value / 1000;
        const millions = value / 1000000;

        if (millions >= 1) {
            return `${millions.toFixed(fixedNumberUntil)}M`;
        }
        else if (thousands >= 1) {
            return `${thousands.toFixed(fixedNumberUntil)}K`;
        }

        return `${value}`;
    }

    return (
        <div className="details-specifical-combat__container">
            <div className="details-specifical-combat__navigate">
                <div className="btn-shadow select-combat" onClick={() => navigate(`/general-analysis?id=${details.combatLogId}`)}>
                    <FontAwesomeIcon
                        icon={faDeleteLeft}
                    />
                    <div>{t("SelectCombat")}</div>
                </div>
                <h5>{t("Players")}</h5>
                <div className="btn-shadow search-icon" onClick={() => setShowSearch((item) => !item)}>
                    <FontAwesomeIcon
                        icon={showSearch ? faMagnifyingGlassMinus : faMagnifyingGlassPlus}
                    />
                    <div>{t("Search")}</div>
                </div>
                <div className="boss">
                    <div>{details.name}</div>
                    <div className={`combat-number ${details.isWin ? 'win' : 'lose'}`}>{details.number}</div>
                </div>
                {playersDeath?.length === 0 &&
                    <div className="no-deaths">
                        <span>{t("ZeroDeaths")}</span>
                    </div>
                }
            </div>
            {showSearch &&
                <div className="mb-3 search-people">
                    <label htmlFor="inputUsername" className="form-label">{t("SearchPlayer")}</label>
                    <div className="add-new-people__search-input">
                        <input type="text" className="form-control" placeholder={t("TypeUsername") || ""} id="inputUsername"
                            ref={filterContent} onChange={handlerSearch} />
                        <FontAwesomeIcon
                            icon={faXmark}
                            title={t("Clean") || ""}
                            onClick={cleanSearch}
                        />
                    </div>
                </div>
            }
            {(combatPlayers.length > 0 && screenSize.width > maxWidth) &&
                <div className="form-check form-switch">
                    <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" onChange={() => setShowCommonStatistics((item) => !item)} />
                    <label className="form-check-label" htmlFor="flexSwitchCheckChecked">{showCommonStatistics ? t("HideCommonStatistics") : t("ShowCommonStatistics")}</label>
                </div>
            }
            {showCommonStatistics &&
                <DetailsSpecificalCombatChart
                    combatPlayers={selectedPlayers}
                />
            }
            <PersonalTabs
                tab={details.tab}
                tabs={[
                    {
                        id: 0,
                        header: t("Dashboard"),
                        content: <Dashboard
                            details={details}
                            combatPlayers={combatPlayers}
                            playersDeath={playersDeath ? playersDeath : []}
                            getValueShortName={getValueShortName}
                        />
                    },
                    {
                        id: 1,
                        header: t("Details"),
                        content: <Details
                            combatPlayers={selectedPlayers}
                            details={details}
                            getValueShortName={getValueShortName}
                            t={t}
                        />
                    }
                ]}
            />
        </div>
    );
}

export default DetailsSpecificalCombat;