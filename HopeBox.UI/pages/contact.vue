<template>
    <div class="contact-page-wrapper">
        <Header />
        <OffCanvasMobileMenu />
        <Breadcrumb items="Contact" title="Contact" />
        <ContactWrapper />
        <Footer />
    </div>
</template>

<script>
    export default {
        components: {
            Header: () => import('@/components/Header'),
            OffCanvasMobileMenu: () => import('@/components/OffCanvasMobileMenu'),
            Breadcrumb: () => import('@/components/Breadcrumb'),
            ContactWrapper: () => import('@/components/ContactWrapper'),
            Footer: () => import('@/components/Footer'),
        }
    };
</script>


