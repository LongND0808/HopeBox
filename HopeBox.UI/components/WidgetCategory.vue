<template>
    <div class="widget">
        <h3 class="widget-title">{{ widgetTitle }}</h3>
        <div class="separator-line">
            <img class="me-1" src="/images/shape/line-t2.png" alt="image">
        </div>
        <div class="widget-category">
            <nuxt-link :to="cat.url" v-for="(cat, index) in categories" :key="index">
                {{ cat.title }} <span>({{ cat.count }})</span>
            </nuxt-link>
        </div>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                widgetTitle: "Causes Categories",
                categories: [
                    {
                        title: "Clean Water",
                        count: "18",
                        url: "#"
                    },
                    {
                        title: "Children Education",
                        count: "06",
                        url: "#"
                    },
                    {
                        title: "Healty Food",
                        count: "15",
                        url: "#"
                    },
                    {
                        title: "Food And Water",
                        count: "24",
                        url: "#"
                    },
                    {
                        title: "National Charity",
                        count: "08",
                        url: "#"
                    }
                ]
            }
        },
    };
</script>