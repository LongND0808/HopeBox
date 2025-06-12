<template>
    <footer class="footer-area" :style="{ backgroundImage: `url('/images/shape/footer-line.png')` }">
        <div class="footer-main">
            <div class="container">
                <div class="row mtn-40">
                    <div class="col-sm-12 col-md-6 col-lg-4 col-xl-4 mt-40">
                        <div class="widget-item">
                            <div class="about-widget">
                                <nuxt-link to="/" class="footer-logo">
                                    <img src="/images/logo/logo.png" alt="Logo">
                                </nuxt-link>
                                <p>
                                    HopeBox - Chung tay kết nối và hỗ trợ cộng đồng. Cùng nhau tạo nên những điều tốt đẹp cho cuộc sống!
                                </p>
                                <div class="widget-total-raised">
                                    <h4 class="raised-title">Tổng số tiền đã quyên góp:</h4>
                                    <div class="raised-amount">880tr₫</div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6 col-md-6 col-lg-4 col-xl-4 mt-40">
                        <div class="widget-item">
                            <h4 class="widget-title line-style">Thư viện ảnh</h4>
                            <div class="widget-gallery">
                                <div class="row row-cols-3 row-gutter-10 mtn-10">
                                    <div class="col mt-10" v-for="i in 6" :key="i">
                                        <div class="gallery-item">
                                            <img :src="`/images/photos/gallery${i}.jpg`" alt="ảnh thư viện">
                                            <a class="icon" href="#">
                                                <i class="icofont-instagram"></i>
                                            </a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6 col-md-6 col-lg-4 col-xl-4 mt-40">
                        <div class="widget-item menu-wrap-two-column">
                            <h4 class="widget-title line-style">Liên kết nhanh</h4>
                            <div class="widget-menu-wrap">
                                <div class="row">
                                    <div class="col-6">
                                        <ul class="nav-menu nav">
                                            <li><a href="#">Về chúng tôi</a></li>
                                            <li><a href="#">Điều khoản Blog</a></li>
                                            <li><a href="#">Điều kiện sử dụng</a></li>
                                            <li><a href="#">Chính sách bảo mật</a></li>
                                            <li><a href="#">Tài liệu hướng dẫn</a></li>
                                            <li><a href="#">Nhà tài trợ</a></li>
                                        </ul>
                                    </div>
                                    <div class="col-6">
                                        <ul class="nav-menu nav align-right">
                                            <li><a href="#">Trở thành tình nguyện viên</a></li>
                                            <li><a href="#">Quyên góp nhanh</a></li>
                                            <li><a href="#">Góp quỹ</a></li>
                                            <li><a href="#">Hỗ trợ thực phẩm và nước</a></li>
                                            <li><a href="#">Cơ sở y tế</a></li>
                                            <li><a href="#">Liên hệ chúng tôi</a></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="footer-bottom">
            <div class="container">
                <div class="footer-bottom-content">
                    <div class="row align-items-center">
                        <div class="col-12">
                            <div class="widget-copyright text-center">
                                <p>
                                    &copy; 2023 <span>HopeBox</span>. Tạo nên từ tâm huyết <i class="icofont-heart-alt"></i> bởi <a href="#">Đội ngũ HopeBox</a>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </footer>
</template>

<script>
export default {};
</script>
