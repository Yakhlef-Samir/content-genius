import { useState } from 'react';
import { ContentGenerator } from './components/ContentGenerator/ContentGenerator';
import { authService } from './services/api';
import './App.css';

function App() {
    const [generatedContent, setGeneratedContent] = useState<string>('');
    const [isAuthenticated, setIsAuthenticated] = useState(authService.isAuthenticated());

    const handleContentGeneration = (content: string) => {
        setGeneratedContent(content);
    };

    const handleLogout = () => {
        authService.logout();
        setIsAuthenticated(false);
    };

    if (!isAuthenticated) {
        return (
            <div className="login-container">
                <h1>Content Genius</h1>
                <p>Please log in to continue</p>
                {/* Add Login component here */}
            </div>
        );
    }

    return (
        <div className="app">
            <header className="app-header">
                <h1>Content Genius</h1>
                <button onClick={handleLogout} className="logout-button">
                    Logout
                </button>
            </header>

            <main className="app-main">
                <ContentGenerator onGenerate={handleContentGeneration} />

                {generatedContent && (
                    <div className="generated-content">
                        <h2>Generated Content</h2>
                        <div className="content-display">
                            {generatedContent}
                        </div>
                    </div>
                )}
            </main>
        </div>
    );
}

export default App;
