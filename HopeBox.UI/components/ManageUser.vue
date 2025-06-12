<template>
  <div class="user-management">
    <div class="main-content">
      <div class="header">
        <h2>Quản lý người dùng</h2>
        <button class="add-btn" @click="openAddModal">+ Thêm người dùng</button>
      </div>

      <table class="user-table">
        <thead>
          <tr>
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
            <td>{{ user.fullName }}</td>
            <td>{{ user.email }}</td>
            <td>{{ user.phoneNumber || 'N/A' }}</td>
            <td>{{ getGenderLabel(user.gender) }}</td>
            <td>{{ getStatusLabel(user.userStatus) }}</td>
            <td>{{ user.point }}</td>
            <td>{{ getOrganizationName(user.organizationId) || 'N/A' }}</td>
            <td>
              <button class="btn edit" @click="editUser(user)"><i class="fa fa-edit"></i></button>
              <button class="btn delete" @click="confirmDelete(user.id)"><i class="fa fa-trash"></i></button>
            </td>
          </tr>
        </tbody>
      </table>

      <!-- Modal -->
      <div v-if="showModal" class="modal-overlay">
        <div class="modal">
          <h3>{{ editingUser ? 'Cập nhật người dùng' : 'Thêm người dùng' }}</h3>
          <input v-model="form.fullName" placeholder="Họ tên" />
          <input v-model="form.email" placeholder="Email" />
          <input v-model="form.phoneNumber" placeholder="Số điện thoại" />
          <input type="date" v-model="form.dateOfBirth" />
          <select v-model.number="form.gender">
            <option v-for="(label, value) in GenderLabel" :key="value" :value="Number(value)">
              {{ label }}
            </option>
          </select>
          <input v-model="form.address" placeholder="Địa chỉ" />
          <input v-model="form.avatarUrl" placeholder="Avatar URL" />
          <input v-model.number="form.point" type="number" placeholder="Tích điểm" min="0" />
          <select v-model="form.organizationId">
            <option value="">Không thuộc tổ chức</option>
            <option v-for="org in organizations" :key="org.id" :value="org.id">
              {{ org.name }}
            </option>
          </select>
          <select v-model.number="form.userStatus">
            <option v-for="(label, value) in UserStatusLabel" :key="value" :value="Number(value)">
              {{ label }}
            </option>
          </select>
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
        const res = await axios.get('https://localhost:7213/api/User/get-all', {
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
        const res = await axios.get('https://localhost:7213/api/Organization/get-all', {
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
    async saveUser() {
      try {
        const payload = {
          ...this.form,
          id: this.form.id || null,
          organizationId: this.form.organizationId || null,
          dateOfBirth: this.form.dateOfBirth ? new Date(this.form.dateOfBirth).toISOString() : null
        };

        const url = this.editingUser
          ? 'https://localhost:7213/api/User/update'
          : 'https://localhost:7213/api/User/add';

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
            'https://localhost:7213/api/User/delete',
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
            await showErrorAlertDark('Lỗi', res.data.message);
          }
        } catch (err) {
          await showErrorAlertDark('Lỗi', err.message);
        }
      }
    },
    closeModal() {
      this.showModal = false;
    },
    getGenderLabel(gender) {
      return GenderLabel[gender] || 'Không xác định';
    },
    getStatusLabel(status) {
      return UserStatusLabel[status] || 'Không xác định';
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
  }

  .main-content {
    margin-left: 250px;
    padding: 30px;
    background: #111226;
    color: white;
    width: 100%;
    min-height: 100vh;
  }

  .header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 20px;
  }

  .add-btn {
    background-color: #fbc658;
    color: #111226;
    border: none;
    padding: 10px 16px;
    border-radius: 8px;
    font-weight: bold;
    cursor: pointer;
  }

  .user-table {
    width: 100%;
    border-collapse: collapse;
    background: #1e1e2f;
    border-radius: 8px;
    overflow: hidden;
  }

  .user-table th,
  .user-table td {
    padding: 12px;
    text-align: left;
    border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  }

  .user-table th {
    background: #292b40;
  }

  .btn {
    border: none;
    padding: 6px 12px;
    border-radius: 6px;
    font-weight: bold;
    cursor: pointer;
    display: inline-flex;
    align-items: center;
    justify-content: center;
  }

  .edit {
    background-color: #36a3f7;
    color: white;
    margin-right: 6px;
  }

  .delete {
    background-color: #ef8157;
    color: white;
  }

  .modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: rgba(0, 0, 0, 0.6);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 1000;
  }

  .modal {
    background: #1e1e2f;
    padding: 24px;
    border-radius: 12px;
    width: 420px;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.4);
    display: flex;
    flex-direction: column;
    gap: 12px;
  }

  .modal h3 {
    color: #f6d579;
  }

  .modal input,
  .modal select {
    padding: 10px;
    border: none;
    border-radius: 6px;
    background: #292b40;
    color: white;
  }

  .modal-actions {
    display: flex;
    justify-content: flex-end;
    gap: 10px;
    margin-top: 10px;
  }

  .btn-success {
    background: #28a745;
    color: white;
  }

  .btn-secondary {
    background: #6c757d;
    color: white;
  }
</style>