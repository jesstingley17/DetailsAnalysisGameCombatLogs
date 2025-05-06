import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';

const DamageDoneGeneralHelper = ({ generalData, getProcentage, combatPlayer, getValueShortName, getSpellValueProcentage }) => {
    const fixedNumberUntil = 2;

    const { t } = useTranslation("helpers/combatDetailsHelper");

    const [showPets, setShowPets] = useState(true);
    const [hideColumns, setHideColumns] = useState([]);
    const [data, setData] = useState(generalData);
    const [totalDamage, setTotalDamage] = useState(combatPlayer.damageDone);

    useEffect(() => {
        const filteredData = showPets ? generalData : generalData.filter(x => !x.isPet);
        setData(filteredData);

        const filteredTotalDamage = filteredData.map(x => x.value);
        const sum = filteredTotalDamage.reduce((a, b) => a + b, 0);
        setTotalDamage(sum);
    }, [showPets]);

    const handleAddToHideColumns = (columnName) => {
        const hiddenCollumns = hideColumns;
        hiddenCollumns.push(columnName);

        setHideColumns(Array.from(hiddenCollumns));
    }

    const handleRemoveFromHideColumns = (columnName) => {
        const hiddenCollumns = hideColumns;
        const newArray = hiddenCollumns.filter(item => item !== columnName);

        setHideColumns(Array.from(newArray));
    }

    const tableTitle = () => {
        return (
            <li className="player-general-data-details__title" key="0">
                <ul>
                    <li>
                        {t("Spell")}
                    </li>
                    <li>
                        {t("Total")}
                    </li>
                    {!hideColumns.includes("Average") &&
                        <li className="allow-hide-column">
                            {t("Average")}
                            <FontAwesomeIcon
                                icon={faXmark}
                                title={t("Hide")}
                                onClick={() => handleAddToHideColumns("Average")}
                            />
                        </li>
                    }
                    <li>
                        {t("DPS")}
                    </li>
                    {!hideColumns.includes("Count") &&
                        <li className="allow-hide-column">
                            {t("Count")}
                            <FontAwesomeIcon
                                icon={faXmark}
                                title={t("Hide")}
                                onClick={() => handleAddToHideColumns("Count")}
                            />
                        </li>
                    }
                    {!hideColumns.includes("Crit") &&
                        <li className="allow-hide-column">
                            {t("Crit")}, %
                            <FontAwesomeIcon
                                icon={faXmark}
                                title={t("Hide")}
                                onClick={() => handleAddToHideColumns("Crit")}
                            />
                        </li>
                    }
                    {!hideColumns.includes("Miss") &&
                        <li className="allow-hide-column">
                            {t("Miss")}, %
                            <FontAwesomeIcon
                                icon={faXmark}
                                title={t("Hide")}
                                onClick={() => handleAddToHideColumns("Miss")}
                            />
                        </li>
                    }
                    {!hideColumns.includes("Max") &&
                        <li className="allow-hide-column">
                            {t("Max")}
                            <FontAwesomeIcon
                                icon={faXmark}
                                title={t("Hide")}
                                onClick={() => handleAddToHideColumns("Max")}
                            />
                        </li>
                    }
                    {!hideColumns.includes("Min") &&
                        <li className="allow-hide-column">
                            {t("Min")}
                            <FontAwesomeIcon
                                icon={faXmark}
                                title={t("Hide")}
                                onClick={() => handleAddToHideColumns("Min")}
                            />
                        </li>
                    }
                </ul>
            </li>
        );
    }

    const hiddenColumns = () => {
        return (
            <ul className="hidden-columns">
                {hideColumns.map((column, index) => (
                    <li key={index} className="allow-hide-column" onClick={() => handleRemoveFromHideColumns(column)}>
                        {t(column)}
                    </li>
                ))}
            </ul>
        );
    }

    return (
        <>
            <li className="player-general-data-details__inherit" key="-1">
                <div className="show-pets">
                    <div className="form-switch">
                        <input className="form-check-input" type="checkbox" role="switch" id="flexSwitchCheckChecked" onChange={() => setShowPets(prev => !prev)} defaultChecked={showPets} />
                        <label className="form-check-label" htmlFor="flexSwitchCheckChecked">{t("ShowPets")}</label>
                    </div>
                </div>
                <div>
                    {t("Total")}: {getValueShortName(totalDamage)}
                </div>
                {hideColumns.length > 0 && hiddenColumns()}
            </li>
            {tableTitle()}
            {data?.map((item) => (
                <li className="player-general-data-details__item" key={item.id}>
                    <ul>
                        <li>
                            {item.spell}
                        </li>
                        <li className="amount">
                            <span>{getValueShortName(item.value)}</span>
                            <span className="procentage">{getSpellValueProcentage(item, combatPlayer.damageDone)}%</span>
                        </li>
                        {!hideColumns.includes("Average") &&
                            <li>
                                {getValueShortName(item.averageValue)}
                            </li>
                        }
                        <li>
                            {item.damagePerSecond.toFixed(fixedNumberUntil)}
                        </li>
                        {!hideColumns.includes("Count") &&
                            <li>
                                {item.castNumber}
                            </li>
                        }
                        {!hideColumns.includes("Crit") &&
                            <li>
                                {getProcentage(item.critNumber, item.castNumber)}%
                            </li>
                        }
                        {!hideColumns.includes("Miss") &&
                            <li>
                                {getProcentage(item.missNumber, item.castNumber)}%
                            </li>
                        }
                        {!hideColumns.includes("Max") &&
                            <li>
                                {getValueShortName(item.maxValue)}
                            </li>
                        }
                        {!hideColumns.includes("Min") &&
                            <li>
                                {getValueShortName(item.minValue)}
                            </li>
                        }
                    </ul>
                </li>
            ))}
        </>
    );
}

export default DamageDoneGeneralHelper;