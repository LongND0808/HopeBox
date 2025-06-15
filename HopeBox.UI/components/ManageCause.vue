<template>
  <div class="cause-management">
    <div class="main-content">
      <div class="header">
        <h2>Quản lý chiến dịch</h2>
        <button class="add-btn" @click="openAddModal">+ Thêm chiến dịch</button>
      </div>

      <table class="cause-table">
        <thead>
          <tr>
            <th>Tiêu đề</th>
            <th>Loại</th>
            <th>Trạng thái</th>
            <th>Mục tiêu (VND)</th>
            <th>Hiện tại (VND)</th>
            <th>Ngày bắt đầu</th>
            <th>Ngày kết thúc</th>
            <th>Tổ chức</th>
            <th>Người tạo</th>
            <th>Hành động</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="cause in causes" :key="cause.id">
            <td>{{ cause.title }}</td>
            <td>{{ getCauseTypeLabel(cause.type) }}</td>
            <td>{{ getCauseStatusLabel(cause.status) }}</td>
            <td>{{ formatCurrency(cause.targetAmount) }}</td>
            <td>{{ formatCurrency(cause.currentAmount) }}</td>
            <td>{{ formatDate(cause.startDate) }}</td>
            <td>{{ formatDate(cause.endDate) }}</td>
            <td>{{ getOrganizationName(cause.organizationId) || 'N/A' }}</td>
            <td>{{ getUserName(cause.createdBy) || 'N/A' }}</td>
            <td>
              <button class="btn edit" @click="editCause(cause)"><i class="fa fa-edit"></i></button>
              <button class="btn delete" @click="confirmDelete(cause.id)"><i class="fa fa-trash"></i></button>
            </td>
          </tr>
        </tbody>
      </table>

      <!-- Modal -->
      <div v-if="showModal" class="modal-overlay">
        <div class="modal">
          <h3>{{ editingCause ? 'Cập nhật chiến dịch' : 'Thêm chiến dịch' }}</h3>
          <input v-model="form.title" placeholder="Tiêu đề" required />
          <textarea v-model="form.description" placeholder="Mô tả"></textarea>
          <textarea v-model="form.detail" placeholder="Chi tiết"></textarea>
          <input v-model="form.heroImage" placeholder="URL ảnh chính" />
          <textarea v-model="form.challenge" placeholder="Thách thức"></textarea>
          <input v-model="form.challengeImage" placeholder="URL ảnh thách thức" />
          <textarea v-model="form.summary" placeholder="Tóm tắt"></textarea>
          <input v-model="form.summaryImage" placeholder="URL ảnh tóm tắt" />
          <select v-model.number="form.type">
            <option v-for="option in CausesTypeOptions" :key="option.value" :value="option.value">
              {{ option.label }}
            </option>
          </select>
          <input type="date" v-model="form.startDate" />
          <input type="date" v-model="form.endDate" />
          <input v-model.number="form.targetAmount" type="number" placeholder="Mục tiêu (VND)" min="0" />
          <input v-model.number="form.currentAmount" type="number" placeholder="Hiện tại (VND)" min="0" />
          <select v-model.number="form.status">
            <option v-for="(label, value) in CausesStatusLabel" :key="value" :value="Number(value)">
              {{ label }}
            </option>
          </select>
          <select v-model="form.organizationId" @change="onOrganizationChange" required>
            <option value="" disabled>Chọn tổ chức</option>
            <option v-for="org in organizations" :key="org.id" :value="org.id">
              {{ org.name }}
            </option>
          </select>
          <select v-model="form.createdBy" :disabled="!form.organizationId" required>
            <option value="" disabled>Chọn người tạo</option>
            <option v-for="user in filteredUsers" :key="user.id" :value="user.id">
              {{ user.fullName }}
            </option>
          </select>
          <div class="modal-actions">
            <button @click="saveCause" class="btn btn-success" :disabled="!form.organizationId || !form.createdBy">Lưu</button>
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
import { CausesStatus, CausesStatusLabel, CausesType, CausesTypeOptions, CausesTypeLabel } from '@/enums/enums';
import { BASE_URL } from '@/utils/constants';

