<template>
  <div class="register-container">
    <div class="register-box">
      <h2 class="title">Tạo Tài Khoản</h2>

      <form @submit.prevent="handleRegister">
        <div class="form-group">
          <label for="fullName">Họ và Tên</label>
          <input type="text" id="fullName" v-model="form.fullName" required />
        </div>

        <div class="form-group">
          <label for="email">Email</label>
          <input type="email" id="email" v-model="form.email" required />
        </div>

        <div class="form-group">
          <label for="phoneNumber">Số điện thoại</label>
          <input type="text" id="phoneNumber" v-model="form.phoneNumber" required />
        </div>

        <div class="form-group">
          <label for="address">Địa chỉ</label>
          <input type="text" id="address" v-model="form.address" />
        </div>

        <div class="form-group">
          <label for="dateOfBirth">Ngày sinh</label>
          <input type="date" id="dateOfBirth" v-model="form.dateOfBirth" required />
        </div>

        <div class="form-group">
          <label for="gender">Giới tính</label>
          <select id="gender" v-model.number="form.gender" required>
            <option value="" disabled>Chọn giới tính</option>
            <option v-for="(label, key) in genderOptions" :key="key" :value="Number(key)">
              {{ label }}
            </option>
          </select>
        </div>

        <div class="form-group">
          <label for="password">Mật khẩu</label>
          <input type="password" id="password" v-model="form.password" required />
        </div>

        <div class="form-group">
          <label for="confirmPassword">Xác nhận mật khẩu</label>
          <input type="password" id="confirmPassword" v-model="form.confirmPassword" required />
        </div>

        <button type="submit" class="btn-register">Đăng Ký</button>

        <div v-if="message" class="message">{{ message }}</div>
      </form>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import { Gender, GenderLabel } from '@/enums/enums.js'

export default {
  data() {
    return {
      form: {
        fullName: '',
        address: '',
        dateOfBirth: '',
        gender: '',
        email: '',
        phoneNumber: '',
        password: '',
        confirmPassword: ''
      },
      message: ''
    }
  },
  computed: {
    genderOptions() {
      return GenderLabel
    }
  },
  methods: {
    async handleRegister() {
      if (this.form.password !== this.form.confirmPassword) {
        this.message = 'Mật khẩu và xác nhận mật khẩu không khớp.'
        return
      }

      try {
        const { confirmPassword, ...formData } = this.form

        const response = await axios.post(
          'https://localhost:7213/api/Authentication/register',
          formData
        )

        if (response.data.status === 200) {
          this.message = 'Đăng ký thành công! Chuyển hướng...'
          setTimeout(() => this.$router.push('/login'), 1500)
        } else {
          this.message = response.data.message || 'Có lỗi xảy ra khi đăng ký.'
        }

      } catch (err) {
        this.message = err.response?.data?.message || 'Có lỗi xảy ra khi đăng ký.'
      }
    }
  }
}
</script>

<style scoped>
    .register-container {
        display: flex;
        justify-content: center;
        align-items: center;
        padding: 100px 0;
        background-color: #fef9f6;
        min-height: 70vh;
    }

    .register-box {
        background: white;
        padding: 40px 30px;
        border-radius: 20px;
        box-shadow: 0 15px 30px rgba(0, 0, 0, 0.05);
        max-width: 500px;
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

    input,
    select {
        width: 100%;
        padding: 10px 15px;
        border: 1px solid #ddd;
        border-radius: 10px;
        transition: 0.3s;
    }

    input:focus,
    select:focus {
        outline: none;
        border-color: #ff7a18;
        box-shadow: 0 0 0 3px rgba(255, 122, 24, 0.1);
    }

    .btn-register {
        width: 100%;
        padding: 12px;
        background: linear-gradient(to right, #ff7a18, #ffb347);
        border: none;
        border-radius: 30px;
        color: white;
        font-weight: bold;
        font-size: 16px;
        cursor: pointer;
        transition: 0.3s;
    }

    .btn-register:hover {
        opacity: 0.9;
    }

    .message {
        margin-top: 15px;
        color: #27ae60;
        text-align: center;
        font-weight: 600;
    }
</style>