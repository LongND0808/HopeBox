<template>
    <div class="donation-form bgcolor-theme3 text-center py-5">
        <div v-if="isLoading">
            <div class="section-title">
                <h5 class="subtitle line-theme-color mb-14">Đang Xác Minh</h5>
                <h2 class="title text-white">Vui Lòng Chờ...</h2>
                <img class="line-shape" src="/images/shape/line-t3.png" alt="loading" />
            </div>
        </div>
        <div v-else>
            <div class="section-title">
                <h5 class="subtitle line-theme-color mb-14">Kết Quả Thanh Toán</h5>
                <h2 class="title text-white">{{ statusMessage }}</h2>
                <p class="text-white">{{ detailMessage }}</p>
                <img class="line-shape" src="/images/shape/line-t3.png" alt="line" />
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

            const response = await axios.post(`https://localhost:7213/api/Donation/vnpay-return`, {
                vnp_TxnRef: query.vnp_TxnRef,
                vnp_ResponseCode: query.vnp_ResponseCode,
                vnp_TransactionNo: query.vnp_TransactionNo
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
