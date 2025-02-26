import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { ContentGenerator } from '../../components/ContentGenerator/ContentGenerator';
import { contentService } from '../../services/api';
import { useCredits } from '../../hooks/useCredits';
import '@testing-library/jest-dom';

// Mock the content service
jest.mock('../../services/api', () => ({
  contentService: {
    generateContent: jest.fn()
  }
}));

// Mock the useCredits hook
jest.mock('../../hooks/useCredits', () => ({
  useCredits: jest.fn()
}));

describe('ContentGenerator', () => {
  beforeEach(() => {
    // Clear all mocks before each test
    jest.clearAllMocks();
    
    // Setup default mock implementation for useCredits
    (useCredits as jest.Mock).mockReturnValue({
      credits: 100,
      refreshCredits: jest.fn(),
      isLoading: false,
      error: null
    });
  });

  it('renders the component with initial state', () => {
    render(<ContentGenerator />);
    
    expect(screen.getByPlaceholderText(/enter your prompt/i)).toBeInTheDocument();
    expect(screen.getByRole('button', { name: /generate/i })).toBeInTheDocument();
    expect(screen.getByRole('combobox')).toBeInTheDocument();
    expect(screen.getByText(/credits remaining: 100/i)).toBeInTheDocument();
  });

  it('handles content generation', async () => {
    const mockGenerateContent = contentService.generateContent as jest.Mock;
    const mockRefreshCredits = jest.fn();
    
    (useCredits as jest.Mock).mockReturnValue({
      credits: 100,
      refreshCredits: mockRefreshCredits,
      isLoading: false,
      error: null
    });
    
    mockGenerateContent.mockResolvedValueOnce({ 
      content: 'Generated content',
      credits: 90
    });

    render(<ContentGenerator />);
    
    const promptInput = screen.getByPlaceholderText(/enter your prompt/i);
    const generateButton = screen.getByRole('button', { name: /generate/i });
    
    fireEvent.change(promptInput, { target: { value: 'Test prompt' } });
    fireEvent.click(generateButton);

    await waitFor(() => {
      expect(mockGenerateContent).toHaveBeenCalledWith('Test prompt', expect.any(String));
      expect(screen.getByText('Generated content')).toBeInTheDocument();
      expect(mockRefreshCredits).toHaveBeenCalled();
    });
  });

  it('displays error message on API failure', async () => {
    const mockGenerateContent = contentService.generateContent as jest.Mock;
    mockGenerateContent.mockRejectedValueOnce(new Error('API Error'));

    render(<ContentGenerator />);
    
    const promptInput = screen.getByPlaceholderText(/enter your prompt/i);
    const generateButton = screen.getByRole('button', { name: /generate/i });
    
    fireEvent.change(promptInput, { target: { value: 'Test prompt' } });
    fireEvent.click(generateButton);

    await waitFor(() => {
      expect(screen.getByText(/failed to generate content/i)).toBeInTheDocument();
    });
  });

  it('disables generate button when no credits', () => {
    (useCredits as jest.Mock).mockReturnValue({
      credits: 0,
      refreshCredits: jest.fn(),
      isLoading: false,
      error: null
    });

    render(<ContentGenerator />);
    
    const generateButton = screen.getByRole('button', { name: /generate/i });
    expect(generateButton).toBeDisabled();
  });
});
