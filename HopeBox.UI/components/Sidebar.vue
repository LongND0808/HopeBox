<template>
  <div class="sidebar" :class="{ 'sidebar-active': isSidebarOpen }">
    <div class="sidebar-wrapper">
      <div class="logo">
        <nuxt-link to="/management" class="logo-text">
          HopeBox
        </nuxt-link>
      </div>
      <ul class="nav">
        <li>
          <nuxt-link to="/management" active-class="active-link">
            <i class="fas fa-tachometer-alt"></i>
            <p>Thống kê chung</p>
          </nuxt-link>
        </li>
        <li>
          <nuxt-link to="/management-users" active-class="active-link">
            <i class="fas fa-users"></i>
            <span>Quản lý người dùng</span>
          </nuxt-link>
        </li>
        <li>
          <nuxt-link to="/management-events" active-class="active-link">
            <i class="fas fa-calendar-alt"></i>
            <span>Quản lý sự kiện</span>
          </nuxt-link>
        </li>
        <li>
          <nuxt-link to="/management-blogs" active-class="active-link">
            <i class="fas fa-blog"></i>
            <span>Quản lý blog</span>
          </nuxt-link>
        </li>
        <li>
          <nuxt-link to="/management-causes" active-class="active-link">
            <i class="fas fa-hand-holding-heart"></i>
            <p>Thông tin chiến dịch</p>
          </nuxt-link>
        </li>
        <li>
          <nuxt-link to="/management-volunteers" active-class="active-link">
            <i class="fas fa-hands-helping"></i>
            <p>Thông tin tình nguyện viên</p>
          </nuxt-link>
        </li>
      </ul>

      <div class="logout-section">
        <button class="logout-btn" @click="logout">
          <i class="fas fa-sign-out-alt"></i>
          <span>Đăng xuất</span>
        </button>
      </div>
    </div>
  </div>
  <button class="hamburger" @click="toggleSidebar">
    <i class="fas fa-bars"></i>
  </button>
</template>

<script>
import axios from 'axios';
import { BASE_URL } from '@/utils/constants';


export default {
  data() {
    return {
      isSidebarOpen: false,
    }
  },
  methods: {
    async logout() {
      try {
        await axios.post(`${BASE_URL}/api/Authentication/logout`, {}, {
          withCredentials: true
        })
        this.$router.push('/')
      } catch (error) {
        console.error('Lỗi khi đăng xuất:', error)
      }
    },
    toggleSidebar() {
      this.isSidebarOpen = !this.isSidebarOpen
    },
  },
}
</script>

<style scoped>
.sidebar {
  background: linear-gradient(180deg, #1e1e2f, #27293d);
  color: white;
  width: 250px;
  height: 100vh;
  position: fixed;
  box-shadow: 2px 0 10px rgba(0, 0, 0, 0.4);
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  font-family: 'Segoe UI', Roboto, sans-serif;
  transition: transform 0.3s ease;
  z-index: 1000;
}

.sidebar-wrapper {
  padding-top: 20px;
  display: flex;
  flex-direction: column;
  height: 100%;
}

.logo {
  text-align: center;
  padding: 20px 0;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

.logo-text {
  color: #fbc658;
  font-size: 24px;
  font-weight: bold;
  text-transform: uppercase;
  letter-spacing: 1px;
  text-decoration: none;
  transition: 0.3s ease;
}

.logo-text:hover {
  opacity: 0.9;
}

.nav {
  list-style: none;
  padding: 0;
  margin: 20px 0;
  flex-grow: 1;
}

.nav li a {
  display: flex;
  align-items: center;
  padding: 12px 20px;
  margin: 6px 10px;
  border-radius: 12px;
  color: #e4e4e4;
  transition: all 0.3s ease;
  font-weight: 500;
  text-decoration: none;
}

.nav li a i {
  margin-right: 12px;
  font-size: 16px;
  width: 20px;
  text-align: center;
}

.nav li a:hover {
  background: rgba(251, 198, 88, 0.1);
  color: #fbc658;
  box-shadow: 0 0 10px rgba(251, 198, 88, 0.2);
}

.active-link {
  background: rgba(251, 198, 88, 0.15);
  color: #fbc658 !important;
  font-weight: 600;
  box-shadow: inset 2px 0 0 #fbc658;
}

.logout-section {
  padding: 20px;
  border-top: 1px solid rgba(255, 255, 255, 0.1);
}

.logout-btn {
  width: 100%;
  background: transparent;
  border: none;
  color: #e4e4e4;
  display: flex;
  align-items: center;
  gap: 10px;
  font-size: 16px;
  padding: 10px 0;
  cursor: pointer;
  transition: 0.3s ease;
}

.logout-btn:hover {
  color: #fbc658;
}

.hamburger {
  display: none;
  position: fixed;
  top: 20px;
  left: 20px;
  background: #fbc658;
  border: none;
  color: #1e1e2f;
  font-size: 20px;
  padding: 10px;
  border-radius: 8px;
  cursor: pointer;
  z-index: 1100;
}

@media (max-width: 1024px) {
  .sidebar {
    width: 250px;
  }
  .logo-text {
    font-size: 20px;
  }
  .nav li a {
    padding: 10px 15px;
    font-size: 14px;
  }
}

@media (max-width: 768px) {
  .sidebar {
    width: 250px;
    transform: translateX(-100%);
  }
  .sidebar-active {
    transform: translateX(0);
  }
  .hamburger {
    display: block;
  }
}
</style>