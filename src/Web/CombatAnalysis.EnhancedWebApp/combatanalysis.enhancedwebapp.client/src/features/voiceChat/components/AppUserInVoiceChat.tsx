import type { RootState } from '@/app/Store';
import { faMicrophone, faMicrophoneSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef } from 'react';
import { useSelector } from 'react-redux';

interface AppUserInVoiceChatProps {
    micStatus: boolean;
    cameraStatus: boolean;
    localStream: MediaStream | null;
}

const AppUserInVoiceChat: React.FC<AppUserInVoiceChatProps> = ({ micStatus, cameraStatus, localStream }) => {
    const appUser = useSelector((state: RootState) => state.user.value);

    const videosRef = useRef<HTMLVideoElement | null>(null);

    useEffect(() => {
        if (!localStream) {
            return;
        }

        if (cameraStatus && videosRef && videosRef.current) {
            videosRef.current.srcObject = localStream;
            videosRef.current.muted = true;
            videosRef.current.autoplay = true;
        }
        else {
            videosRef.current= null;
        }
    }, [cameraStatus, localStream]);

    if (!appUser) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="user me">
            <div className="information">
                <div className="another__username">{appUser?.username}</div>
                <FontAwesomeIcon
                    icon={micStatus ? faMicrophone : faMicrophoneSlash}
                    title="TurnOffMicrophone"
                />
            </div>
            {cameraStatus &&
                <div className="media-content">
                    <video ref={videosRef} muted></video>
                </div>
            }
        </div>
    );
}

export default AppUserInVoiceChat;