import { getInsightParts, type DemoSnapshot } from '@/features/gameLogs/demo/combatInsights';
import { faArrowLeft, faArrowRight, faBolt, faHeart } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useMemo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

import './DemoEncounterPage.scss';

function formatDuration(totalSeconds: number): string {
    const m = Math.floor(totalSeconds / 60);
    const s = totalSeconds % 60;
    return `${m}:${s.toString().padStart(2, '0')}`;
}

const DemoEncounterPage: React.FC = () => {
    const { t } = useTranslation('demo');
    const navigate = useNavigate();

    const [snapshot, setSnapshot] = useState<DemoSnapshot | null>(null);
    const [loadError, setLoadError] = useState(false);

    useEffect(() => {
        let cancelled = false;
        (async () => {
            try {
                const res = await fetch('/demo/combat-demo.json');
                if (!res.ok) {
                    throw new Error(String(res.status));
                }
                const data = (await res.json()) as DemoSnapshot;
                if (!cancelled) {
                    setSnapshot(data);
                }
            } catch {
                if (!cancelled) {
                    setLoadError(true);
                }
            }
        })();
        return () => {
            cancelled = true;
        };
    }, []);

    const insightParts = useMemo(() => (snapshot ? getInsightParts(snapshot) : []), [snapshot]);

    if (loadError) {
        return (
            <div className="demo-page">
                <p className="demo-page__error">{t('LoadError')}</p>
                <button type="button" className="demo-page__btn demo-page__btn--ghost" onClick={() => navigate('/')}>
                    <FontAwesomeIcon icon={faArrowLeft} /> {t('CtaHome')}
                </button>
            </div>
        );
    }

    if (!snapshot) {
        return (
            <div className="demo-page demo-page--loading">
                <div className="demo-page__spinner" aria-hidden />
                <span className="visually-hidden">Loading</span>
            </div>
        );
    }

    return (
        <div className="demo-page">
            <div className="demo-page__bg" aria-hidden />
            <div className="demo-page__inner">
                <header className="demo-page__header">
                    <span className="demo-page__badge">{t('PageTitle')}</span>
                    <h1 className="demo-page__title">{snapshot.encounterName}</h1>
                    <p className="demo-page__sub">{t('PageSubtitle')}</p>
                </header>

                <section className="demo-page__meta">
                    <div>
                        <div className="demo-page__metaLabel">{t('Player')}</div>
                        <div className="demo-page__metaValue">{snapshot.playerDisplayName}</div>
                    </div>
                    <div>
                        <div className="demo-page__metaLabel">{t('Spec')}</div>
                        <div className="demo-page__metaValue">{snapshot.specLabel}</div>
                    </div>
                    <div>
                        <div className="demo-page__metaLabel">{t('Role')}</div>
                        <div className="demo-page__metaValue">{snapshot.roleLabel}</div>
                    </div>
                    <div>
                        <div className="demo-page__metaLabel">{t('Duration')}</div>
                        <div className="demo-page__metaValue">{formatDuration(snapshot.durationSeconds)}</div>
                    </div>
                </section>

                <section className="demo-page__scores" aria-labelledby="demo-scores-heading">
                    <h2 id="demo-scores-heading" className="demo-page__sectionTitle">
                        {t('ScoresTitle')}
                    </h2>
                    <div className="demo-page__scoreGrid">
                        <div className="demo-page__scoreCard">
                            <FontAwesomeIcon icon={faBolt} className="demo-page__scoreIcon" />
                            <div className="demo-page__scoreLabel">{t('DamageScore')}</div>
                            <div className="demo-page__scoreNumber">{snapshot.damageScore.toFixed(1)}%</div>
                        </div>
                        <div className="demo-page__scoreCard">
                            <FontAwesomeIcon icon={faHeart} className="demo-page__scoreIcon demo-page__scoreIcon--heal" />
                            <div className="demo-page__scoreLabel">{t('HealScore')}</div>
                            <div className="demo-page__scoreNumber">{snapshot.healScore.toFixed(1)}%</div>
                        </div>
                    </div>
                </section>

                <section className="demo-page__insights" aria-labelledby="demo-insights-heading">
                    <h2 id="demo-insights-heading" className="demo-page__sectionTitle">
                        {t('InsightsTitle')}
                    </h2>
                    <ol className="demo-page__insightList">
                        {insightParts.map((part, index) => (
                            <li key={part.key + String(index)} className="demo-page__insightItem">
                                <span className="demo-page__insightIndex">{index + 1}</span>
                                <p className="demo-page__insightText">{t(part.key, part.params)}</p>
                            </li>
                        ))}
                    </ol>
                </section>

                <p className="demo-page__disclaimer">{t('Disclaimer')}</p>

                <div className="demo-page__actions">
                    <button type="button" className="demo-page__btn demo-page__btn--ghost" onClick={() => navigate('/')}>
                        <FontAwesomeIcon icon={faArrowLeft} /> {t('CtaHome')}
                    </button>
                    <button type="button" className="demo-page__btn demo-page__btn--primary" onClick={() => navigate('/game-combat-logs')}>
                        {t('CtaCombatLogs')}
                        <FontAwesomeIcon icon={faArrowRight} />
                    </button>
                </div>
            </div>
        </div>
    );
}

export default DemoEncounterPage;
