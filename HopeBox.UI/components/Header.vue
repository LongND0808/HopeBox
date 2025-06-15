<template>
    <header class="header-area header-default" :class="{'is-sticky': isSticky}">
        <div class="container">
            <div class="row align-items-center justify-content-between">
                <div class="col-3 col-sm-2 col-lg-1 pr-0">
                    <div class="header-logo-area">
                        <nuxt-link to="/">
                            <img class="logo-main" style="height: 75px;" src="/images/logo/logo.png" alt="Logo" />
                        </nuxt-link>
                    </div>
                </div>
                <div class="col-9 col-sm-10 col-lg-11">
                    <div class="header-align">
                        <div class="header-navigation-area">
                            <Navigation />
                        </div>
                        <div class="header-action-area">
                            <div class="mobile-menu-toggle d-lg-none">
                                <button class="toggle" @click="mobiletoggleClass('addClass', 'show-mobile-menu')">
                                    <i class="icon-top"></i>
                                    <i class="icon-middle"></i>
                                    <i class="icon-bottom"></i>
                                </button>
                            </div>

                            <template v-if="isLoggedIn">
                                <nuxt-link to="/profile" style="font-size: 14px;"
                                    class="btn-theme btn-gradient btn-slide btn-style orange-btn">
                                    Thông Tin Cá Nhân
                                </nuxt-link>
                                <button @click="handleLogout" style="font-size: 14px;"
                                    class="btn-theme btn-white btn-slide btn-style logout-btn">
                                    Đăng Xuất
                                    <img class="icon icon-img" src="/images/icons/arrow-line-right2.png" alt="Icon">
                                </button>
                            </template>

                            <nuxt-link v-else to="/login" class="btn-theme btn-gradient btn-slide btn-style">
                                Đăng Nhập
                                <img class="icon icon-img" src="/images/icons/arrow-line-right2.png" alt="Icon">
                            </nuxt-link>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </header>
</template>

<script>
import axios from 'axios'
import Navigation from '@/components/Navigation'
import { BASE_URL } from '@/utils/constants';

export default {
    components: { Navigation },

    data() {
        return {
            isSticky: false,
            isLoggedIn: false
        }
    },

    mounted() {
        this.checkLoginStatus()

        window.addEventListener('scroll', () => {
            this.isSticky = window.scrollY >= 200
        })
    },

    methods: {
        mobiletoggleClass(addRemoveClass, className) {
            const el = document.querySelector('#offcanvas-menu')
            if (!el) return
            if (addRemoveClass === 'addClass') el.classList.add(className)
            else el.classList.remove(className)
        },

        async checkLoginStatus() {
            try {
                await axios.get(`${BASE_URL}/api/Authentication/me`, {
                    withCredentials: true
                })
                this.isLoggedIn = true
            } catch (err) {
                if (err.response && err.response.status === 401) {
                    try {
                        await axios.post(`${BASE_URL}/api/Authentication/refresh-token`, {}, {
                            withCredentials: true
                        })

                        const retry = await axios.get(`${BASE_URL}/api/Authentication/me`, {
                            withCredentials: true
                        })

                        this.isLoggedIn = true
                    } catch (refreshErr) {
                        console.warn('Refresh token hết hạn hoặc lỗi', refreshErr)
                        this.isLoggedIn = false
                    }
                } else {
                    this.isLoggedIn = false
                }
            }
        },

        async handleLogout() {
            try {
                await axios.post(`${BASE_URL}/api/Authentication/logout`, {}, {
                    withCredentials: true
                })
            } catch (error) {
                console.error('Lỗi khi đăng xuất:', error)
            }

            this.isLoggedIn = false
            this.$router.push('/')
        }
    }
}
</script>
