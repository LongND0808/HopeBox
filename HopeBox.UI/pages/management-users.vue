<template>
    <div>
        <Sidebar />
        <ManageUser/>
        <ManagementFooter />
    </div>
</template>

<script>

    export default {
        components: {
            Sidebar: () => import('@/components/Sidebar'),
            ManageUser: () => import('@/components/ManageUser'),
            ManagementFooter: () => import('@/components/ManagementFooter')
        }
    }
</script>