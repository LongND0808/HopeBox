<!-- causesdetail.vue -->
<template>
  <div class="main-container">
    <Header />
    <OffCanvasMobileMenu />
    <Breadcrumb items="Thông tin chiến dịch" title="Thông tin chiến dịch" />
    <CausesDetailsContent :id="$route.query.id" />
    <Footer />
  </div>
</template>

<script>
export default {
  components: {
    Header: () => import('@/components/Header'),
    OffCanvasMobileMenu: () => import('@/components/OffCanvasMobileMenu'),
    Breadcrumb: () => import('@/components/Breadcrumb'),
    CausesDetailsContent: () => import('@/components/CausesDetailsContent'),
    Footer: () => import('@/components/Footer'),
  },
};
</script>
