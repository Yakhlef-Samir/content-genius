import axios from 'axios';
import { contentService, authService } from '../../services/api';

jest.mock('axios');

const mockAxios = axios as jest.Mocked<typeof axios>;

describe('API Services', () => {
  beforeEach(() => {
    jest.clearAllMocks();
    localStorage.clear();
  });

  describe('contentService', () => {
    it('should generate content successfully', async () => {
      const mockResponse = { 
        data: { 
          content: 'Generated content',
          credits: 90
        } 
      };
      mockAxios.post.mockResolvedValueOnce(mockResponse);

      const result = await contentService.generateContent('Test prompt', 'article');

      expect(result).toEqual(mockResponse.data);
      expect(mockAxios.post).toHaveBeenCalledWith(
        expect.stringContaining('/content/generate'),
        { prompt: 'Test prompt', type: 'article' }
      );
    });

    it('should get credits balance successfully', async () => {
      const mockResponse = { data: 100 };
      mockAxios.get.mockResolvedValueOnce(mockResponse);

      const result = await contentService.getCreditsBalance();

      expect(result).toBe(100);
      expect(mockAxios.get).toHaveBeenCalledWith(
        expect.stringContaining('/content/credits')
      );
    });
  });

  describe('authService', () => {
    it('should login successfully', async () => {
      const mockResponse = { data: { token: 'test-token' } };
      mockAxios.post.mockResolvedValueOnce(mockResponse);

      const email = 'test@example.com';
      const password = 'password';
      await authService.login(email, password);

      expect(localStorage.getItem('token')).toBe('test-token');
      expect(mockAxios.post).toHaveBeenCalledWith(
        expect.stringContaining('/auth/login'),
        { email, password }
      );
    });

    it('should handle logout', () => {
      localStorage.setItem('token', 'test-token');
      authService.logout();
      expect(localStorage.getItem('token')).toBeNull();
    });

    it('should check authentication status', () => {
      localStorage.setItem('token', 'test-token');
      expect(authService.isAuthenticated()).toBe(true);

      localStorage.removeItem('token');
      expect(authService.isAuthenticated()).toBe(false);
    });
  });
});
