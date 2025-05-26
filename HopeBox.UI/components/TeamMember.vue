<template>
    <div class="team-member">
        <div class="thumb">
            <img :src="member.imgSrc" :alt="member.name">
            <div class="social-icons">
                <a :href="list.url" v-for="(list, index) in member.socialLink" :key="index">
                    <i :class="list.icon"></i>
                </a>
            </div>
        </div>
        <div class="content">
            <div class="member-info">
                <h4 class="name">
                    <a :href="member.url">{{ member.name }}</a>
                </h4>
                <h6 class="designation">{{ member.designation }}</h6>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        props: ['member']
    };
</script>