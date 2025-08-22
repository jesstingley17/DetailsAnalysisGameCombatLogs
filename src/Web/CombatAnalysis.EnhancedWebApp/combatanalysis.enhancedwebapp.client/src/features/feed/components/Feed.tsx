import type { RootState } from '@/app/Store';
import Loading from '@/shared/components/Loading';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import CommunicationMenu from './CommunicationMenu';
import FeedParticipants from './FeedParticipants';
import CreateUserPost from './post/CreateUserPost';

const Feed: React.FC = () => {
    const { t } = useTranslation('communication/feed');

    const myself = useSelector((state: RootState) => state.user.value);

    if (!myself) {
        return (
            <>
                <CommunicationMenu
                    currentMenuItem={0}
                    hasSubMenu={false}
                />
                <Loading />
            </>
        );
    }

    return (
        <>
            <div className="communication-content">
                <CreateUserPost
                    user={myself}
                    owner={myself?.username}
                    t={t}
                />
                <FeedParticipants
                    myself={myself}
                    t={t}
                />
            </div>
            <CommunicationMenu
                currentMenuItem={0}
                hasSubMenu={false}
            />
        </>
    );
}

export default Feed;