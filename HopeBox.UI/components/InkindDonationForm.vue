<template>
    <div class="inkind-donation-form-section">
        <div class="form-header">
            <h4 class="form-title">Quyên Góp Hiện Vật</h4>
            <p class="form-description">Hãy chia sẻ những món quà ý nghĩa để giúp đỡ cộng đồng</p>
        </div>

        <form @submit.prevent="handleSubmit" class="inkind-form">
            <div class="form-group">
                <label for="inKind" class="form-label">
                    <i class="fas fa-gift"></i>
                    Mô tả hiện vật <span class="required">*</span>
                </label>
                <textarea
                    id="inKind"
                    v-model="formData.inKind"
                    class="form-control"
                    rows="4"
                    placeholder="Vui lòng mô tả chi tiết hiện vật bạn muốn quyên góp..."
                    :class="{ 'is-invalid': errors.inKind }"
                    maxlength="2000"
                ></textarea>
                <div class="char-count">{{ formData.inKind.length }}/2000</div>
                <div v-if="errors.inKind" class="invalid-feedback">{{ errors.inKind }}</div>
            </div>

            <div class="form-group">
                <label for="senderAddress" class="form-label">
                    <i class="fas fa-map-marker-alt"></i>
                    Địa chỉ gửi <span class="required">*</span>
                </label>
                <input
                    id="senderAddress"
                    v-model="formData.senderAddress"
                    type="text"
                    class="form-control"
                    placeholder="Nhập địa chỉ nơi bạn muốn gửi hiện vật..."
                    :class="{ 'is-invalid': errors.senderAddress }"
                    maxlength="200"
                />
                <div v-if="errors.senderAddress" class="invalid-feedback">{{ errors.senderAddress }}</div>
            </div>

            <div class="form-group">
                <label for="estimatedDeliveryDate" class="form-label">
                    <i class="fas fa-calendar-alt"></i>
                    Ngày giao dự kiến
                </label>
                <input
                    id="estimatedDeliveryDate"
                    v-model="formData.estimatedDeliveryDate"
                    type="date"
                    class="form-control"
                    :min="minDate"
                />
            </div>

            <div class="form-group">
                <label for="message" class="form-label">
                    <i class="fas fa-comment"></i>
                    Lời nhắn (tùy chọn)
                </label>
                <textarea
                    id="message"
                    v-model="formData.message"
                    class="form-control"
                    rows="3"
                    placeholder="Để lại lời nhắn ý nghĩa..."
                    maxlength="500"
                ></textarea>
                <div class="char-count">{{ (formData.message || '').length }}/500</div>
            </div>

            <div class="form-group">
                <div class="form-check">
                    <input
                        id="isAnonymous"
                        v-model="formData.isAnonymous"
                        type="checkbox"
                        class="form-check-input"
                    />
                    <label for="isAnonymous" class="form-check-label">
                        <i class="fas fa-user-secret"></i>
                        Quyên góp ẩn danh
                    </label>
                </div>
            </div>

            <div class="form-actions">
                <button
                    type="button"
                    @click="resetForm"
                    class="btn btn-secondary"
                    :disabled="isSubmitting"
                >
                    <i class="fas fa-undo"></i>
                    Đặt lại
                </button>
                <button
                    type="submit"
                    class="btn btn-primary"
                    :disabled="isSubmitting || !isFormValid"
                >
                    <span v-if="isSubmitting">
                        <i class="fas fa-spinner fa-spin"></i>
                        Đang gửi...
                    </span>
                    <span v-else>
                        <i class="fas fa-heart"></i>
                        Gửi quyên góp
                    </span>
                </button>
            </div>
        </form>

        <!-- Success Message -->
        <div v-if="showSuccessMessage" class="success-message">
            <div class="success-content">
                <i class="fas fa-check-circle"></i>
                <h5>Quyên góp thành công!</h5>
                <p>Cảm ơn bạn đã đóng góp. Chúng tôi sẽ liên hệ với bạn sớm nhất.</p>
            </div>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import { showSuccessAlert, showErrorAlert, showWarningAlert, showConfirmDialog } from '@/utils/alertHelper';
import { BASE_URL } from '@/utils/constants'

