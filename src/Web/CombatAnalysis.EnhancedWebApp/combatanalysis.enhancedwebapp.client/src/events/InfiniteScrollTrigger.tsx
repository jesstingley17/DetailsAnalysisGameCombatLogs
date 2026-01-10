import { useEffect, useRef } from 'react';

interface Props {
    onLoadMore: () => void;
    hasMore: boolean;
    isLoading: boolean;
}

const InfiniteScrollTrigger: React.FC<Props> = ({ onLoadMore, hasMore, isLoading }) => {
    const ref = useRef<HTMLDivElement | null>(null);

    useEffect(() => {
        if (!ref.current || !hasMore || isLoading) {
            return;
        }

        const observer = new IntersectionObserver(
            ([entry]) => {
                if (entry.isIntersecting) {
                    onLoadMore();
                }
            },
            { threshold: 1.0 }
        );

        observer.observe(ref.current);

        return () => observer.disconnect();
    }, [onLoadMore, hasMore, isLoading]);

    return (
        <div ref={ref} style={{ height: 1 }} />
    );
}

export default InfiniteScrollTrigger;