<template>
    <section class="service-area service-default-area">
        <div class="container">
            <div class="row mtn-30" data-aos="fade-up" data-aos-duration="1000">
                <div class="col-md-6 col-lg-4 mt-30" v-for="(service, index) in services" :key="index">
                    <div class="icon-box-item" :class="service.addClassName">
                        <div class="icon-box-top">
                            <div class="icon-box">
                                <img class="icon-img" :src="service.icon" alt="icon" />
                            </div>
                            <h4 class="title">{{ service.title }}</h4>
                        </div>
                        <div class="content">
                            <div class="separator-line">
                                <img src="/images/shape/line-s1.png" alt="shape" />
                            </div>
                            <p>{{ service.desc }}</p>
                            <nuxt-link :to="{ path: '/causes', query: { type: service.type } }"
                                class="btn-theme btn-white btn-border btn-size-md">
                                Chi Tiết
                                <img class="icon icon-img" src="/images/icons/arrow-line-right.png" alt="Icon" />
                            </nuxt-link>
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
                services: [
                    {
                        icon: "/images/icons/s1.png",
                        title: "Nước sạch",
                        desc: "Hỗ trợ xây dựng hệ thống lọc và cung cấp nước sạch cho trẻ em vùng cao – nơi mà nguồn nước sinh hoạt còn thiếu thốn và không đảm bảo vệ sinh.",
                        addClassName: "item-one",
                        type: 1
                    },
                    {
                        icon: "/images/icons/s2.png",
                        title: "Thực phẩm",
                        desc: "Đóng góp lương thực và thực phẩm thiết yếu nhằm cải thiện bữa ăn hằng ngày cho trẻ em nghèo thiếu dinh dưỡng tại các bản làng miền núi.",
                        addClassName: "item-two",
                        type: 2
                    },
                    {
                        icon: "/images/icons/s3.png",
                        title: "Sức khỏe",
                        desc: "Tổ chức khám bệnh miễn phí, cung cấp thuốc men và dụng cụ y tế cơ bản để nâng cao sức khỏe cho trẻ em vùng sâu vùng xa.",
                        addClassName: "item-three",
                        type: 3
                    },
                    {
                        icon: "/images/icons/s4.png",
                        title: "Giáo dục",
                        desc: "Hỗ trợ xây dựng trường lớp, cung cấp sách vở và học bổng giúp trẻ em vùng cao có cơ hội tiếp cận tri thức.",
                        addClassName: "item-four",
                        type: 4
                    },
                    {
                        icon: "/images/icons/s5.png",
                        title: "Nơi Trú",
                        desc: "Cải thiện điều kiện nhà ở, xây dựng nhà kiên cố để trẻ em vùng cao có nơi ở an toàn và ấm áp.",
                        addClassName: "item-five",
                        type: 5
                    },
                    {
                        icon: "/images/icons/s6.png",
                        title: "Quần áo",
                        desc: "Cung cấp quần áo ấm và đồ dùng thiết yếu giúp trẻ em vượt qua mùa đông khắc nghiệt vùng núi.",
                        addClassName: "item-six",
                        type: 6
                    }
                ]
            }
        },
    };
</script>