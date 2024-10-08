import { fileURLToPath, URL } from "node:url";

import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
import mockServer from "vite-plugin-mock-server";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue(), mockServer({ urlPrefixes: ["/api-mock/"] })],
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url))
    }
  },
  // server: {
  //   proxy: {
  //     '/api': 'http://localhost:62231'
  //   }
  // },
  build: {
    assetsInlineLimit: 0
  }
});
