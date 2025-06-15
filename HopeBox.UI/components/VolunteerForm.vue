<template>
    <section class="volunteer-form-area">
        <div class="container">
            <div class="row">
                <div class="col-lg-12">
                    <div class="volunteer-form">
                        <div class="section-title">
                            <h5 class="subtitle">Join With Us</h5>
                            <h2 class="title">If You Interest! You Can Join With Us <span>As A Volunteer.</span></h2>
                            <p>Tham gia cùng chúng tôi để tạo ra những thay đổi tích cực trong cộng đồng. Hãy đăng ký
                                làm tình nguyện viên cho các chiến dịch mà bạn quan tâm.</p>
                        </div>
                    </div>

                    <!-- Danh sách các chiến dịch -->
                    <div class="causes-list-section" v-if="causedata && causedata.length > 0">
                        <div class="row mtn-30">
                            <div class="col-md-6 col-lg-4 mt-30" v-for="(cause, index) in causedata" :key="index">
                                <div class="causes-item volunteer-cause-item">
                                    <div class="thumb">
                                        <img :src="cause.heroImage || '/images/default-cause.jpg'" :alt="cause.title">
                                    </div>
                                    <div class="content">
                                        <h4 class="title">
                                            <a href="#" @click.prevent>{{ cause.title }}</a>
                                        </h4>
                                        <p class="description">{{ cause.description }}</p>
                                        <div class="summary-section" v-if="cause.summary">
                                            <p class="summary"><strong>Tóm tắt:</strong> {{ cause.summary }}</p>
                                        </div>

                                        <!-- Hiển thị trạng thái đăng ký -->
                                        <div class="volunteer-status" v-if="cause.volunteerStatus">
                                            <span class="status-badge" :class="getStatusClass(cause.volunteerStatus)">
                                                {{ getStatusText(cause.volunteerStatus) }}
                                            </span>
                                        </div>
                                    </div>
                                    <div class="causes-footer volunteer-footer">
                                        <button
                                            class="btn-theme btn-gradient btn-slide no-border volunteer-register-btn"
                                            type="button" @click="registerVolunteer(cause)"
                                            :disabled="isRegistering || cause.volunteerStatus === 'registered'"
                                            :class="{ 'disabled': cause.volunteerStatus === 'registered' }">
                                            <span v-if="isRegistering && selectedCauseId === cause.id">
                                                <i class="fa fa-spinner fa-spin"></i> Đang đăng ký...
                                            </span>
                                            <span v-else-if="cause.volunteerStatus === 'registered'">
                                                Đã đăng ký
                                            </span>
                                            <span v-else>
                                                Đăng ký
                                            </span>
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Thông báo khi không có dữ liệu -->
                    <div class="no-causes-message" v-else-if="!loading">
                        <p>Hiện tại chưa có chiến dịch nào cần tình nguyện viên.</p>
                    </div>

                    <!-- Loading state -->
                    <div class="loading-state" v-if="loading">
                        <div class="text-center">
                            <i class="fa fa-spinner fa-spin fa-2x"></i>
                            <p>Đang tải dữ liệu...</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Thay thế modal bằng notification overlay -->
        <div v-if="notification.show" class="notification-overlay" @click="hideNotification">
            <div class="notification-content" :class="notification.type" @click.stop>
                <div class="notification-header">
                    <i :class="notification.type === 'success' ? 'fa fa-check-circle' : 'fa fa-exclamation-circle'"></i>
                    <span>{{ notification.type === 'success' ? 'Thành công' : 'Lỗi' }}</span>
                    <button class="close-btn" @click="hideNotification">&times;</button>
                </div>
                <div class="notification-body">
                    {{ notification.message }}
                </div>
            </div>
        </div>
    </section>
</template>

<script>
import axios from 'axios';
import { BASE_URL } from '@/utils/constants';

