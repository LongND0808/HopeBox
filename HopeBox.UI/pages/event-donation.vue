<template>
    <div class="contact-page-wrapper">
        <Header />
        <OffCanvasMobileMenu />
        <Breadcrumb items="Quyên Góp Sự Kiện" title="Quyên Góp Sự Kiện" />
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