export default {
    name: 'InkindDonationForm',
    props: {
        eventId: {
            type: String,
            required: true
        }
    },
    data() {
        return {
            formData: {
                inKind: '',
                senderAddress: '',
                message: '',
                isAnonymous: false,
                estimatedDeliveryDate: null
            },
            errors: {},
            isSubmitting: false,
            showSuccessMessage: false,
            userInfo: null
        }
    },
    computed: {
        isFormValid() {
            return this.formData.inKind.trim().length >= 10 && 
                   this.formData.senderAddress.trim().length >= 5;
        },
        minDate() {
            const tomorrow = new Date();
            tomorrow.setDate(tomorrow.getDate() + 1);
            return tomorrow.toISOString().split('T')[0];
        }
    },
    mounted() {
        this.getCurrentUser();
    },
    methods: {
        async getCurrentUser() {
            try {
                const response = await axios.get(`${BASE_URL}/api/Authentication/me`, {
                    withCredentials: true
                });

                if (response.data.status === 200) {
                    this.userInfo = response.data.responseData;
                }
            } catch (error) {
                console.error('Lỗi khi lấy thông tin user:', error);
            }
        },

        validateForm() {
            this.errors = {};

            if (!this.formData.inKind.trim()) {
                this.errors.inKind = 'Vui lòng nhập thông tin hiện vật';
            } else if (this.formData.inKind.trim().length < 10) {
                this.errors.inKind = 'Mô tả hiện vật phải ít nhất 10 ký tự';
            }

            if (!this.formData.senderAddress.trim()) {
                this.errors.senderAddress = 'Vui lòng nhập địa chỉ gửi';
            } else if (this.formData.senderAddress.trim().length < 5) {
                this.errors.senderAddress = 'Địa chỉ phải ít nhất 5 ký tự';
            }

            return Object.keys(this.errors).length === 0;
        },

        async handleSubmit() {
            if (!this.userInfo) {
                await this.handleUnauthenticatedUser();
                return;
            }

            if (!this.validateForm()) {
                return;
            }

            const confirmResult = await showConfirmDialog(
                'Xác nhận quyên góp',
                'Bạn có chắc chắn muốn gửi quyên góp hiện vật này?',
                'Gửi quyên góp',
                'Hủy'
            );

            if (!confirmResult.isConfirmed) return;

            try {
                this.isSubmitting = true;

                const requestData = {
                    eventId: this.eventId,
                    inKind: this.formData.inKind.trim(),
                    senderAddress: this.formData.senderAddress.trim(),
                    message: this.formData.message?.trim() || null,
                    isAnonymous: this.formData.isAnonymous,
                    estimatedDeliveryDate: this.formData.estimatedDeliveryDate || null
                };

                const response = await axios.post(
                    `${BASE_URL}/api/InkindDonation/create`,
                    requestData,
                    {
                        withCredentials: true,
                        headers: {
                            'Content-Type': 'application/json'
                        }
                    }
                );

                if (response.data.status === 200 || response.data.status === 201) {
                    this.showSuccessMessage = true;
                    this.resetForm();
                    
                    await showSuccessAlert(
                        'Quyên góp thành công!',
                        'Cảm ơn bạn đã đóng góp. Chúng tôi sẽ liên hệ với bạn sớm nhất.'
                    );

                    setTimeout(() => {
                        this.showSuccessMessage = false;
                    }, 5000);
                } else {
                    await showErrorAlert(
                        'Quyên góp thất bại',
                        response.data.message || 'Có lỗi xảy ra khi gửi quyên góp'
                    );
                }
            } catch (error) {
                console.error('Lỗi khi gửi quyên góp:', error);
                await this.handleSubmitError(error);
            } finally {
                this.isSubmitting = false;
            }
        },

        async handleUnauthenticatedUser() {
            const result = await showConfirmDialog(
                'Yêu cầu đăng nhập',
                'Bạn cần đăng nhập để quyên góp hiện vật. Chuyển đến trang đăng nhập?',
                'Đăng nhập',
                'Hủy'
            );

            if (result.isConfirmed) {
                this.$router.push('/login');
            }
        },

        async handleSubmitError(error) {
            if (error.response) {
                const status = error.response.status;
                const message = error.response.data?.message || 'Có lỗi xảy ra';

                switch (status) {
                    case 401:
                        await showWarningAlert(
                            'Phiên đăng nhập hết hạn',
                            'Vui lòng đăng nhập lại để tiếp tục.'
                        );
                        this.$router.push('/login');
                        break;
                    case 400:
                        await showErrorAlert('Dữ liệu không hợp lệ', message);
                        break;
                    case 404:
                        await showErrorAlert('Không tìm thấy sự kiện', 'Sự kiện này có thể đã bị xóa hoặc không tồn tại.');
                        break;
                    case 500:
                        await showErrorAlert(
                            'Lỗi hệ thống',
                            'Đã xảy ra lỗi từ phía server. Vui lòng thử lại sau.'
                        );
                        break;
                    default:
                        await showErrorAlert(
                            'Có lỗi xảy ra',
                            'Không thể hoàn thành quyên góp. Vui lòng thử lại sau.'
                        );
                }
            } else {
                await showErrorAlert(
                    'Lỗi kết nối',
                    'Không thể kết nối đến server. Vui lòng kiểm tra kết nối mạng.'
                );
            }
        },

        resetForm() {
            this.formData = {
                inKind: '',
                senderAddress: '',
                message: '',
                isAnonymous: false,
                estimatedDeliveryDate: null
            };
            this.errors = {};
            this.showSuccessMessage = false;
        }
    }
}
</script>

<style scoped>
@import '@/assets/scss/component/_inkind-donation-form.scss';
</style>
