import type { SpellsDataModel } from '../types/SpellsDataModel';

import './CustomTooltip.scss';

interface CustomTooltipProps {
    payload: SpellsDataModel[];
    t: (key: string) => string;
}

const CustomTooltip: React.FC<CustomTooltipProps> = ({ payload, t }) => {
    if (payload && payload.length) {
        return (
            <div className="customTooltip">
                <div className="tooltipDetails">
                    <p className="label">{payload[0].name}</p>
                    <p>{t("Value")}: {payload[0].value}</p>
                </div>
            </div>
        );
    }

    return null;
}

export default CustomTooltip;