<template>
  <div class="user-management">
    <div class="main-content">
      <div class="header">
        <h2>Quản lý người dùng</h2>
        <button class="add-btn" @click="openAddModal">
          <i class="fas fa-plus"></i> Thêm người dùng
        </button>
      </div>

      <table class="user-table">
        <thead>
          <tr>
            <th>Avatar</th>
            <th>Họ tên</th>
            <th>Email</th>
            <th>SĐT</th>
            <th>Giới tính</th>
            <th>Trạng thái</th>
            <th>Tích điểm</th>
            <th>Tổ chức</th>
            <th>Hành động</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="user in users" :key="user.id">
            <td>
              <img :src="user.avatarUrl || '/images/placeholder.jpg'" class="avatar" alt="Avatar" />
            </td>
            <td>{{ user.fullName }}</td>
            <td>{{ user.email }}</td>
            <td>{{ user.phoneNumber || 'N/A' }}</td>
            <td>{{ getGenderLabel(user.gender) }}</td>
            <td>
              <span :class="getStatusClass(user.userStatus)">
                {{ getStatusLabel(user.userStatus) }}
              </span>
            </td>
            <td>{{ user.point }}</td>
            <td>{{ getOrganizationName(user.organizationId) || 'N/A' }}</td>
            <td>
              <button class="btn edit" @click="editUser(user)">
                <i class="fas fa-edit"></i>
              </button>
              <button class="btn delete" @click="confirmDelete(user.id)">
                <i class="fas fa-trash"></i>
              </button>
            </td>
          </tr>
        </tbody>
      </table>

      <!-- Modal -->
      <div v-if="showModal" class="modal-overlay">
        <div class="modal">
          <h3>{{ editingUser ? 'Cập nhật người dùng' : 'Thêm người dùng' }}</h3>
          <div class="avatar-container">
            <img :src="form.avatarUrl || '/images/placeholder.jpg'" alt="Avatar" class="avatar-preview"
              @click="$refs.fileInput.click()" />
            <input type="file" ref="fileInput" accept="image/*" style="display: none" @change="handleAvatarUpload" />
          </div>
          <div class="form-group">
            <label>Họ tên</label>
            <input v-model="form.fullName" placeholder="Họ tên" />
          </div>
          <div class="form-group">
            <label>Email</label>
            <input v-model="form.email" placeholder="Email" />
          </div>
          <div class="form-group">
            <label>Số điện thoại</label>
            <input v-model="form.phoneNumber" placeholder="Số điện thoại" />
          </div>
          <div class="form-group">
            <label>Ngày sinh</label>
            <input type="date" v-model="form.dateOfBirth" />
          </div>
          <div class="form-group">
            <label>Giới tính</label>
            <select v-model.number="form.gender">
              <option v-for="(label, value) in GenderLabel" :key="value" :value="Number(value)">
                {{ label }}
              </option>
            </select>
          </div>
          <div class="form-group">
            <label>Địa chỉ</label>
            <input v-model="form.address" placeholder="Địa chỉ" />
          </div>
          <div class="form-group">
            <label>Tích điểm</label>
            <input v-model.number="form.point" type="number" placeholder="Tích điểm" min="0" />
          </div>
          <div class="form-group">
            <label>Tổ chức</label>
            <select v-model="form.organizationId">
              <option value="">Không thuộc tổ chức</option>
              <option v-for="org in organizations" :key="org.id" :value="org.id">
                {{ org.name }}
              </option>
            </select>
          </div>
          <div class="form-group">
            <label>Trạng thái</label>
            <select v-model.number="form.userStatus">
              <option v-for="(label, value) in UserStatusLabel" :key="value" :value="Number(value)">
                {{ label }}
              </option>
            </select>
          </div>
          <div class="modal-actions">
            <button @click="saveUser" class="btn btn-success">Lưu</button>
            <button @click="closeModal" class="btn btn-secondary">Hủy</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
  import axios from 'axios';
  import { showSuccessAlertDark, showErrorAlertDark, showConfirmDialogDark } from '@/utils/alertHelper';
  import { UserStatus, UserStatusLabel, Gender, GenderLabel } from '@/enums/enums';
  import { BASE_URL } from '@/utils/constants';

  export default {
    data() {
      return {
        users: [],
        organizations: [],
        showModal: false,
        editingUser: null,
        form: {
          id: '',
          fullName: '',
          email: '',
          phoneNumber: '',
          dateOfBirth: '',
          gender: Gender.UNKNOWN,
          address: '',
          avatarUrl: '',
          organizationId: '',
          userStatus: UserStatus.ACTIVE,
          point: 0
        },
        GenderLabel,
        UserStatusLabel
      };
    },
    methods: {
      async fetchUsers() {
        try {
          const res = await axios.get(`${BASE_URL}/api/User/get-all`, {
            withCredentials: true
          });
          if (res.data.status === 200) {
            this.users = res.data.responseData;
          }
        } catch (err) {
          console.error('Lỗi tải danh sách người dùng:', err);
          await showErrorAlertDark('Lỗi', 'Không thể tải danh sách người dùng.');
        }
      },
      async fetchOrganizations() {
        try {
          const res = await axios.get(`${BASE_URL}/api/Organization/get-all`, {
            withCredentials: true
          });
          if (res.data.status === 200) {
            this.organizations = res.data.responseData;
          }
        } catch (err) {
          console.error('Lỗi tải danh sách tổ chức:', err);
          await showErrorAlertDark('Lỗi', 'Không thể tải danh sách tổ chức.');
        }
      },
      getOrganizationName(organizationId) {
        const org = this.organizations.find(o => o.id === organizationId);
        return org ? org.name : null;
      },
      getGenderLabel(gender) {
        return GenderLabel[gender] || 'Không xác định';
      },
      getStatusLabel(status) {
        return UserStatusLabel[status] || 'Không xác định';
      },
      getStatusClass(status) {
        switch (status) {
          case UserStatus.ACTIVE:
            return 'user-status-active';
          case UserStatus.INACTIVE:
            return 'user-status-inactive';
          case UserStatus.PENDING:
            return 'user-status-pending';
          case UserStatus.SUSPENDED:
            return 'user-status-suspended';
          case UserStatus.BANNED:
            return 'user-status-banned';
          case UserStatus.DELETED:
            return 'user-status-deleted';
          default:
            return '';
        }
      },
      openAddModal() {
        this.editingUser = null;
        this.form = {
          id: '',
          fullName: '',
          email: '',
          phoneNumber: '',
          dateOfBirth: '',
          gender: Gender.UNKNOWN,
          address: '',
          avatarUrl: '',
          organizationId: '',
          userStatus: UserStatus.ACTIVE,
          point: 0
        };
        this.showModal = true;
      },
      editUser(user) {
        this.editingUser = user;
        const dateOfBirth = user.dateOfBirth
          ? new Date(user.dateOfBirth).toISOString().split('T')[0]
          : '';
        this.form = {
          id: user.id || '',
          fullName: user.fullName || '',
          email: user.email || '',
          phoneNumber: user.phoneNumber || '',
          dateOfBirth,
          gender: Number(user.gender) || Gender.UNKNOWN,
          address: user.address || '',
          avatarUrl: user.avatarUrl || '',
          userStatus: Number(user.userStatus) || UserStatus.ACTIVE,
          point: Number(user.point) || 0,
          organizationId: user.organizationId || ''
        };
        this.showModal = true;
      },
      async handleAvatarUpload(event) {
        const file = event.target.files[0];
        if (!file) return;

        try {
          const formData = new FormData();
          formData.append('file', file);

          const userId = this.form.id || this.editingUser?.id || '';

          const res = await axios.post(
            `${BASE_URL}/api/User/admin-change-avatar?userId=${encodeURIComponent(userId)}`,
            formData,
            {
              headers: {
                'Content-Type': 'multipart/form-data'
              },
              withCredentials: true
            }
          );

          if (res.data.status === 200) {
            this.form.avatarUrl = res.data.responseData;
            await showSuccessAlertDark('Thành công', 'Ảnh đại diện đã được cập nhật.');
            this.fetchUsers();
          } else {
            await showErrorAlertDark('Lỗi', res.data.message);
          }
        } catch (err) {
          await showErrorAlertDark('Lỗi', err.message);
        }
      },
      async saveUser() {
        try {
          const payload = {
            ...this.form,
            id: this.form.id || null,
            organizationId: this.form.organizationId || null,
            dateOfBirth: this.form.dateOfBirth ? new Date(this.form.dateOfBirth).toISOString() : null
          };

          const url = this.editingUser
            ? `${BASE_URL}/api/User/update`
            : `${BASE_URL}/api/User/add`;

          const res = await axios.post(url, payload, {
            withCredentials: true
          });

          if ([200, 201].includes(res.data.status)) {
            await this.fetchUsers();
            await showSuccessAlertDark('Thành công', res.data.message);
            this.closeModal();
          } else {
            await showErrorAlertDark('Lỗi', res.data.message);
          }
        } catch (err) {
          await showErrorAlertDark('Lỗi', err.message);
        }
      },
      async confirmDelete(id) {
        const result = await showConfirmDialogDark(
          'Bạn có chắc chắn?',
          'Hành động này sẽ xóa người dùng!',
          'Xóa',
          'Hủy'
        );

        if (result.isConfirmed) {
          try {
            const res = await axios.post(
              `${BASE_URL}/api/User/delete`,
              id,
              {
                headers: { 'Content-Type': 'application/json' },
                withCredentials: true
              }
            );
            if (res.data.status === 200) {
              await this.fetchUsers();
              await showSuccessAlertDark('Đã xóa!', 'Người dùng đã bị xóa.');
            } else {
              showErrorAlertDark('Lỗi', res.data.message);
            }
          } catch (err) {
            await showErrorAlertDark('Lỗi', err.message);
          }
        }
      },
      closeModal() {
        this.showModal = false;
      }
    },
    async mounted() {
      await Promise.all([this.fetchUsers(), this.fetchOrganizations()]);
    }
  };
