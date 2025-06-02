import type { NavigationGuard } from 'vue-router'
export type MiddlewareKey = string
declare module "D:/Project/HopeBox/HopeBox/HopeBox.UI/node_modules/nuxt/dist/pages/runtime/composables" {
  interface PageMeta {
    middleware?: MiddlewareKey | NavigationGuard | Array<MiddlewareKey | NavigationGuard>
  }
}