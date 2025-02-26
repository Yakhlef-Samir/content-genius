/// <reference types="vite/client" />

declare global {
  interface ImportMetaEnv {
    readonly VITE_API_URL: string;
    // plus de variables d'environnement si nécessaire
  }

  interface ImportMeta {
    readonly env: ImportMetaEnv;
  }
}

export {};
