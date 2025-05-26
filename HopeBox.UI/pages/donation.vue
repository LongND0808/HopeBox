<template>
    <div class="donation-page-wrapper">
        <Header />
        <OffCanvasMobileMenu />
        <Breadcrumb items="Become donor" title="Become donor" />

        <div class="donation-wrapper">
            <div class="container">
                <div class="row">
                    <div class="col-lg-10 m-auto">
                        <DonationForm />
                    </div>
                </div>
            </div>
        </div>

        <Footer />
    </div>
</template>

<script>
    export default {
        components: {
            Header: () => import('@/components/Header'),
            OffCanvasMobileMenu: () => import('@/components/OffCanvasMobileMenu'),
            Breadcrumb: () => import('@/components/Breadcrumb'),
            DonationForm: () => import('@/components/DonationForm'),
            Footer: () => import('@/components/Footer'),
        }
    };
</script>


