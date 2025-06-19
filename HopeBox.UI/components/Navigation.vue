<template>
    <ul class="main-menu nav">
        <li>
            <nuxt-link to="/">Trang Chủ</nuxt-link>
        </li>
        <li>
            <nuxt-link to="/about">Giới Thiệu</nuxt-link>
        </li>
        <li>
            <nuxt-link to="/causes">Chiến Dịch</nuxt-link>
        </li>
        <li>
            <nuxt-link to="/blog">Blog</nuxt-link>
        </li>
        <li class="has-submenu">
            <nuxt-link>Khác</nuxt-link>
            <ul class="submenu-nav">
                <li>
                    <nuxt-link to="/event-details">Sự Kiện</nuxt-link>
                </li>
                <li>
                    <nuxt-link to="/volunteer">Tình Nguyện Viên</nuxt-link>
                </li>
            </ul>
        </li>
        <li>
            <nuxt-link to="/event-donation">Quyên Góp Sự Kiện</nuxt-link>
        </li>
        <!-- <li>
            <nuxt-link to="/contact">Liên hệ</nuxt-link>
        </li> -->
    </ul>
</template>