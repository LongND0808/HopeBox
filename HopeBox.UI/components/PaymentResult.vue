<template>
    <div class="donation-form bgcolor-theme3 text-center py-5">
        <div v-if="isLoading">
            <div class="section-title">
                <h5 class="subtitle line-theme-color mb-14">Đang Xác Minh</h5>
                <h2 class="title text-white">Vui Lòng Chờ...</h2>
                <img class="line-shape" src="/images/shape/line-t1.png" alt="loading" />
            </div>
        </div>
        <div v-else>
            <div class="section-title">
                <h5 class="subtitle line-theme-color mb-14">Kết Quả Thanh Toán</h5>
                <h2 class="title text-white">{{ statusMessage }}</h2>
                <p class="text-white">{{ detailMessage }}</p>
                <img class="line-shape" src="/images/shape/line-t1.png" alt="line" />
            </div>
            <nuxt-link to="/" class="btn-theme btn-gradient btn-slide mt-4">
                Quay về trang chủ
                <img class="icon icon-img" src="/images/icons/arrow-line-right2.png" alt="icon">
            </nuxt-link>
        </div>
        <div class="layer-style">
            <img class="layer-style1" src="/images/shape/form-shape1.png" alt="layer" />
            <img class="layer-style2" src="/images/shape/form-shape2.png" alt="layer" />
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import { showSuccessAlert, showErrorAlert } from '@/utils/alertHelper.js';
import { BASE_URL } from '@/utils/constants';
import { PaymentMethod } from '@/enums/enums.js';

export default {
    data() {
        return {
            isLoading: true,
            statusMessage: '',
            detailMessage: ''
        };
    },
    async mounted() {
        try {
            const query = this.$route.query;
            const paymentMethod = Number(query.paymentMethod);

            if (paymentMethod === PaymentMethod.VIETQR) {
                // Handle VietQR transaction verification
                const response = await axios.post(`${BASE_URL}/api/Donation/track-email-transaction?tradingCode=${query.tradingCode}`, {}, {
                    withCredentials: true
                });

                const result = response.data;

                if (result.status === 200 && result.responseData === true) {
                    this.statusMessage = 'Thanh toán thành công!';
                    this.detailMessage = (result.message || 'Cảm ơn bạn đã quyên góp!') + ' Hóa đơn đã được gửi vào Gmail của bạn.';
                    await showSuccessAlert('Thanh toán thành công', this.detailMessage);
                } else {
                    this.statusMessage = 'Thanh toán thất bại';
                    this.detailMessage = result.message || 'Không tìm thấy giao dịch khớp với mã giao dịch.';
                    await showErrorAlert('Thanh toán thất bại', this.detailMessage);
                }
            } else {
                // Handle VNPay transaction verification
                const response = await axios.post(`${BASE_URL}/api/Donation/vnpay-return`, {
                    vnp_TxnRef: query.vnp_TxnRef,
                    vnp_ResponseCode: query.vnp_ResponseCode,
                    vnp_TransactionNo: query.vnp_TransactionNo
                }, {
                    withCredentials: true
                });

                const result = response.data;

                if (result.status === 200 && result.responseData === true) {
                    this.statusMessage = 'Thanh toán thành công!';
                    this.detailMessage = (result.message || 'Cảm ơn bạn đã quyên góp!') + ' Hóa đơn đã được gửi vào Gmail của bạn.';
                    await showSuccessAlert('Thanh toán thành công', this.detailMessage);
                } else {
                    this.statusMessage = 'Thanh toán thất bại';
                    this.detailMessage = result.message || 'Thanh toán không thành công. Vui lòng thử lại.';
                    await showErrorAlert('Thanh toán thất bại', this.detailMessage);
                }
            }
        } catch (error) {
            this.statusMessage = 'Lỗi hệ thống';
            this.detailMessage = 'Có lỗi xảy ra khi xác minh thanh toán.';
            await showErrorAlert('Lỗi', this.detailMessage);
        } finally {
            this.isLoading = false;
        }
    }
};
</script>

<style scoped>
.donation-form {
    position: relative;
    padding: 40px;
    border-radius: 12px;
    overflow: hidden;
}

.section-title {
    text-align: center;
    margin-bottom: 30px;
}

.subtitle {
    font-size: 1rem;
    font-weight: 600;
    text-transform: uppercase;
}

.title {
    font-size: 2rem;
    font-weight: 700;
}

.line-shape {
    margin-top: 10px;
}

.btn-theme {
    padding: 12px 30px;
    font-size: 1rem;
    font-weight: 600;
    border-radius: 8px;
    text-transform: uppercase;
}

.btn-gradient {
    background: linear-gradient(90deg, #fbc658, #f6a93b);
    color: #111226;
    border: none;
}

.btn-slide {
    display: inline-flex;
    align-items: center;
    gap: 10px;
}

.icon-img {
    width: 20px;
    height: 20px;
}

.layer-style {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    pointer-events: none;
}

.layer-style1 {
    position: absolute;
    top: 0;
    left: 0;
}

.layer-style2 {
    position: absolute;
    bottom: 0;
    right: 0;
}

@media (max-width: 768px) {
    .donation-form {
        padding: 20px;
    }
}
</style>