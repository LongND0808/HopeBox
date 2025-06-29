<template>
    <div class="blog-management-container">
      <Sidebar />
      <div class="main-content">
        <ManageBlog />
        <ManagementFooter />
      </div>
    </div>
  </template>
  
  <script>
  export default {
    components: {
      Sidebar: () => import('@/components/Sidebar'),
      ManageBlog: () => import('@/components/ManageBlog'),
      ManagementFooter: () => import('@/components/ManagementFooter')
    }
  }
  </script>
  
  <style scoped>
  .blog-management-container {
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
    .blog-management-container {
      flex-direction: column;
    }
    .main-content {
      margin-left: 0;
    }
  }
  </style>
