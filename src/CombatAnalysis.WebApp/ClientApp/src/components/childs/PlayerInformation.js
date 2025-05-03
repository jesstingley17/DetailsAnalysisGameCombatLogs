import { memo } from "react";
import CommonPlayerInform from './CommonPlayerInform';

const PlayerInformation = ({ combatPlayers, combatDetails, getValueShortName }) => {
    return (
        <ul>
            {combatPlayers?.map((player) => (
                    <li key={player.id} className="card">
                        <div className="card-body">
                            <h5 className="card-title">{player.username}</h5>
                        </div>
                        <CommonPlayerInform
                            player={player}
                            combatDetails={combatDetails}
                            getValueShortName={getValueShortName}
                        />
                    </li>
                ))}
        </ul>
    );
}

export default memo(PlayerInformation);