<template>
  <div class="login-container">
    <div class="login-box">
      <h2 class="title">Đăng Nhập</h2>

      <div v-if="error" class="error-message">{{ error }}</div>

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
          <nuxt-link to="/register">Kích hoạt tài khoản</nuxt-link>
        </div>

      </form>
    </div>
  </div>
</template>

<script>
  import axios from 'axios'

  export default {
    data() {
      return {
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
        this.error = ''
        try {
          const response = await axios.post(
            'https://localhost:7213/api/Authentication/login',
            {
              loginEmail: this.form.username,
              password: this.form.password
            },
            { withCredentials: true }
          )

          if (response.data.status === 200) {
            this.$router.push('/')
          } else {
            this.error = response.data.message || 'Đăng nhập thất bại.'
          }
        } catch (err) {
          if (err.response && err.response.data && err.response.data.message) {
            this.error = err.response.data.message
          } else {
            this.error = 'Đã xảy ra lỗi khi đăng nhập.'
          }
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

  .title {
    text-align: center;
    font-size: 28px;
    font-weight: 700;
    color: #232323;
    margin-bottom: 30px;
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