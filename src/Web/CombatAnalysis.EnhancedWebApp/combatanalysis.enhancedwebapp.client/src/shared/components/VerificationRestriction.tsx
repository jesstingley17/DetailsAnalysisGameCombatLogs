import { faCircleQuestion } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';

import './VerificationRestriction.scss';

const VerificationRestriction: React.FC<{ contentText: string, infoText: string }> = ({ contentText, infoText }) => {
    const [showForbiddenInfo, setShowForbiddenInfo] = useState(false);

    return (
        <div className="forbidden">
            <div className="forbidden__content">{contentText}</div>
            <FontAwesomeIcon
                icon={faCircleQuestion}
                className={showForbiddenInfo ? "forbidden__info_active" : "forbidden__info_hide"}
                onClick={() => setShowForbiddenInfo((prev) => !prev)}
            />
            {showForbiddenInfo &&
                <div className="forbidden__info">{infoText}</div>
            }
        </div>
    );
}

export default VerificationRestriction;