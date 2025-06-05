
import type { DefineComponent, SlotsType } from 'vue'
type IslandComponent<T extends DefineComponent> = T & DefineComponent<{}, {refresh: () => Promise<void>}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, SlotsType<{ fallback: { error: unknown } }>>
type HydrationStrategies = {
  hydrateOnVisible?: IntersectionObserverInit | true
  hydrateOnIdle?: number | true
  hydrateOnInteraction?: keyof HTMLElementEventMap | Array<keyof HTMLElementEventMap> | true
  hydrateOnMediaQuery?: string
  hydrateAfter?: number
  hydrateWhen?: boolean
  hydrateNever?: true
}
type LazyComponent<T> = (T & DefineComponent<HydrationStrategies, {}, {}, {}, {}, {}, {}, { hydrated: () => void }>)
interface _GlobalComponents {
      'AboutOne': typeof import("../components/AboutOne.vue")['default']
    'AboutTwo': typeof import("../components/AboutTwo.vue")['default']
    'BlogDetailsContent': typeof import("../components/BlogDetailsContent.vue")['default']
    'BlogGridWrapper': typeof import("../components/BlogGridWrapper.vue")['default']
    'BlogStyleOne': typeof import("../components/BlogStyleOne.vue")['default']
    'Breadcrumb': typeof import("../components/Breadcrumb.vue")['default']
    'CausesDetailsContent': typeof import("../components/CausesDetailsContent.vue")['default']
    'CausesOne': typeof import("../components/CausesOne.vue")['default']
    'CausesWrapper': typeof import("../components/CausesWrapper.vue")['default']
    'ContactWrapper': typeof import("../components/ContactWrapper.vue")['default']
    'DonationForm': typeof import("../components/DonationForm.vue")['default']
    'DonationStyleOne': typeof import("../components/DonationStyleOne.vue")['default']
    'DonnerList': typeof import("../components/DonnerList.vue")['default']
    'EventDetailsContent': typeof import("../components/EventDetailsContent.vue")['default']
    'EventsOne': typeof import("../components/EventsOne.vue")['default']
    'Footer': typeof import("../components/Footer.vue")['default']
    'FunFactOne': typeof import("../components/FunFactOne.vue")['default']
    'Header': typeof import("../components/Header.vue")['default']
    'HeroOne': typeof import("../components/HeroOne.vue")['default']
    'LoginForm': typeof import("../components/LoginForm.vue")['default']
    'MobileNavigation': typeof import("../components/MobileNavigation.vue")['default']
    'Navigation': typeof import("../components/Navigation.vue")['default']
    'OffCanvasMobileMenu': typeof import("../components/OffCanvasMobileMenu.vue")['default']
    'PaymentResult': typeof import("../components/PaymentResult.vue")['default']
    'ServiceOne': typeof import("../components/ServiceOne.vue")['default']
    'ShapeWithAnimation': typeof import("../components/ShapeWithAnimation.vue")['default']
    'SidebarWrapper': typeof import("../components/SidebarWrapper.vue")['default']
    'Sponsor': typeof import("../components/Sponsor.vue")['default']
    'TeamMember': typeof import("../components/TeamMember.vue")['default']
    'TeamOne': typeof import("../components/TeamOne.vue")['default']
    'TeamTwo': typeof import("../components/TeamTwo.vue")['default']
    'TestimonialOne': typeof import("../components/TestimonialOne.vue")['default']
    'UserInfo': typeof import("../components/UserInfo.vue")['default']
    'VolunteerForm': typeof import("../components/VolunteerForm.vue")['default']
    'WidgetCategory': typeof import("../components/WidgetCategory.vue")['default']
    'WidgetSearch': typeof import("../components/WidgetSearch.vue")['default']
    'WidgetTag': typeof import("../components/WidgetTag.vue")['default']
    'WidgetUrgentCauses': typeof import("../components/WidgetUrgentCauses.vue")['default']
    'NuxtWelcome': typeof import("../node_modules/nuxt/dist/app/components/welcome.vue")['default']
    'NuxtLayout': typeof import("../node_modules/nuxt/dist/app/components/nuxt-layout")['default']
    'NuxtErrorBoundary': typeof import("../node_modules/nuxt/dist/app/components/nuxt-error-boundary.vue")['default']
    'ClientOnly': typeof import("../node_modules/nuxt/dist/app/components/client-only")['default']
    'DevOnly': typeof import("../node_modules/nuxt/dist/app/components/dev-only")['default']
    'ServerPlaceholder': typeof import("../node_modules/nuxt/dist/app/components/server-placeholder")['default']
    'NuxtLink': typeof import("../node_modules/nuxt/dist/app/components/nuxt-link")['default']
    'NuxtLoadingIndicator': typeof import("../node_modules/nuxt/dist/app/components/nuxt-loading-indicator")['default']
    'NuxtTime': typeof import("../node_modules/nuxt/dist/app/components/nuxt-time.vue")['default']
    'NuxtRouteAnnouncer': typeof import("../node_modules/nuxt/dist/app/components/nuxt-route-announcer")['default']
    'NuxtImg': typeof import("../node_modules/nuxt/dist/app/components/nuxt-stubs")['NuxtImg']
    'NuxtPicture': typeof import("../node_modules/nuxt/dist/app/components/nuxt-stubs")['NuxtPicture']
    'NuxtPage': typeof import("../node_modules/nuxt/dist/pages/runtime/page")['default']
    'NoScript': typeof import("../node_modules/nuxt/dist/head/runtime/components")['NoScript']
    'Link': typeof import("../node_modules/nuxt/dist/head/runtime/components")['Link']
    'Base': typeof import("../node_modules/nuxt/dist/head/runtime/components")['Base']
    'Title': typeof import("../node_modules/nuxt/dist/head/runtime/components")['Title']
    'Meta': typeof import("../node_modules/nuxt/dist/head/runtime/components")['Meta']
    'Style': typeof import("../node_modules/nuxt/dist/head/runtime/components")['Style']
    'Head': typeof import("../node_modules/nuxt/dist/head/runtime/components")['Head']
    'Html': typeof import("../node_modules/nuxt/dist/head/runtime/components")['Html']
    'Body': typeof import("../node_modules/nuxt/dist/head/runtime/components")['Body']
    'NuxtIsland': typeof import("../node_modules/nuxt/dist/app/components/nuxt-island")['default']
    'NuxtRouteAnnouncer': IslandComponent<typeof import("../node_modules/nuxt/dist/app/components/server-placeholder")['default']>
      'LazyAboutOne': LazyComponent<typeof import("../components/AboutOne.vue")['default']>
    'LazyAboutTwo': LazyComponent<typeof import("../components/AboutTwo.vue")['default']>
    'LazyBlogDetailsContent': LazyComponent<typeof import("../components/BlogDetailsContent.vue")['default']>
    'LazyBlogGridWrapper': LazyComponent<typeof import("../components/BlogGridWrapper.vue")['default']>
    'LazyBlogStyleOne': LazyComponent<typeof import("../components/BlogStyleOne.vue")['default']>
    'LazyBreadcrumb': LazyComponent<typeof import("../components/Breadcrumb.vue")['default']>
    'LazyCausesDetailsContent': LazyComponent<typeof import("../components/CausesDetailsContent.vue")['default']>
    'LazyCausesOne': LazyComponent<typeof import("../components/CausesOne.vue")['default']>
    'LazyCausesWrapper': LazyComponent<typeof import("../components/CausesWrapper.vue")['default']>
    'LazyContactWrapper': LazyComponent<typeof import("../components/ContactWrapper.vue")['default']>
    'LazyDonationForm': LazyComponent<typeof import("../components/DonationForm.vue")['default']>
    'LazyDonationStyleOne': LazyComponent<typeof import("../components/DonationStyleOne.vue")['default']>
    'LazyDonnerList': LazyComponent<typeof import("../components/DonnerList.vue")['default']>
    'LazyEventDetailsContent': LazyComponent<typeof import("../components/EventDetailsContent.vue")['default']>
    'LazyEventsOne': LazyComponent<typeof import("../components/EventsOne.vue")['default']>
    'LazyFooter': LazyComponent<typeof import("../components/Footer.vue")['default']>
    'LazyFunFactOne': LazyComponent<typeof import("../components/FunFactOne.vue")['default']>
    'LazyLoginForm': typeof import("../components/LoginForm.vue")['default']
    'LazyHeader': LazyComponent<typeof import("../components/Header.vue")['default']>
    'LazyHeroOne': LazyComponent<typeof import("../components/HeroOne.vue")['default']>
    'LazyMobileNavigation': LazyComponent<typeof import("../components/MobileNavigation.vue")['default']>
    'LazyNavigation': LazyComponent<typeof import("../components/Navigation.vue")['default']>
    'LazyPaymentResult': typeof import("../components/PaymentResult.vue")['default']
    'LazyOffCanvasMobileMenu': LazyComponent<typeof import("../components/OffCanvasMobileMenu.vue")['default']>
    'LazyServiceOne': LazyComponent<typeof import("../components/ServiceOne.vue")['default']>
    'LazyShapeWithAnimation': LazyComponent<typeof import("../components/ShapeWithAnimation.vue")['default']>
    'LazySidebarWrapper': LazyComponent<typeof import("../components/SidebarWrapper.vue")['default']>
    'LazySponsor': LazyComponent<typeof import("../components/Sponsor.vue")['default']>
    'LazyTeamMember': LazyComponent<typeof import("../components/TeamMember.vue")['default']>
    'LazyTeamOne': LazyComponent<typeof import("../components/TeamOne.vue")['default']>
    'LazyTeamTwo': LazyComponent<typeof import("../components/TeamTwo.vue")['default']>
    'LazyTestimonialOne': LazyComponent<typeof import("../components/TestimonialOne.vue")['default']>
    'LazyVolunteerForm': LazyComponent<typeof import("../components/VolunteerForm.vue")['default']>
    'LazyUserInfo': typeof import("../components/UserInfo.vue")['default']
    'LazyWidgetCategory': LazyComponent<typeof import("../components/WidgetCategory.vue")['default']>
    'LazyWidgetSearch': LazyComponent<typeof import("../components/WidgetSearch.vue")['default']>
    'LazyWidgetTag': LazyComponent<typeof import("../components/WidgetTag.vue")['default']>
    'LazyWidgetUrgentCauses': LazyComponent<typeof import("../components/WidgetUrgentCauses.vue")['default']>
    'LazyNuxtWelcome': LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/welcome.vue")['default']>
    'LazyNuxtLayout': LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/nuxt-layout")['default']>
    'LazyNuxtErrorBoundary': LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/nuxt-error-boundary.vue")['default']>
    'LazyClientOnly': LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/client-only")['default']>
    'LazyDevOnly': LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/dev-only")['default']>
    'LazyServerPlaceholder': LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/server-placeholder")['default']>
    'LazyNuxtLink': LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/nuxt-link")['default']>
    'LazyNuxtLoadingIndicator': LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/nuxt-loading-indicator")['default']>
    'LazyNuxtTime': LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/nuxt-time.vue")['default']>
    'LazyNuxtRouteAnnouncer': LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/nuxt-route-announcer")['default']>
    'LazyNuxtImg': LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/nuxt-stubs")['NuxtImg']>
    'LazyNuxtPicture': LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/nuxt-stubs")['NuxtPicture']>
    'LazyNuxtPage': LazyComponent<typeof import("../node_modules/nuxt/dist/pages/runtime/page")['default']>
    'LazyNoScript': LazyComponent<typeof import("../node_modules/nuxt/dist/head/runtime/components")['NoScript']>
    'LazyLink': LazyComponent<typeof import("../node_modules/nuxt/dist/head/runtime/components")['Link']>
    'LazyBase': LazyComponent<typeof import("../node_modules/nuxt/dist/head/runtime/components")['Base']>
    'LazyTitle': LazyComponent<typeof import("../node_modules/nuxt/dist/head/runtime/components")['Title']>
    'LazyMeta': LazyComponent<typeof import("../node_modules/nuxt/dist/head/runtime/components")['Meta']>
    'LazyStyle': LazyComponent<typeof import("../node_modules/nuxt/dist/head/runtime/components")['Style']>
    'LazyHead': LazyComponent<typeof import("../node_modules/nuxt/dist/head/runtime/components")['Head']>
    'LazyHtml': LazyComponent<typeof import("../node_modules/nuxt/dist/head/runtime/components")['Html']>
    'LazyBody': LazyComponent<typeof import("../node_modules/nuxt/dist/head/runtime/components")['Body']>
    'LazyNuxtIsland': LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/nuxt-island")['default']>
    'LazyNuxtRouteAnnouncer': LazyComponent<IslandComponent<typeof import("../node_modules/nuxt/dist/app/components/server-placeholder")['default']>>
}

