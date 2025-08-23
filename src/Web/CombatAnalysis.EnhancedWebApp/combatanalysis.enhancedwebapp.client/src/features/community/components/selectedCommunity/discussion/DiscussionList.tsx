import { faMagnifyingGlassMinus, faMagnifyingGlassPlus, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState, type ChangeEvent, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import type { CommunityDiscussionModel } from '../../../types/CommunityDiscussionModel';

import './DiscussionList.scss';

interface DiscussionListProps {
    discussions: CommunityDiscussionModel[];
    setShowDiscussion: (value: SetStateAction<boolean>) => void;
    handleDiscussion: (discussion: CommunityDiscussionModel) => void;
}

const DiscussionList: React.FC<DiscussionListProps> = ({ discussions, setShowDiscussion, handleDiscussion }) => {
    const { t } = useTranslation("communication/community/discussion");

    const [showSearchPeople, setShowSearchPeople] = useState(false);
    const [filteredContent, setFilteredContent] = useState("");

    const hidePeopleInspectionMode = () => {
        setShowDiscussion(false);
    }

    const openDiscussion = (item: CommunityDiscussionModel) => {
        setShowDiscussion(false);

        handleDiscussion(item);
    }

    const filterDiscussionsHandle = (event: ChangeEvent<HTMLInputElement>) => {
        const content = event.target.value;

        setFilteredContent(content);
    }

    const clear = () => {
        setFilteredContent("");
    }

    return (
        <div className="discussion-inspection">
            <div className="title">
                {showSearchPeople
                    ? <FontAwesomeIcon
                        icon={faMagnifyingGlassMinus}
                        title={t("HideSearchDiscussion")}
                        onClick={() => setShowSearchPeople(false)}
                    />
                    : <FontAwesomeIcon
                        icon={faMagnifyingGlassPlus}
                        title={t("ShowSearchDiscussion")}
                        onClick={() => setShowSearchPeople(true)}
                    />
                }
                <div>{t("Discussion")}</div>
            </div>
            <div className={`mb-3 discussion-inspection__search${showSearchPeople ? "_active" : ""}`}>
                <label htmlFor="inputDiscussion" className="form-label">{t("SearchDiscussion")}</label>
                <div className="discussion-inspection__search-input">
                    <input type="text" className="form-control" placeholder={t("TypeDiscussion")} id="inputDiscussion"
                        onChange={filterDiscussionsHandle} value={filteredContent} />
                    <FontAwesomeIcon
                        icon={faXmark}
                        title={t("Clean")}
                        onClick={clear}
                    />
                </div>
            </div>
            <div className="divide"></div>
            <ul className="discussion-inspection__content">
                {filteredContent !== ""
                    ? discussions?.filter(x => x.title.toLowerCase().startsWith(filteredContent.toLowerCase())).map((item) => (
                        <li key={item.id} title={item.title} onClick={() => openDiscussion(item)}>
                            {item.title}
                        </li>
                    ))
                    : discussions?.map((item) => (
                        <li key={item.id} title={item.title} onClick={() => openDiscussion(item)}>
                            {item.title}
                        </li>
                    ))
                }
            </ul>
            <div className="item-result">
                <input type="button" value={t("Close")} className="btn btn-secondary" onClick={hidePeopleInspectionMode} />
            </div>
        </div>
    );
}

export default DiscussionList;