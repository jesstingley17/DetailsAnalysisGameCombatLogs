import "../../styles/customTooltip.scss";

const CustomTooltip = ({ payload, t }) => {
    if (payload && payload.length) {
        return (
            <div className="customTooltip">
                <div className="tooltipDetails">
                    <p className="label">{payload[0].payload.name}</p>
                    <p>{t("Value")}: {payload[0].payload.value}</p>
                </div>
            </div>
        );
    }

    return null;
};

export default CustomTooltip;