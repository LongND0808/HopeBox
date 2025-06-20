<template>
    <section class="events-area events-default-area">
        <div class="container">
            <div class="row">
                <div class="col-lg-12">
                    <div class="events-form">
                        <div class="section-title">
                            <h5 class="subtitle">Sự kiện mới nhất</h5>
                            <h2 class="title">Tham gia các <span>sự kiện gây quỹ</span> mới cùng HopeBox</h2>
                            <p>Khám phá và tham gia các sự kiện từ thiện ý nghĩa. Hãy quyên góp để tạo ra những thay đổi
                                tích cực trong cộng đồng.</p>
                        </div>
                    </div>

                    <div class="row mb-4 filter-controls">
                        <div class="col-md-4 mb-2">
                            <input v-model="searchTitle" type="text" class="form-control"
                                placeholder="Tìm kiếm theo tên sự kiện...">
                        </div>
                        <div class="col-md-4 mb-2">
                            <input v-model="searchCauseTitle" type="text" class="form-control"
                                placeholder="Tìm kiếm theo tên chiến dịch...">
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
                            <p>Đang tải danh sách sự kiện...</p>
                        </div>
                    </div>

                    <div class="events-list-section" v-else-if="eventsData && eventsData.length > 0">
                        <div class="row">
                            <div class="col-12 col-md-6 col-lg-4 mb-4" v-for="(event, index) in eventsData"
                                :key="event.id || index">
                                <div class="event-item">
                                    <div class="thumb">
                                        <img :src="event.bannerImage || '/images/default-event.jpg'" :alt="event.title"
                                            @error="handleImageError">
                                    </div>
                                    <div class="content">
                                        <h4 class="title">
                                            <nuxt-link :to="`/event-details?id=${event.id}`">
                                                {{ event.title }}
                                            </nuxt-link>
                                        </h4>
                                        <p class="description">{{ event.description }}</p>
                                        <div class="event-time">
                                            <p><strong>Thời gian:</strong> {{ formatEventDate(event.startDate,
                                                event.endDate) }}</p>
                                            <p v-if="event.location"><strong>Địa điểm:</strong> {{ event.location }}</p>
                                        </div>
                                        <div class="event-cause" v-if="event.causeTitle">
                                            <p><strong>Chiến dịch:</strong> {{ event.causeTitle }}</p>
                                        </div>
                                    </div>

                                    <div class="events-footer">
                                        <div class="admin">
                                            <h5>
                                                <span class="icon-img">
                                                    <img src="/images/icons/admin1.png" alt="Icon">
                                                </span>
                                                <div class="admin-info">
                                                    <span class="admin-name">{{ event.createdByName || 'HopeBox'
                                                        }}</span>
                                                    <span class="organization-name" v-if="event.organizationName">{{
                                                        event.organizationName }}</span>
                                                </div>
                                            </h5>
                                        </div>
                                        <nuxt-link :to="{
                                            path: '/event-donation-detail',
                                            query: { eventId: event.id }
                                        }" class="btn-theme btn-border-gradient gray-border btn-size-md">
                                            <span>
                                                Quyên góp
                                                <img class="icon icon-img"
                                                    src="/images/icons/arrow-line-right-gradient.png" alt="Icon">
                                            </span>
                                        </nuxt-link>
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

                    <div class="no-events-message" v-else-if="!loading">
                        <p>
                            <i class="fa fa-info-circle"></i>
                            Hiện tại chưa có sự kiện nào.
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </section>
</template>

<script>
import axios from 'axios';
import { showErrorAlert } from '@/utils/alertHelper';

export default {
    name: 'EventDetail',
    data() {
        return {
            eventsData: [],
            loading: false,
            searchTitle: '',
            searchCauseTitle: '',
            pageSize: 6,
            currentPage: 1,
            totalPages: 0,
            totalRecords: 0,
            pageSizeOptions: [3, 6, 9, 12, 15]
        }
    },
    computed: {
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
        this.fetchEventsData();
    },
    methods: {
        async fetchEventsData() {
            try {
                this.loading = true;

                const requestData = {
                    title: this.searchTitle || null,
                    causeTitle: this.searchCauseTitle || null,
                    pageIndex: this.currentPage,
                    pageSize: this.pageSize
                };

                const response = await axios.get(
                    'https://localhost:7213/api/Event/get-events-donation-detail-by-filter',
                    {
                        params: requestData,
                        headers: {
                            'Content-Type': 'application/json'
                        }
                    }
                );

                if (response.data.status === 200) {
                    const responseData = response.data.responseData;
                    this.eventsData = responseData.pagedData || [];
                    this.totalRecords = responseData.totalRecords;
                    this.totalPages = responseData.totalPages;
                } else {
                    await showErrorAlert('Lỗi tải dữ liệu', response.data.message || 'Không thể tải danh sách sự kiện');
                    this.eventsData = [];
                }
            } catch (error) {
                console.error('Lỗi khi lấy dữ liệu events:', error);
                await showErrorAlert(
                    'Lỗi kết nối',
                    'Không thể kết nối đến server. Vui lòng kiểm tra kết nối mạng và thử lại.'
                );
                this.eventsData = [];
            } finally {
                this.loading = false;
            }
        },

        async handleSearch() {
            this.currentPage = 1;
            await this.fetchEventsData();
        },

        async handlePageSizeChange() {
            this.currentPage = 1;
            await this.fetchEventsData();
        },

        async goToPage(page) {
            if (page >= 1 && page <= this.totalPages && page !== this.currentPage) {
                this.currentPage = page;
                await this.fetchEventsData();
            }
        },

        getRemainingAmount(event) {
            return Math.max(event.targetAmount - event.currentAmount, 0);
        },

        formatEventDate(startDate, endDate) {
            try {
                const start = new Date(startDate);
                const end = new Date(endDate);

                const startFormatted = this.formatSingleDate(start);

                if (this.isSameDate(start, end)) {
                    return startFormatted;
                }

                const endFormatted = this.formatSingleDate(end);
                return `${startFormatted} - ${endFormatted}`;
            } catch (error) {
                console.error('Error formatting date:', error);
                return 'Ngày không xác định';
            }
        },

        formatSingleDate(date) {
            const day = date.getDate();
            const month = date.getMonth() + 1;
            const year = date.getFullYear();

            return `${day}/${month}/${year}`;
        },

        isSameDate(date1, date2) {
            return date1.getDate() === date2.getDate() &&
                date1.getMonth() === date2.getMonth() &&
                date1.getFullYear() === date2.getFullYear();
        },

        handleImageError(event) {
            event.target.src = '/images/default-event.jpg';
        }
    }
}
</script>

<style scoped>
@import '@/assets/scss/component/_event-donation.scss';
</style>