import { memo } from 'react';
import type { CombatPlayerModel } from '../../types/CombatPlayerModel';
import PlayerInfoItem from './PlayerInfoItem';

const PlayerInfo: React.FC<{ combatPlayers: CombatPlayerModel[] }> = ({ combatPlayers }) => {
    return (
        <div className="details">
            <ul>
                {combatPlayers?.map((combatPlayer) => (
                    <li key={combatPlayer.id} className="card">
                        <div className="card-body">
                            <h5 className="card-title">{combatPlayer.player.username}</h5>
                        </div>
                        <PlayerInfoItem
                            stats={combatPlayer.stats}
                        />
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default memo(PlayerInfo);