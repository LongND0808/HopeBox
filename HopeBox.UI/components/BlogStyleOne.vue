<template>
    <section class="blog-area blog-default-area">
        <div class="container">
            <div class="row">
                <div class="col-md-8 col-lg-6 m-auto">
                    <div class="section-title text-center" data-aos="fade-up" data-aos-duration="1000">
                        <h5 class="subtitle line-theme-color">Bài Viết Mới</h5>
                        <h2 class="title title-style">
                            Tin Tức Mới Nhất Từ Blog HopeBox
                            <img class="img-shape" src="/images/shape/3.png" alt="Hình ảnh HopeBox">
                        </h2>
                    </div>
                </div>
            </div>
            <div class="row mtn-150" data-aos="fade-up" data-aos-duration="1000">
                <div class="col-md-6 col-lg-4 mt-150" v-for="(blog, index) in blogs" :key="index">
                    <div class="post-item">
                        <div class="thumb">
                            <nuxt-link to="/blog-details">
                                <img :src="blog.imgSrc" :alt="blog.title">
                            </nuxt-link>
                            <div class="meta-date">
                                <nuxt-link to="/blog"><span>{{ blog.metaDate }}</span> {{ blog.metaMonth }}</nuxt-link>
                            </div>
                            <div class="shape-line"></div>
                        </div>
                        <div class="content">
                            <div class="inner-content">
                                <div class="meta">
                                    <nuxt-link to="/blog" class="post-category">{{ blog.category }}</nuxt-link>
                                    <div class="post-share">
                                        <button class="icon-share">
                                            <img src="/images/icons/share-line-gradient.png" alt="Chia sẻ">
                                        </button>
                                        <ul>
                                            <li v-for="(social, index) in blog.socials" :key="index">
                                                <a :href="social.url" :class="social.addColorClass" target="_blank" rel="noopener">
                                                    <i :class="social.icon"></i>
                                                </a>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <h4 class="title">
                                    <nuxt-link to="/blog-details">{{ blog.title }}</nuxt-link>
                                </h4>
                                <p>{{ blog.desc }}</p>
                            </div>
                            <div class="post-footer">
                                <nuxt-link to="/blog-details" class="btn-theme btn-border-gradient btn-size-xs">
                                    <span>Xem Chi Tiết <img class="icon icon-img" src="/images/icons/arrow-line-right-gradient.png" alt="Mũi tên"></span>
                                </nuxt-link>
                                <nuxt-link to="/blog" class="post-author">Tác giả: HopeBox Team</nuxt-link>
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
                blogs: [
                    {
                        imgSrc: "/images/blog/1.jpg",
                        metaDate: 15,
                        metaMonth: "Tháng 6",
                        category: "Giáo dục",
                        title: "Cùng Chung Tay Giúp Trẻ Em Vươn Lên",
                        desc: "Chúng tôi đang nỗ lực mang đến cơ hội học tập tốt hơn cho trẻ em khó khăn trên khắp đất nước.",
                        socials: [
                            { icon: "icofont-facebook", url: "#", addColorClass: "color-facebook" },
                            { icon: "icofont-twitter", url: "#", addColorClass: "color-twitter" },
                            { icon: "icofont-dribbble", url: "#", addColorClass: "color-dribbble" },
                            { icon: "icofont-pinterest", url: "#", addColorClass: "color-pinterest" }
                        ]
                    },
                    {
                        imgSrc: "/images/blog/2.jpg",
                        metaDate: 10,
                        metaMonth: "Tháng 7",
                        category: "Sức khỏe",
                        title: "Kêu Gọi Hỗ Trợ Y Tế Cho Trẻ Em Thiếu May Mắn",
                        desc: "Chiến dịch chăm sóc sức khỏe miễn phí nhằm giúp trẻ em nghèo vượt qua bệnh tật.",
                        socials: [
                            { icon: "icofont-facebook", url: "#", addColorClass: "color-facebook" },
                            { icon: "icofont-twitter", url: "#", addColorClass: "color-twitter" },
                            { icon: "icofont-dribbble", url: "#", addColorClass: "color-dribbble" },
                            { icon: "icofont-pinterest", url: "#", addColorClass: "color-pinterest" }
                        ]
                    },
                    {
                        imgSrc: "/images/blog/3.jpg",
                        metaDate: 17,
                        metaMonth: "Tháng 3",
                        category: "Nước sạch",
                        title: "Nước Sạch Cho Mọi Nhà - Hành Trình Của Chúng Tôi",
                        desc: "Mục tiêu cung cấp nguồn nước sạch và an toàn cho cộng đồng vùng sâu vùng xa.",
                        socials: [
                            { icon: "icofont-facebook", url: "#", addColorClass: "color-facebook" },
                            { icon: "icofont-twitter", url: "#", addColorClass: "color-twitter" },
                            { icon: "icofont-dribbble", url: "#", addColorClass: "color-dribbble" },
                            { icon: "icofont-pinterest", url: "#", addColorClass: "color-pinterest" }
                        ]
                    },
                ]
            }
        },
    };
</script>
