<template>
    <div class="volunteer-management p-6">
        <div class="header flex justify-between items-center mb-6 flex-wrap gap-4">
            <h2 class="text-3xl font-bold text-yellow-400">Quản lý đơn tình nguyện</h2>
        </div>

        <div class="table-container overflow-x-auto max-w-full mx-auto">
            <table class="w-full border-collapse bg-gray-800 rounded-lg shadow-lg">
                <thead>
                    <tr class="bg-gray-700 text-yellow-400 text-sm font-semibold">
                        <th class="p-4 text-left">Avatar</th>
                        <th class="p-4 text-left">Họ tên</th>
                        <th class="p-4 text-left">Email</th>
                        <th class="p-4 text-left">SĐT</th>
                        <th class="p-4 text-left">Chiến dịch</th>
                        <th class="p-4 text-left">Ngày đăng ký</th>
                        <th class="p-4 text-left">Trạng thái</th>
                        <th class="p-4 text-left">Hành động</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="volunteer in volunteers" :key="volunteer.volunteer.id"
                        class="hover:bg-gray-700 transition-colors">
                        <td data-label="Avatar">
                            <img :src="volunteer.user.avatarUrl || '/images/placeholder.jpg'" class="avatar"
                                alt="Avatar" />
                        </td>
                        <td data-label="Họ tên" class="p-4">{{ volunteer.user.fullName || 'N/A' }}</td>
                        <td data-label="Email" class="p-4">{{ volunteer.user.email || 'N/A' }}</td>
                        <td data-label="SĐT" class="p-4">{{ volunteer.user.phoneNumber || 'N/A' }}</td>
                        <td data-label="Chiến dịch" class="p-4">{{ volunteer.cause.title || 'N/A' }}</td>
                        <td data-label="Ngày đăng ký" class="p-4">{{ formatDate(volunteer.volunteer.joinDate) }}</td>
                        <td data-label="Trạng thái" class="p-4">
                            <span :class="getStatusClass(volunteer.volunteer.status)">
                                {{ getStatusLabel(volunteer.volunteer.status) }}
                            </span>
                        </td>
                        <td data-label="Hành động" class="p-4 flex gap-2 flex-wrap">
                            <button v-if="volunteer.volunteer.status === VolunteerStatus.PENDING"
                                class="btn approve bg-green-600 hover:bg-green-700 text-white px-3 py-1 rounded text-sm"
                                @click="approveVolunteer(volunteer.volunteer.id, volunteer.cause.id)">
                                <i class="fas fa-check mr-1"></i> Phê duyệt
                            </button>
                            <button v-if="volunteer.volunteer.status === VolunteerStatus.PENDING"
                                class="btn reject bg-red-600 hover:bg-red-700 text-white px-3 py-1 rounded text-sm"
                                @click="rejectVolunteer(volunteer.volunteer.id, volunteer.cause.id)">
                                <i class="fas fa-times mr-1"></i> Từ chối
                            </button>
                            <button v-if="volunteer.volunteer.status === VolunteerStatus.APPROVED"
                                class="btn view bg-blue-600 hover:bg-blue-700 text-white px-3 py-1 rounded text-sm"
                                @click="viewDetails(volunteer.volunteer.id)">
                                Đã duyệt
                            </button>
                            <button v-if="volunteer.volunteer.status === VolunteerStatus.REJECTED"
                                class="btn view bg-blue-600 hover:bg-blue-700 text-white px-3 py-1 rounded text-sm"
                                @click="viewDetails(volunteer.volunteer.id)">
                                Từ chối
                            </button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <!-- Modal for Volunteer Details -->
        <div v-if="showDetailsModal"
            class="modal-overlay fixed inset-0 bg-black bg-opacity-70 flex items-center justify-center z-50">
            <div class="modal bg-gray-800 p-8 rounded-xl w-11/12 max-w-3xl max-h-[90vh] overflow-y-auto">
                <h3 class="text-2xl font-bold text-yellow-400 mb-6">Chi tiết đơn tình nguyện</h3>
                <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                    <!-- Volunteer and User Information -->
                    <div class="space-y-4">
                        <h4 class="text-lg font-semibold text-white">Thông tin tình nguyện viên</h4>
                        <div class="form-group flex flex-col">
                            <label class="text-gray-300 font-medium">Avatar</label>
                            <img :src="selectedVolunteer.user.avatarUrl || '/images/placeholder.jpg'"
                                class="w-20 h-20 rounded-full object-cover border-2 border-yellow-400 mt-2"
                                alt="Avatar" />
                        </div>
                        <div class="form-group flex flex-col">
                            <label class="text-gray-300 font-medium">Họ tên</label>
                            <p class="bg-gray-700 p-3 rounded text-white">{{ selectedVolunteer.user.fullName || 'N/A' }}
                            </p>
                        </div>
                        <div class="form-group flex flex-col">
                            <label class="text-gray-300 font-medium">Email</label>
                            <p class="bg-gray-700 p-3 rounded text-white">{{ selectedVolunteer.user.email || 'N/A' }}
                            </p>
                        </div>
                        <div class="form-group flex flex-col">
                            <label class="text-gray-300 font-medium">Số điện thoại</label>
                            <p class="bg-gray-700 p-3 rounded text-white">{{ selectedVolunteer.user.phoneNumber || 'N/A'
                                }}</p>
                        </div>
                        <div class="form-group flex flex-col">
                            <label class="text-gray-300 font-medium">Ngày sinh</label>
                            <p class="bg-gray-700 p-3 rounded text-white">{{
                                formatDate(selectedVolunteer.user.dateOfBirth) || 'N/A' }}</p>
                        </div>
                        <div class="form-group flex flex-col">
                            <label class="text-gray-300 font-medium">Địa chỉ</label>
                            <p class="bg-gray-700 p-3 rounded text-white">{{ selectedVolunteer.user.address || 'N/A' }}
                            </p>
                        </div>
                        <div class="form-group flex flex-col">
                            <label class="text-gray-300 font-medium">Tích điểm</label>
                            <p class="bg-gray-700 p-3 rounded text-white">{{ selectedVolunteer.user.point || 0 }}</p>
                        </div>
                        <div class="form-group flex flex-col">
                            <label class="text-gray-300 font-medium">Trạng thái đơn</label>
                            <p class="bg-gray-700 p-3 rounded"
                                :class="getStatusClass(selectedVolunteer.volunteer.status)">
                                {{ getStatusLabel(selectedVolunteer.volunteer.status) }}
                            </p>
                        </div>
                        <div class="form-group flex flex-col">
                            <label class="text-gray-300 font-medium">Ngày đăng ký</label>
                            <p class="bg-gray-700 p-3 rounded text-white">{{
                                formatDate(selectedVolunteer.volunteer.joinDate) || 'N/A' }}</p>
                        </div>
                    </div>
                    <!-- Campaign Information -->
                    <div class="space-y-4">
                        <h4 class="text-lg font-semibold text-white">Thông tin chiến dịch</h4>
                        <div class="form-group flex flex-col">
                            <label class="text-gray-300 font-medium">Tiêu đề</label>
                            <p class="bg-gray-700 p-3 rounded text-white">{{ selectedVolunteer.cause.title || 'N/A' }}
                            </p>
                        </div>
                        <div class="form-group flex flex-col">
                            <label class="text-gray-300 font-medium">Mô tả</label>
                            <p class="bg-gray-700 p-3 rounded text-white">{{ selectedVolunteer.cause.description ||
                                'N/A' }}</p>
                        </div>
                        <div class="form-group flex flex-col">
                            <label class="text-gray-300 font-medium">Chi tiết</label>
                            <p class="bg-gray-700 p-3 rounded text-white">{{ selectedVolunteer.cause.detail || 'N/A' }}
                            </p>
                        </div>
                        <div class="form-group flex flex-col">
                            <label class="text-gray-300 font-medium">Ngày bắt đầu</label>
                            <p class="bg-gray-700 p-3 rounded text-white">{{
                                formatDate(selectedVolunteer.cause.startDate) || 'N/A' }}</p>
                        </div>
                        <div class="form-group flex flex-col">
                            <label class="text-gray-300 font-medium">Ngày kết thúc</label>
                            <p class="bg-gray-700 p-3 rounded text-white">{{ formatDate(selectedVolunteer.cause.endDate)
                                || 'N/A' }}</p>
                        </div>
                        <div class="form-group flex flex-col">
                            <label class="text-gray-300 font-medium">Tiến độ quyên góp</label>
                            <div class="bg-gray-700 p-3 rounded">
                                <div class="w-full bg-gray-600 rounded-full h-4 overflow-hidden">
                                    <div class="bg-yellow-400 h-full"
                                        :style="{ width: `${calculateProgress(selectedVolunteer.cause.currentAmount, selectedVolunteer.cause.targetAmount)}%` }">
                                    </div>
                                </div>
                                <p class="text-white mt-2 text-sm">
                                    {{ formatCurrency(selectedVolunteer.cause.currentAmount) }} / {{
                                    formatCurrency(selectedVolunteer.cause.targetAmount) }}
                                </p>
                            </div>
                        </div>
                        <div class="form-group flex flex-col">
                            <label class="text-gray-300 font-medium">Hình ảnh chính</label>
                            <img :src="selectedVolunteer.cause.heroImage || '/images/placeholder.jpg'"
                                class="w-full h-40 object-cover rounded mt-2" alt="Cause Hero Image" />
                        </div>
                    </div>
                </div>
                <div class="modal-actions flex justify-end gap-4 mt-8">
                    <button class="btn btn-secondary bg-gray-600 hover:bg-gray-700 text-white px-6 py-2 rounded"
                        @click="closeDetailsModal">Đóng</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import axios from 'axios';
    import { showSuccessAlertDark, showErrorAlertDark, showConfirmDialogDark } from '@/utils/alertHelper';
    import { VolunteerStatus, VolunteerStatusLabel } from '@/enums/enums';
    import { BASE_URL } from '@/utils/constants';

    export default {
        data() {
            return {
                volunteers: [],
                showDetailsModal: false,
                selectedVolunteer: null,
                VolunteerStatus,
                VolunteerStatusLabel
            };
        },
        methods: {
            async fetchVolunteers() {
                try {
                    const res = await axios.get(`${BASE_URL}/api/Volunteer/get-all`, {
                        withCredentials: true
                    });
                    if (res.data.status === 200) {
                        this.volunteers = await Promise.all(
                            res.data.responseData.map(async (volunteer) => {
                                const detailRes = await axios.get(`${BASE_URL}/api/Volunteer/get-volunteer-details?volunteerId=${volunteer.id}`, {
                                    withCredentials: true
                                });
                                return detailRes.data.status === 200 ? detailRes.data.responseData : null;
                            })
                        ).then(results => results.filter(v => v !== null));
                    } else {
                        await showErrorAlertDark('Lỗi', res.data.message);
                    }
                } catch (err) {
                    console.error('Lỗi tải danh sách tình nguyện viên:', err);
                    await showErrorAlertDark('Lỗi', 'Không thể tải danh sách tình nguyện viên.');
                }
            },
            async approveVolunteer(volunteerId, causeId) {
                const result = await showConfirmDialogDark(
                    'Xác nhận phê duyệt',
                    'Bạn có chắc chắn muốn phê duyệt đơn tình nguyện này?',
                    'Phê duyệt',
                    'Hủy'
                );
                if (result.isConfirmed) {
                    try {
                        const res = await axios.post(
                            `${BASE_URL}/api/Volunteer/approve-volunteer`,
                            { volunteerId, causeId },
                            { withCredentials: true }
                        );
                        if (res.data.status === 200) {
                            await this.fetchVolunteers();
                            await showSuccessAlertDark('Thành công', 'Đơn tình nguyện đã được phê duyệt.');
                        } else {
                            await showErrorAlertDark('Lỗi', res.data.message);
                        }
                    } catch (err) {
                        await showErrorAlertDark('Lỗi', err.message);
                    }
                }
            },
            async rejectVolunteer(volunteerId, causeId) {
                const result = await showConfirmDialogDark(
                    'Xác nhận từ chối',
                    'Bạn có chắc chắn muốn từ chối đơn tình nguyện này?',
                    'Từ chối',
                    'Hủy'
                );
                if (result.isConfirmed) {
                    try {
                        const res = await axios.post(
                            `${BASE_URL}/api/Volunteer/reject-volunteer`,
                            { volunteerId, causeId },
                            { withCredentials: true }
                        );
                        if (res.data.status === 200) {
                            await this.fetchVolunteers();
                            await showSuccessAlertDark('Thành công', 'Đơn tình nguyện đã bị từ chối.');
                        } else {
                            await showErrorAlertDark('Lỗi', res.data.message);
                        }
                    } catch (err) {
                        await showErrorAlertDark('Lỗi', err.message);
                    }
                }
            },
            async viewDetails(volunteerId) {
                try {
                    const res = await axios.get(`${BASE_URL}/api/Volunteer/get-volunteer-details?volunteerId=${volunteerId}`, {
                        withCredentials: true
                    });
                    if (res.data.status === 200) {
                        this.selectedVolunteer = res.data.responseData;
                        this.showDetailsModal = true;
                    } else {
                        await showErrorAlertDark('Lỗi', res.data.message);
                    }
                } catch (err) {
                    await showErrorAlertDark('Lỗi', 'Không thể tải chi tiết tình nguyện viên.');
                }
            },
            getStatusLabel(status) {
                return VolunteerStatusLabel[status] || 'Không xác định';
            },
            getStatusClass(status) {
                switch (status) {
                    case VolunteerStatus.PENDING:
                        return 'text-yellow-500';
                    case VolunteerStatus.APPROVED:
                        return 'text-green-500';
                    case VolunteerStatus.REJECTED:
                        return 'text-red-500';
                    default:
                        return 'text-gray-500';
                }
            },
            formatDate(date) {
                if (!date) return 'N/A';
                return new Date(date).toLocaleDateString('vi-VN', {
                    year: 'numeric',
                    month: '2-digit',
                    day: '2-digit'
                });
            },
            formatCurrency(amount) {
                if (!amount) return '0 VNĐ';
                return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(amount);
            },
            calculateProgress(current, target) {
                if (!current || !target) return 0;
                return Math.min((current / target) * 100, 100);
            },
            closeDetailsModal() {
                this.showDetailsModal = false;
                this.selectedVolunteer = null;
            }
        },
        async mounted() {
            await this.fetchVolunteers();
        }
    };
