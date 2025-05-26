<template>
    <div class="main-container">
        <Header />
        <OffCanvasMobileMenu />
        <Breadcrumb items="Event details" title="Event details" />
        <EventDetailsContent />
        <Footer />
    </div>
</template>

<script>
    export default {
        components: {
            Header: () => import('@/components/Header'),
            OffCanvasMobileMenu: () => import('@/components/OffCanvasMobileMenu'),
            Breadcrumb: () => import('@/components/Breadcrumb'),
            EventDetailsContent: () => import('@/components/EventDetailsContent'),
            Footer: () => import('@/components/Footer'),
        }
    };
</script>


