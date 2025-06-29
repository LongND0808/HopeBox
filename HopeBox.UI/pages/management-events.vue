<template>
    <div class="event-management-container">
      <Sidebar />
      <div class="main-content">
        <ManageEvent />
        <ManagementFooter />
      </div>
    </div>
  </template>
  
  <script>
  export default {
    components: {
      Sidebar: () => import('@/components/Sidebar'),
      ManageEvent: () => import('@/components/ManageEvent'),
      ManagementFooter: () => import('@/components/ManagementFooter')
    }
  }
  </script>
  
  <style scoped>
  .event-management-container {
    display: flex;
    flex-direction: row;
    width: 100%;
  }
  
  .main-content {
    margin-left: 250px;
    width: 100%;
    background: #111226;
    min-height: 100vh;
    color: white;
    transition: margin-left 0.3s ease;
  }
  
  @media (max-width: 768px) {
    .event-management-container {
      flex-direction: column;
    }
    .main-content {
      margin-left: 0;
    }
  }
  </style>
