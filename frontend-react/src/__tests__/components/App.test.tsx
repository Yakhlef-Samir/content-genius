import { render, screen } from '@testing-library/react';
import App from '../../App';
import '@testing-library/jest-dom';

jest.mock('../../services/api', () => ({
  authService: {
    isAuthenticated: jest.fn().mockReturnValue(true)
  }
}));

describe('App', () => {
  it('renders the app with ContentGenerator when authenticated', () => {
    render(<App />);
    expect(screen.getByTestId('content-generator')).toBeInTheDocument();
  });
});
