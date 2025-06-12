<template>
  <footer class="footer">
    <div class="container">
      <p>© {{ new Date().getFullYear() }} HopeBox. All rights reserved.</p>
    </div>
  </footer>
</template>

<style scoped>
.footer {
  padding: 16px;
  text-align: center;
  background: #1e1e2f;
  color: #ccc;
  font-size: 14px;
  border-top: 1px solid rgba(255, 255, 255, 0.05);
  width: 100%;
  z-index: 10;
}

@media (max-width: 768px) {
  .footer {
    padding: 12px;
    font-size: 12px;
  }
}
</style>