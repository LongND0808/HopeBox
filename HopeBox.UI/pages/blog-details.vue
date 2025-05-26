<template>
    <div class="blog-page-wrapper">
        <Header />
        <OffCanvasMobileMenu />
        <Breadcrumb items="Blog details" title="Blog details" />
        <BlogDetailsContent />
        <Footer />
    </div>
</template>

<script>
    export default {
        components: {
            Header: () => import('@/components/Header'),
            OffCanvasMobileMenu: () => import('@/components/OffCanvasMobileMenu'),
            Breadcrumb: () => import('@/components/Breadcrumb'),
            BlogDetailsContent: () => import('@/components/BlogDetailsContent'),
            Footer: () => import('@/components/Footer'),
        }
    };
</script>