</script>

<style scoped>
  .user-management {
    display: flex;
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  }

  .main-content {
    margin-left: 250px;
    padding: 30px;
    background: #1a1a2e;
    color: #e0e0e0;
    width: 100%;
    min-height: 100vh;
  }

  .header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 20px;
  }

  .header h2 {
    font-size: 1.8rem;
    color: #f6d579;
  }

  .add-btn {
    background-color: #fbc658;
    color: #111226;
    border: none;
    padding: 10px 16px;
    border-radius: 8px;
    font-weight: bold;
    cursor: pointer;
    transition: background-color 0.3s;
  }

  .add-btn:hover {
    background-color: #e0b450;
  }

  .user-table {
    width: 100%;
    border-collapse: collapse;
    background: #222238;
    border-radius: 8px;
    overflow: hidden;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.3);
  }

  .user-table th,
  .user-table td {
    padding: 14px;
    text-align: left;
    border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  }

  .user-table th {
    background: #292b40;
    color: #f6d579;
    font-weight: 600;
  }

  .user-table tbody tr:hover {
    background: #2a2a42;
  }

  .avatar {
    width: 40px;
    height: 40px;
    border-radius: 50%;
    object-fit: cover;
  }

  .user-status-active {
    color: #28a745;
  }

  .user-status-inactive {
    color: #6c757d;
  }

  .user-status-pending {
    color: #e6950b;
  }

  .user-status-suspended {
    color: #ff9800;
  }

  .user-status-banned {
    color: #dc3545;
  }

  .user-status-deleted {
    color: #6c757d;
  }

  .btn {
    border: none;
    padding: 8px;
    border-radius: 6px;
    cursor: pointer;
    transition: background-color 0.3s;
  }

  .edit {
    background-color: #36a3f7;
    color: white;
    margin-right: 8px;
  }

  .edit:hover {
    background-color: #2e8bd4;
  }

  .delete {
    background-color: #ef8157;
    color: white;
  }

  .delete:hover {
    background-color: #d66f4b;
  }

  .modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: rgba(0, 0, 0, 0.7);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 1000;
  }

  .modal {
    background: #222238;
    padding: 24px;
    border-radius: 12px;
    width: 600px;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.4);
    display: flex;
    flex-direction: column;
    gap: 16px;
    overflow-y: auto;
  }

  .modal h3 {
    color: #f6d579;
    margin: 0;
    font-size: 1.5rem;
  }

  .avatar-container {
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 10px;
  }

  .avatar-preview {
    width: 100px;
    height: 100px;
    border-radius: 50%;
    object-fit: cover;
    cursor: pointer;
    border: 2px solid #f6d579;
  }

  .change-avatar-btn {
    background: #6c757d;
    color: white;
    border: none;
    padding: 8px 16px;
    border-radius: 6px;
    cursor: pointer;
    transition: background-color 0.3s;
  }

  .change-avatar-btn:hover {
    background: #5a6268;
  }

  .form-group {
    display: flex;
    flex-direction: column;
    gap: 8px;
  }

  .form-group label {
    color: #e0e0e0;
    font-weight: 500;
  }

  .modal input,
  .modal select {
    padding: 10px;
    border: none;
    border-radius: 6px;
    background: #292b40;
    color: #e0e0e0;
    font-size: 1rem;
    transition: border-color 0.3s;
  }

  .modal input:focus,
  .modal select:focus {
    outline: none;
    border: 1px solid #f6d579;
  }

  .modal-actions {
    display: flex;
    justify-content: flex-end;
    gap: 12px;
    margin-top: 16px;
  }

  .btn-success {
    background: #28a745;
    color: white;
    padding: 10px 20px;
    border-radius: 6px;
  }

  .btn-success:hover {
    background: #218838;
  }

  .btn-secondary {
    background: #6c757d;
    color: white;
    padding: 10px 20px;
    border-radius: 6px;
  }

  .btn-secondary:hover {
    background: #5a6268;
  }

  .modal::-webkit-scrollbar,
  .modal-content::-webkit-scrollbar {
    width: 8px;
  }

  .modal::-webkit-scrollbar-track,
  .modal-content::-webkit-scrollbar-track {
    background: #1e1e2f;
  }

  .modal::-webkit-scrollbar-thumb,
  .modal-content::-webkit-scrollbar-thumb {
    background: #4a4a6a;
    border-radius: 4px;
  }

  .modal::-webkit-scrollbar-thumb:hover,
  .modal-content::-webkit-scrollbar-thumb:hover {
    background: #5a5a7a;
  }
</style>