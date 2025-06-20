<template>
    <div class="sidebar-area">
        <WidgetSearch />
        
        <WidgetCategory />

        <WidgetUrgentCauses />

        <WidgetTag />
    </div>
</template>

<script>
    export default {
        components: {
            WidgetSearch: () => import('@/components/WidgetSearch'),
            WidgetCategory: () => import('@/components/WidgetCategory'),
            WidgetUrgentCauses: () => import('@/components/WidgetUrgentCauses'),
            WidgetTag: () => import('@/components/WidgetTag'),
        },
    };
</script>
