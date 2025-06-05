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
                                    <div class="event-author">By: {{ event.createdByName }}, <span>{{ event.organizationName }}</span></div>
                                </div>
                                <h3 class="event-title">{{ event.title }}.</h3>
                                <p>{{ event.description }}</p>
                                <p class="mb-34">{{ event.detail }}</p>
                                <div class="map-content">
                                    <iframe
                                        src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3151.8402891185374!2d144.95373631590425!3d-37.81720974201477!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x6ad65d4c2b349649%3A0xb6899234e561db11!2sEnvato!5e0!3m2!1sen!2ssg!4v1607294780661!5m2!1sen!2ssg"></iframe>
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

export default {
    components: {
        SidebarWrapper: () => import('@/components/SidebarWrapper'),
    },
    props: {
        id: { type: String, required: false }
    },
    data() {
        return { event: null };
    },
    mounted() {
        this.fetchEventDetails();
    },
    methods: {
        async fetchEventDetails() {
            try {
                const response = await axios.get('https://localhost:7213/api/Event/get-by-nearest-event');
                this.event = response.data.responseData;
            } catch (err) {
                console.error("Lỗi khi gọi API:", err);
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
    }
};
</script>