<template>
  <div class="blog-page-wrapper">
    <Header />
    <OffCanvasMobileMenu />
    <Breadcrumb items="Blog Details" title="Blog Details" />
    <BlogDetailsContent :id="$route.query.id" />
    <div id="gap" style="margin-bottom: 240px;"></div>
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
    TestimonialOne: () => import('@/components/TestimonialOne'),
    Footer: () => import('@/components/Footer'),
  },
  
};
</script>
