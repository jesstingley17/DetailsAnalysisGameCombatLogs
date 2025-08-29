import * as signalR from '@microsoft/signalr';
import { useEffect, useRef, useState, type RefObject, type SetStateAction } from 'react';
import AppUserInVoiceChat from './AppUserInVoiceChat';
import VoiceChatUser from './VoiceChatUser';

interface VoiceChatContentProps {
	hubConnection: signalR.HubConnection | null;
	peerConnections: Map<string, RTCPeerConnection>;
	stream: MediaStream | null;
	mediaRequestsAsync: () => Promise<void>;
	micStatus: boolean;
	cameraStatus: boolean;
	screenSharing: boolean;
	setScreenSharing: (value: SetStateAction<boolean>) => void;
	screenSharingVideoRef: RefObject<HTMLVideoElement | null>;
	audioOutputDeviceId: string;
}

const VoiceChatContent: React.FC<VoiceChatContentProps> = ({
	hubConnection,
	peerConnections,
	stream,
	mediaRequestsAsync,
	micStatus,
	cameraStatus,
	screenSharing,
	setScreenSharing,
	screenSharingVideoRef,
	audioOutputDeviceId
}) => {
	const [usersId, setUsersId] = useState<string[]>([]);
	const [myId, setMyId] = useState("");
	const [otherScreenSharing, setOtherScreenSharing] = useState(false);

	const otherScreenSharingVideoRef = useRef(null);
	const otherScreenSharingUserIdRef = useRef("");

	useEffect(() => {
		if (!hubConnection) {
			return;
		}

		callConnectedUsers();
	}, [hubConnection]);

	useEffect(() => {
		if (otherScreenSharing) {
			setScreenSharing(false);
		}
	}, [otherScreenSharing]);

	useEffect(() => {
		if (!myId || !hubConnection) {
			return;
		}

		const handleReceiveConnectedUsers = async (connectedUsersId: string[] | null) => {
			const anotherUsers = connectedUsersId?.filter((userId) => userId !== myId) ?? [];
			setUsersId(anotherUsers);

			await mediaRequestsAsync();
		}

		const handleUserLeft = (userId: string) => {
			if (userId === otherScreenSharingUserIdRef.current) {
				setOtherScreenSharing(false);
				otherScreenSharingUserIdRef.current = "";
			}

			const anotherUsers = usersId.filter(element => element !== userId);
			setUsersId(anotherUsers);
		}

		const handleUserJoined = (userId: string) => {
			const joinedUsers = Object.assign([], usersId);
			joinedUsers.push(userId);

			setUsersId(joinedUsers);
		}

		hubConnection.on("ReceiveConnectedUsers", handleReceiveConnectedUsers);
		hubConnection.on("UserJoined", handleUserJoined);
		hubConnection.on("UserLeft", handleUserLeft);

		return () => {
			hubConnection.off("ReceiveConnectedUsers", handleReceiveConnectedUsers);
			hubConnection.off("UserJoined", handleUserJoined);
			hubConnection.off("UserLeft", handleUserLeft);
		}
	}, [hubConnection, myId, usersId]);

	const callConnectedUsers = () => {
		hubConnection?.on("Connected", (userId) => {
			setMyId(userId);
		});
	}

    return (
		<div className="voice__content">
			{screenSharing &&
				<div className="sharing">
					<video ref={screenSharingVideoRef}></video>
				</div>
			}
			{otherScreenSharing &&
				<div className="sharing">
					<video ref={otherScreenSharingVideoRef}></video>
				</div>
			}
			<ul className={`users ${otherScreenSharing || screenSharing ? "sharing-content" : ""}`}>
				<li>
					<AppUserInVoiceChat
						micStatus={micStatus}
						cameraStatus={cameraStatus}
						localStream={stream}
					/>
				</li>
				{usersId.map((userId) =>
					<li key={userId}>
						<VoiceChatUser
							userId={userId}
							hubConnection={hubConnection}
							peerConnection={peerConnections.get(userId)}
							otherScreenSharingVideoRef={otherScreenSharingVideoRef}
							otherScreenSharingUserIdRef={otherScreenSharingUserIdRef}
							otherScreenSharing={otherScreenSharing}
							setOtherScreenSharing={setOtherScreenSharing}
							audioOutputDeviceId={audioOutputDeviceId}
						/>
					</li>
				)}
			</ul>
		</div>
    );
}

export default VoiceChatContent;