export default {
    data() {
        return {
            causedata: [],
            loading: true,
            isRegistering: false,
            selectedCauseId: null,
            notification: {
                show: false,
                type: '',
                message: ''
            }
        }
    },
    mounted() {
        this.fetchCauseData();
    },
    methods: {
        async fetchCauseData() {
            try {
                this.loading = true;
                const response = await axios.get(`${BASE_URL}/api/Cause/get-all`);
                this.causedata = response.data.responseData || [];

                await this.checkVolunteerStatus();
            } catch (error) {
                console.error('Lỗi khi lấy dữ liệu chiến dịch:', error);
                this.showNotification('error', 'Không thể tải dữ liệu chiến dịch. Vui lòng thử lại sau.');
                this.causedata = [];
            } finally {
                this.loading = false;
            }
        },

        async checkVolunteerStatus() {
            try {
                const token = this.getAuthToken();
                if (!token) return;

                for (let cause of this.causedata) {
                    const registeredCauses = JSON.parse(localStorage.getItem('registeredCauses') || '[]');
                    if (registeredCauses.includes(cause.id)) {
                        cause.volunteerStatus = 'registered';
                    }
                }
            } catch (error) {
                console.error('Lỗi khi kiểm tra trạng thái đăng ký:', error);
            }
        },

        async registerVolunteer(cause) {
            try {
                this.isRegistering = true;
                this.selectedCauseId = cause.id;

                const requestData = {
                    causeId: cause.id
                };

                const response = await axios.post(
                    `${BASE_URL}/api/Volunteer/register-volunteer`,
                    requestData,
                    {
                        withCredentials: true,
                        headers: {
                            'Content-Type': 'application/json'
                        }
                    }
                );

                if (response.data.status === 201) {
                    cause.volunteerStatus = 'registered';

                    const registeredCauses = JSON.parse(localStorage.getItem('registeredCauses') || '[]');
                    if (!registeredCauses.includes(cause.id)) {
                        registeredCauses.push(cause.id);
                        localStorage.setItem('registeredCauses', JSON.stringify(registeredCauses));
                    }

                    this.showNotification('success', response.data.message || 'Đăng ký tình nguyện viên thành công!');
                } else {
                    this.showNotification('error', response.data.message || 'Có lỗi xảy ra khi đăng ký.');
                }

            } catch (error) {
                console.error('Lỗi khi đăng ký tình nguyện viên:', error);

                if (error.response) {
                    const status = error.response.status;
                    const message = error.response.data?.message || 'Có lỗi xảy ra';

                    switch (status) {
                        case 401:
                            this.showNotification('error', 'Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.');
                            this.$router.push('/login');
                            break;
                        case 409:
                            this.showNotification('error', 'Bạn đã đăng ký tình nguyện viên cho chiến dịch này rồi.');
                            cause.volunteerStatus = 'registered';
                            break;
                        case 400:
                            this.showNotification('error', message);
                            break;
                        default:
                            this.showNotification('error', 'Có lỗi xảy ra khi đăng ký. Vui lòng thử lại sau.');
                    }
                } else {
                    this.showNotification('error', 'Không thể kết nối đến server. Vui lòng kiểm tra kết nối mạng.');
                }
            } finally {
                this.isRegistering = false;
                this.selectedCauseId = null;
            }
        },

        showNotification(type, message) {
            this.notification = {
                show: true,
                type,
                message
            };

            // Tự động ẩn sau 3 giây
            setTimeout(() => {
                this.hideNotification();
            }, 3000);
        },

        hideNotification() {
            this.notification.show = false;
        },

        getStatusClass(status) {
            switch (status) {
                case 'registered':
                    return 'status-registered';
                case 'pending':
                    return 'status-pending';
                case 'approved':
                    return 'status-approved';
                default:
                    return '';
            }
        },

        getStatusText(status) {
            switch (status) {
                case 'registered':
                    return 'Đã đăng ký';
                case 'pending':
                    return 'Đang chờ duyệt';
                case 'approved':
                    return 'Đã được duyệt';
                default:
                    return '';
            }
        }
    }
}
</script>

<style scoped>
.notification-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 9999;
}

.notification-content {
    background: white;
    border-radius: 8px;
    padding: 20px;
    min-width: 300px;
    max-width: 500px;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.15);
    animation: slideIn 0.3s ease-out;
}

.notification-content.success {
    border-left: 4px solid #28a745;
}

.notification-content.error {
    border-left: 4px solid #dc3545;
}

.notification-header {
    display: flex;
    align-items: center;
    margin-bottom: 10px;
}

.notification-header i {
    margin-right: 10px;
    font-size: 20px;
}

.notification-header .fa-check-circle {
    color: #28a745;
}

.notification-header .fa-exclamation-circle {
    color: #dc3545;
}

.close-btn {
    margin-left: auto;
    background: none;
    border: none;
    font-size: 20px;
    cursor: pointer;
}

@keyframes slideIn {
    from {
        transform: translateY(-50px);
        opacity: 0;
    }

    to {
        transform: translateY(0);
        opacity: 1;
    }
}

@media (max-width: 768px) {
    .volunteer-register-btn {
        font-size: 14px;
        padding: 10px 15px;
    }
}
</style>
