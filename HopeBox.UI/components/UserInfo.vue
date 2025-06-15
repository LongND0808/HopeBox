<template>
  <div class="user-info-container">
    <div class="user-info" v-if="user">
      <div class="user-header row">
        <div class="col-md-8 d-flex align-items-center">
          <div class="user-title">
            <h2>Thông Tin Người Dùng</h2>
            <ul>
              <li><strong>Tên:</strong> {{ user.fullName || user.username }}</li>
              <li><strong>Email:</strong> {{ user.email || 'Chưa cập nhật' }}</li>
              <li><strong>Số điện thoại:</strong> {{ user.phoneNumber || 'Chưa cập nhật' }}</li>
              <li><strong>Ngày sinh:</strong> {{ formattedDateOfBirth }}</li>
              <li><strong>Giới tính:</strong> {{ genderText }}</li>
              <li><strong>Điểm:</strong> {{ user.point !== undefined ? user.point : 'Chưa cập nhật' }}</li>
              <li><strong>Địa chỉ:</strong>
                {{ user.address !== undefined && user.address !== null ? user.address : 'Chưa cập nhật' }}
              </li>
            </ul>
          </div>
        </div>

        <div class="col-md-4 text-md-end text-center" v-if="user.avatarUrl">
          <div class="user-avatar d-inline-block">
            <img :src="user.avatarUrl" alt="Avatar" />
          </div>
        </div>
      </div>
    </div>

    <div v-else-if="loading" class="loading">Đang tải thông tin...</div>
    <div v-else class="no-info">Không có thông tin người dùng.</div>
  </div>
</template>


<script>
  import axios from 'axios';
  import { Gender, GenderLabel } from '@/enums/enums.js';
  import { BASE_URL } from '@/utils/constants';

  export default {
    data() {
      return {
        user: null,
        loading: true,
      }
    },
    computed: {
      formattedDateOfBirth() {
        if (!this.user || !this.user.dateOfBirth) return 'Chưa cập nhật'
        const date = new Date(this.user.dateOfBirth)
        return date.toLocaleDateString('vi-VN')
      },
      genderText() {
        if (!this.user || this.user.gender === null || this.user.gender === undefined) return 'Chưa cập nhật'
        return GenderLabel[this.user.gender] || 'Chưa cập nhật'
      }
    },

    async mounted() {
      try {
        const res = await axios.get(`${BASE_URL}/api/Authentication/me`, {
          withCredentials: true,
        })
        this.user = res.data.responseData
      } catch (err) {
        console.error('Lỗi khi lấy thông tin người dùng:', err)
      } finally {
        this.loading = false
      }
    },
  }
</script>

<style scoped>
  .user-info-container {
    max-width: 1200px;
    margin: 0 auto;
    padding: 40px;
    font-family: 'Arial', sans-serif;
  }

  .user-info {
    background: #ffffff;
    border-radius: 20px;
    box-shadow: 0 8px 24px rgba(0, 0, 0, 0.15);
    padding: 50px;
    text-align: left;
    border-left: 8px solid #ff6200;
    transition: transform 0.3s ease;
  }

  .user-info:hover {
    transform: translateY(-5px);
  }

  .user-header {
    display: flex;
    align-items: center;
    margin-bottom: 30px;
  }

  .user-avatar {
    margin-right: 30px;
  }

  .user-avatar img {
    width: 400px;
    height: 400px;
    border-radius: 50%;
    object-fit: cover;
    border: 5px solid #ff6200;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
    transition: transform 0.3s ease;
  }

  .user-avatar img:hover {
    transform: scale(1.05);
  }

  .user-title h2 {
    font-size: 36px;
    margin: 0 0 10px 0;
    color: #333;
    border-bottom: 3px solid #ff6200;
    padding-bottom: 15px;
  }

  .user-title .user-id {
    font-size: 18px;
    color: #777;
    margin: 0;
  }

  .user-info ul {
    list-style: none;
    padding: 0;
    margin: 0;
  }

  .user-info li {
    margin-bottom: 20px;
    font-size: 20px;
    color: #555;
    display: flex;
    align-items: center;
  }

  .user-info li strong {
    color: #333;
    margin-right: 12px;
    min-width: 150px;
  }

  .loading,
  .no-info {
    text-align: center;
    padding: 40px;
    color: #777;
    background: #f9f9f9;
    border-radius: 12px;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  }
</style>