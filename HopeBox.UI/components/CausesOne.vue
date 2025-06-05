<template>
    <section class="causes-area causes-default-area">
        <div class="container">
            <div class="row">
                <div class="col-lg-8 m-auto">
                    <div class="section-title text-center" data-aos="fade-up" data-aos-duration="1000">
                        <h5 class="subtitle line-theme-color">Hoạt động gây quỹ</h5>
                        <h2 class="title title-style">
                            Chung tay vì trẻ em vùng cao Việt Nam
                            <img class="img-shape" src="/images/shape/3.png" alt="Image">
                        </h2>
                    </div>
                </div>
            </div>
            <div class="row mtn-30" data-aos="fade-up" data-aos-duration="1000">
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
                                <nuxt-link to="/causes-details">{{ causes.title }}</nuxt-link>
                            </h4>
                            <p>{{ causes.desc }}</p>
                        </div>
                        <div class="causes-footer">
                            <div class="admin">
                                <h5>
                                    <nuxt-link to="#">
                                        <span class="icon-img">
                                            <img src="/images/icons/admin1.png" alt="Icon">
                                        </span> {{ causes.name }}
                                    </nuxt-link>
                                </h5>
                            </div>
                            <nuxt-link :to="{
                                            path: '/donation',
                                            query: { causesId: causes.id }
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
        </div>
    </section>
</template>

<script>
    import axios from 'axios';

    export default {
        data() {
            return {
                causesData: []
            };
        },
        mounted() {
            axios.get('https://localhost:7213/api/Cause/get-cause-one')
                .then(async response => {
                    const causes = response.data.responseData;

                    const causesWithUser = await Promise.all(causes.map(async (item) => {
                        let userName = 'Ẩn danh';
                        try {
                            const userRes = await axios.get(`https://localhost:7213/api/User/get-by-id?id=${item.createdBy}`);
                            userName = userRes.data.responseData.fullName;
                        } catch (err) {
                            console.warn('Lỗi lấy tên người dùng:', err);
                        }

                        return {
                            id: item.id,
                            imgSrc: item.heroImage || 'default.jpg',
                            title: item.title,
                            desc: item.description,
                            name: userName,
                            infoList: [
                                {
                                    infoTitle: "Mục tiêu",
                                    amount: `${item.targetAmount}₫`
                                },
                                {
                                    infoTitle: "Đã góp",
                                    amount: `${item.currentAmount}₫`
                                },
                                {
                                    infoTitle: "Còn thiếu",
                                    amount: `${Math.max(item.targetAmount - item.currentAmount, 0)}₫`
                                }
                            ]
                        };
                    }));

                    this.causesData = causesWithUser;
                })
                .catch(error => {
                    console.error('Lỗi khi gọi API Causes:', error);
                });
        }
    };
</script>