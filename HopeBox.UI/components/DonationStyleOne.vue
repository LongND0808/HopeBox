<template>
  <section class="donate-area donate-default-area bgcolor-theme3" v-if="causesData.length">
    <div class="container">
      <div class="row">
        <div class="col-lg-7 col-xxl-7" v-for="(cause, index) in causesData" :key="index">
          <div class="content" data-aos="fade-up" data-aos-duration="1000">
            <div class="section-title">
              <h5 class="subtitle line-theme-color mb-7">Chiến dịch nổi bật</h5>
              <h2 class="title title-style text-white">
                {{ cause.title }}
                <img class="img-shape" src="/images/shape/3.png" alt="Image" />
              </h2>
            </div>
            <div class="donate-form">
              <form action="#">
                <div class="amount-info">
                  <div class="donate-amount">20.000đ</div>
                  <div class="donate-amount">50.000đ</div>
                  <div class="donate-amount">200.000đ</div>
                  <div class="donate-amount donate-custom-amount">
                    <input class="form-control" type="text" placeholder="Số tiền khác (đ)" />
                  </div>
                </div>
                <div class="btn-wrp">
                  <a href="javascript:void(0)" class="btn-theme btn-gradient btn-slide">
                    Quyên góp ngay
                    <img class="icon icon-img" src="/images/icons/arrow-line-right2.png" alt="Icon" />
                  </a>
                  <nuxt-link to="/event-details" class="btn-theme btn-gradient btn-border">
                    Tham gia sự kiện
                    <img class="icon icon-img" src="/images/icons/arrow-line-right2-gradient.png" alt="Icon" />
                  </nuxt-link>
                </div>
              </form>
            </div>
          </div>
        </div>

        <div class="col-lg-5 col-xxl-4 offset-xxl-1 position-static">
          <div class="donners-content">
            <div class="thumb-bg-layer" :style="{ backgroundImage: `url(${causesData[0].imgSrc})` }"></div>
            <div class="donners-info" data-aos="fade-up" data-aos-duration="1000">
              <h3>Những tấm lòng vàng</h3>
              <p>
                Hàng trăm mạnh thường quân đã cùng chung tay mang hy vọng đến các bản làng xa xôi.
                Bạn cũng có thể là một phần của hành trình ấy.
              </p>
              <div class="donners-item">
                <div class="donner-item"><img src="/images/photos/donner1.png" alt="Image" /></div>
                <div class="donner-item"><img src="/images/photos/donner2.png" alt="Image" /></div>
                <div class="donner-item"><img src="/images/photos/donner3.png" alt="Image" /></div>
                <div class="donner-item donner-number">+286</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      causesData: []
    };
  },
  mounted() {
    axios
      .get('https://localhost:7213/api/Cause/get-cause-highest-target')
      .then(async (response) => {
        const cause = response.data.responseData;
        const list = Array.isArray(cause) ? cause : [cause]; // đảm bảo là mảng

        const causesWithUser = await Promise.all(
          list.map(async (item) => {
            return {
              imgSrc: item.heroImage || 'default.jpg',
              title: item.title,
            };
          })
        );

        this.causesData = causesWithUser;
      })
      .catch((error) => {
        console.error('Lỗi khi gọi API Causes:', error);
      });
  }
};
</script>
