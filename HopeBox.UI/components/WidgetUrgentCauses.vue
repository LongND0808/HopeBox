<template>
    <div class="widget">
        <h3 class="widget-title">Chiến Dịch Quan Trọng</h3>
        <div class="separator-line">
            <img src="/images/shape/line-t2.png" alt="image">
        </div>
        
        <!-- Loading State -->
        <div v-if="loading" class="widget-causes-item">
            <div class="loading-placeholder">
                <p>Đang tải thông tin chiến dịch...</p>
            </div>
        </div>

        <!-- Error State -->
        <div v-else-if="error" class="widget-causes-item">
            <div class="error-placeholder">
                <p>{{ error }}</p>
                <button @click="fetchMostUrgentCause" class="btn-retry">Thử lại</button>
            </div>
        </div>

        <!-- Main Content -->
        <div v-else-if="causeData" class="widget-causes-item">
            <div class="thumb">
                <img :src="causeData.heroImage || '/images/causes/w1.jpg'" 
                     :alt="causeData.title"
                     @error="handleImageError">
            </div>
            <div class="content">
                <h4 class="title">
                    <nuxt-link :to="`/causes-details?id=${causeData.id}`">
                        {{ causeData.title }}
                    </nuxt-link>
                </h4>
                <ul class="donate-info">
                    <li class="info-item">
                        <span class="info-title">Mục tiêu:</span>
                        <span class="amount">{{ formatCurrency(causeData.targetAmount) }}</span>
                    </li>
                    <li class="info-item">
                        <span class="info-title">Đã quyên góp:</span>
                        <span class="amount">{{ formatCurrency(causeData.currentAmount) }}</span>
                    </li>
                </ul>
                
                <!-- Progress Bar -->
                <div class="progress-item">
                    <div class="progress-line">
                        <div class="progress-bar-line" :style="{ width: completionPercentage + '%' }">
                            <span class="percent">{{ Math.round(completionPercentage) }}%</span>
                        </div>
                    </div>
                </div>
                
                <!-- Donation Form -->
                <form @submit.prevent="handleDonation">
                    <div class="amount-info">
                        <div class="donate-amount" 
                             v-for="amount in predefinedAmounts" 
                             :key="amount"
                             :class="{ active: selectedAmount === amount }"
                             @click="selectAmount(amount)">
                            {{ formatCurrency(amount) }}
                        </div>
                        <div class="donate-amount donate-custom-amount mr-0">
                            <input class="form-control" 
                                   type="number" 
                                   v-model="customAmount"
                                   placeholder="Số tiền khác"
                                   @focus="clearSelectedAmount">
                        </div>
                    </div>
                    <nuxt-link :to="`/donation?causeId=${causeData.id}&amount=${getDonationAmount()}`" 
                               class="btn-theme btn-gradient btn-slide">
                        <span>
                            Quyên góp ngay 
                            <img class="icon icon-img" src="/images/icons/arrow-line-right2.png" alt="Icon">
                        </span>
                    </nuxt-link>
                </form>
            </div>
        </div>

        <!-- No Data State -->
        <div v-else class="widget-causes-item">
            <div class="no-data-placeholder">
                <p>Hiện tại chưa có chiến dịch nào cần hỗ trợ khẩn cấp.</p>
            </div>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import { BASE_URL } from '@/utils/constants';

export default {
    name: 'MostUrgentCause',
    data() {
        return {
            causeData: null,
            loading: false,
            error: null,
            selectedAmount: null,
            customAmount: '',
            predefinedAmounts: [50000, 100000, 200000, 500000] // VND amounts
        }
    },
    computed: {
        /**
         * Tính phần trăm hoàn thành của chiến dịch
         * @returns {number} Phần trăm từ 0-100
         */
        completionPercentage() {
            if (!this.causeData || !this.causeData.targetAmount || this.causeData.targetAmount <= 0) {
                return 0;
            }
            
            const percentage = (this.causeData.currentAmount / this.causeData.targetAmount) * 100;
            
            // Giới hạn từ 0-100%
            return Math.min(Math.max(percentage, 0), 100);
        }
    },
    mounted() {
        this.fetchMostUrgentCause();
    },
    methods: {
        async fetchMostUrgentCause() {
            try {
                this.loading = true;
                this.error = null;

                const response = await axios.get(`${BASE_URL}/api/Cause/get-most-urgent-cause`, {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                });

                if (response.data.status === 200) {
                    this.causeData = response.data.responseData;
                    console.log('Most urgent cause loaded:', this.causeData);
                } else {
                    this.error = response.data.message || 'Không thể tải thông tin chiến dịch';
                    console.error('API Error:', response.data.message);
                }
            } catch (error) {
                console.error('Error fetching most urgent cause:', error);
                
                if (error.response) {
                    // Server responded with error status
                    this.error = `Lỗi server: ${error.response.data?.message || error.response.statusText}`;
                } else if (error.request) {
                    // Request was made but no response received
                    this.error = 'Không thể kết nối đến server. Vui lòng kiểm tra kết nối mạng.';
                } else {
                    // Something else happened
                    this.error = 'Đã xảy ra lỗi không xác định.';
                }
            } finally {
                this.loading = false;
            }
        },

        /**
         * Format số tiền theo định dạng Việt Nam
         * @param {number} amount - Số tiền cần format
         * @returns {string} Số tiền đã được format
         */
        formatCurrency(amount) {
            if (!amount || amount === 0) {
                return '0 ₫';
            }
            
            try {
                return new Intl.NumberFormat('vi-VN', {
                    style: 'currency',
                    currency: 'VND',
                    minimumFractionDigits: 0,
                    maximumFractionDigits: 0
                }).format(amount);
            } catch (error) {
                // Fallback nếu Intl.NumberFormat không hoạt động
                return amount.toLocaleString('vi-VN') + ' ₫';
            }
        },

        /**
         * Chọn số tiền quyên góp định sẵn
         * @param {number} amount - Số tiền được chọn
         */
        selectAmount(amount) {
            this.selectedAmount = amount;
            this.customAmount = ''; // Clear custom amount khi chọn predefined
        },

        /**
         * Clear selected amount khi focus vào custom input
         */
        clearSelectedAmount() {
            this.selectedAmount = null;
        },

        /**
         * Lấy số tiền quyên góp hiện tại (selected hoặc custom)
         * @returns {number} Số tiền quyên góp
         */
        getDonationAmount() {
            if (this.customAmount && !isNaN(this.customAmount)) {
                return parseInt(this.customAmount);
            }
            
            if (this.selectedAmount) {
                return this.selectedAmount;
            }
            
            return this.predefinedAmounts[0]; // Default amount
        },

        /**
         * Xử lý submit form quyên góp
         */
        handleDonation() {
            const amount = this.getDonationAmount();
            
            if (!amount || amount <= 0) {
                alert('Vui lòng chọn số tiền quyên góp hợp lệ');
                return;
            }
            
            // Redirect to donation page
            this.$router.push({
                path: '/donation',
                query: {
                    causeId: this.causeData.id,
                    amount: amount
                }
            });
        },

        /**
         * Xử lý lỗi khi load ảnh
         * @param {Event} event - Image error event
         */
        handleImageError(event) {
            event.target.src = '/images/causes/w1.jpg'; // Fallback image
        }
    }
}
</script>