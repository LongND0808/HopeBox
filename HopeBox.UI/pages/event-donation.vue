<template>
    <div class="event-donation-page-wrapper">
        <Header />
        <OffCanvasMobileMenu />
        <Breadcrumb items="Sự kiện" title="Sự kiện" />
        <EventDonationWrapper />
        <Footer />
    </div>
</template>

<script>
    export default {
        components: {
            Header: () => import('@/components/Header'),
            OffCanvasMobileMenu: () => import('@/components/OffCanvasMobileMenu'),
            Breadcrumb: () => import('@/components/Breadcrumb'),
            EventDonationWrapper: () => import('@/components/EventDonationWrapper'),
            Footer: () => import('@/components/Footer'),
        }
    };
</script>


