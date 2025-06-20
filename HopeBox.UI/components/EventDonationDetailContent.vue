<template>
    <section class="event-donation-detail-area">
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12">
                    <div class="event-donation-column">
                        <div class="event-detail-form">
                            <div class="event-detail-wrapper">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div class="section-title">
                                            <h5 class="subtitle line-theme-color">Chi Tiết Sự Kiện</h5>
                                            <h2 class="title">{{ eventData?.title || 'Đang tải...' }}</h2>
                                            <p>{{ eventData?.description || 'Mô tả sự kiện đang được tải...' }}</p>
                                        </div>
                                    </div>
                                </div>

                                <div class="row mb-4" v-if="eventData?.bannerImage">
                                    <div class="col-lg-12">
                                        <div class="event-banner">
                                            <img :src="eventData.bannerImage" :alt="eventData.title"
                                                class="img-fluid rounded" @error="handleImageError">
                                        </div>
                                    </div>
                                </div>
                                <div class="row" v-if="eventData">
                                    <div class="col-lg-6">
                                        <div class="event-info-section">
                                            <h4>Thông Tin Sự Kiện</h4>
                                            <div class="info-item">
                                                <strong>Thời gian:</strong>
                                                {{ formatEventDate(eventData.startDate, eventData.endDate) }}
                                            </div>
                                            <div class="info-item">
                                                <strong>Địa điểm:</strong> {{ eventData.location || 'Chưa xác định' }}
                                            </div>
                                            <div class="info-item">
                                                <strong>Người tạo:</strong> {{ eventData.createdByName || 'HopeBox' }}
                                            </div>
                                            <div class="info-item">
                                                <strong>Tổ chức:</strong> {{ eventData.organizationName || 'HopeBox Foundation' }}
                                            </div>
                                            <div class="info-item">
                                                <strong>Chiến dịch:</strong> {{ eventData.causeTitle || 'Chưa có thông tin' }}
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row mt-4" v-if="eventData?.detail">
                                    <div class="col-lg-12">
                                        <div class="event-detail-description">
                                            <h4>Chi Tiết Sự Kiện</h4>
                                            <p>{{ eventData.detail }}</p>
                                        </div>
                                    </div>
                                </div>

                                <!-- Inkind Donation Form -->
                                
                            </div>
                        </div>

                        <div class="event-map-area">
                            <div class="event-info-content">
                                <div class="event-info-item">
                                    <div class="icon">
                                        <img class="icon-img" src="/images/icons/c1.png" alt="Icon">
                                    </div>
                                    <div class="content">
                                        <h4>Ngày Diễn Ra</h4>
                                        <img class="line-icon" src="/images/shape/line-s1.png" alt="Image-HopeBox">
                                        <p>{{ formatDate(eventData?.startDate) }}</p>
                                        <p>{{ formatDate(eventData?.endDate) }}</p>
                                    </div>
                                </div>
                                <div class="event-info-item">
                                    <div class="icon icon-time">
                                        <img class="icon-img" src="/images/icons/c2.png" alt="Icon">
                                    </div>
                                    <div class="content">
                                        <h4>Thời Gian</h4>
                                        <img class="line-icon" src="/images/shape/line-s1.png" alt="Image-HopeBox">
                                        <p>{{ formatTime(eventData?.startDate) }} - {{ formatTime(eventData?.endDate) }}
                                        </p>
                                    </div>
                                </div>
                                <div class="event-info-item mb-0 pb-0">
                                    <div class="icon icon-location">
                                        <img class="icon-img" src="/images/icons/c3.png" alt="Icon">
                                    </div>
                                    <div class="content">
                                        <h4>Địa Điểm</h4>
                                        <img class="line-icon" src="/images/shape/line-s1.png" alt="Image-HopeBox">
                                        <p>{{ eventData?.location || 'Đang cập nhật...' }}</p>
                                        <p v-if="eventData?.formattedAddress">{{ eventData.formattedAddress }}</p>
                                    </div>
                                </div>
                            </div>

                            <div v-if="eventData?.latitude && eventData?.longitude" id="openmap"
                                style="height: 500px; width: 100%; border-radius: 8px;">
                            </div>
                            <div v-else-if="eventData?.location" class="map-placeholder"
                                style="height: 500px; display: flex; align-items: center; justify-content: center; background: #f5f5f5; border-radius: 8px;">
                                <div class="text-center">
                                    <i class="fas fa-map-marker-alt fa-3x text-muted mb-3"></i>
                                    <h5>{{ eventData.location }}</h5>
                                    <p class="text-muted">Đang tải bản đồ...</p>
                                    <button @click="searchAndDisplayLocation" class="btn btn-primary btn-sm">
                                        Hiển thị trên bản đồ
                                    </button>
                                </div>
                            </div>
                        </div>
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
    name: 'EventDonationDetailContent',
    props: {
        eventId: {
            type: String,
            required: true
        }
    },
    data() {
        return {
            eventData: null,
            loading: false,
            map: null,
            openMapApiKey: 'IqlcvXxfiNb80OBsA5PTRDDEUHlQ7qSS'
        }
    },
    mounted() {
        this.fetchEventDetail();
        this.loadOpenMapScript();
    },
    methods: {

        async fetchEventDetail() {
            try {
                this.loading = true;

                const response = await axios.get(
                    `https://localhost:7213/api/Event/get-event-donation-detail/${this.eventId}`
                );

                if (response.data.status === 200) {
                    this.eventData = response.data.responseData;

                    if (this.eventData?.latitude && this.eventData?.longitude) {
                        this.$nextTick(() => {
                            this.initializeMap();
                        });
                    }
                } else {
                    await showErrorAlert('Lỗi', 'Không thể tải thông tin sự kiện');
                }
            } catch (error) {
                console.error('Error fetching event detail:', error);
                await showErrorAlert('Lỗi', 'Không thể kết nối đến server');
            } finally {
                this.loading = false;
            }
        },

        loadOpenMapScript() {
            if (window.maplibregl) {
                return;
            }

            const cssLink = document.createElement('link');
            cssLink.rel = 'stylesheet';
            cssLink.href = 'https://unpkg.com/@openmapvn/openmapvn-gl@latest/dist/maplibre-gl.css';
            document.head.appendChild(cssLink);

            const script = document.createElement('script');
            script.src = 'https://unpkg.com/@openmapvn/openmapvn-gl@latest/dist/maplibre-gl.js';
            script.onload = () => {
                if (this.eventData?.latitude && this.eventData?.longitude) {
                    this.initializeMap();
                }
            };
            document.head.appendChild(script);
        },

        initializeMap() {
            if (!window.maplibregl || !this.eventData?.latitude || !this.eventData?.longitude) {
                return;
            }

            try {
                this.map = new maplibregl.Map({
                    container: 'openmap',
                    style: `https://maptiles.openmap.vn/styles/day-v1/style.json?apikey=${this.openMapApiKey}`,
                    center: [this.eventData.longitude, this.eventData.latitude],
                    zoom: 15,
                    maplibreLogo: true
                });

                const marker = new maplibregl.Marker({
                    color: '#e74c3c'
                })
                    .setLngLat([this.eventData.longitude, this.eventData.latitude])
                    .addTo(this.map);

                const popup = new maplibregl.Popup({
                    offset: 25,
                    closeButton: true,
                    closeOnClick: false
                })
                    .setLngLat([this.eventData.longitude, this.eventData.latitude])
                    .setHTML(`
                    <div style="padding: 10px; max-width: 250px;">
                        <h6 style="margin: 0 0 8px 0; font-weight: bold;">${this.eventData.title}</h6>
                        <p style="margin: 0 0 5px 0; font-size: 14px;">
                            <i class="fas fa-map-marker-alt"></i> ${this.eventData.location}
                        </p>
                        <p style="margin: 0 0 5px 0; font-size: 14px;">
                            <i class="fas fa-calendar"></i> ${this.formatDate(this.eventData.startDate)}
                        </p>
                    </div>
                `)
                    .addTo(this.map);

                this.map.addControl(new maplibregl.NavigationControl(), 'top-right');
                this.map.addControl(new maplibregl.FullscreenControl(), 'top-right');

            } catch (error) {
                console.error('Error initializing OpenMap:', error);
            }
        },

        async searchAndDisplayLocation() {
            if (!this.eventData?.location) return;

            try {
                const response = await axios.get(
                    `https://localhost:7213/api/Event/search-places`,
                    { params: { keyword: this.eventData.location } }
                );

                if (response.data.status === 200 && response.data.responseData.length > 0) {
                    const firstPlace = response.data.responseData[0];

                    const detailResponse = await axios.get(
                        `https://localhost:7213/api/Event/place-detail/${firstPlace.id}`
                    );

                    if (detailResponse.data.status === 200) {
                        const placeDetail = detailResponse.data.responseData;

                        this.eventData.latitude = placeDetail.latitude;
                        this.eventData.longitude = placeDetail.longitude;
                        this.eventData.formattedAddress = placeDetail.label;

                        this.$nextTick(() => {
                            this.initializeMap();
                        });
                    }
                }
            } catch (error) {
                console.error('Error searching location:', error);
            }
        },

        formatEventDate(startDate, endDate) {
            if (!startDate || !endDate) return 'Chưa xác định';

            const start = new Date(startDate);
            const end = new Date(endDate);

            const startFormatted = start.toLocaleDateString('vi-VN');
            const endFormatted = end.toLocaleDateString('vi-VN');

            if (startFormatted === endFormatted) {
                return startFormatted;
            }

            return `${startFormatted} - ${endFormatted}`;
        },

        formatDate(dateString) {
            if (!dateString) return '';
            return new Date(dateString).toLocaleDateString('vi-VN');
        },

        formatTime(dateString) {
            if (!dateString) return '';
            return new Date(dateString).toLocaleTimeString('vi-VN', {
                hour: '2-digit',
                minute: '2-digit'
            });
        },

        handleImageError(event) {
            event.target.src = '/images/default-event.jpg';
        },

        handlePackagesSelected(selectedPackages) {
            console.log('Selected packages:', selectedPackages);
            // Store selected packages for later use
            this.selectedPackages = selectedPackages;
        },


        handleProceedDonation(donationData) {
            console.log('Proceed to donation:', donationData);

            this.$router.push({
                path: '/donation',
                query: {
                    eventId: this.eventId,
                    type: 'package',
                    packages: JSON.stringify(donationData.packages),
                    totalAmount: donationData.totalAmount
                }
            });
        }
    },

    beforeDestroy() {
        if (this.map) {
            this.map.remove();
        }
    }
}
</script>

<style scoped>
@import '@/assets/scss/component/_event-donation-detail.scss';
</style>