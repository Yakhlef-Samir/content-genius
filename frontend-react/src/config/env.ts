interface EnvConfig {
  API_URL: string;
}

export const getConfig = (): EnvConfig => {
  // En environnement de test, utiliser une URL par défaut
  if (process.env.NODE_ENV === 'test') {
    return {
      API_URL: 'http://localhost:5000/api'
    };
  }

  // En environnement de développement/production, utiliser Vite
  return {
    API_URL: import.meta?.env?.VITE_API_URL || 'http://localhost:5000/api'
  };
};
