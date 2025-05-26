<template>
    <div class="mobile-navigation">
        <nav class="offcanvas-navigation" id="offcanvas-navigation">
            <ul>
                <li>
                    <nuxt-link to="/">Home</nuxt-link>
                </li>
                <li>
                    <nuxt-link to="/about">About</nuxt-link>
                </li>
                <li class="has-children">
                    <nuxt-link to="/causes">Causes</nuxt-link>
                    <ul class="sub-menu">
                        <li>
                            <nuxt-link to="/causes">Causes</nuxt-link>
                        </li>
                        <li>
                            <nuxt-link to="/causes-details">Causes Details</nuxt-link>
                        </li>
                    </ul>
                </li>
                <li class="has-children">
                    <nuxt-link to="/blog">Blog</nuxt-link>
                    <ul class="sub-menu">
                        <li>
                            <nuxt-link to="/blog">Blog</nuxt-link>
                        </li>
                        <li>
                            <nuxt-link to="/blog-details">Blog Details</nuxt-link>
                        </li>
                    </ul>
                </li>
                <li class="has-children">
                    <nuxt-link to="/event-details">Pages</nuxt-link>
                    <ul class="sub-menu">
                        <li>
                            <nuxt-link to="/event-details">Event Details</nuxt-link>
                        </li>
                        <li>
                            <nuxt-link to="/volunteer">Volunteer</nuxt-link>
                        </li>
                        <li>
                            <nuxt-link to="/donation">Donation</nuxt-link>
                        </li>
                    </ul>
                </li>
                <li>
                    <nuxt-link to="/contact">Contact</nuxt-link>
                </li>
            </ul>
        </nav>
    </div>
</template>

<script>
    export default {
        mounted() {
            const offCanvasNav = document.querySelector('#offcanvas-navigation');
            const offCanvasNavSubMenu = offCanvasNav.querySelectorAll('.sub-menu');
            const anchorLinks = offCanvasNav.querySelectorAll('a');
        
            for (let i = 0; i < offCanvasNavSubMenu.length; i++){
                offCanvasNavSubMenu[i].insertAdjacentHTML("beforebegin", "<span class='menu-expand'><i></i></span>");
            }
        
            const menuExpand = offCanvasNav.querySelectorAll('.menu-expand');
            const numMenuExpand = menuExpand.length;
        
            for (let i = 0; i < numMenuExpand; i++) {
                menuExpand[i].addEventListener("click", (e) => {sideMenuExpand(e)});
            }
        
            for (let i = 0; i < anchorLinks.length; i++) {
                anchorLinks[i].addEventListener("click", () => {closeMobileMenu()});
            }

            const sideMenuExpand = (e) => {
                e.currentTarget.parentElement.classList.toggle('active');
            }
            const closeMobileMenu = () => {
                const offcanvasMobileMenu = document.querySelector('#offcanvas-mobile-menu');
                offcanvasMobileMenu?.classList.remove('active');
            }
        }
    };
</script>

<style lang="scss" >
    .offcanvas-navigation {
        ul {
            padding-left: 0;
            li {
                border-bottom: 1px solid rgba(255, 255, 255, 0.15);
                &.has-children {
                    .sub-menu {
                        height: 0;
                        opacity: 0;
                        transition: 0.3s;
                        visibility: hidden;
                    }
                    &.active > {
                        .sub-menu {
                            height: 100%;
                            opacity: 1;
                            visibility: visible;
                            padding-left: 0;
                        }
                    }
                }
                a {
                    color: $black;
                    font-size: 16px;
                    font-weight: 500;
                    padding: 5px 0;
                    display: block;
                    &:hover {
                        color: $theme-color;
                    }
                }
            }
        }

        ul {
            &.sub-menu {
                margin-left: 15px;
                transition: 0.4s;
                li {
                    list-style: none;
                    border: none;
                    a {
                        color: $black;
                        font-weight: 400;
                        &:hover {
                            color: $theme-color;
                        }
                    }
                    .sub-menu {
                        li {
                            a {
                                font-size: 13px;
                            }
                        }
                    }
                }
            }
            li {
                &.has-children {
                    position: relative;
                    display: block;
                    a {
                        display: inline-block;
                    }

                    &.active > {
                        .menu-expand {
                            background-color: rgba($black, 0.2);
                            i {
                                &:before {
                                    transform: rotate(0);
                                }
                            }
                        }
                    }

                    .menu-expand {
                        top: 5px;
                        right: 0;
                        width: 35px;
                        height: 35px;
                        cursor: pointer;
                        line-height: 35px;
                        position: absolute;
                        text-align: center;
                        background-color: rgba($black, 0.1);
                        &:hover {
                            background-color: rgba($black, 0.2);
                        }

                        i {
                            display: block;
                            border-bottom: 1px solid $black;
                            position: relative;
                            width: 10px;
                            text-align: center;
                            top: 50%;
                            left: 50%;
                            transform: translate(-50%, -50%);
                            &:before {
                                width: 100%;
                                content: "";
                                border-bottom: 1px solid $black;
                                display: block;
                                position: absolute;
                                top: 0;
                                transform: rotate(90deg);
                                transition: 0.4s;
                            }
                        }
                    }
                }
            }
        }
    }
</style>