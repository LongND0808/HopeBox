<template>
  <section class="causes-details-area" v-if="cause">
    <div class="container">
      <div class="row">
        <div class="col-12">
          <div class="causes-details-column">
            <div class="causes-details-content">
              <div class="causes-details">
                <h3 class="cause-title">{{ cause.title }}</h3>
                <div class="donate-info-wrp">
                  <div class="row">
                    <div class="col-md-4">
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
                          <span class="amount">
                            {{ (cause.targetAmount - cause.currentAmount) < 0 ? '0 ₫' : ((cause.targetAmount -
                              cause.currentAmount) / 1_000_000).toFixed(1) + 'tr ₫' }} </span>
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

                <div class="timeline-wrapper my-4">
                  <div class="timeline">
                    <div class="timeline-line"></div>
                    <div class="timeline-milestone" :class="{ active: cause.status >= CausesStatus.ONGOING }">
                      <div class="flag">
                        <span class="flag-label">Đang diễn ra quyên góp</span>
                      </div>
                    </div>

                    <div class="timeline-milestone milestone-2"
                      :class="{ active: cause.status >= CausesStatus.COMPLETED }">
                      <div class="flag">
                        <span class="flag-label">Hoàn thành quyên góp</span>
                      </div>
                    </div>


                    <div class="timeline-milestone milestone-3"
                      :class="{ active: cause.status >= CausesStatus.EXECUTE }">
                      <div class="flag">
                        <span class="flag-label">Tiến hành từ thiện</span>
                      </div>
                    </div>

                    <div class="timeline-milestone milestone-4"
                      :class="{ active: cause.status >= CausesStatus.CLOSED }">
                      <div class="flag">
                        <span class="flag-label">Kết thúc chiến dịch</span>
                      </div>
                    </div>
                  </div>
                </div>

                <div class="thumb">
                  <img class="w-50 d-block mx-auto my-3" :src="cause.heroImage" alt="image">
                </div>
                <p>{{ cause.detail }}</p>

                <h3>Vấn đề khó khăn</h3>
                <img class="w-50 d-block mx-auto mb-3" :src="cause.challengeImage" alt="Challenge image">
                <p>{{ cause.challenge }}</p>

                <h3>Tổng quan</h3>
                <img class="w-50 d-block mx-auto mb-3" :src="cause.summaryImage" alt="Summary image">
                <p>{{ cause.summary }}</p>
              </div>

              <DonnerList :causeId="cause.id" />
              <DonationForm :causeId="cause.id" />
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
  import axios from 'axios';
  import { BASE_URL } from '@/utils/constants';
  import { CausesStatus } from '@/enums/enums.js';

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
        CausesStatus
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
          const response = await axios.get(`${BASE_URL}/api/Cause/get-by-id?id=${this.id}`);
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

<style scoped>
  .timeline-wrapper {
    position: relative;
    margin: 20px 0;
  }

  .timeline {
    display: flex;
    align-items: center;
    position: relative;
    height: 50px;
    margin-top: 80px;
    margin-bottom: 80px;
    padding: 0 80px;
  }

  .timeline-line {
    position: absolute;
    top: 50%;
    left: 0;
    right: 0;
    height: 4px;
    background-color: #e0e0e0;
    z-index: 1;
   margin: 0 30px;
  }

  .timeline-milestone {
    position: relative;
    flex: 1;
    text-align: center;
    z-index: 2;
  }

  .timeline-milestone::before {
    content: '';
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    width: 16px;
    height: 16px;
    background-color: #e0e0e0;
    border-radius: 50%;
    transition: background-color 0.3s ease;
  }

  .timeline-milestone.active::before {
    background-color: #e88010;
  }

  .flag {
    position: absolute;
    top: -40px;
    left: 50%;
    transform: translateX(-50%);
    background-color: #f8f9fa;
    padding: 5px 10px;
    border-radius: 4px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    display: flex;
    align-items: center;
    transition: transform 0.3s ease, background-color 0.3s ease;
  }

  .timeline-milestone.active .flag {
    background-color: #e88010;
    color: white;
    transform: translateX(-50%) scale(1.1);
  }

  .flag-label {
    font-size: 12px;
    font-weight: bold;
    white-space: nowrap;
  }

  .timeline-milestone.milestone-2 {
    margin-left: 25%;
  }

  .timeline-milestone.milestone-3 {
    margin-left: 25%;
  }

  .timeline-milestone.milestone-4 {
    margin-left: 25%;
  }

  .timeline-milestone.active .flag {
    animation: wave 1.5s infinite;
  }

  @keyframes wave {
    0% {
      transform: translateX(-50%) rotate(0deg);
    }

    50% {
      transform: translateX(-50%) rotate(5deg);
    }

    100% {
      transform: translateX(-50%) rotate(0deg);
    }
  }
</style>