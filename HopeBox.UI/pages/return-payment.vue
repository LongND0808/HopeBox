<template>
    <div class="donation-page-wrapper">
        <Header />
        <OffCanvasMobileMenu />
        <Breadcrumb items="Kết quả thanh toán" title="Thanh toán VNPay" />

        <div class="donation-wrapper">
            <div class="container">
                <PaymentResult
                    :isLoading="isLoading"
                    :paymentSuccess="paymentSuccess"
                    :paymentMessage="paymentMessage"
                />
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
        Footer: () => import('@/components/Footer'),
        PaymentResult: () => import('@/components/PaymentResult'),
    },
    data() {
        return {
            isLoading: true,
            paymentSuccess: false,
            paymentMessage: '',
        };
    },
};
</script>
