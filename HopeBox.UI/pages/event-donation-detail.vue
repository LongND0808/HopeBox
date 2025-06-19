<template>
    <div class="event-donation-detail-page-wrapper">
        <Header />
        <OffCanvasMobileMenu />
        <Breadcrumb items="Quyên góp Sự kiện Chi Tiết" title="Quyên góp Sự kiện Chi Tiết" />

        <div>
            <EventDonationDetailContent :eventId="$route.query.eventId" />
        </div>

        <Footer />
    </div>
</template>

<script>
export default {
    components: {
        Header: () => import('@/components/Header'),
        OffCanvasMobileMenu: () => import('@/components/OffCanvasMobileMenu'),
        Breadcrumb: () => import('@/components/Breadcrumb'),
        EventDonationDetailContent: () => import('@/components/EventDonationDetailContent'),
        Footer: () => import('@/components/Footer'),
    }
};
</script>