export default {
  data() {
    return {
      causes: [],
      users: [],
      organizations: [],
      showModal: false,
      editingCause: null,
      form: {
        id: '',
        title: '',
        description: '',
        detail: '',
        heroImage: '',
        challenge: '',
        challengeImage: '',
        summary: '',
        summaryImage: '',
        type: CausesType.WATER,
        startDate: '',
        endDate: '',
        targetAmount: 0,
        currentAmount: 0,
        status: CausesStatus.PENDING,
        createdBy: '',
        organizationId: ''
      },
      CausesStatusLabel,
      CausesTypeOptions
    };
  },
  computed: {
    filteredUsers() {
      if (!this.form.organizationId) return [];
      return this.users.filter(user => user.organizationId === this.form.organizationId);
    }
  },
  methods: {
    async fetchCauses() {
      try {
        const res = await axios.get(`${BASE_URL}/api/Cause/get-all`, {
          withCredentials: true
        });
        if (res.data.status === 200) {
          this.causes = res.data.responseData;
        }
      } catch (err) {
        console.error('Lỗi tải danh sách chiến dịch:', err);
        await showErrorAlertDark('Lỗi', 'Không thể tải danh sách chiến dịch.');
      }
    },
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
    getUserName(userId) {
      const user = this.users.find(u => u.id === userId);
      return user ? user.fullName : null;
    },
    openAddModal() {
      this.editingCause = null;
      this.form = {
        id: '',
        title: '',
        description: '',
        detail: '',
        heroImage: '',
        challenge: '',
        challengeImage: '',
        summary: '',
        summaryImage: '',
        type: CausesType.WATER,
        startDate: '',
        endDate: '',
        targetAmount: 0,
        currentAmount: 0,
        status: CausesStatus.PENDING,
        createdBy: '',
        organizationId: ''
      };
      this.showModal = true;
    },
    editCause(cause) {
      this.editingCause = cause;
      const startDate = cause.startDate
        ? new Date(cause.startDate).toISOString().split('T')[0]
        : '';
      const endDate = cause.endDate
        ? new Date(cause.endDate).toISOString().split('T')[0]
        : '';
      this.form = {
        id: cause.id || '',
        title: cause.title || '',
        description: cause.description || '',
        detail: cause.detail || '',
        heroImage: cause.heroImage || '',
        challenge: cause.challenge || '',
        challengeImage: cause.challengeImage || '',
        summary: cause.summary || '',
        summaryImage: cause.summaryImage || '',
        type: Number(cause.type) || CausesType.WATER,
        startDate,
        endDate,
        targetAmount: Number(cause.targetAmount) || 0,
        currentAmount: Number(cause.currentAmount) || 0,
        status: Number(cause.status) || CausesStatus.PENDING,
        createdBy: cause.createdBy || '',
        organizationId: cause.organizationId || ''
      };
      this.showModal = true;
    },
    onOrganizationChange() {
      this.form.createdBy = '';
    },
    async saveCause() {
      if (!this.form.organizationId || !this.form.createdBy) {
        await showErrorAlertDark('Lỗi', 'Vui lòng chọn tổ chức và người tạo.');
        return;
      }
      try {
        const payload = {
          ...this.form,
          id: this.form.id || null,
          startDate: this.form.startDate ? new Date(this.form.startDate).toISOString() : null,
          endDate: this.form.endDate ? new Date(this.form.endDate).toISOString() : null,
          createdBy: this.form.createdBy || null,
          organizationId: this.form.organizationId || null
        };

        const url = this.editingCause
          ? `${BASE_URL}/api/Cause/update`
          : `${BASE_URL}/api/Cause/add`;

        const res = await axios.post(url, payload, {
          withCredentials: true
        });

        if ([200, 201].includes(res.data.status)) {
          await this.fetchCauses();
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
        'Hành động này sẽ xóa chiến dịch!',
        'Xóa',
        'Hủy'
      );

      if (result.isConfirmed) {
        try {
          const res = await axios.post(
            `${BASE_URL}/api/Cause/delete`,
            id,
            {
              headers: { 'Content-Type': 'application/json' },
              withCredentials: true
            }
          );
          if (res.data.status === 200) {
            await this.fetchCauses();
            await showSuccessAlertDark('Đã xóa!', 'Chiến dịch đã bị xóa.');
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
    getCauseTypeLabel(type) {
      return CausesTypeLabel[type] || 'Không xác định';
    },
    getCauseStatusLabel(status) {
      return CausesStatusLabel[status] || 'Không xác định';
    },
    formatCurrency(amount) {
      return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(amount);
    },
    formatDate(date) {
      return date ? new Date(date).toLocaleDateString('vi-VN') : 'N/A';
    }
  },
  async mounted() {
    await Promise.all([this.fetchCauses(), this.fetchUsers(), this.fetchOrganizations()]);
  }
};
</script>

<style scoped>
  .cause-management {
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

  .cause-table {
    width: 100%;
    border-collapse: collapse;
    background: #1e1e2f;
    border-radius: 8px;
    overflow: hidden;
  }

  .cause-table th,
  .cause-table td {
    padding: 12px;
    text-align: left;
    border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  }

  .cause-table th {
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
    background-color: #296695;
    color: white;
    margin-right: 6px;
  }

  .delete {
    background-color: #985237;
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
  .modal select,
  .modal textarea {
    padding: 10px;
    border: none;
    border-radius: 6px;
    background: #292b40;
    color: white;
  }

  .modal textarea {
    min-height: 80px;
    resize: vertical;
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