<template>
    <div class="blog-page-wrapper">
        <Header />
        <OffCanvasMobileMenu />
        <Breadcrumb items="Blog" title="All blog posts" />
        <BlogGridWrapper />
        <Footer />
    </div>
</template>

<script>
    export default {
        components: {
            Header: () => import('@/components/Header'),
            OffCanvasMobileMenu: () => import('@/components/OffCanvasMobileMenu'),
            Breadcrumb: () => import('@/components/Breadcrumb'),
            BlogGridWrapper: () => import('@/components/BlogGridWrapper'),
            Footer: () => import('@/components/Footer'),
        }
    };
</script>


