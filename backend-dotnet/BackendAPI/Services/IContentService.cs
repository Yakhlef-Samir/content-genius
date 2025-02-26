using BackendAPI.Models;

namespace BackendAPI.Services
{
    public interface IContentService
    {
        /// <summary>
        /// Génère du contenu basé sur un prompt en utilisant l'IA
        /// </summary>
        /// <param name="request">La requête de génération de contenu</param>
        /// <returns>La réponse contenant le contenu généré</returns>
        Task<ContentGenerationResponse> GenerateContentAsync(ContentGenerationRequest request);

        /// <summary>
        /// Vérifie si l'utilisateur a encore des crédits de génération disponibles
        /// </summary>
        /// <param name="userId">L'ID de l'utilisateur</param>
        /// <returns>True si l'utilisateur peut générer du contenu, False sinon</returns>
        Task<bool> CanGenerateContent(string userId);

        /// <summary>
        /// Enregistre le contenu généré dans l'historique de l'utilisateur
        /// </summary>
        /// <param name="userId">L'ID de l'utilisateur</param>
        /// <param name="content">Le contenu généré</param>
        /// <returns>True si l'enregistrement a réussi, False sinon</returns>
        Task<bool> SaveGeneratedContent(string userId, ContentGenerationResponse content);
    }
}
