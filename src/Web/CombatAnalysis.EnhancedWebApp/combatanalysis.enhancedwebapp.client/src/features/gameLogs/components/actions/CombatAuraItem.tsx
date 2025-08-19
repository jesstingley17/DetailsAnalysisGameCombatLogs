import { faEye, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState, type SetStateAction } from 'react';
import type { CombatAuraModel } from '../../types/CombatAuraModel';
import CombatAuraTargets from './CombatAuraTargets';

interface CombatAuraItemProps {
    selectedCreatorAuras: CombatAuraModel[];
    pinnedAuras: CombatAuraModel[];
    selectedCreator: string;
    setDefaultAurasWhenPin: (value: SetStateAction<CombatAuraModel[]>) => void;
    removeQuotes: (str: string) => string;
    t: (key: string) => string;
}

const CombatAuraItem: React.FC<CombatAuraItemProps> = ({ selectedCreatorAuras, pinnedAuras, selectedCreator, setDefaultAurasWhenPin, removeQuotes, t }) => {
    const [auras, setAuras] = useState(new Map());
    const [defaultAuras, setDefaultAuras] = useState(new Map());
    const [selectedAura, setSelectedAura] = useState("");
    const [showTargets, setShowTargets] = useState(false);

    useEffect(() => {
        makeCreatorAurasMap();
    }, [selectedCreatorAuras]);

    useEffect(() => {
        if (selectedCreatorAuras.length === 0) {
            return;
        }

        const auraMap = makeCreatorAurasMap();
        setDefaultAuras(auraMap);
    }, [selectedCreator]);

    useEffect(() => {
        if (pinnedAuras.length === 0) {
            setAuras(defaultAuras);

            return;
        }

        const auraMap = makeCreatorAurasMap();
        updateAuras(auraMap);
    }, [pinnedAuras]);

    const makeCreatorAurasMap = (): Map<string, CombatAuraModel[]> => {
        const auraMap = new Map<string, CombatAuraModel[]>();

        selectedCreatorAuras?.forEach(aura => {
            if (auraMap.has(aura.name)) {
                const creatorAuras = auraMap.get(aura.name);
                if (creatorAuras) {
                    creatorAuras.push(aura);

                    auraMap.set(aura.name, creatorAuras);
                }
            } else {
                auraMap.set(aura.name, [aura]);
            }
        });

        setAuras(auraMap);

        return auraMap;
    }

    const updateAuras = (newAuras: Map<string, CombatAuraModel[]>) => {
        const auraMap = new Map();
        let makeDefaultAuras = new Array<CombatAuraModel>();

        pinnedAuras?.forEach(pinnedAura => {
            newAuras?.forEach((value, key) => {
                const times = value.map(aura => ({ start: aura.startTime, finish: aura.finishTime, data: aura }));

                const included = times.filter(auraTime => auraTime.start <= pinnedAura.startTime && auraTime.finish <= pinnedAura.finishTime);

                const isContains = included.length > 0;
                if (isContains) {
                    const newData = included.map(aura => aura.data);
                    makeDefaultAuras = makeDefaultAuras.concat(newData);
                    auraMap.set(key, newData);
                }
            });
        });

        setAuras(auraMap);
        setDefaultAurasWhenPin(makeDefaultAuras);
    }

    const handleSelectAura = (auraName: string) => {
        if (selectedAura === auraName && showTargets) {
            setShowTargets(false);
            setSelectedAura("");

            return;
        }

        setShowTargets(true);
        setSelectedAura(auraName);
    }

    const handlePinAura = (auraName: string, aura: CombatAuraModel) => {
        const contains = pinnedAuras.filter(pin => pin.name === auraName).length > 0;
        if (contains) {
            return;
        }

        pinnedAuras.push(aura);
    }

    return (
        <ul className="creator-auras">
            {Array.from(auras.entries()).map(([key, value]) => (
                <li key={key} className="creator-auras__details">
                    <ul className="details-collection">
                        <li className={`details-collection__spell${pinnedAuras.find((aura: CombatAuraModel) => aura.name === key) ? '' : '_ready'}`}
                            onClick={() => pinnedAuras.includes(key) ? null : handlePinAura(key, value)}>
                            {!pinnedAuras.includes(key) &&
                                <FontAwesomeIcon
                                    icon={faPlus}
                                />
                            }
                            <div>{removeQuotes(key)}</div>
                        </li>
                        <li>{value.length}</li>
                        <li>
                            <div className={`btn-shadow ${selectedAura === key ? 'details-opened' : ''}`} onClick={() => handleSelectAura(key)}>
                                <FontAwesomeIcon
                                    icon={faEye}
                                />
                                <div>{t("Targets")}</div>
                            </div>
                            {(showTargets && selectedAura === key) &&
                                <CombatAuraTargets
                                    combatAuras={value}
                                />
                            }
                        </li>
                    </ul>
                </li>
            ))}
        </ul>
    );
}

export default CombatAuraItem;