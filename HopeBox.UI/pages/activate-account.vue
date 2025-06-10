<template>
  <section>
    <div class="main-container">
      <Header />
      <OffCanvasMobileMenu />
      <ActivateAccountForm />
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
      ActivateAccountForm: () => import('@/components/ActivateAccountForm'),
      TestimonialOne: () => import('@/components/TestimonialOne'),
      Sponsor: () => import('@/components/Sponsor'),
      Footer: () => import('@/components/Footer'),
    }
  }
</script>