declare module 'vue' {
  export interface GlobalComponents extends _GlobalComponents { }
}

export const AboutOne: typeof import("../components/AboutOne.vue")['default']
export const AboutTwo: typeof import("../components/AboutTwo.vue")['default']
export const BlogDetailsContent: typeof import("../components/BlogDetailsContent.vue")['default']
export const BlogGridWrapper: typeof import("../components/BlogGridWrapper.vue")['default']
export const BlogStyleOne: typeof import("../components/BlogStyleOne.vue")['default']
export const Breadcrumb: typeof import("../components/Breadcrumb.vue")['default']
export const CausesDetailsContent: typeof import("../components/CausesDetailsContent.vue")['default']
export const CausesOne: typeof import("../components/CausesOne.vue")['default']
export const CausesWrapper: typeof import("../components/CausesWrapper.vue")['default']
export const ContactWrapper: typeof import("../components/ContactWrapper.vue")['default']
export const DonationForm: typeof import("../components/DonationForm.vue")['default']
export const DonationStyleOne: typeof import("../components/DonationStyleOne.vue")['default']
export const DonnerList: typeof import("../components/DonnerList.vue")['default']
export const EventDetailsContent: typeof import("../components/EventDetailsContent.vue")['default']
export const EventsOne: typeof import("../components/EventsOne.vue")['default']
export const Footer: typeof import("../components/Footer.vue")['default']
export const FunFactOne: typeof import("../components/FunFactOne.vue")['default']
export const Header: typeof import("../components/Header.vue")['default']
export const HeroOne: typeof import("../components/HeroOne.vue")['default']
export const LoginForm: typeof import("../components/LoginForm.vue")['default']
export const MobileNavigation: typeof import("../components/MobileNavigation.vue")['default']
export const Navigation: typeof import("../components/Navigation.vue")['default']
export const OffCanvasMobileMenu: typeof import("../components/OffCanvasMobileMenu.vue")['default']
export const PaymentResult: typeof import("../components/PaymentResult.vue")['default']
export const ServiceOne: typeof import("../components/ServiceOne.vue")['default']
export const ShapeWithAnimation: typeof import("../components/ShapeWithAnimation.vue")['default']
export const SidebarWrapper: typeof import("../components/SidebarWrapper.vue")['default']
export const Sponsor: typeof import("../components/Sponsor.vue")['default']
export const TeamMember: typeof import("../components/TeamMember.vue")['default']
export const TeamOne: typeof import("../components/TeamOne.vue")['default']
export const TeamTwo: typeof import("../components/TeamTwo.vue")['default']
export const TestimonialOne: typeof import("../components/TestimonialOne.vue")['default']
export const UserInfo: typeof import("../components/UserInfo.vue")['default']
export const VolunteerForm: typeof import("../components/VolunteerForm.vue")['default']
export const WidgetCategory: typeof import("../components/WidgetCategory.vue")['default']
export const WidgetSearch: typeof import("../components/WidgetSearch.vue")['default']
export const WidgetTag: typeof import("../components/WidgetTag.vue")['default']
export const WidgetUrgentCauses: typeof import("../components/WidgetUrgentCauses.vue")['default']
export const NuxtWelcome: typeof import("../node_modules/nuxt/dist/app/components/welcome.vue")['default']
export const NuxtLayout: typeof import("../node_modules/nuxt/dist/app/components/nuxt-layout")['default']
export const NuxtErrorBoundary: typeof import("../node_modules/nuxt/dist/app/components/nuxt-error-boundary.vue")['default']
export const ClientOnly: typeof import("../node_modules/nuxt/dist/app/components/client-only")['default']
export const DevOnly: typeof import("../node_modules/nuxt/dist/app/components/dev-only")['default']
export const ServerPlaceholder: typeof import("../node_modules/nuxt/dist/app/components/server-placeholder")['default']
export const NuxtLink: typeof import("../node_modules/nuxt/dist/app/components/nuxt-link")['default']
export const NuxtLoadingIndicator: typeof import("../node_modules/nuxt/dist/app/components/nuxt-loading-indicator")['default']
export const NuxtTime: typeof import("../node_modules/nuxt/dist/app/components/nuxt-time.vue")['default']
export const NuxtRouteAnnouncer: typeof import("../node_modules/nuxt/dist/app/components/nuxt-route-announcer")['default']
export const NuxtImg: typeof import("../node_modules/nuxt/dist/app/components/nuxt-stubs")['NuxtImg']
export const NuxtPicture: typeof import("../node_modules/nuxt/dist/app/components/nuxt-stubs")['NuxtPicture']
export const NuxtPage: typeof import("../node_modules/nuxt/dist/pages/runtime/page")['default']
export const NoScript: typeof import("../node_modules/nuxt/dist/head/runtime/components")['NoScript']
export const Link: typeof import("../node_modules/nuxt/dist/head/runtime/components")['Link']
export const Base: typeof import("../node_modules/nuxt/dist/head/runtime/components")['Base']
export const Title: typeof import("../node_modules/nuxt/dist/head/runtime/components")['Title']
export const Meta: typeof import("../node_modules/nuxt/dist/head/runtime/components")['Meta']
export const Style: typeof import("../node_modules/nuxt/dist/head/runtime/components")['Style']
export const Head: typeof import("../node_modules/nuxt/dist/head/runtime/components")['Head']
export const Html: typeof import("../node_modules/nuxt/dist/head/runtime/components")['Html']
export const Body: typeof import("../node_modules/nuxt/dist/head/runtime/components")['Body']
export const NuxtIsland: typeof import("../node_modules/nuxt/dist/app/components/nuxt-island")['default']
export const NuxtRouteAnnouncer: IslandComponent<typeof import("../node_modules/nuxt/dist/app/components/server-placeholder")['default']>
export const LazyAboutOne: LazyComponent<typeof import("../components/AboutOne.vue")['default']>
export const LazyAboutTwo: LazyComponent<typeof import("../components/AboutTwo.vue")['default']>
export const LazyBlogDetailsContent: LazyComponent<typeof import("../components/BlogDetailsContent.vue")['default']>
export const LazyBlogGridWrapper: LazyComponent<typeof import("../components/BlogGridWrapper.vue")['default']>
export const LazyBlogStyleOne: LazyComponent<typeof import("../components/BlogStyleOne.vue")['default']>
export const LazyBreadcrumb: LazyComponent<typeof import("../components/Breadcrumb.vue")['default']>
export const LazyCausesDetailsContent: LazyComponent<typeof import("../components/CausesDetailsContent.vue")['default']>
export const LazyCausesOne: LazyComponent<typeof import("../components/CausesOne.vue")['default']>
export const LazyCausesWrapper: LazyComponent<typeof import("../components/CausesWrapper.vue")['default']>
export const LazyContactWrapper: LazyComponent<typeof import("../components/ContactWrapper.vue")['default']>
export const LazyDonationForm: LazyComponent<typeof import("../components/DonationForm.vue")['default']>
export const LazyDonationStyleOne: LazyComponent<typeof import("../components/DonationStyleOne.vue")['default']>
export const LazyDonnerList: LazyComponent<typeof import("../components/DonnerList.vue")['default']>
export const LazyEventDetailsContent: LazyComponent<typeof import("../components/EventDetailsContent.vue")['default']>
export const LazyEventsOne: LazyComponent<typeof import("../components/EventsOne.vue")['default']>
export const LazyFooter: LazyComponent<typeof import("../components/Footer.vue")['default']>
export const LazyFunFactOne: LazyComponent<typeof import("../components/FunFactOne.vue")['default']>
export const LazyLoginForm: typeof import("../components/LoginForm.vue")['default']
export const LazyHeader: LazyComponent<typeof import("../components/Header.vue")['default']>
export const LazyHeroOne: LazyComponent<typeof import("../components/HeroOne.vue")['default']>
export const LazyMobileNavigation: LazyComponent<typeof import("../components/MobileNavigation.vue")['default']>
export const LazyNavigation: LazyComponent<typeof import("../components/Navigation.vue")['default']>
export const LazyPaymentResult: typeof import("../components/PaymentResult.vue")['default']
export const LazyOffCanvasMobileMenu: LazyComponent<typeof import("../components/OffCanvasMobileMenu.vue")['default']>
export const LazyServiceOne: LazyComponent<typeof import("../components/ServiceOne.vue")['default']>
export const LazyShapeWithAnimation: LazyComponent<typeof import("../components/ShapeWithAnimation.vue")['default']>
export const LazySidebarWrapper: LazyComponent<typeof import("../components/SidebarWrapper.vue")['default']>
export const LazySponsor: LazyComponent<typeof import("../components/Sponsor.vue")['default']>
export const LazyTeamMember: LazyComponent<typeof import("../components/TeamMember.vue")['default']>
export const LazyTeamOne: LazyComponent<typeof import("../components/TeamOne.vue")['default']>
export const LazyTeamTwo: LazyComponent<typeof import("../components/TeamTwo.vue")['default']>
export const LazyTestimonialOne: LazyComponent<typeof import("../components/TestimonialOne.vue")['default']>
export const LazyUserInfo: typeof import("../components/UserInfo.vue")['default']
export const LazyVolunteerForm: LazyComponent<typeof import("../components/VolunteerForm.vue")['default']>
export const LazyWidgetCategory: LazyComponent<typeof import("../components/WidgetCategory.vue")['default']>
export const LazyWidgetSearch: LazyComponent<typeof import("../components/WidgetSearch.vue")['default']>
export const LazyWidgetTag: LazyComponent<typeof import("../components/WidgetTag.vue")['default']>
export const LazyWidgetUrgentCauses: LazyComponent<typeof import("../components/WidgetUrgentCauses.vue")['default']>
export const LazyNuxtWelcome: LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/welcome.vue")['default']>
export const LazyNuxtLayout: LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/nuxt-layout")['default']>
export const LazyNuxtErrorBoundary: LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/nuxt-error-boundary.vue")['default']>
export const LazyClientOnly: LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/client-only")['default']>
export const LazyDevOnly: LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/dev-only")['default']>
export const LazyServerPlaceholder: LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/server-placeholder")['default']>
export const LazyNuxtLink: LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/nuxt-link")['default']>
export const LazyNuxtLoadingIndicator: LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/nuxt-loading-indicator")['default']>
export const LazyNuxtTime: LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/nuxt-time.vue")['default']>
export const LazyNuxtRouteAnnouncer: LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/nuxt-route-announcer")['default']>
export const LazyNuxtImg: LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/nuxt-stubs")['NuxtImg']>
export const LazyNuxtPicture: LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/nuxt-stubs")['NuxtPicture']>
export const LazyNuxtPage: LazyComponent<typeof import("../node_modules/nuxt/dist/pages/runtime/page")['default']>
export const LazyNoScript: LazyComponent<typeof import("../node_modules/nuxt/dist/head/runtime/components")['NoScript']>
export const LazyLink: LazyComponent<typeof import("../node_modules/nuxt/dist/head/runtime/components")['Link']>
export const LazyBase: LazyComponent<typeof import("../node_modules/nuxt/dist/head/runtime/components")['Base']>
export const LazyTitle: LazyComponent<typeof import("../node_modules/nuxt/dist/head/runtime/components")['Title']>
export const LazyMeta: LazyComponent<typeof import("../node_modules/nuxt/dist/head/runtime/components")['Meta']>
export const LazyStyle: LazyComponent<typeof import("../node_modules/nuxt/dist/head/runtime/components")['Style']>
export const LazyHead: LazyComponent<typeof import("../node_modules/nuxt/dist/head/runtime/components")['Head']>
export const LazyHtml: LazyComponent<typeof import("../node_modules/nuxt/dist/head/runtime/components")['Html']>
export const LazyBody: LazyComponent<typeof import("../node_modules/nuxt/dist/head/runtime/components")['Body']>
export const LazyNuxtIsland: LazyComponent<typeof import("../node_modules/nuxt/dist/app/components/nuxt-island")['default']>
export const LazyNuxtRouteAnnouncer: LazyComponent<IslandComponent<typeof import("../node_modules/nuxt/dist/app/components/server-placeholder")['default']>>

export const componentNames: string[]
