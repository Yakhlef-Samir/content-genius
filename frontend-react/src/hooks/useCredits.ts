import { useState, useEffect, useCallback } from 'react';
import { contentService } from '../services/api';

export const useCredits = () => {
    const [credits, setCredits] = useState<number>(0);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const fetchCredits = useCallback(async () => {
        try {
            const balance = await contentService.getCreditsBalance();
            setCredits(balance);
            setError(null);
        } catch (err: any) {
            setError(err.message || 'Failed to fetch credits');
        } finally {
            setIsLoading(false);
        }
    }, []);

    useEffect(() => {
        fetchCredits();
    }, [fetchCredits]);

    const refreshCredits = useCallback(() => {
        setIsLoading(true);
        fetchCredits();
    }, [fetchCredits]);

    return {
        credits,
        isLoading,
        error,
        refreshCredits,
    };
};
