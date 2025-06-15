<template>
    <section class="event-details-area" v-if="event">
        <div class="container">
            <div class="row">
                <div class="col-12">
                    <div class="event-details-column">
                        <div class="event-details-content">
                            <div class="event-details">
                                <div class="thumb">
                                    <!-- <img class="mb-40 w-100" src="public/images/events/d1.jpg" alt="Image-HopeBox"> -->
                                    <img class="mb-40 w-100" :src="event.bannerImage || 'public/images/events/d1.jpg'"
                                        :alt="event.title">
                                </div>
                                <div class="event-time-info">
                                    <div class="info-item">
                                        <div class="icon"><img src="/images/icons/e1.png" alt="Icon-Image"></div>
                                        <h4>{{ formatDate(event.startDate) }}</h4>
                                    </div>
                                    <div class="info-item">
                                        <div class="icon"><img src="/images/icons/e2.png" alt="Icon-Image"></div>
                                        <h4>{{ formatTimeRange(event.startDate, event.endDate) }}</h4>
                                    </div>
                                    <div class="info-item event-location">
                                        <div class="icon"><img src="/images/icons/e3.png" alt="Icon-Image"></div>
                                        <h4>{{ event.location }}</h4>
                                    </div>
                                </div>
                                <div class="event-category-post">
                                    <div class="event-category">Thiện Nguyện</div>
                                    <div class="event-author">By: {{ event.createdByName }}, <span>{{
                                            event.organizationName }}</span></div>
                                </div>
                                <h3 class="event-title">{{ event.title }}.</h3>
                                <p>{{ event.description }}</p>
                                <p class="mb-34">{{ event.detail }}</p>

                                <div class="map-content">
                                    <div v-if="event.latitude && event.longitude" id="openmap"
                                        style="height: 400px; width: 100%; border-radius: 8px;">
                                    </div>
                                    <div v-else-if="event.location" class="map-placeholder"
                                        style="height: 400px; display: flex; align-items: center; justify-content: center; background: #f5f5f5; border-radius: 8px;">
                                        <div class="text-center">
                                            <i class="fas fa-map-marker-alt fa-3x text-muted mb-3"></i>
                                            <h5>Vị trí: {{ event.location }}</h5>
                                            <p class="text-muted">Đang tải bản đồ...</p>
                                            <button @click="searchAndDisplayLocation" class="btn btn-primary btn-sm">
                                                Hiển thị trên bản đồ
                                            </button>
                                        </div>
                                    </div>
                                </div>
                                <!-- <h3>Other Information of Event</h3>
                                <p>Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem
                                    Ipsum has been the industry's standard dummy text ever since the 1500 an when an
                                    unknown printer took a galley of type and scrambled it to make a type specimen book.
                                    It has survived not only five centuries, but also the leap into electronic
                                    typesetting, remaining essentially unchanged was popularised in the 1960s with the
                                    release of etraset sheets containing Lorem Ipsum passages.</p>
                                <p class="mb-35">Lorem Ipsum is simply dummy text of the printing and typesetting
                                    industry. Lorem Ipsum has been the industry's standard dummy text ever since the
                                    1500 an when an unknown printer took a galley of type and scrambled it to make a
                                    type specimen book.</p> -->
                                <h3>Current Sponsors.</h3>
                                <p class="mb-35">Lorem Ipsum is simply dummy text of the printing and typesetting
                                    industry. Lorem Ipsum has been the industry's standard dummy text ever since the
                                    1500 an when an unknown printer took a galley of type and scrambled it to make a
                                    type specimen book.</p>
                                <div class="brand-logo-area brand-logo-default-area p-0">
                                    <div class="brand-logo-content p-0">
                                        <div class="row row-cols-3 row-cols-sm-5 mtn-50">
                                            <div class="col mt-50">
                                                <div class="brand-logo-item">
                                                    <img src="/images/brand-logo/1.png" alt="Image-HopeBox">
                                                </div>
                                            </div>
                                            <div class="col mt-50">
                                                <div class="brand-logo-item">
                                                    <img src="/images/brand-logo/2.png" alt="Image-HopeBox">
                                                </div>
                                            </div>
                                            <div class="col mt-50">
                                                <div class="brand-logo-item">
                                                    <img src="/images/brand-logo/3.png" alt="Image-HopeBox">
                                                </div>
                                            </div>
                                            <div class="col mt-50">
                                                <div class="brand-logo-item">
                                                    <img src="/images/brand-logo/4.png" alt="Image-HopeBox">
                                                </div>
                                            </div>
                                            <div class="col mt-50">
                                                <div class="brand-logo-item">
                                                    <img src="/images/brand-logo/5.png" alt="Image-HopeBox">
                                                </div>
                                            </div>
                                            <div class="col mt-50">
                                                <div class="brand-logo-item">
                                                    <img src="/images/brand-logo/6.png" alt="Image-HopeBox">
                                                </div>
                                            </div>
                                            <div class="col mt-50">
                                                <div class="brand-logo-item">
                                                    <img src="/images/brand-logo/7.png" alt="Image-HopeBox">
                                                </div>
                                            </div>
                                            <div class="col mt-50">
                                                <div class="brand-logo-item">
                                                    <img src="/images/brand-logo/8.png" alt="Image-HopeBox">
                                                </div>
                                            </div>
                                            <div class="col mt-50">
                                                <div class="brand-logo-item">
                                                    <img src="/images/brand-logo/9.png" alt="Image-HopeBox">
                                                </div>
                                            </div>
                                            <div class="col mt-50">
                                                <div class="brand-logo-item">
                                                    <img src="/images/brand-logo/10.png" alt="Image-HopeBox">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <h3 class="mt-n8 pt-5">Tại Sao Nên Tham Gia Các Dự Án Từ Thiện?</h3>
                                <p>Tham gia các dự án từ thiện là cơ hội để bạn lan tỏa yêu thương và tạo ra sự thay đổi
                                    tích cực trong cộng đồng. Không chỉ giúp đỡ những người cần hỗ trợ, các hoạt động từ
                                    thiện còn mang lại cơ hội phát triển bản thân, rèn luyện kỹ năng làm việc nhóm, lãnh
                                    đạo và đồng cảm. Bạn sẽ được gặp gỡ những con người có chung lý tưởng, mở rộng mối
                                    quan hệ và cảm nhận niềm vui từ việc đóng góp cho xã hội.</p>
                                <p class="mb-44">Hơn nữa, tham gia từ thiện giúp bạn nhìn nhận cuộc sống từ những góc độ
                                    mới, trân trọng hơn những gì mình đang có và nuôi dưỡng tinh thần trách nhiệm với
                                    cộng đồng. Đặc biệt, dự án từ thiện HopeBox của chúng tôi không chỉ dừng lại ở việc
                                    hỗ trợ vật chất mà còn tập trung vào việc trao quyền, mang lại hy vọng và cơ hội để
                                    những người khó khăn vươn lên trong cuộc sống. Tham gia HopeBox, bạn sẽ trở thành
                                    một phần của hành trình truyền cảm hứng, xây dựng tương lai tốt đẹp hơn cho những
                                    người cần nó nhất.</p>
                                <div class="btn-wrp">
                                    <nuxt-link to="/event-details" class="btn-theme btn-gradient btn-slide">
                                        <span>Join Now <img class="icon icon-img"
                                                src="/images/icons/arrow-line-right2.png" alt="Icon"></span>
                                    </nuxt-link>
                                    <a class="btn-theme btn-border" href="tel:0123456789">
                                        <img class="icon icon-img icon-style" src="/images/icons/call.png" alt="Icon">
                                        0123456789
                                    </a>
                                </div>
                            </div>
                        </div>
                        <SidebarWrapper />
                    </div>
                </div>
            </div>
        </div>
    </section>
    <section v-else>
        <p>Đang tải sự kiện...</p>
    </section>
