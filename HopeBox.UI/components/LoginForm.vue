<template>
  <div class="login-container">
    <div class="login-box">
      <div class="tab-switch">
        <button :class="{ active: activeTab === 'user' }" @click="activeTab = 'user'">
          Người Dùng
        </button>
        <button :class="{ active: activeTab === 'admin' }" @click="activeTab = 'admin'">
          Quản Lý
        </button>
      </div>

      <h2 class="title">Đăng Nhập {{ activeTab === 'admin' ? 'Quản Lý' : '' }}</h2>

      <form @submit.prevent="handleLogin">
        <div class="form-group">
          <label for="username">Tên đăng nhập</label>
          <input type="text" id="username" v-model="form.username" placeholder="Nhập tên đăng nhập" required />
        </div>

        <div class="form-group">
          <label for="password">Mật khẩu</label>
          <input type="password" id="password" v-model="form.password" placeholder="Nhập mật khẩu" required />
        </div>

        <button type="submit" class="btn-login">Đăng Nhập</button>

        <div class="signup-link">
          <span>Chưa có tài khoản?</span>
          <nuxt-link to="/register">Đăng ký</nuxt-link>
        </div>

        <div class="signup-link">
          <nuxt-link to="/activate-account">Kích hoạt tài khoản</nuxt-link>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
  import axios from 'axios'
  import { showSuccessAlert, showErrorAlert } from '@/utils/alertHelper'

  export default {
    data() {
      return {
        activeTab: 'user',
        form: {
          username: '',
          password: ''
        },
        error: '',
        loading: false
      }
    },
    methods: {
      async handleLogin() {
        this.loading = true
        try {
          const endpoint =
            this.activeTab === 'admin'
              ? 'https://hopebox-api.roz.io.vn/api/Authentication/admin-login'
              : 'https://hopebox-api.roz.io.vn/api/Authentication/login'

          const response = await axios.post(
            endpoint,
            {
              loginEmail: this.form.username,
              password: this.form.password
            },
            { withCredentials: true }
          )

          if (response.data.status === 200) {
            await showSuccessAlert('Đăng nhập thành công', response.data.message || 'Chào mừng bạn!')
            if (this.activeTab === 'admin') {
              this.$router.push('/management')
            } else {
              this.$router.push('/')
            }
          } else {
            await showErrorAlert('Đăng nhập thất bại', response.data.message || 'Vui lòng kiểm tra lại thông tin.')
          }
        } catch (err) {
          await showErrorAlert(
            'Lỗi hệ thống',
            err.response?.data?.message || 'Đã xảy ra lỗi không xác định khi đăng nhập.'
          )
        } finally {
          this.loading = false
        }
      }
    }
  }
</script>

<style scoped>
  .login-container {
    display: flex;
    justify-content: center;
    align-items: center;
    padding: 100px 0;
    background-color: #fef9f6;
    min-height: 70vh;
  }

  .login-box {
    background: white;
    padding: 40px 30px;
    border-radius: 20px;
    box-shadow: 0 15px 30px rgba(0, 0, 0, 0.05);
    max-width: 420px;
    width: 100%;
  }

  .tab-switch {
    display: flex;
    justify-content: space-around;
    margin-bottom: 20px;
  }

  .tab-switch button {
    flex: 1;
    padding: 10px;
    background-color: #eee;
    border: none;
    border-radius: 10px 10px 10px 10px;
    font-weight: bold;
    cursor: pointer;
    transition: 0.3s;
    color: rgb(187, 187, 187);
  }

  .tab-switch button.active {
    background-color: white;
    color: #ff7a18;
  }

  .title {
    text-align: center;
    font-size: 26px;
    font-weight: 700;
    color: #232323;
    margin-bottom: 20px;
  }

  .form-group {
    margin-bottom: 20px;
  }

  label {
    font-weight: 600;
    margin-bottom: 8px;
    color: #444;
    display: block;
  }

  input {
    width: 100%;
    padding: 10px 15px;
    border: 1px solid #ddd;
    border-radius: 10px;
    transition: 0.3s;
  }

  input:focus {
    outline: none;
    border-color: #ff7a18;
    box-shadow: 0 0 0 3px rgba(255, 122, 24, 0.1);
  }

  .btn-login {
    width: 100%;
    padding: 12px;
    background: linear-gradient(to right, #ff7a18, #ffb347);
    border: none;
    border-radius: 30px;
    color: white;
    font-weight: bold;
    font-size: 16px;
    cursor: pointer;
    margin-top: 10px;
    transition: 0.3s;
  }

  .btn-login:hover {
    opacity: 0.9;
  }

  .signup-link {
    margin-top: 20px;
    text-align: center;
    font-size: 14px;
  }

  .signup-link a {
    color: #ff7a18;
    font-weight: bold;
    margin-left: 5px;
    text-decoration: none;
  }

  .error-message {
    margin-top: 15px;
    color: #d63031;
    text-align: center;
    font-weight: 600;
  }
</style>