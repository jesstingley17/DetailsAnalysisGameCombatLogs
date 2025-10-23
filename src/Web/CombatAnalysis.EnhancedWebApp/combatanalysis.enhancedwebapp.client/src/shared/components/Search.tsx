import { useLazyFindUserQuery } from '@/features/user/api/User.api';
import PeopleItem from '@/features/user/components/people/PeopleItem';
import type { AppUserModel } from '@/features/user/types/AppUserModel';
import { useRef, useState, type ChangeEvent } from 'react';
import logger from '@/utils/Logger';

const Search: React.FC<{ t: (key: string) => string }> = ({ t }) => {
    const [users, setUsers] = useState<AppUserModel[]>([]);
    const [showSearch, setShowSearch] = useState(false);

    const [findUser] = useLazyFindUserQuery();

    const searchText = useRef<HTMLInputElement | null>(null);

    const findUserAsync = async (prefix: string) => {
        try {
            const users = await findUser(prefix).unwrap();

            if (users) {
                setUsers(users);
                setShowSearch(true);
            }
        } catch (e) {
            logger.error("Failed to loading users", e);
        }
    }

    const searchTextHandle = async (e: ChangeEvent<HTMLInputElement>) => {
        if (e.target.value.length === 0) {
            setUsers([]);
            return;
        }

        await findUserAsync(e.target.value);
    }

    return (
        <div className="search">
            <div className="search__container">
                <input type="text" className="form-control" placeholder={t("UsersSearch")} id="inputUsername" autoComplete="off" ref={searchText}
                    onChange={searchTextHandle}
                />
            </div>
            <div className={`search__content${showSearch ? "_active" : ""}`}>
                <div>{t("Users")}</div>
                <div className="container">
                    {users.length === 0
                        ? <div className="empty">{t("Empty")}</div>
                        : <ul className="people__cards">
                            {users?.map((user) => (
                                    <li key={user.id}>
                                        <PeopleItem
                                            targetUser={user}
                                        />
                                    </li>
                                ))
                            }
                        </ul>
                    }
                </div>
                <div className="close" onClick={() => setShowSearch(false)}>{t("Close")}</div>
            </div>
        </div>
    );
}

export default Search;