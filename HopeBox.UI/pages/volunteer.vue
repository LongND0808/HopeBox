<template>
    <div class="volunteer-page-wrapper">
        <Header />
        <OffCanvasMobileMenu />
        <Breadcrumb items="Become volunteer" title="Become volunteer" />
        <VolunteerForm />
        <TeamTwo />
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
            Breadcrumb: () => import('@/components/Breadcrumb'),
            VolunteerForm: () => import('@/components/VolunteerForm'),
            TeamTwo: () => import('@/components/TeamTwo'),
            FunFactOne: () => import('@/components/FunFactOne'),
            Sponsor: () => import('@/components/Sponsor'),
            Footer: () => import('@/components/Footer'),
        }
    };
</script>


