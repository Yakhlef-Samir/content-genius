using BackendAPI.Models;

namespace BackendAPI.Services
{
    public interface IOpenAIService
    {
        /// <summary>
        /// Génère du contenu en utilisant l'API OpenAI
        /// </summary>
        /// <param name="prompt">Le prompt pour la génération</param>
        /// <param name="maxTokens">Le nombre maximum de tokens à générer</param>
        /// <returns>La réponse contenant le contenu généré</returns>
        Task<ContentGenerationResponse> GenerateAsync(string prompt, int maxTokens);
    }
}
