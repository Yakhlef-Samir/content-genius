import React, { useState, useCallback } from 'react';
import { contentService } from '../../services/api';
import { useCredits } from '../../hooks/useCredits';
import './ContentGenerator.css';

interface ContentGeneratorProps {
    onGenerate?: (content: string) => void;
}

export const ContentGenerator: React.FC<ContentGeneratorProps> = ({ onGenerate }) => {
    const [prompt, setPrompt] = useState('');
    const [contentType, setContentType] = useState('article');
    const [generatedContent, setGeneratedContent] = useState('');
    const [isGenerating, setIsGenerating] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const { credits, refreshCredits } = useCredits();

    const handleGenerate = useCallback(async () => {
        if (!prompt.trim()) {
            setError('Please enter a prompt');
            return;
        }

        setIsGenerating(true);
        setError(null);

        try {
            const response = await contentService.generateContent(prompt, contentType);
            setGeneratedContent(response.content);
            if (onGenerate) {
                onGenerate(response.content);
            }
            refreshCredits();
        } catch (error) {
            setError('Failed to generate content');
        } finally {
            setIsGenerating(false);
        }
    }, [prompt, contentType, onGenerate, refreshCredits]);

    return (
        <div className="content-generator" data-testid="content-generator">
            <div className="credits-display">
                Credits remaining: {credits}
            </div>
            <div className="input-section">
                <textarea
                    value={prompt}
                    onChange={(e) => setPrompt(e.target.value)}
                    placeholder="Enter your prompt"
                    disabled={isGenerating}
                />
                <select
                    value={contentType}
                    onChange={(e) => setContentType(e.target.value)}
                    disabled={isGenerating}
                >
                    <option value="article">Article</option>
                    <option value="blog">Blog Post</option>
                    <option value="social">Social Media</option>
                </select>
                <button
                    onClick={handleGenerate}
                    disabled={isGenerating || !credits}
                >
                    {isGenerating ? 'Generating...' : 'Generate'}
                </button>
            </div>
            {error && <div className="error">{error}</div>}
            {generatedContent && (
                <div className="output-section">
                    <h3>Generated Content:</h3>
                    <div className="generated-content">{generatedContent}</div>
                </div>
            )}
        </div>
    );
};
