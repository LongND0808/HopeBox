<template>
    <div class="widget mb-0 pb-3">
        <h3 class="widget-title">Popular Tags</h3>
        <div class="separator-line">
            <img src="/images/shape/line-t2.png" alt="image">
        </div>
        <div class="widget-tags">
            <ul>
                <li v-for="(tag, index) in tagList" :key="index">
                    <nuxt-link :to="tag.url">{{ tag.title }}</nuxt-link>
                </li>
            </ul>
        </div>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                tagList: [
                    {
                        title: "Clean Water",
                        url: "#"
                    },
                    {
                        title: "Education",
                        url: "#"
                    },
                    {
                        title: "Health",
                        url: "#"
                    },
                    {
                        title: "Medicine",
                        url: "#"
                    },
                    {
                        title: "Poor",
                        url: "#"
                    },
                    {
                        title: "Children",
                        url: "#"
                    },
                    {
                        title: "Charity",
                        url: "#"
                    },
                    {
                        title: "Non Profit",
                        url: "#"
                    },
                ]
            }
        },
    };
</script>