<template>
    <div class="donners-info">
        <h3 class="donners-title">Nhà hảo tâm</h3>
        <div class="donners-list-wrapper">
            <div v-if="donnerList.length">
                <div class="donner-item" v-for="donner in sortedDonnerList" :key="donner.id">
                    <img :src="donner.avatarUrl" :alt="donner.fullName" class="donner-avatar" />
                    <div class="donner-info">
                        <div class="donner-name">{{ donner.fullName }}</div>
                        <div class="donner-message">{{ donner.message || 'Cảm ơn vì đã ủng hộ!' }}</div>
                    </div>
                    <div class="donner-amount">
                        {{ (donner.donationAmount) }}<span class="amount-unit"> ₫</span>
                    </div>
                </div>
            </div>
            <div v-else class="no-donners">Chưa có nhà hảo tâm nào.</div>
        </div>
    </div>
</template>

<script>
    import axios from 'axios';
    import { BASE_URL } from '@/utils/constants';

    export default {
        props: {
            causeId: {
                type: String,
                required: true
            }
        },
        data() {
            return {
                donnerList: [],
                defaultAvatar: '/images/photos/default-avatar.png'
            };
        },
        computed: {
            sortedDonnerList() {
                return [...this.donnerList].sort((a, b) => b.donationAmount - a.donationAmount);
            }
        },
        mounted() {
            this.fetchDonations();
        },
        methods: {
            async fetchDonations() {
                try {
                    const donationResponse = await axios.get(`${BASE_URL}/api/Donation/get-all`);
                    const donations = donationResponse.data.responseData.filter(
                        donation => donation.causeId === this.causeId && donation.status === 1
                    );

                    const userIds = [...new Set(donations.map(donation => donation.userId))];
                    const userPromises = userIds.map(userId =>
                        axios.get(`${BASE_URL}/api/User/get-by-id?id=${userId}`)
                    );
                    const userResponses = await Promise.all(userPromises);
                    const users = userResponses.reduce((acc, response) => {
                        const user = response.data.responseData;
                        acc[user.id] = user;
                        return acc;
                    }, {});

                    this.donnerList = donations.map(donation => {
                        const user = users[donation.userId] || {};
                        return {
                            id: donation.id,
                            avatarUrl: donation.isAnonymous ? this.defaultAvatar : (user.avatarUrl || this.defaultAvatar),
                            fullName: donation.isAnonymous ? 'Nhà hảo tâm' : (user.fullName || 'Unknown'),
                            donationAmount: donation.donationAmount || donation.amount || 0,
                            message: donation.message || ''
                        };
                    });
                } catch (err) {
                    console.error('Error fetching donations:', err);
                }
            }
        }
    };
</script>

<style scoped>
    .donners-info {
        margin-top: 2rem;
    }

    .donners-title {
        font-size: 24px;
        font-weight: bold;
        margin-bottom: 1rem;
        color: #222;
    }

    .donners-list-wrapper {
        max-height: 300px;
        overflow-y: auto;
        padding: 20px;
        border: 2px solid black;
        border-radius: 5px;
    }

    .donner-item {
        display: flex;
        align-items: center;
        background-color: #fff;
        border-radius: 12px;
        padding: 0.75rem 1rem;
        margin-bottom: 0.75rem;
        box-shadow: 0 2px 6px rgba(0, 0, 0, 0.05);
        transition: all 0.2s ease;
    }

    .donner-item:hover {
        transform: translateY(-2px);
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
    }

    .donner-avatar {
        width: 48px;
        height: 48px;
        border-radius: 50%;
        object-fit: cover;
        margin-right: 1rem;
    }

    .donner-info {
        flex-grow: 1;
    }

    .donner-name {
        font-weight: 600;
        font-size: 1rem;
        color: #333;
    }

    .donner-message {
        font-size: 0.9rem;
        color: #777;
    }

    .donner-amount {
        font-weight: bold;
        font-size: 1rem;
        color: #2e7d32;
        white-space: nowrap;
    }

    .amount-unit {
        font-weight: normal;
        margin-left: 2px;
    }

    .no-donners {
        text-align: center;
        color: #999;
        font-size: 0.95rem;
        padding: 1rem 0;
    }

    .donners-list-wrapper::-webkit-scrollbar {
        width: 8px;
    }

    .donners-list-wrapper::-webkit-scrollbar-track {
        background: #e2e2e2;
        border-radius: 20px;
    }

    .donners-list-wrapper::-webkit-scrollbar-thumb {
        background: #515165;
        border-radius: 4px;
    }

    .donners-list-wrapper::-webkit-scrollbar-thumb:hover {
        background: #5a5a7a;
    }
</style>