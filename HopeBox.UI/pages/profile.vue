<template>
    <div class="main-container">
        <Header />
        <OffCanvasMobileMenu />
        <UserInfo />
        <TestimonialOne />
        <Sponsor />
        <Footer />
    </div>
</template>

<script>
  export default {
    components: {
      Header: () => import('@/components/Header'),
      OffCanvasMobileMenu: () => import('@/components/OffCanvasMobileMenu'),
      UserInfo: () => import('@/components/UserInfo'),
      TestimonialOne: () => import('@/components/TestimonialOne'),
      Sponsor: () => import('@/components/Sponsor'),
      Footer: () => import('@/components/Footer'),
    }
  }
</script>