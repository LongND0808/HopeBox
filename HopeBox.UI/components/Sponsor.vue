<template>
    <section class="brand-logo-area brand-logo-default-area">
        <div class="container">
            <div class="row" data-aos="fade-up" data-aos-duration="1000">
                <div class="col-lg-4">
                    <div class="section-title text-center text-lg-start">
                        <h2 class="title title-style mt-xl-30">
                            Nhà Tài Trợ. 
                            <img class="img-shape" src="/images/shape/3.png" alt="Image-HopeBox">
                        </h2>
                    </div>
                </div>
                <div class="col-lg-8">
                    <div class="brand-logo-content">
                        <div class="row row-cols-3 row-cols-sm-5 mtn-50">
                            <div class="col mt-50" v-for="(brand, index) in brands" :key="index">
                                <div class="brand-logo-item">
                                    <img :src="brand.logoSrc" :alt="brand.alt">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</template>

<script>
    export default {
        data() {
            return {
                brands: [
                    {
                        logoSrc: "/images/brand-logo/1.png",
                        alt: "Brand Name"
                    },
                    {
                        logoSrc: "/images/brand-logo/2.png",
                        alt: "Brand Name"
                    },
                    {
                        logoSrc: "/images/brand-logo/3.png",
                        alt: "Brand Name"
                    },
                    {
                        logoSrc: "/images/brand-logo/4.png",
                        alt: "Brand Name"
                    },
                    {
                        logoSrc: "/images/brand-logo/5.png",
                        alt: "Brand Name"
                    },
                    {
                        logoSrc: "/images/brand-logo/6.png",
                        alt: "Brand Name"
                    },
                    {
                        logoSrc: "/images/brand-logo/7.png",
                        alt: "Brand Name"
                    },
                    {
                        logoSrc: "/images/brand-logo/8.png",
                        alt: "Brand Name"
                    },
                    {
                        logoSrc: "/images/brand-logo/9.png",
                        alt: "Brand Name"
                    },
                    {
                        logoSrc: "/images/brand-logo/10.png",
                        alt: "Brand Name"
                    },
                ]
            }
        },
    };
</script>