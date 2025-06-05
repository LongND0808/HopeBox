<template>
  <section>
    <div class="main-container">
      <Header />
      <OffCanvasMobileMenu />
      <LoginForm />
      <TestimonialOne />
      <Sponsor />
      <Footer />
    </div>
  </section>
</template>

<script>
  export default {
    components: {
      Header: () => import('@/components/Header'),
      OffCanvasMobileMenu: () => import('@/components/OffCanvasMobileMenu'),
      LoginForm: () => import('@/components/LoginForm'),
      TestimonialOne: () => import('@/components/TestimonialOne'),
      Sponsor: () => import('@/components/Sponsor'),
      Footer: () => import('@/components/Footer'),
    }
  }
</script>