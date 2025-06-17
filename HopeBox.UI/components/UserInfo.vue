<template>
  <div class="user-info-container">
    <div class="user-info" v-if="user">
      <div class="user-header row">
        <div class="col-md-8 d-flex align-items-center">
          <div class="user-title">
            <h2>Thông Tin Người Dùng</h2>
            <ul>
              <li><strong>Tên:</strong> {{ user.fullName || user.username || 'Chưa cập nhật' }}</li>
              <li><strong>Email:</strong> {{ user.email || 'Chưa cập nhật' }}</li>
              <li><strong>Số điện thoại:</strong> {{ user.phoneNumber || 'Chưa cập nhật' }}</li>
              <li><strong>Ngày sinh:</strong> {{ formattedDateOfBirth }}</li>
              <li><strong>Giới tính:</strong> {{ genderText }}</li>
              <li><strong>Điểm:</strong> {{ user.point !== undefined ? user.point : 'Chưa cập nhật' }}</li>
              <li><strong>Địa chỉ:</strong> {{ user.address || 'Chưa cập nhật' }}</li>
            </ul>
            <button class="edit-profile-btn" @click="openEditModal">Chỉnh sửa thông tin</button>
          </div>
        </div>
        <div class="col-md-4 text-md-end text-center">
          <div class="user-avatar d-inline-block" @click="triggerFileInput">
            <img :src="user.avatarUrl || defaultAvatar" alt="Avatar" />
            <div class="avatar-overlay">
              <span>Thay đổi ảnh</span>
            </div>
          </div>
          <input type="file" ref="fileInput" accept="image/*" style="display: none" @change="uploadAvatar" />
        </div>
      </div>
    </div>

    <div v-else-if="loading" class="loading">Đang tải thông tin...</div>
    <div v-else class="no-info">Không có thông tin người dùng.</div>

    <!-- Modal for editing profile -->
    <div v-if="showEditModal" class="modal-overlay">
      <div class="modal-content">
        <h3>Chỉnh sửa thông tin cá nhân</h3>
        <form @submit.prevent="submitProfileUpdate">
          <div class="form-group">
            <label for="fullName">Tên đầy đủ</label>
            <input v-model="editForm.fullName" id="fullName" type="text" placeholder="Nhập tên đầy đủ" />
          </div>
          <div class="form-group">
            <label for="phoneNumber">Số điện thoại</label>
            <input v-model="editForm.phoneNumber" id="phoneNumber" type="tel" placeholder="Nhập số điện thoại" />
          </div>
          <div class="form-group">
            <label for="dateOfBirth">Ngày sinh</label>
            <input v-model="editForm.dateOfBirth" id="dateOfBirth" type="date" />
          </div>
          <div class="form-group">
            <label for="gender">Giới tính</label>
            <select v-model="editForm.gender" id="gender">
              <option value="" disabled>Chọn giới tính</option>
              <option v-for="(label, value) in GenderLabel" :key="value" :value="value">{{ label }}</option>
            </select>
          </div>
          <div class="form-group">
            <label for="address">Địa chỉ</label>
            <input v-model="editForm.address" id="address" type="text" placeholder="Nhập địa chỉ" />
          </div>
          <div class="modal-actions">
            <button type="button" class="cancel-btn" @click="closeEditModal">Hủy</button>
            <button type="submit" class="submit-btn">Lưu</button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
  import axios from 'axios';
  import { Gender, GenderLabel } from '@/enums/enums.js';
  import { BASE_URL } from '@/utils/constants';
  import { showSuccessAlert, showErrorAlert } from '@/utils/alertHelper';


  export default {
    data() {
      return {
        user: null,
        loading: true,
        showEditModal: false,
        editForm: {
          fullName: '',
          phoneNumber: '',
          dateOfBirth: '',
          gender: 0,
          address: ''
        },
        defaultAvatar: '/images/placeholder.jpg'
      };
    },
    computed: {
      formattedDateOfBirth() {
        if (!this.user || !this.user.dateOfBirth) return 'Chưa cập nhật';
        const date = new Date(this.user.dateOfBirth);
        return date.toLocaleDateString('vi-VN');
      },
      genderText() {
        if (!this.user || this.user.gender === null || this.user.gender === undefined) return 0;
        return GenderLabel[this.user.gender] || 'Chưa cập nhật';
      },
      GenderLabel() {
        return GenderLabel;
      }
    },
    async mounted() {
      await this.fetchUser();
    },
    methods: {
      async fetchUser() {
        try {
          this.loading = true;
          const res = await axios.get(`${BASE_URL}/api/Authentication/me`, {
            withCredentials: true
          });
          this.user = res.data.responseData;
          this.resetEditForm();
        } catch (err) {
          console.error('Lỗi khi lấy thông tin người dùng:', err);
          this.$toast.error('Không thể tải thông tin người dùng');
        } finally {
          this.loading = false;
        }
      },
      triggerFileInput() {
        this.$refs.fileInput.click();
      },
      async uploadAvatar(event) {
        const file = event.target.files[0];
        if (!file) return;

        const formData = new FormData();
        formData.append('file', file);

        try {
          const res = await axios.post(`${BASE_URL}/api/User/change-avatar`, formData, {
            withCredentials: true,
            headers: { 'Content-Type': 'multipart/form-data' }
          });

          if (res.data.status === 200) {
            this.user.avatarUrl = res.data.responseData;
            await showSuccessAlert('Thành công', res.data.message || 'Đổi ảnh đại diện thành công');
          } else {
            await showErrorAlert('Lỗi', res.data.message || 'Đổi ảnh đại diện thất bại');
          }
        } catch (err) {
          await showSuccessAlert('Thành công', 'Lỗi khi đổi ảnh đại diện');
        } finally {
          this.$refs.fileInput.value = '';
        }
      },
      openEditModal() {
        this.resetEditForm();
        this.showEditModal = true;
      },
      closeEditModal() {
        this.showEditModal = false;
      },
      resetEditForm() {
        this.editForm = {
          fullName: this.user?.fullName || '',
          phoneNumber: this.user?.phoneNumber || '',
          dateOfBirth: this.user?.dateOfBirth ? new Date(this.user.dateOfBirth).toISOString().split('T')[0] : '',
          gender: this.user?.gender !== undefined ? this.user.gender : 0,
          address: this.user?.address || ''
        };
      },
      async submitProfileUpdate() {
        try {
          const payload = {
            ...this.editForm,
            gender: Number(this.editForm.gender),
          };

          const res = await axios.post(`${BASE_URL}/api/User/update-user-info`, payload, {
            withCredentials: true
          });

          if (res.data.status === 200) {
            await showSuccessAlert('Thành công', res.data.message || 'Cập nhật thông tin thành công');
            await this.fetchUser();
            this.closeEditModal();
          } else {
            await showErrorAlert('Lỗi', res.data.message || 'Không thể cập nhật thông tin');
          }
        } catch (err) {
          console.error('Lỗi khi cập nhật thông tin:', err);
          await showErrorAlert('Lỗi', 'Đã xảy ra lỗi khi cập nhật thông tin');
        }
      }
    }
  };
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
    position: relative;
    margin-right: 30px;
    cursor: pointer;
  }

  .user-avatar img {
    width: 300px;
    height: 400px;
    border-radius: 20px;
    object-fit: cover;
    border: 5px solid #ff6200;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
    transition: transform 0.3s ease, opacity 0.3s ease;
  }

  .user-avatar:hover img {
    transform: scale(1.05);
    opacity: 0.7;
  }

  .avatar-overlay {
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: rgba(0, 0, 0, 0.5);
    border-radius: 15px;
    display: flex;
    align-items: center;
    justify-content: center;
    opacity: 0;
    transition: opacity 0.3s ease;
    color: #fff;
    font-size: 16px;
    font-weight: bold;
  }

  .user-avatar:hover .avatar-overlay {
    opacity: 1;
  }

  .user-title h2 {
    font-size: 36px;
    margin: 0 0 10px 0;
    color: #333;
    border-bottom: 3px solid #ff6200;
    padding-bottom: 15px;
  }

  .user-info ul {
    list-style: none;
    padding: 0;
    margin: 0 0 20px 0;
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

  .edit-profile-btn {
    background: #ff6200;
    color: #fff;
    border: none;
    padding: 10px 20px;
    border-radius: 8px;
    font-size: 16px;
    cursor: pointer;
    transition: background 0.3s ease;
  }

  .edit-profile-btn:hover {
    background: #e55a00;
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

  /* Modal Styles */
  .modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 1000;
  }

  .modal-content {
    background: #fff;
    border-radius: 12px;
    padding: 30px;
    width: 100%;
    max-width: 500px;
    box-shadow: 0 8px 24px rgba(0, 0, 0, 0.2);
  }

  .modal-content h3 {
    margin: 0 0 20px;
    color: #333;
    border-bottom: 2px solid #ff6200;
    padding-bottom: 10px;
  }

  .form-group {
    margin-bottom: 20px;
  }

  .form-group label {
    display: block;
    margin-bottom: 5px;
    color: #333;
    font-weight: bold;
  }

  .form-group input,
  .form-group select {
    width: 100%;
    padding: 10px;
    border: 1px solid #ccc;
    border-radius: 8px;
    font-size: 16px;
  }

  .form-group input:focus,
  .form-group select:focus {
    outline: none;
    border-color: #ff6200;
    box-shadow: 0 0 5px rgba(255, 98, 0, 0.3);
  }

  .modal-actions {
    display: flex;
    justify-content: flex-end;
    gap: 10px;
  }

  .cancel-btn,
  .submit-btn {
    padding: 10px 20px;
    border: none;
    border-radius: 8px;
    font-size: 16px;
    cursor: pointer;
    transition: background 0.3s ease;
  }

  .cancel-btn {
    background: #ccc;
    color: #333;
  }

  .cancel-btn:hover {
    background: #bbb;
  }

  .submit-btn {
    background: #ff6200;
    color: #fff;
  }

  .submit-btn:hover {
    background: #e55a00;
  }

  .error-message {
    color: #d32f2f;
    margin-top: 10px;
  }

  .success-message {
    color: #2e7d32;
    margin-top: 10px;
  }

  /* Responsive Adjustments */
  @media (max-width: 768px) {
    .user-info-container {
      padding: 20px;
    }

    .user-info {
      padding: 30px;
    }

    .user-avatar img {
      width: 150px;
      height: 150px;
    }

    .user-title h2 {
      font-size: 28px;
    }

    .user-info li {
      font-size: 16px;
    }

    .modal-content {
      margin: 0 20px;
    }
  }
</style>