import { ComputedRef, Ref } from 'vue'
export type LayoutKey = "default"
declare module "E:/EXE/HopeBox/HopeBox.UI/node_modules/nuxt/dist/pages/runtime/composables" {
  interface PageMeta {
    layout?: false | LayoutKey | Ref<LayoutKey> | ComputedRef<LayoutKey>
  }
}