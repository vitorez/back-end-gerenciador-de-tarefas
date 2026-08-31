import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// porta 4200 para bater com a policy de CORS "AllowAngular" do backend
export default defineConfig({
  plugins: [react()],
  server: { port: 4200, strictPort: true }
})
