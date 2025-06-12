<template>
    <div>
        <Sidebar />
        <ManageCause/>
        <ManagementFooter />
    </div>
</template>

<script>

    export default {
        components: {
            Sidebar: () => import('@/components/Sidebar'),
            ManageCause: () => import('@/components/ManageCause'),
            ManagementFooter: () => import('@/components/ManagementFooter')
        }
    }
</script>