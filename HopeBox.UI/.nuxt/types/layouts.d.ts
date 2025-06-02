import { ComputedRef, Ref } from 'vue'
export type LayoutKey = "default"
declare module "D:/Project/HopeBox/HopeBox/HopeBox.UI/node_modules/nuxt/dist/pages/runtime/composables" {
  interface PageMeta {
    layout?: false | LayoutKey | Ref<LayoutKey> | ComputedRef<LayoutKey>
  }
}