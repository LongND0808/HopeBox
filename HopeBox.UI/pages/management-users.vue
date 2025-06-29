<template>
    <div class="user-management-container">
      <Sidebar />
      <div class="main-content">
        <ManageUser />
        <ManagementFooter />
      </div>
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
  
  <style scoped>
  .user-management-container {
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
    .user-management-container {
      flex-direction: column;
    }
    .main-content {
      margin-left: 0;
    }
  }
  </style>