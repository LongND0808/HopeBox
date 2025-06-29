<template>
    <section class="volunteer-form-area">
        <div class="container">
            <div class="row">
                <div class="col-lg-12">
                    <div class="volunteer-form">
                        <div class="section-title">
                            <h5 class="subtitle">Tham gia cùng chúng tôi</h5>
                            <h2 class="title">Nếu bạn quan tâm! Bạn có thể tham gia cùng chúng tôi <span>như một tình nguyện viên.</span></h2>
                            <p>Tham gia cùng chúng tôi để tạo ra những thay đổi tích cực trong cộng đồng. Hãy đăng ký
                                làm tình nguyện viên cho các chiến dịch mà bạn quan tâm.</p>
                        </div>
                    </div>

                    <div class="row mb-4 filter-controls">
                        <div class="col-md-4 mb-2">
                            <input v-model="searchName" type="text" class="form-control"
                                placeholder="Tìm kiếm theo tên chiến dịch...">
                        </div>
                        <div class="col-md-4 mb-2">
                            <select v-model="searchType" class="form-select">
                                <option value="">Tất cả loại chiến dịch</option>
                                <option v-for="option in causesTypeOptions" :key="option.key" :value="option.value">
                                    {{ option.label }}
                                </option>
                            </select>
                        </div>
                        <div class="col-md-2 mb-2">
                            <select v-model="pageSize" class="form-select" @change="handlePageSizeChange">
                                <option v-for="size in pageSizeOptions" :key="size" :value="size">
                                    {{ size }} mục/trang
                                </option>
                            </select>
                        </div>
                        <div class="col-md-2 mb-2 d-flex align-items-center">
                            <button class="btn btn-search w-100" @click="handleSearch" :disabled="loading">
                                <span v-if="loading">
                                    <i class="fa fa-spinner fa-spin"></i> Đang tìm...
                                </span>
                                <span v-else>
                                    Tìm kiếm
                                </span>
                            </button>
                        </div>
                    </div>

                    <div class="loading-state" v-if="loading">
                        <div class="text-center">
                            <i class="fa fa-spinner fa-spin fa-3x"></i>
                            <p>Đang tải danh sách chiến dịch...</p>
                        </div>
                    </div>

                    <div class="causes-list-section" v-else-if="causesData && causesData.length > 0">
                        <div class="volunteer-causes-list">
                            <div class="volunteer-cause-item-horizontal" v-for="(cause, index) in causesData"
                                :key="cause.id || index">

                                <div class="cause-image">
                                    <img :src="cause.heroImage || '/images/default-cause.jpg'" :alt="cause.title"
                                        @error="handleImageError">
                                </div>

                                <div class="cause-content">
                                    <div class="cause-header">
                                        <h4 class="cause-title">
                                            <a href="#" @click.prevent>{{ cause.title }}</a>
                                        </h4>

                                        <div v-if="cause.isVolunteerRegistered && cause.volunteerJoinDate">
                                            <small class="registration-date">
                                                <i class="fa fa-calendar"></i>
                                                Đăng ký: {{ formatDate(cause.volunteerJoinDate) }}
                                            </small>
                                        </div>

                                        <div class="volunteer-status" v-if="cause.isVolunteerRegistered">
                                            <span class="status-badge"
                                                :class="getStatusBadgeClass(cause.volunteerStatus)">
                                                <i :class="getStatusIcon(cause.volunteerStatus)"></i>
                                                {{ getStatusText(cause.volunteerStatus) }}
                                            </span>
                                        </div>
                                    </div>

                                    <p class="cause-description">{{ cause.description }}</p>

                                    <div class="cause-summary" v-if="cause.summary">
                                        <p class="summary-text">
                                            <strong>Tóm tắt:</strong> {{ cause.summary }}
                                        </p>
                                    </div>


                                </div>

                                <div class="cause-actions">
                                    <div class="action-content">
                                        <button class="volunteer-register-btn-horizontal" type="button"
                                            @click="handleVolunteerAction(cause)"
                                            :disabled="isProcessing || isButtonDisabled(cause)"
                                            :class="getButtonClass(cause)">
                                            <span v-if="isProcessing && selectedCauseId === cause.id">
                                                <i class="fa fa-spinner fa-spin"></i>
                                                {{ getProcessingText(cause) }}
                                            </span>
                                            <span v-else>
                                                <i :class="getButtonIcon(cause)"></i>
                                                {{ getButtonText(cause) }}
                                            </span>
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="pagination-container" v-if="totalPages > 1">
                            <button class="pagination-button" :disabled="currentPage === 1"
                                @click="goToPage(currentPage - 1)" title="Trang trước">
                                <i class="fa fa-chevron-left"></i>
                            </button>

                            <button v-for="page in visiblePages" :key="page" @click="goToPage(page)"
                                class="pagination-button" :class="{ active: page === currentPage }"
                                :title="`Trang ${page}`">
                                {{ page }}
                            </button>

                            <button class="pagination-button" :disabled="currentPage === totalPages"
                                @click="goToPage(currentPage + 1)" title="Trang sau">
                                <i class="fa fa-chevron-right"></i>
                            </button>
                        </div>
                    </div>

                    <div class="no-causes-message" v-else-if="!loading">
                        <p>
                            <i class="fa fa-info-circle"></i>
                            Hiện tại chưa có chiến dịch nào cần tình nguyện viên.
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </section>
</template>

