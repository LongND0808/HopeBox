<template>
    <section class="causes-area causes-default-area">
        <div class="container">
            <div class="row mb-4 filter-controls">
                <div class="col-md-4 mb-2">
                    <input v-model="searchName" type="text" class="form-control" placeholder="Tìm kiếm theo tên">
                </div>
                <div class="col-md-4 mb-2">
                    <select v-model="searchType" class="form-select" @change="handleCauseTypeChange">
                        <option value="">Tất cả chiến dịch</option>
                        <option v-for="(value, key) in CausesType" :key="key" :value="value">
                            {{ CausesTypeLabel[value] }}
                        </option>
                    </select>
                </div>
                <div class="col-md-2 mb-2">
                    <select v-model="pageSize" class="form-select" @change="handlePageSizeChange">
                        <option v-for="size in [3,6,9,12,15]" :key="size" :value="size">{{ size }} mục/trang</option>
                    </select>
                </div>
                <div class="col-md-2 mb-2 d-flex align-items-center">
                    <button class="btn btn-search w-100" @click="handleSearch">Tìm kiếm</button>
                </div>

            </div>


            <div class="row mtn-30">
                <div class="col-md-6 col-lg-4 mt-30" v-for="(causes, index) in causesData" :key="index">
                    <div class="causes-item">
                        <div class="thumb">
                            <img :src="causes.imgSrc" :alt="causes.title">
                        </div>
                        <div class="content">
                            <ul class="donate-info">
                                <li class="info-item" v-for="(info, idx) in causes.infoList" :key="idx">
                                    <span class="info-title">{{ info.infoTitle }}:</span>
                                    <span class="amount">{{ info.amount }}</span>
                                </li>
                            </ul>
                            <h4 class="title">
                                <nuxt-link v-if="causes && causes.id" :to="'/causes-details?id=' + causes.id">
                                    {{ causes.title }}
                                </nuxt-link>
                            </h4>
                            <p>{{ causes.desc }}</p>
                        </div>
                        <div class="causes-footer">
                            <div class="admin">
                                <h5>
                                    <nuxt-link to="/causes">
                                        <span class="icon-img">
                                            <img src="/images/icons/admin1.png" alt="Icon">
                                        </span> {{ causes.name }}
                                    </nuxt-link>
                                </h5>
                            </div>
                            <nuxt-link :to="{
                                            path: '/donation',
                                            query: { causeId: causes.id }
                                        }" class="btn-theme btn-border-gradient gray-border btn-size-md">
                                <span>
                                    Quyên góp
                                    <img class="icon icon-img" src="/images/icons/arrow-line-right-gradient.png"
                                        alt="Icon">
                                </span>
                            </nuxt-link>
                        </div>
                    </div>
                </div>
            </div>

            <div class="pagination-container">
                <button class="pagination-button" :disabled="currentPage === 1"
                    @click="goToPage(currentPage - 1)">←</button>

                <button v-for="page in totalPages" :key="page" @click="goToPage(page)" class="pagination-button"
                    :class="{ active: page === currentPage  }">
                    {{ page }}
                </button>

                <button class="pagination-button" :disabled="currentPage === totalPages"
                    @click="goToPage(currentPage + 1)">→</button>
            </div>

        </div>
    </section>
</template>

<script>
    import axios from 'axios';
    import { CausesType, CausesTypeLabel } from '@/enums/enums.js';
    import { BASE_URL } from '@/utils/constants';

    export default {
        props: {
            queryType: {
                type: Number,
                default: 0
            }
        },
        data() {
            return {
                causesData: [],
                searchName: '',
                searchType: '',
                pageSize: 6,
                currentPage: 1,
                totalPages: 1,
                CausesTypeLabel,
                CausesType
            };
        },
        methods: {
            async fetchCauses() {
                try {
                    const response = await axios.post(`${BASE_URL}/api/Cause/get-cause-by-filter`, {
                        name: this.searchName || null,
                        causeType: this.searchType || null,
                        pageIndex: this.currentPage,
                        pageSize: this.pageSize
                    });

                    const { responseData } = response.data;
                    this.totalPages = Math.ceil(responseData.totalRecords / this.pageSize);

                    const causesWithInfo = await Promise.all(responseData.pagedData.map(async (item) => {
                        let creatorName = 'Ẩn danh';
                        try {
                            const userRes = await axios.get(`${BASE_URL}/api/User/get-by-id?id=${item.createdBy}`);
                            creatorName = userRes.data.responseData.fullName;
                        } catch (err) {
                            console.warn('Không lấy được tên người tạo:', err);
                        }

                        return {
                            imgSrc: item.heroImage || 'default.jpg',
                            id: item.id,
                            title: item.title,
                            desc: item.description,
                            name: creatorName,
                            infoList: [
                                {
                                    infoTitle: "Mục tiêu",
                                    amount: `${(item.targetAmount / 1_000_000).toFixed(1)}tr ₫`
                                },
                                {
                                    infoTitle: "Đóng góp",
                                    amount: `${(item.currentAmount / 1_000_000).toFixed(1)}tr ₫`
                                },
                                {
                                    infoTitle: "Còn thiếu",
                                    amount: `${(Math.max(item.targetAmount - item.currentAmount, 0) / 1_000_000).toFixed(1)}tr ₫`
                                }
                            ]
                        };
                    }));

                    this.causesData = causesWithInfo;

                } catch (error) {
                    console.error('Lỗi khi tìm kiếm Causes:', error);
                }
            },
            handleSearch() {
                this.currentPage = 1;
                this.fetchCauses();
            },
            handlePageSizeChange() {
                this.currentPage = 1;
                this.fetchCauses();
            },
            handleCauseTypeChange() {
                this.currentPage = 1;
                this.fetchCauses();
            },
            goToPage(page) {
                if (page >= 1 && page <= this.totalPages) {
                    this.currentPage = page;
                    this.fetchCauses();
                }
            }
        },
        mounted() {
            if (this.queryType) {
                this.searchType = this.queryType;
            }
            this.fetchCauses();
        }
    };
</script>