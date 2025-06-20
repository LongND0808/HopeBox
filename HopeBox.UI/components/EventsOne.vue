<template>
    <section class="events-area events-default-area">
        <div class="container">
            <div class="row">
                <div class="col-lg-8">
                    <div class="section-title" data-aos="fade-up" data-aos-duration="1000">
                        <h5 class="subtitle line-theme-color mb-7">Sự kiện mới nhất</h5>
                        <h2 class="title title-style">
                            Tham gia các sự kiện gây quỹ mới cùng HopeBox
                            <img class="img-shape" :src="'/images/shape/3.png'" alt="Image">
                        </h2>
                    </div>

                    <!-- Loading State -->
                    <div v-if="loading" class="events-content events-list" data-aos="fade-up" data-aos-duration="1000">
                        <div class="loading-skeleton">
                            <div class="skeleton-item" v-for="n in 3" :key="n">
                                <div class="skeleton-thumb"></div>
                                <div class="skeleton-content">
                                    <div class="skeleton-info"></div>
                                    <div class="skeleton-title"></div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Events List -->
                    <div v-else-if="eventsData && eventsData.length > 0" class="events-content events-list"
                        data-aos="fade-up" data-aos-duration="1000">
                        <div class="event-item" v-for="(event, index) in eventsData" :key="event.id || index">
                            <div class="thumb">
                                <img class="thumb-img" :src="event.bannerImage || '/images/events/default.jpg'"
                                    :alt="event.title" @error="handleImageError">
                                <nuxt-link :to="`/event-details?id=${event.id}`"
                                    class="btn-theme btn-gradient btn-size-sm">
                                    Tham gia ngay
                                    <img class="icon icon-img" :src="'/images/icons/arrow-line-right.png'" alt="Icon">
                                </nuxt-link>
                            </div>
                            <div class="content">
                                <div class="event-info">
                                    {{ formatEventDate(event.startDate, event.endDate) }} //
                                    <span>{{ event.organizationName || 'HopeBox' }}</span>
                                </div>
                                <h4 class="event-name">
                                    <nuxt-link :to="`/event-details?id=${event.id}`">
                                        {{ event.title }}
                                    </nuxt-link>
                                </h4>
                            </div>
                        </div>
                    </div>

                    <!-- No Events State -->
                    <div v-else class="events-content events-list" data-aos="fade-up" data-aos-duration="1000">
                        <div class="no-events-message">
                            <div class="text-center">
                                <img :src="'/images/icons/calendar.png'" alt="No Events" class="no-events-icon">
                                <h4>Hiện tại chưa có sự kiện nào</h4>
                                <p>Hãy quay lại sau để xem các sự kiện mới nhất từ HopeBox</p>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-8 offset-2 col-sm-8 offset-sm-2 col-md-8 offset-md-2 col-lg-4 offset-lg-0">
                    <div class="layer-style">
                        <div class="thumb tilt-animation">
                            <img :src="'/images/photos/event1.png'" alt="Image">
                            <div class="play-video-btn">
                                <a class="btn-play play-video-popup wave-btn"
                                    href="https://www.youtube.com/watch?v=knKuY0cFJHc">
                                    <span></span>
                                    <span></span>
                                    <span></span>
                                    <div class="icon">
                                        <img :src="'/images/icons/play.png'" alt="Icon">
                                    </div>
                                </a>
                            </div>
                        </div>
                        <div class="shape-style1">
                            <img :src="'/images/shape/line1.png'" alt="Image">
                        </div>
                        <div class="shape-style2">
                            <img :src="'/images/shape/line2.png'" alt="Image">
                        </div>
                        <div class="shape-style3">
                            <img :src="'/images/shape/line3.png'" alt="Image">
                        </div>
                        <div class="shape-style4">
                            <img :src="'/images/shape/line4.png'" alt="Image">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</template>

<script>
import axios from 'axios';

export default {
    name: 'EventOne',
    data() {
        return {
            eventsData: [],
            loading: false,
            error: null
        }
    },
    mounted() {
        this.fetchUpcomingEvents();
    },
    methods: {
        async fetchUpcomingEvents() {
            try {
                this.loading = true;
                this.error = null;

                const response = await axios.get('https://localhost:7213/api/Event/get-upcoming-events', {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                });

                if (response.data.status === 200) {
                    this.eventsData = response.data.responseData || [];
                    console.log('Successfully loaded upcoming events:', this.eventsData.length);
                } else {
                    console.error('API Error:', response.data.message);
                    this.error = response.data.message;
                    this.eventsData = [];
                }
            } catch (error) {
                console.error('Error fetching upcoming events:', error);
                this.error = 'Không thể tải danh sách sự kiện. Vui lòng thử lại sau.';
                this.eventsData = [];
            } finally {
                this.loading = false;
            }
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

            const monthNames = [
                '', 'Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6',
                'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'
            ];

            return `${day} ${monthNames[month]}, ${year}`;
        },

        isSameDate(date1, date2) {
            return date1.getDate() === date2.getDate() &&
                date1.getMonth() === date2.getMonth() &&
                date1.getFullYear() === date2.getFullYear();
        },

        handleImageError(event) {
            event.target.src = '/images/events/default.jpg';
        },

        retryFetch() {
            this.fetchUpcomingEvents();
        }
    }
}
</script>
