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
    compatibilityDate: '2025-06-13',
  },

  app: {
    head: {
        script: [
            {
                src: 'https://mirabot.tenten.vn/mirabot.js?f=LoFY8M7BQ6mwb24OqTM5vtIYkhZ5fGwfMh4tU9QkW35lQLh0WgUVWWTi3QmmVxZOo6Arg48npepoDTD7JygIjcgi9SiFRa6jC9WW&t=873',
                async: true,
                type: 'text/javascript'
            }
        ]
    }
  }
})
