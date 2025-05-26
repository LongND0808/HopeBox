<template>
    <section class="hero-area slider-default">
        <div class="hero-content">
            <div class="container">
                <div class="row">
                    <div class="col-md-6 col-lg-6 col-xl-7">
                        <div class="content">
                            <div class="subtitle-content">
                                <img src="/images/icons/1.png" alt="image">
                                <h6>Thay đổi tương lai.</h6>
                            </div>
                            <div class="tittle-wrp">
                                <h2>Hãy Chung Tay Để <span>Thay Đổi</span> <br> Số Phận.</h2>
                            </div>
                            <div class="btn-wrp">
                                <nuxt-link to="/causes" class="btn-theme btn-gradient btn-slide btn-style">
                                    Cảnh đời cơ cực
                                    <img class="icon icon-img" src="/images/icons/arrow-line-right2.png" alt="Icon">
                                </nuxt-link>
                                <nuxt-link to="/contact" class="btn-theme btn-border btn-black btn-style">
                                    Quyên góp 
                                    <img class="icon icon-img" src="/images/icons/arrow-right-line-dark.png" alt="Icon">
                                </nuxt-link>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-5 offset-md-1 col-lg-5 offset-lg-1 col-xl-5 offset-xl-0">
                        <div class="layer-style">
                            <div class="thumb scene">
                                <span class="scene-layer" data-depth="0.20">                     
                                    <img class="" src="/images/slider/1.jpg" alt="Image">
                                </span>
                                <div class="shape-circle scene">
                                    <span class="scene-layer" data-depth="0.10">
                                        <img src="/images/shape/circle1.png" alt="Image">
                                    </span>
                                    <span class="scene-layer" data-depth="0.40">                        
                                        <img class="circle-img" src="/images/shape/2.png" alt="Image">
                                    </span>
                                </div>
                            </div>
                            <div class="shape-style1 scene">
                                <span class="scene-layer" data-depth="0.30">
                                    <img src="/images/shape/1.png" alt="Image">
                                </span>
                            </div> 
                            <ShapeWithAnimation addClassName="shape-style1" dataDepth="0.3" imgSrc="/images/shape/1.png" />
                            <div class="donate-circle-wrp">
                                <div class="donate-content">
                                    <div class="raised-amount">200tr₫</div>
                                    <img class="line-shape-img" src="/images/shape/line-s2.png" alt="Image">
                                    <h5 class="donate-title">Tổng quyên góp</h5>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="slider-shape">
                <div class="shape-style1">
                    <img class="shape-img1" src="/images/shape/map1.png" alt="Image">
                </div>
                <div class="shape-style2">
                    <img class="shape-img2" src="/images/shape/map2.png" alt="Image">
                </div>
                <div class="shape-style3">
                    <img class="shape-img3" src="/images/shape/banner-line1.png" alt="Image">
                </div>
                <div class="shape-style4">
                    <img class="shape-img3" src="/images/shape/banner-line2.png" alt="Image">
                </div>
            </div>
        </div>
    </section>
</template>

<script>
    export default {
        components: {
            ShapeWithAnimation: () => import('@/components/ShapeWithAnimation'),
        },
    };
</script>
