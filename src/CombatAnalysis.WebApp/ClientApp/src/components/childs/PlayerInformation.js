import { memo } from "react";
import CommonPlayerInform from './CommonPlayerInform';

const PlayerInformation = ({ combatPlayers, details, getValueShortName }) => {
    return (
        <ul>
            {combatPlayers?.map((player) => (
                    <li key={player.id} className="card">
                        <div className="card-body">
                            <h5 className="card-title">{player.username}</h5>
                        </div>
                        <CommonPlayerInform
                            player={player}
                            details={details}
                            getValueShortName={getValueShortName}
                        />
                    </li>
                ))}
        </ul>
    );
}

export default memo(PlayerInformation);