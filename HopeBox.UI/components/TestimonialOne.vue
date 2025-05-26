<template>
    <section class="testimonial-area testimonial-default-area bgcolor-theme3 position-relative">
        <div class="container">
            <div class="row">
                <div class="col-md-12 col-lg-4 col-xl-5 position-static">
                    <div class="title-wrp" :style="{ backgroundImage: `url('/images/testimonial/bg-testi1.jpg')` }">
                        <div class="section-title">
                            <h5 class="subtitle line-white-color text-white mb-7">Phản hồi</h5>
                            <h2 class="title title-style text-white">
                                Mọi người nói gì về chúng tôi
                                <img class="img-shape" src="/images/shape/3.png" alt="Image-HopeBox">
                            </h2>
                        </div>
                    </div>
                </div>
                <div class="col-md-12 col-lg-7 col-xl-6 position-static">
                    <div class="testimonial-content">
                        <div class="testimonial-slider-content">
                            <div class="testimonial-slider-container">
                                <swiper
                                    :speed="1000"
                                    :loop="true"
                                    :navigation="swiperOptions.navigation"
                                >
                                    <swiper-slide class="testimonial-single" v-for="(testimonial, index) in testimonials" :key="index">
                                        <div class="client-thumb-wrp">
                                            <div class="client-thumb">
                                                <img :src="testimonial.imgSrc" :alt="testimonial.name">
                                            </div>
                                            <div class="quote-icon">“</div>
                                        </div>
                                        <div class="client-content">
                                            <p>{{ testimonial.desc }}</p>
                                        </div>
                                        <div class="client-info">
                                            <img class="shape-line-img" src="/images/shape/line-t1.png" alt="Image-HopeBox">
                                            <h4 class="name">{{ testimonial.name }}</h4>
                                            <h6 class="designation">{{ testimonial.designation }}</h6>
                                        </div>
                                    </swiper-slide>
                                </swiper>
                                <!-- Add Arrows -->
                                <div class="navigation-wrp">
                                    <div class="swiper-button-prev">
                                        <img class="icon-img" src="/images/icons/test-arrow-left.png" alt="Image-Icon">
                                    </div>
                                    <div class="swiper-button-next">
                                        <img class="icon-img" src="/images/icons/test-arrow-right.png" alt="Image-Icon">
                                    </div>
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
import { Swiper, SwiperSlide } from "swiper/vue"
import SwiperCore, { Navigation } from 'swiper/core'
SwiperCore.use([Navigation])

import "swiper/css"

export default {
    components: {
        Swiper,
        SwiperSlide
    },
    data() {
        return {
            swiperOptions: {
                navigation: {
                    nextEl: '.swiper-button-next',
                    prevEl: '.swiper-button-prev',
                },
            },

            testimonials: [
                {
                    imgSrc: "/images/testimonial/1.png",
                    desc: "Nhờ các anh chị tình nguyện viên, lớp học của chúng em giờ đã có bàn ghế mới, không còn phải ngồi đất như trước. Em rất vui và biết ơn ạ!",
                    name: "Lầu A Páo",
                    designation: "Học sinh lớp 3, Hà Giang"
                },
                {
                    imgSrc: "/images/testimonial/2.png",
                    desc: "Tôi thật sự cảm động khi thấy các bạn trẻ không quản ngại khó khăn mang áo ấm và lương thực lên bản cho tụi nhỏ. Xin cảm ơn vì đã luôn đồng hành cùng người dân vùng cao.",
                    name: "Giàng Thị Dợ",
                    designation: "Phụ huynh, Lào Cai"
                }
            ]
        }
    },
};
</script>
