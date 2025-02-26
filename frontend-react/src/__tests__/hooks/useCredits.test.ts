import { renderHook, act } from '@testing-library/react';
import { useCredits } from '../../hooks/useCredits';
import { contentService } from '../../services/api';

// Mock the contentService
jest.mock('../../services/api', () => ({
    contentService: {
        getCreditsBalance: jest.fn()
    }
}));

describe('useCredits', () => {
    beforeEach(() => {
        jest.clearAllMocks();
    });

    it('should initialize with loading state and no credits', () => {
        const { result } = renderHook(() => useCredits());

        expect(result.current.credits).toBe(0);
        expect(result.current.isLoading).toBe(true);
        expect(result.current.error).toBeNull();
    });

    it('should fetch credits on mount', async () => {
        const mockCredits = 100;
        (contentService.getCreditsBalance as jest.Mock).mockResolvedValueOnce(mockCredits);

        let result: any;
        await act(async () => {
            const hook = renderHook(() => useCredits());
            result = hook.result;
            // Wait for all state updates to complete
            await new Promise(resolve => setTimeout(resolve, 0));
        });

        expect(result.current.credits).toBe(mockCredits);
        expect(result.current.isLoading).toBe(false);
        expect(result.current.error).toBeNull();
    });

    it('should handle error when fetching credits', async () => {
        const errorMessage = 'Failed to fetch credits';
        (contentService.getCreditsBalance as jest.Mock).mockRejectedValueOnce(new Error(errorMessage));

        let result: any;
        await act(async () => {
            const hook = renderHook(() => useCredits());
            result = hook.result;
            // Wait for all state updates to complete
            await new Promise(resolve => setTimeout(resolve, 0));
        });

        expect(result.current.credits).toBe(0);
        expect(result.current.isLoading).toBe(false);
        expect(result.current.error).toBe(errorMessage);
    });

    it('should update credits when refreshCredits is called', async () => {
        const initialCredits = 100;
        const updatedCredits = 90;
        
        (contentService.getCreditsBalance as jest.Mock)
            .mockResolvedValueOnce(initialCredits)
            .mockResolvedValueOnce(updatedCredits);

        let result: any;
        await act(async () => {
            const hook = renderHook(() => useCredits());
            result = hook.result;
            // Wait for initial fetch
            await new Promise(resolve => setTimeout(resolve, 0));
        });

        expect(result.current.credits).toBe(initialCredits);

        // Call refreshCredits
        await act(async () => {
            await result.current.refreshCredits();
            // Wait for all state updates to complete
            await new Promise(resolve => setTimeout(resolve, 0));
        });

        expect(result.current.credits).toBe(updatedCredits);
        expect(contentService.getCreditsBalance).toHaveBeenCalledTimes(2);
    });
});