</template>

<script>
import axios from 'axios';
import { BASE_URL } from '@/utils/constants';

export default {
    components: {
        SidebarWrapper: () => import('@/components/SidebarWrapper'),
    },
    props: {
        id: { type: String, required: false }
    },
    data() {
        return { 
            event: null,
            map: null,
            openMapApiKey: 'IqlcvXxfiNb80OBsA5PTRDDEUHlQ7qSS'
        };
    },
    mounted() {
        this.fetchEventDetails();
        this.loadOpenMapScript();
    },
    methods: {
        async fetchEventDetails() {
            try {
                const response = await axios.get(`${BASE_URL}/api/Event/get-by-nearest-event`);
                this.event = response.data.responseData;
                
                // Khởi tạo bản đồ sau khi có dữ liệu event
                if (this.event && this.event.latitude && this.event.longitude) {
                    this.$nextTick(() => {
                        this.initializeMap();
                    });
                }
            } catch (err) {
                console.error("Lỗi khi gọi API:", err);
            }
        },

        loadOpenMapScript() {
            // Kiểm tra nếu script đã được load
            if (window.maplibregl) {
                return;
            }

            // Load CSS
            const cssLink = document.createElement('link');
            cssLink.rel = 'stylesheet';
            cssLink.href = 'https://unpkg.com/@openmapvn/openmapvn-gl@latest/dist/maplibre-gl.css';
            document.head.appendChild(cssLink);

            // Load JavaScript
            const script = document.createElement('script');
            script.src = 'https://unpkg.com/@openmapvn/openmapvn-gl@latest/dist/maplibre-gl.js';
            script.onload = () => {
                // Script đã load, khởi tạo bản đồ nếu có dữ liệu
                if (this.event && this.event.latitude && this.event.longitude) {
                    this.initializeMap();
                }
            };
            document.head.appendChild(script);
        },

        initializeMap() {
            if (!window.maplibregl || !this.event || !this.event.latitude || !this.event.longitude) {
                return;
            }

            try {
                // Khởi tạo bản đồ OpenMap
                this.map = new maplibregl.Map({
                    container: 'openmap',
                    style: `https://maptiles.openmap.vn/styles/day-v1/style.json?apikey=${this.openMapApiKey}`,
                    center: [this.event.longitude, this.event.latitude], // [lng, lat]
                    zoom: 15,
                    maplibreLogo: true
                });

                // Thêm marker cho vị trí sự kiện
                const marker = new maplibregl.Marker({
                    color: '#e74c3c' // Màu đỏ cho marker
                })
                .setLngLat([this.event.longitude, this.event.latitude])
                .addTo(this.map);

                // Thêm popup với thông tin sự kiện
                const popup = new maplibregl.Popup({
                    offset: 25,
                    closeButton: true,
                    closeOnClick: false
                })
                .setLngLat([this.event.longitude, this.event.latitude])
                .setHTML(`
                    <div style="padding: 10px; max-width: 250px;">
                        <h6 style="margin: 0 0 8px 0; font-weight: bold;">${this.event.title}</h6>
                        <p style="margin: 0 0 5px 0; font-size: 14px;">
                            <i class="fas fa-map-marker-alt"></i> ${this.event.formattedAddress || this.event.location}
                        </p>
                        <p style="margin: 0 0 5px 0; font-size: 14px;">
                            <i class="fas fa-calendar"></i> ${this.formatDate(this.event.startDate)}
                        </p>
                        <p style="margin: 0; font-size: 14px;">
                            <i class="fas fa-clock"></i> ${this.formatTimeRange(this.event.startDate, this.event.endDate)}
                        </p>
                    </div>
                `)
                .addTo(this.map);

                // Thêm controls
                this.map.addControl(new maplibregl.NavigationControl(), 'top-right');
                this.map.addControl(new maplibregl.FullscreenControl(), 'top-right');

                console.log('OpenMap initialized successfully');
            } catch (error) {
                console.error('Error initializing OpenMap:', error);
            }
        },

        async searchAndDisplayLocation() {
            if (!this.event || !this.event.location) return;

            try {
                // Gọi API để tìm kiếm vị trí
                const response = await this.$axios.get('/api/Event/search-places', {
                    params: { keyword: this.event.location }
                });

                if (response.data.status === 200 && response.data.responseData.length > 0) {
                    const firstPlace = response.data.responseData[0];
                    
                    // Lấy chi tiết địa điểm
                    const detailResponse = await axios.get(`${BASE_URL}/api/Event/place-detail/${firstPlace.id}`);
                    
                    if (detailResponse.data.status === 200) {
                        const placeDetail = detailResponse.data.responseData;
                        
                        // Cập nhật event với tọa độ mới
                        this.event.latitude = placeDetail.latitude;
                        this.event.longitude = placeDetail.longitude;
                        this.event.formattedAddress = placeDetail.label;
                        
                        // Khởi tạo bản đồ
                        this.$nextTick(() => {
                            this.initializeMap();
                        });
                    }
                }
            } catch (error) {
                console.error('Error searching location:', error);
            }
        },

        formatDate(dateStr) {
            if (!dateStr) return '';
            const date = new Date(dateStr);
            return date.toLocaleDateString('vi-VN');
        },

        formatTimeRange(start, end) {
            if (!start || !end) return '';
            const s = new Date(start);
            const e = new Date(end);
            return `${s.getHours()}:${s.getMinutes().toString().padStart(2, '0')} - ${e.getHours()}:${e.getMinutes().toString().padStart(2, '0')}`;
        }
    },

    beforeDestroy() {
        // Cleanup map khi component bị destroy
        if (this.map) {
            this.map.remove();
        }
    }
};
</script>

<style scoped>
.map-content {
    margin: 30px 0;
    border-radius: 8px;
    overflow: hidden;
    box-shadow: 0 2px 10px rgba(0,0,0,0.1);
}

.map-placeholder {
    border: 2px dashed #ddd;
}

.map-placeholder .btn {
    margin-top: 10px;
}

/* Tùy chỉnh popup của OpenMap */
:deep(.maplibregl-popup-content) {
    border-radius: 8px;
    box-shadow: 0 4px 12px rgba(0,0,0,0.15);
}

:deep(.maplibregl-popup-tip) {
    border-top-color: #fff;
}
</style>