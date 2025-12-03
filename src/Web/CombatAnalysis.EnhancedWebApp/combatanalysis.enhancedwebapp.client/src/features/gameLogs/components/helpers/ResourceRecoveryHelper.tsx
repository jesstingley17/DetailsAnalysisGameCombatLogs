import { useEffect, useState, type JSX } from 'react';
import useTime from '../../../../shared/hooks/useTime';
import {
    useGetResourceRecoveryByFilterQuery,
    useGetResourceRecoveryCountByFilterQuery,
    useGetResourceRecoveryUniqueFilterValuesQuery
} from '../../api/ResourcesRecovery.api';
import DetailsFilter from './DetailsFilter';
import PaginationHelper from './PaginationHelper';

interface ResourceRecoveryHelperProps {
    combatPlayerId: number;
    pageSize: number;
    getUserNameWithoutRealm: (username: string) => string;
    t: (key: string) => string;
}

const ResourceRecoveryHelper: React.FC<ResourceRecoveryHelperProps> = ({ combatPlayerId, pageSize, getUserNameWithoutRealm, t }) => {
    const { getTimeWithoutMs } = useTime();

    const [page, setPage] = useState(1);
    const [selectedFilter, setSelectedFilter] = useState({ filter: "None", value: -1 });

    const { data: count, isLoading: countIsLoading } = useGetResourceRecoveryCountByFilterQuery(
        { combatPlayerId, filter: selectedFilter.filter, filterValue: selectedFilter.value }
    );
    const { data, isLoading } = useGetResourceRecoveryByFilterQuery(
        { combatPlayerId, filter: selectedFilter.filter, filterValue: selectedFilter.value, page, pageSize }
    );

    const totalPages = Math.ceil(count ?? 1 / pageSize);

    useEffect(() => {
        setPage(1);
    }, [selectedFilter]);

    const tableTitle = (): JSX.Element => {
        return (
            <li className="player-data-details__title" key="0">
                <ul>
                    <li>
                        {t("Spell")}
                    </li>
                    <li>
                        {t("Time")}
                    </li>
                    <li>
                        {t("Value")}
                    </li>
                    <li>
                        {t("Creator")}
                    </li>
                </ul>
            </li>
        );
    }

    if (isLoading || countIsLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <>
            <div className="player-filter-details">
                <DetailsFilter
                    combatPlayerId={combatPlayerId}
                    setSelectedFilter={setSelectedFilter}
                    selectedFilter={selectedFilter}
                    filter="Creator"
                    filterName={t("Creator")}
                    useGetUniqueFilterValuesQuery={useGetResourceRecoveryUniqueFilterValuesQuery}
                    t={t}
                />
                <DetailsFilter
                    combatPlayerId={combatPlayerId}
                    setSelectedFilter={setSelectedFilter}
                    selectedFilter={selectedFilter}
                    filter="Spell"
                    filterName={t("Spell")}
                    useGetUniqueFilterValuesQuery={useGetResourceRecoveryUniqueFilterValuesQuery}
                    t={t}
                />
            </div>
            <ul className="player-data-details">
                {tableTitle()}
                {data?.map((item) => (
                    <li className="player-data-details__item" key={item.id}>
                        <ul>
                            <li>{item.spell}</li>
                            <li>
                                <div>{getTimeWithoutMs(item.time)}</div>
                            </li>
                            <li>{item.value}</li>
                            <li>{getUserNameWithoutRealm(item.creator)}</li>
                        </ul>
                    </li>
                ))}
            </ul>
            <PaginationHelper
                setPage={setPage}
                page={page}
                totalPages={totalPages}
                t={t}
            />
        </>
    );
}

export default ResourceRecoveryHelper;