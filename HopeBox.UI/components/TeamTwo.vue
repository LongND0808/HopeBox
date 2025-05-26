<template>
    <section class="team-area">
        <div class="container">
            <div class="row">
                <div class="col-lg-8 m-auto">
                    <div class="section-title text-center">
                        <h5 class="subtitle line-theme-color mb-7">Volunteer Team</h5>
                        <h2 class="title title-style">Talented Volunteer Team of HopeBox. <img class="img-shape" src="/images/shape/3.png" alt="Image-HopeBox"></h2>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12">
                    <div class="team-member-items">
                        <div class="row">
                            <div class="col-sm-6 col-lg-4" v-for="(member, index) in teamMembers" :key="index">
                                <TeamMember :member="member" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</template>

<script>
    export default {
        components: {
            TeamMember: () => import('@/components/TeamMember'),
        },

        data() {
            return {
                teamMembers: [
                    {
                        imgSrc: "/images/team/1.jpg",
                        name: "Harvey Harrington",
                        designation: "Senior Designer",
                        url: "#",
                        socialLink: [
                            {
                                icon: "icofont-facebook",
                                url: "#"
                            },
                            {
                                icon: "icofont-skype",
                                url: "#"
                            },
                            {
                                icon: "icofont-twitter",
                                url: "#"
                            }
                        ]
                    },
                    {
                        imgSrc: "/images/team/2.jpg",
                        name: "Alejandra Bobbitt",
                        designation: "Senior Volunteer",
                        url: "#",
                        socialLink: [
                            {
                                icon: "icofont-facebook",
                                url: "#"
                            },
                            {
                                icon: "icofont-skype",
                                url: "#"
                            },
                            {
                                icon: "icofont-twitter",
                                url: "#"
                            }
                        ]
                    },
                    {
                        imgSrc: "/images/team/3.jpg",
                        name: "Sherika Hankins",
                        designation: "Senior Developer",
                        url: "#",
                        socialLink: [
                            {
                                icon: "icofont-facebook",
                                url: "#"
                            },
                            {
                                icon: "icofont-skype",
                                url: "#"
                            },
                            {
                                icon: "icofont-twitter",
                                url: "#"
                            }
                        ]
                    },
                    {
                        imgSrc: "/images/team/4.jpg",
                        name: "Destiny Kendall",
                        designation: "Marketing Manager",
                        url: "#",
                        socialLink: [
                            {
                                icon: "icofont-facebook",
                                url: "#"
                            },
                            {
                                icon: "icofont-skype",
                                url: "#"
                            },
                            {
                                icon: "icofont-twitter",
                                url: "#"
                            }
                        ]
                    },
                    {
                        imgSrc: "/images/team/5.jpg",
                        name: "Marleen Larry",
                        designation: "PHP Expert",
                        url: "#",
                        socialLink: [
                            {
                                icon: "icofont-facebook",
                                url: "#"
                            },
                            {
                                icon: "icofont-skype",
                                url: "#"
                            },
                            {
                                icon: "icofont-twitter",
                                url: "#"
                            }
                        ]
                    },
                    {
                        imgSrc: "/images/team/6.jpg",
                        name: "Digital Marketer",
                        designation: "Senior Developer",
                        url: "#",
                        socialLink: [
                            {
                                icon: "icofont-facebook",
                                url: "#"
                            },
                            {
                                icon: "icofont-skype",
                                url: "#"
                            },
                            {
                                icon: "icofont-twitter",
                                url: "#"
                            }
                        ]
                    }
                ]
            }
        },
    };
</script>