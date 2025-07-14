<template>
    <section class="top-donors-area">
        <div class="container mx-auto px-4">
            <div class="text-center mb-12 relative">
                <h2 class="text-xl font-semibold text-orange-600 mb-2" style="color: #f97316;">Các nhà hảo tâm lớn</h2>
            </div>
            <div class="donners-list-wrapper">
                <div class="grid grid-cols-1 gap-4">
                    <div v-for="(row, rowIndex) in donorRows" :key="rowIndex"
                        class="grid grid-cols-1 md:grid-cols-5 gap-4">
                        <div v-for="(donor, index) in row" :key="donor.userId"
                            style="margin-bottom: 10px !important; margin-top: 10px !important;"
                            class="donner-item flex items-center space-x-3 p-3">
                            <div class="row">
                                <div class="col-md-2 d-flex justify-content-center">
                                    <img :src="donor.avatarUrl || 'https://pub-dc597dd9f97242ceb1fc0179075fabfa.r2.dev/avatars/008f1239-beb4-430b-a66d-a263f8c2393a.jpg'"
                                        :alt="donor.userName" class="donner-avatar">
                                </div>
                                <div class="col-md-10">
                                    <div class="donner-info flex-1">
                                        <div class="donner-name text-sm font-medium text-gray-800">{{ donor.userName }}
                                        </div>
                                        <div class="donner-message text-xs text-gray-600">Số lượt quyên góp: {{
                                            donor.donationCount }}</div>
                                    </div>
                                    <div class="donner-amount font-semibold text-green-600 text-sm">
                                        Tổng quyên góp: {{ formatAmount(donor.totalAmount) }}<span class="amount-unit">
                                            ₫</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div v-if="donors.length === 0" class="no-donners text-center text-gray-500 py-2">
                        Chưa có nhà hảo tâm nào.
                    </div>
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
                donors: [],
                defaultAvatar: 'https://pub-dc597dd9f97242ceb1fc0179075fabfa.r2.dev/avatars/008f1239-beb4-430b-a66d-a263f8c2393a.jpg'
            };
        },
        computed: {
            donorRows() {
                const rows = [];
                for (let i = 0; i < Math.ceil(this.donors.length / 5); i++) {
                    rows.push(this.donors.slice(i * 5, (i + 1) * 5));
                }
                return rows;
            }
        },
        mounted() {
            axios.get(`${BASE_URL}/api/User/top-donors`)
                .then(response => {
                    if (response.data.status === 200) {
                        this.donors = response.data.responseData;
                    }
                })
                .catch(error => {
                    console.error('Lỗi khi gọi API Top Donors:', error);
                });
        },
        methods: {
            formatAmount(amount) {
                if (amount < 1_000_000) {
                    return Math.round(amount / 1000) + 'k';
                } else {
                    return (amount / 1_000_000).toFixed(1).replace(/\.0$/, '') + 'tr';
                }
            }
        }
    };
</script>

<style scoped>
    .top-donors-area {
        padding: 4rem 0;
        background: linear-gradient(to bottom, #ffffff, #f9fafb);
        position: relative;
        overflow: hidden;
    }

    .top-donors-area::before {
        content: '';
        position: absolute;
        top: 10%;
        left: 10%;
        width: 250px;
        height: 250px;
        background: radial-gradient(circle, rgba(255, 165, 0, 0.1) 0%, rgba(255, 255, 255, 0) 70%);
        border-radius: 50%;
        z-index: 0;
    }

    .donners-list-wrapper {
        max-height: 800px;
        overflow-y: auto;
        padding: 1rem;
        border: 2px solid #e5e7eb;
        border-radius: 0.5rem;
        background-color: #ffffff;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
    }

    .donners-list-wrapper::-webkit-scrollbar {
        width: 6px;
    }

    .donners-list-wrapper::-webkit-scrollbar-track {
        background: #e2e2e2;
        border-radius: 10px;
    }

    .donners-list-wrapper::-webkit-scrollbar-thumb {
        background: #515165;
        border-radius: 4px;
    }

    .donners-list-wrapper::-webkit-scrollbar-thumb:hover {
        background: #5a5a7a;
    }

    .donner-item {
        border-left: 3px solid #f97316;
        box-shadow: 0 1px 4px rgba(0, 0, 0, 0.05);
        border-radius: 0.375rem;
        transition: all 0.2s ease;
    }

    .donner-item:hover {
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
        transform: translateY(-2px);
    }

    .donner-avatar {
        width: 100px;
        height: 100px;
        border-radius: 50%;
        object-fit: cover;
        border: 2px solid #fee2b3;
    }

    .donner-name {
        font-size: 0.875rem;
        font-weight: 500;
        color: #1f2937;
    }

    .donner-message {
        font-size: 0.75rem;
        color: #6b7280;
    }

    .donner-amount {
        font-size: 0.875rem;
        color: #2e7d32;
    }

    .amount-unit {
        font-weight: normal;
        margin-left: 2px;
    }

    .no-donners {
        font-size: 0.875rem;
        color: #9ca3af;
        padding: 0.5rem 0;
    }

    @media (min-width: 768px) {
        .donners-list-wrapper .grid-cols-1 {
            grid-template-columns: repeat(5, minmax(0, 1fr));
        }
    }
</style>