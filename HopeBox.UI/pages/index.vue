<template>
    <div>
    </div>
    <div class="home-default-wrapper">
        <Header />
        <OffCanvasMobileMenu />
        <HeroOne />
        <ServiceOne />
        <AboutOne />
        <CausesOne />
        <DonationStyleOne />
        <EventsOne />
        <TestimonialOne />
        <BlogStyleOne />
        <FunFactOne />
        <Sponsor />
        <Footer />
    </div>
</template>

<script>
    export default {
        components: {
            Header: () => import('@/components/Header'),
            OffCanvasMobileMenu: () => import('@/components/OffCanvasMobileMenu'),
            HeroOne: () => import('@/components/HeroOne'),
            ServiceOne: () => import('@/components/ServiceOne'),
            AboutOne: () => import('@/components/AboutOne'),
            CausesOne: () => import('@/components/CausesOne'),
            DonationStyleOne: () => import('@/components/DonationStyleOne'),
            FunFactOne: () => import('@/components/FunFactOne'),
            EventsOne: () => import('@/components/EventsOne'),
            TestimonialOne: () => import('@/components/TestimonialOne'),
            BlogStyleOne: () => import('@/components/BlogStyleOne'),
            Sponsor: () => import('@/components/Sponsor'),
            Footer: () => import('@/components/Footer'),
        },
    };
</script>