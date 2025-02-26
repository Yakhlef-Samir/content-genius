import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import App from '../../App';

describe('App Component', () => {
  test('renders app component', () => {
    render(<App />);
    expect(screen).toBeDefined();
  });
});