</script>

<style scoped>
    th,
    td {
        padding: 14px;
        text-align: left;
        border-bottom: 1px solid rgba(255, 255, 255, 0.1);
    }

    .avatar {
        width: 40px;
        height: 40px;
        border-radius: 50%;
        object-fit: cover;
    }

    .table-container::-webkit-scrollbar {
        width: 8px;
    }

    .table-container::-webkit-scrollbar-track {
        background: #1e1e2f;
    }

    .table-container::-webkit-scrollbar-thumb {
        background: #4a4a6a;
        border-radius: 4px;
    }

    .table-container::-webkit-scrollbar-thumb:hover {
        background: #5a5a7a;
    }

    .modal::-webkit-scrollbar {
        width: 8px;
    }

    .modal::-webkit-scrollbar-track {
        background: #1e1e2f;
    }

    .modal::-webkit-scrollbar-thumb {
        background: #4a4a6a;
        border-radius: 4px;
    }

    .modal::-webkit-scrollbar-thumb:hover {
        background: #5a5a7a;
    }

    /* Responsive table for mobile */
    @media (max-width: 768px) {
        .table-container table {
            font-size: 0.9rem;
        }

        .table-container thead {
            display: none;
        }

        .table-container tbody,
        .table-container tr,
        .table-container td {
            display: block;
            width: 100%;
        }

        .table-container tr {
            margin-bottom: 15px;
            border-bottom: 1px solid rgba(255, 255, 255, 0.1);
        }

        .table-container td {
            display: flex;
            align-items: center;
            justify-content: flex-start;
            padding: 10px;
            position: relative;
            text-align: left;
        }

        .table-container td::before {
            content: attr(data-label);
            font-weight: bold;
            color: #f6d579;
            width: 40%;
            flex-shrink: 0;
            text-align: left;
        }

        .table-container td:not(:first-child) {
            border-top: none;
        }

        .table-container td:last-child {
            display: flex;
            justify-content: flex-start;
            gap: 8px;
            flex-wrap: wrap;
        }
    }

    /* Responsive modal for mobile */
    @media (max-width: 768px) {
        .modal {
            padding: 4rem;
            width: 95%;
            max-height: 95vh;
        }

        .modal h3 {
            font-size: 1.5rem;
        }

        .modal .form-group img {
            width: 60px;
            height: 60px;
        }

        .modal-actions {
            flex-direction: column;
            gap: 2rem;
        }

        .modal-actions button {
            width: 100%;
            text-align: center;
        }
    }

    @media (max-width: 480px) {
        .volunteer-management {
            padding: 2rem;
        }

        .header h2 {
            font-size: 1.5rem;
        }

        .table-container td::before {
            width: 50%;
        }

        .modal {
            padding: 3rem;
        }
    }
</style>