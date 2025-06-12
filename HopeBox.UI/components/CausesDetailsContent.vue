<template>
  <section class="causes-details-area" v-if="cause">
    <div class="container">
      <div class="row">
        <div class="col-12">
          <div class="causes-details-column">
            <div class="causes-details-content">
              <div class="causes-details">
                <div class="thumb">
                  <img class="w-100" :src="cause.heroImage" alt="image">
                </div>
                <h3 class="cause-title">{{ cause.title }}</h3>
                <div class="donate-info-wrp">
                  <div class="row">
                    <div class="col-md-6">
                      <ul class="donate-info">
                        <li class="info-item">
                          <span class="info-title">Mục tiêu:</span>
                          <span class="amount">{{ (cause.targetAmount / 1_000_000).toFixed(1) }}tr ₫</span>
                        </li>
                        <li class="info-item">
                          <span class="info-title">Đóng góp:</span>
                          <span class="amount">{{ (cause.currentAmount / 1_000_000).toFixed(1) }}tr ₫</span>
                        </li>
                        <li class="info-item">
                          <span class="info-title">Còn thiếu:</span>
                          <span class="amount">{{ ((cause.targetAmount - cause.currentAmount) / 1_000_000).toFixed(1)
                            }}tr ₫</span>
                        </li>

                      </ul>
                    </div>
                    <div class="col-md-6">
                      <div class="progress-item">
                        <div class="progress-line">
                          <div class="progress-bar-line" :style="{ width: progress + '%' }">
                            <span class="percent">{{ progress }}%</span>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
                <p>{{ cause.detail }}</p>

                <h3>Vấn đề khó khăn</h3>
                <img class="w-100 mb-3" :src="cause.challengeImage" alt="Challenge image">
                <p>{{ cause.challenge }}</p>

                <h3>Tổng quan</h3>
                <img class="w-100 mb-3" :src="cause.summaryImage" alt="Summary image">
                <p>{{ cause.summary }}</p>
              </div>

              <DonnerList />
              <DonationForm :causeId="cause.id"/>
            </div>
            <SidebarWrapper />
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
  import axios from 'axios';

  export default {
    props: {
      id: {
        type: String,
        required: true
      }
    },
    data() {
      return {
        cause: null,
      };
    },
    computed: {
      progress() {
        if (!this.cause || this.cause.targetAmount === 0) return 0;
        return Math.min(
          100,
          Math.round((this.cause.currentAmount / this.cause.targetAmount) * 100)
        );
      }
    },
    mounted() {
      this.fetchCauseDetails();
    },
    methods: {
      async fetchCauseDetails() {
        try {
          const response = await axios.get(`https://hopebox-api.roz.io.vn/api/Cause/get-by-id?id=${this.id}`);
          this.cause = response.data.responseData;
        } catch (err) {
          console.error("Lỗi khi gọi API:", err);
        }
      }
    },
    components: {
      DonnerList: () => import('@/components/DonnerList'),
      DonationForm: () => import('@/components/DonationForm'),
      SidebarWrapper: () => import('@/components/SidebarWrapper'),
    },
  };
</script>