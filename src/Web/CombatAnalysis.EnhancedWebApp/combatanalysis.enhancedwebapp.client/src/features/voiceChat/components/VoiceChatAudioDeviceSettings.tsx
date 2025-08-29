import { memo, useEffect, useState, type RefObject, type SetStateAction } from 'react';

interface VoiceChatAudioDeviceSettingsProps {
	t: (key: string) => string;
	peerConnectionsRef: RefObject<Map<string, RTCPeerConnection>>;
	turnOnMicrophone: boolean;
	stream: MediaStream;
	audioInputDeviceId: string;
	setAudioInputDeviceId: (value: SetStateAction<string>) => void;
	audioOutputDeviceId: string;
	setAudioOutputDeviceId: (value: SetStateAction<string>) => void;
}

const VoiceChatAudioDeviceSettings: React.FC<VoiceChatAudioDeviceSettingsProps> = ({
	t,
	peerConnectionsRef,
	turnOnMicrophone,
	stream,
	audioInputDeviceId,
	setAudioInputDeviceId,
	audioOutputDeviceId,
	setAudioOutputDeviceId
}) => {
	const [audioInputDevices, setAudioInputDevices] = useState<MediaDeviceInfo[]>([]);
	const [audioOutputDevices, setAudioOutputDevices] = useState<MediaDeviceInfo[]>([]);

	useEffect(() => {
		const fetchDevices = async () => {
			const { audioInputs, audioOutputs } = await getAvailableAudioDevicesAsync();

			setAudioInputDeviceId(audioInputDeviceId || audioInputs[0]?.deviceId);
			setAudioOutputDeviceId(audioOutputDeviceId || audioOutputs[0]?.deviceId);

			setAudioInputDevices(audioInputs);
			setAudioOutputDevices(audioOutputs);
		};

		fetchDevices();
	}, []);

	const getAvailableAudioDevicesAsync = async () => {
		const devices = await navigator.mediaDevices.enumerateDevices();
		const audioInputs = devices.filter(device => device.kind === "audioinput" && device.deviceId !== "communications");
		const audioOutputs = devices.filter(device => device.kind === "audiooutput" && device.deviceId !== "communications");

		return { audioInputs, audioOutputs };
	}

	const switchAudioInputDevice = async (deviceId: string) => {
		if (!stream) {
			return;
		}

		const newStream = await navigator.mediaDevices.getUserMedia({ audio: { deviceId: { exact: deviceId } } });
		const newAudioTrack = newStream.getAudioTracks()[0];
		newAudioTrack.enabled = turnOnMicrophone;

		const oldAudioTrack = stream.getAudioTracks()[0];
		stream.removeTrack(oldAudioTrack);
		stream.addTrack(newAudioTrack);

		for (const peerConnection of peerConnectionsRef.current.values()) {
			const sender = peerConnection.getSenders().find(s => s.track && s.track.kind === "audio");
			if (sender) sender.replaceTrack(newAudioTrack);
		}

		setAudioInputDeviceId(deviceId);
	}

	const switchAudioOutputDevice = (deviceId: string) => {
		setAudioOutputDeviceId(deviceId);
	}

	const renderDeviceList = (devices: MediaDeviceInfo[], currentDeviceId: string, switchDeviceFunction: (deviceId: string) => void) => (
		<select
			className="form-select"
			value={currentDeviceId}
			onChange={async (e) => await switchDeviceFunction(e.target.value)}
		>
			{devices.map((device: MediaDeviceInfo, index: number) => (
				<option key={index} value={device.deviceId}>
					{device.label.split("-")[0]}
				</option>
			))}
		</select>
	)

	return (
		<div className="device-toolbar">
			<div className="device-toolbar__title">{t("SoundSettings")}</div>
			{renderDeviceList(audioOutputDevices, audioOutputDeviceId, switchAudioOutputDevice)}
			<div className="device-toolbar__title">{t("MicrophoneSettings")}</div>
			{renderDeviceList(audioInputDevices, audioInputDeviceId, switchAudioInputDevice)}
		</div>
	);
}

export default memo(VoiceChatAudioDeviceSettings);