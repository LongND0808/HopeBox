// nuxt.config.ts
export default defineNuxtConfig({
  css: [
    'bootstrap/dist/css/bootstrap.min.css',
    '@/assets/scss/style.scss',
    '@fortawesome/fontawesome-free/css/all.min.css'
  ],

  vite: {
    css: {
      preprocessorOptions: {
        scss: {
          additionalData: '@import "./assets/scss/_variables.scss";'
        },
      },
    },
  },

  plugins: ['@/plugins/aos'],

  ssr: true,

  nitro: {
    preset: 'cloudflare',
    logLevel: 'silent',
  },
})
