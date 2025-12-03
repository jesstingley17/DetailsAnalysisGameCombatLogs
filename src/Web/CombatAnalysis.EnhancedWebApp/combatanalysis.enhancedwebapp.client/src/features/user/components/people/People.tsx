import type { RootState } from '@/app/Store';
import CommunicationMenu from '@/shared/components/CommunicationMenu';
import Loading from '@/shared/components/Loading';
import { useCallback, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useGetUsersQuery } from '../../api/User.api';
import type { AppUserModel } from '../../types/AppUserModel';
import PeopleItem from './PeopleItem';

import './People.scss';

const peopleInterval = 3000;

const People: React.FC = () => {
    const { t } = useTranslation('communication/people/people');

    const myself = useSelector((state: RootState) => state.user.value);

    const [skipFetching, setSkipFetching] = useState(true);

    const { people, isLoading } = useGetUsersQuery(undefined, {
        pollingInterval: peopleInterval,
        skip: skipFetching,
        selectFromResult: ({ data, isLoading }) => ({
            people: data !== undefined ? data.filter((item) => item.id !== myself?.id) : [],
            isLoading
        }),
    });

    useEffect(() => {
        setSkipFetching(!myself ? true : false);
    }, [myself]);

    const peopleListFilter = useCallback((value: AppUserModel) => {
        if (value.id !== myself?.id) {
            return value;
        }
    }, []);

    if (isLoading || !people) {
        return (
            <>
                <CommunicationMenu
                    currentMenuItem={4}
                    hasSubMenu={false}
                />
                <Loading />
            </>
        );
    }

    return (
        <>
            <div className="communication-content people">
                <div>
                    <div className="people__title">{t("People")}</div>
                </div>
                <ul className="people__cards">
                    {people?.filter(peopleListFilter).map((item: AppUserModel) => (
                            <li className="person" key={item.id}>
                                <PeopleItem
                                    targetUser={item}
                                />
                            </li>
                        ))
                    }
                </ul>
            </div>
            <CommunicationMenu
                currentMenuItem={4}
                hasSubMenu={false}
            />
        </>
    );
}

export default People;