<script>
import axios from 'axios';
import { BASE_URL } from '@/utils/constants';
import { showSuccessAlert, showErrorAlert, showWarningAlert, showConfirmDialog } from '@/utils/alertHelper';
import {
    CausesType,
    CausesTypeLabel,
    CausesTypeOptions,
    VolunteerStatus,
    VolunteerStatusLabel
} from '@/enums/enums';

export default {
    name: 'VolunteerForm',
    data() {
        return {
            causesData: [],
            loading: false,
            searchName: '',
            searchType: '',
            pageSize: 6,
            currentPage: 1,
            totalPages: 0,
            totalRecords: 0,
            userInfo: null,
            isProcessing: false,
            selectedCauseId: null,

            pageSizeOptions: [3, 6, 9, 12, 15],

            VolunteerStatus: VolunteerStatus
        }
    },
    computed: {
        // Computed properties
        causesTypeOptions() {
            return CausesTypeOptions;
        },

        visiblePages() {
            const pages = [];
            const start = Math.max(1, this.currentPage - 2);
            const end = Math.min(this.totalPages, this.currentPage + 2);

            for (let i = start; i <= end; i++) {
                pages.push(i);
            }
            return pages;
        }
    },
    mounted() {
        this.initializeData();
    },
    methods: {
        async initializeData() {
            await this.getCurrentUser();
            await this.fetchCausesData();
        },

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

        async fetchCausesData() {
            try {
                this.loading = true;

                const requestData = {
                    name: this.searchName || null,
                    causeType: this.searchType || null,
                    pageIndex: this.currentPage,
                    pageSize: this.pageSize
                };

                const response = await axios.post(
                    `${BASE_URL}/api/Cause/get-cause-by-filter-with-user-status`,
                    requestData,
                    {
                        withCredentials: true,
                        headers: {
                            'Content-Type': 'application/json'
                        }
                    }
                );

                if (response.data.status === 200) {
                    const responseData = response.data.responseData;
                    this.causesData = responseData.pagedData || [];
                    this.totalRecords = responseData.totalRecords;
                    this.totalPages = responseData.totalPages;
                } else {
                    await showErrorAlert('Lỗi tải dữ liệu', response.data.message || 'Không thể tải danh sách chiến dịch');
                    this.causesData = [];
                }
            } catch (error) {
                console.error('Lỗi khi lấy dữ liệu causes:', error);

                if (error.response?.status === 401) {
                    await this.fetchCausesDataWithoutAuth();
                } else {
                    await showErrorAlert(
                        'Lỗi kết nối',
                        'Không thể kết nối đến server. Vui lòng kiểm tra kết nối mạng và thử lại.'
                    );
                    this.causesData = [];
                }
            } finally {
                this.loading = false;
            }
        },

        async fetchCausesDataWithoutAuth() {
            try {
                const requestData = {
                    name: this.searchName || null,
                    causeType: this.searchType || null,
                    pageIndex: this.currentPage,
                    pageSize: this.pageSize
                };

                const response = await axios.post(
                    `${BASE_URL}/api/Cause/get-cause-by-filter`,
                    requestData,
                    {
                        headers: {
                            'Content-Type': 'application/json'
                        }
                    }
                );

                if (response.data.status === 200) {
                    const responseData = response.data.responseData;
                    this.causesData = responseData.pagedData || [];
                    this.totalRecords = responseData.totalRecords;
                    this.totalPages = responseData.totalPages;
                } else {
                    await showErrorAlert('Lỗi tải dữ liệu', response.data.message || 'Không thể tải danh sách chiến dịch');
                }
            } catch (error) {
                console.error('Lỗi khi lấy dữ liệu causes (không auth):', error);
                await showErrorAlert(
                    'Lỗi hệ thống',
                    'Đã xảy ra lỗi không xác định. Vui lòng thử lại sau.'
                );
            }
        },

        async handleSearch() {
            this.currentPage = 1;
            await this.fetchCausesData();
        },

        async handlePageSizeChange() {
            this.currentPage = 1;
            await this.fetchCausesData();
        },

        async goToPage(page) {
            if (page >= 1 && page <= this.totalPages && page !== this.currentPage) {
                this.currentPage = page;
                await this.fetchCausesData();
            }
        },

        async handleVolunteerAction(cause) {
            if (!this.userInfo) {
                await this.handleUnauthenticatedUser();
                return;
            }
            if (!cause.isVolunteerRegistered) {
                await this.registerVolunteer(cause);
            } else {
                if (cause.volunteerStatus === this.VolunteerStatus.PENDING) {
                    await this.showRegistrationInfo(cause);
                } else if (cause.volunteerStatus === this.VolunteerStatus.APPROVED) {
                    await this.showRegistrationInfo(cause);
                } else if (cause.volunteerStatus === this.VolunteerStatus.REJECTED) {
                    await this.handleRejectedRegistration(cause);
                }
            }
        },

        async handleUnauthenticatedUser() {
            const result = await showConfirmDialog(
                'Yêu cầu đăng nhập',
                'Bạn cần đăng nhập để đăng ký làm tình nguyện viên. Chuyển đến trang đăng nhập?',
                'Đăng nhập',
                'Hủy'
            );

            if (result.isConfirmed) {
                this.$router.push('/login');
            }
        },

        async showRegistrationInfo(cause) {
            const statusText = this.getStatusText(cause.volunteerStatus);
            const joinDate = cause.volunteerJoinDate ? this.formatDate(cause.volunteerJoinDate) : 'Không xác định';

            await showSuccessAlert(
                'Thông tin đăng ký',
                `Bạn đã đăng ký tình nguyện viên cho chiến dịch "${cause.title}"\n\nTrạng thái: ${statusText}\nNgày đăng ký: ${joinDate}`
            );
        },

        async handleRejectedRegistration(cause) {
            const result = await showConfirmDialog(
                'Đăng ký lại',
                `Đăng ký tình nguyện viên trước đó của bạn cho chiến dịch "${cause.title}" đã bị từ chối. Bạn có muốn đăng ký lại không?`,
                'Đăng ký lại',
                'Hủy'
            );

            if (result.isConfirmed) {
                await this.registerVolunteer(cause);
            }
        },

        async registerVolunteer(cause) {
            try {
                const confirmResult = await showConfirmDialog(
                    'Xác nhận đăng ký',
                    `Bạn có chắc chắn muốn đăng ký làm tình nguyện viên cho chiến dịch "${cause.title}"?`,
                    'Đăng ký',
                    'Hủy'
                );
                if (!confirmResult.isConfirmed) return;

                this.isProcessing = true;
                this.selectedCauseId = cause.id;

                let userId = this.userInfo?.id;
                if (!userId) {
                    try {
                        const meRes = await axios.get(`${BASE_URL}/api/Authentication/me`, {
                            withCredentials: true
                        });
                        if (meRes.data.status === 200) {
                            userId = meRes.data.responseData.id;
                            this.userInfo = meRes.data.responseData;
                        } else {
                            await showErrorAlert('Lỗi xác thực', 'Không thể lấy thông tin người dùng, vui lòng đăng nhập lại.');
                            return;
                        }
                    } catch (e) {
                        await showErrorAlert('Lỗi xác thực', 'Không thể lấy thông tin người dùng, vui lòng đăng nhập.');
                        this.$router.push('/login');
                        return;
                    }
                }

                const requestData = {
                    userId: userId,
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
                    cause.isVolunteerRegistered = true;
                    cause.volunteerStatus = this.VolunteerStatus.PENDING;
                    cause.volunteerJoinDate = new Date().toISOString();

                    await showSuccessAlert(
                        'Đăng ký thành công!',
                        response.data.message || 'Bạn đã đăng ký làm tình nguyện viên thành công.'
                    );
                } else {
                    await showErrorAlert(
                        'Đăng ký thất bại',
                        response.data.message || 'Có lỗi xảy ra khi đăng ký. Vui lòng thử lại.'
                    );
                }

            } catch (error) {
                console.error('Lỗi khi đăng ký tình nguyện viên:', error);
                await this.handleRegistrationError(error, cause);
            } finally {
                this.isProcessing = false;
                this.selectedCauseId = null;
            }
        },

        async handleRegistrationError(error, cause) {
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
                    case 409:
                        await showWarningAlert(
                            'Đã đăng ký trước đó',
                            'Bạn đã đăng ký tình nguyện viên cho chiến dịch này rồi.'
                        );
                        cause.isVolunteerRegistered = true;
                        break;
                    case 400:
                        await showErrorAlert('Dữ liệu không hợp lệ', message);
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
                            'Không thể hoàn thành đăng ký. Vui lòng thử lại sau.'
                        );
                }
            } else {
                await showErrorAlert(
                    'Lỗi kết nối',
                    'Không thể kết nối đến server. Vui lòng kiểm tra kết nối mạng.'
                );
            }
        },

        isButtonDisabled(cause) {
            return cause.isVolunteerRegistered && cause.volunteerStatus === this.VolunteerStatus.APPROVED;
        },

        getButtonClass(cause) {
            const baseClass = 'volunteer-register-btn';

            if (!cause.isVolunteerRegistered) {
                return `${baseClass} btn-register`;
            }

            switch (cause.volunteerStatus) {
                case this.VolunteerStatus.PENDING:
                    return `${baseClass} btn-pending`;
                case this.VolunteerStatus.APPROVED:
                    return `${baseClass} btn-approved disabled`;
                case this.VolunteerStatus.REJECTED:
                    return `${baseClass} btn-rejected`;
                default:
                    return `${baseClass} btn-register`;
            }
        },

        getButtonText(cause) {
            if (!cause.isVolunteerRegistered) {
                return 'Đăng ký tình nguyện';
            }

            switch (cause.volunteerStatus) {
                case this.VolunteerStatus.PENDING:
                    return 'Xem thông tin';
                case this.VolunteerStatus.APPROVED:
                    return 'Đã được duyệt';
                case this.VolunteerStatus.REJECTED:
                    return 'Đăng ký lại';
                default:
                    return 'Đăng ký tình nguyện';
            }
        },

        getButtonIcon(cause) {
            if (!cause.isVolunteerRegistered) {
                return 'fa fa-hand-paper-o';
            }

            switch (cause.volunteerStatus) {
                case this.VolunteerStatus.PENDING:
                    return 'fa fa-info-circle';
                case this.VolunteerStatus.APPROVED:
                    return 'fa fa-check-circle';
                case this.VolunteerStatus.REJECTED:
                    return 'fa fa-refresh';
                default:
                    return 'fa fa-hand-paper-o';
            }
        },

        getProcessingText(cause) {
            if (!cause.isVolunteerRegistered || cause.volunteerStatus === this.VolunteerStatus.REJECTED) {
                return 'Đang đăng ký...';
            }
            return 'Đang xử lý...';
        },

        getStatusBadgeClass(status) {
            switch (status) {
                case this.VolunteerStatus.PENDING:
                    return 'badge-pending';
                case this.VolunteerStatus.APPROVED:
                    return 'badge-approved';
                case this.VolunteerStatus.REJECTED:
                    return 'badge-rejected';
                default:
                    return 'badge-pending';
            }
        },

        getStatusText(status) {
            return VolunteerStatusLabel[status] || 'Không xác định';
        },

        getStatusIcon(status) {
            switch (status) {
                case this.VolunteerStatus.PENDING:
                    return 'fa fa-clock-o';
                case this.VolunteerStatus.APPROVED:
                    return 'fa fa-check-circle';
                case this.VolunteerStatus.REJECTED:
                    return 'fa fa-times-circle';
                default:
                    return 'fa fa-question-circle';
            }
        },

        formatDate(dateString) {
            try {
                const date = new Date(dateString);
                return date.toLocaleDateString('vi-VN', {
                    year: 'numeric',
                    month: '2-digit',
                    day: '2-digit',
                    hour: '2-digit',
                    minute: '2-digit'
                });
            } catch (error) {
                return 'Không xác định';
            }
        },

        handleImageError(event) {
            event.target.src = '/images/default-cause.jpg';
        }
    }
}
</script>

<style scoped>
@import '@/assets/scss/component/_volunteer-form.scss';
</style>
