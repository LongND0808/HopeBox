<template>
  <div class="event-management">
    <div class="main-content">
      <div class="header">
        <h2>Quản lý sự kiện</h2>
        <button class="add-btn" @click="openAddModal">
          <i class="fas fa-plus"></i> Thêm sự kiện
        </button>
      </div>

      <div class="table-container">
        <table class="event-table">
          <thead>
            <tr>
              <th>Hình ảnh</th>
              <th>Tên sự kiện</th>
              <th>Mô tả</th>
              <th>Ngày bắt đầu</th>
              <th>Ngày kết thúc</th>
              <th>Địa điểm</th>
              <th>Trạng thái</th>
              <th>Hành động</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="event in events" :key="event.id">
              <td data-label="Hình ảnh">
                <img :src="event.bannerImage || '/images/placeholder.jpg'" class="avatar" alt="Hình ảnh sự kiện" />
              </td>
              <td data-label="Tên sự kiện">{{ event.title }}</td>
              <td data-label="Mô tả">{{ event.description || 'N/A' }}</td>
              <td data-label="Ngày bắt đầu">{{ formatDate(event.startDate) }}</td>
              <td data-label="Ngày kết thúc">{{ formatDate(event.endDate) }}</td>
              <td data-label="Địa điểm">{{ event.location || 'N/A' }}</td>
              <td data-label="Trạng thái">
                <span :class="getStatusClass(event.status)">
                  {{ getStatusLabel(event.status) }}
                </span>
              </td>
              <td data-label="Hành động">
                <button class="btn edit" @click="editEvent(event)">
                  <i class="fas fa-edit"></i>
                </button>
                <button class="btn delete" @click="confirmDelete(event.id)">
                  <i class="fas fa-trash"></i>
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Modal -->
      <div v-if="showModal" class="modal-overlay">
        <div class="modal">
          <h3>{{ editingEvent ? 'Cập nhật sự kiện' : 'Thêm sự kiện' }}</h3>
          <div class="avatar-container">
            <img :src="form.bannerImage || '/images/placeholder.jpg'" alt="Hình ảnh sự kiện" class="avatar-preview"
              @click="$refs.fileInput.click()" />
            <input type="file" ref="fileInput" accept="image/*" style="display: none" @change="handleImageUpload" />
          </div>
          <div class="form-group">
            <label>Tên sự kiện</label>
            <input v-model="form.title" placeholder="Tên sự kiện" required />
          </div>
          <div class="form-group">
            <label>Mô tả</label>
            <textarea v-model="form.description" placeholder="Mô tả sự kiện" rows="3"></textarea>
          </div>
          <div class="form-group">
            <label>Chi tiết</label>
            <textarea v-model="form.detail" placeholder="Chi tiết sự kiện" rows="4"></textarea>
          </div>
          <div class="form-group">
            <label>Ngày bắt đầu</label>
            <input type="datetime-local" v-model="form.startDate" required />
          </div>
          <div class="form-group">
            <label>Ngày kết thúc</label>
            <input type="datetime-local" v-model="form.endDate" />
          </div>
          <div class="form-group">
            <label>Địa điểm</label>
            <input v-model="form.location" placeholder="Địa điểm" />
          </div>
          <div class="form-group">
            <label>Trạng thái</label>
            <select v-model.number="form.status" required>
              <option v-for="(label, value) in EventStatusLabel" :key="value" :value="Number(value)">
                {{ label }}
              </option>
            </select>
          </div>
          <div class="modal-actions">
            <button @click="saveEvent" class="btn btn-success" :disabled="!form.title || !form.startDate || !form.status.toString()">Lưu</button>
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
import { BASE_URL } from '@/utils/constants';

const EventStatus = {
  UPCOMING: 0,
  ONGOING: 1,
  COMPLETED: 2,
  CANCELLED: 3
};

const EventStatusLabel = {
  [EventStatus.UPCOMING]: 'Sắp diễn ra',
  [EventStatus.ONGOING]: 'Đang diễn ra',
  [EventStatus.COMPLETED]: 'Đã hoàn thành',
  [EventStatus.CANCELLED]: 'Đã hủy'
};

export default {
  data() {
    return {
      events: [],
      showModal: false,
      editingEvent: null,
      EventStatusLabel,
      form: {
        id: '',
        title: '',
        description: '',
        detail: '',
        startDate: '',
        endDate: '',
        location: '',
        status: EventStatus.UPCOMING,
        bannerImage: '',
        latitude: null,
        longitude: null,
        formattedAddress: '',
        targetAmount: 0,
        currentAmount: 0,
        createdBy: '',
        organizationId: ''
      }
    };
  },
  methods: {
    async fetchEvents() {
      try {
        const response = await axios.get(`${BASE_URL}/api/Event/get-all`, {
          withCredentials: true
        });
        if (response.data.status === 200) {
          this.events = response.data.responseData.map(event => ({
            ...event,
            bannerImage: event.bannerImage || '/images/placeholder.jpg'
          }));
        } else {
          await showErrorAlertDark('Lỗi', response.data.message || 'Không thể tải danh sách sự kiện.');
        }
      } catch (error) {
        await showErrorAlertDark('Lỗi', 'Không thể tải danh sách sự kiện.');
        console.error('Lỗi tải danh sách sự kiện:', error);
      }
    },
    getStatusLabel(status) {
      return EventStatusLabel[status] || 'Không xác định';
    },
    getStatusClass(status) {
      switch (status) {
        case EventStatus.UPCOMING:
          return 'status-pending';
        case EventStatus.ONGOING:
          return 'status-active';
        case EventStatus.COMPLETED:
          return 'status-inactive';
        case EventStatus.CANCELLED:
          return 'status-banned';
        default:
          return '';
      }
    },
    openAddModal() {
      this.editingEvent = null;
      this.form = {
        id: '',
        title: '',
        description: '',
        detail: '',
        startDate: '',
        endDate: '',
        location: '',
        status: EventStatus.UPCOMING,
        bannerImage: '',
        latitude: null,
        longitude: null,
        formattedAddress: '',
        targetAmount: 0,
        currentAmount: 0,
        createdBy: '',
        organizationId: ''
      };
      if (this.$refs.fileInput) this.$refs.fileInput.value = '';
      this.showModal = true;
    },
    editEvent(event) {
      const startDate = event.startDate ? new Date(event.startDate).toISOString().slice(0, 16) : '';
      const endDate = event.endDate ? new Date(event.endDate).toISOString().slice(0, 16) : '';
      this.form = {
        id: event.id || '',
        title: event.title || '',
        description: event.description || '',
        detail: event.detail || '',
        startDate,
        endDate,
        location: event.location || '',
        status: Number(event.status) || EventStatus.UPCOMING,
        bannerImage: event.bannerImage || '',
        latitude: event.latitude || null,
        longitude: event.longitude || null,
        formattedAddress: event.formattedAddress || '',
        targetAmount: event.targetAmount || 0,
        currentAmount: event.currentAmount || 0,
        createdBy: event.createdBy || '',
        organizationId: event.organizationId || ''
      };
      this.editingEvent = event;
      this.showModal = true;
    },
    async handleImageUpload(event) {
      const file = event.target.files[0];
      if (!file) return;

      try {
        const formData = new FormData();
        formData.append('file', file);
        const eventId = this.form.id || this.editingEvent?.id || '';
        const response = await axios.post(
          `${BASE_URL}/api/Event/change-image?eventId=${encodeURIComponent(eventId)}`,
          formData,
          {
            headers: { 'Content-Type': 'multipart/form-data' },
            withCredentials: true
          }
        );

        if (response.data.status === 200 && response.data.responseData) {
          this.form.bannerImage = response.data.responseData;
          await showSuccessAlertDark('Thành công', 'Ảnh sự kiện đã được cập nhật.');
          this.fetchEvents();
        } else {
          await showErrorAlertDark('Lỗi', response.data.message || 'Không thể tải ảnh lên.');
        }
      } catch (error) {
        await showErrorAlertDark('Lỗi', 'Không thể tải ảnh lên.');
        console.error('Lỗi tải ảnh:', error);
      }
    },
    async saveEvent() {
      if (!this.form.title || !this.form.startDate || !this.form.status.toString()) {
        await showErrorAlertDark('Lỗi', 'Vui lòng nhập đầy đủ thông tin bắt buộc: Tên sự kiện, Ngày bắt đầu, Trạng thái.');
        return;
      }

      try {
        const payload = {
          ...this.form,
          id: this.form.id || null,
          startDate: this.form.startDate ? new Date(this.form.startDate).toISOString() : null,
          endDate: this.form.endDate ? new Date(this.form.endDate).toISOString() : null,
          description: this.form.description || null,
          detail: this.form.detail || null,
          location: this.form.location || null,
          bannerImage: this.form.bannerImage || null,
          latitude: this.form.latitude || null,
          longitude: this.form.longitude || null,
          formattedAddress: this.form.formattedAddress || null,
          targetAmount: this.form.targetAmount || 0,
          currentAmount: this.form.currentAmount || 0,
          createdBy: this.form.createdBy || null,
          organizationId: this.form.organizationId || null
        };

        const url = this.editingEvent
          ? `${BASE_URL}/api/Event/update`
          : `${BASE_URL}/api/Event/add`;

        const response = await axios.post(url, payload, {
          withCredentials: true
        });

        if ([200, 201].includes(response.data.status)) {
          await showSuccessAlertDark('Thành công', this.editingEvent ? 'Cập nhật sự kiện thành công!' : 'Thêm sự kiện thành công!');
          this.fetchEvents();
          this.closeModal();
        } else {
          await showErrorAlertDark('Lỗi', response.data.message || 'Không thể lưu thông tin sự kiện.');
        }
      } catch (error) {
        await showErrorAlertDark('Lỗi', 'Không thể lưu thông tin sự kiện.');
        console.error('Lỗi lưu sự kiện:', error);
      }
    },
    async confirmDelete(id) {
      const result = await showConfirmDialogDark(
        'Bạn có chắc chắn?',
        'Hành động này sẽ xóa sự kiện!',
        'Xóa',
        'Hủy'
      );

      if (result.isConfirmed) {
        try {
          const response = await axios.post(
            `${BASE_URL}/api/Event/delete`,
            id,
            {
              headers: { 'Content-Type': 'application/json' },
              withCredentials: true
            }
          );
          if (response.data.status === 200) {
            await showSuccessAlertDark('Thành công', 'Xóa sự kiện thành công!');
            this.fetchEvents();
          } else {
            await showErrorAlertDark('Lỗi', response.data.message || 'Không thể xóa sự kiện.');
          }
        } catch (error) {
          await showErrorAlertDark('Lỗi', 'Không thể xóa sự kiện.');
          console.error('Lỗi xóa sự kiện:', error);
        }
      }
    },
    formatDate(dateStr) {
      if (!dateStr) return 'N/A';
      const date = new Date(dateStr);
      return date.toLocaleString('vi-VN', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit'
      });
    },
    closeModal() {
      this.showModal = false;
      this.editingEvent = null;
      if (this.$refs.fileInput) this.$refs.fileInput.value = '';
    }
  },
  mounted() {
    this.fetchEvents();
  }
};
</script>

<style scoped>
.event-management {
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

.main-content {
  padding: 20px;
  background: #111226;
  color: #e0e0e0;
  width: 100%;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
  flex-wrap: wrap;
  gap: 10px;
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
  display: flex;
  align-items: center;
  gap: 8px;
}

.add-btn:hover {
  background-color: #e0b450;
}

.table-container {
  overflow-x: auto;
  max-width: 1200px;
  margin: 0 auto;
}

.event-table {
  width: 100%;
  border-collapse: collapse;
  background: #222238;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.3);
}

.event-table th,
.event-table td {
  padding: 14px;
  text-align: left;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

.event-table th {
  background: #292b40;
  color: #f6d579;
  font-weight: 600;
  position: sticky;
  top: 0;
  z-index: 1;
}

.event-table tbody tr:hover {
  background: #2a2a42;
}

.avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  object-fit: cover;
}

.status-active {
  color: #28a745;
  font-weight: bold;
}

.status-inactive {
  color: #6c757d;
  font-weight: bold;
}

.status-pending {
  color: #ffc107;
  font-weight: bold;
}

.status-banned {
  color: #dc3545;
  font-weight: bold;
}

.btn {
  background: none;
  border: none;
  color: white;
  cursor: pointer;
  font-size: 1rem;
  padding: 5px 10px;
  border-radius: 4px;
  transition: background 0.3s;
}

.edit {
  color: #ffc107;
}

.edit:hover {
  background: rgba(255, 193, 7, 0.2);
}

.delete {
  color: #dc3545;
}

.delete:hover {
  background: rgba(220, 53, 69, 0.2);
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
  padding: 20px;
  border-radius: 12px;
  width: 90%;
  max-width: 600px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.4);
  display: flex;
  flex-direction: column;
  gap: 16px;
  max-height: 100%;
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
  width: 80px;
  height: 80px;
  border-radius: 50%;
  object-fit: cover;
  cursor: pointer;
  border: 2px solid #f6d579;
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
.modal select,
.modal textarea {
  padding: 10px;
  border: none;
  border-radius: 6px;
  background: #292b40;
  color: #e0e0e0;
  font-size: 1rem;
  transition: border-color 0.3s;
}

.modal input:focus,
.modal select:focus,
.modal textarea:focus {
  outline: none;
  border: 1px solid #f6d579;
}

.modal textarea {
  min-height: 80px;
  resize: vertical;
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
.table-container::-webkit-scrollbar {
  width: 8px;
}

.modal::-webkit-scrollbar-track,
.table-container::-webkit-scrollbar-track {
  background: #1e1e2f;
}

.modal::-webkit-scrollbar-thumb,
.table-container::-webkit-scrollbar-thumb {
  background: #4a4a6a;
  border-radius: 4px;
}

.modal::-webkit-scrollbar-thumb:hover,
.table-container::-webkit-scrollbar-thumb:hover {
  background: #5a5a7a;
}

/* Responsive Styles */
@media (max-width: 1024px) {
  .main-content {
    padding: 15px;
  }

  .header h2 {
    font-size: 1.5rem;
  }

  .add-btn {
    padding: 8px 12px;
    font-size: 0.9rem;
  }

  .event-table th,
  .event-table td {
    padding: 10px;
  }

  .avatar {
    width: 32px;
    height: 32px;
  }
}

@media (max-width: 768px) {
  .main-content {
    padding: 10px;
  }

  .header {
    flex-direction: column;
    align-items: flex-start;
  }

  .event-table {
    font-size: 0.9rem;
  }

  .event-table th,
  .event-table td {
    padding: 8px;
  }

  .event-table thead {
    display: none;
  }

  .event-table tbody,
  .event-table tr,
  .event-table td {
    display: block;
    width: 100%;
  }

  .event-table tr {
    margin-bottom: 15px;
    border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  }

  .event-table td {
    display: flex;
    align-items: center;
    justify-content: flex-start;
    padding: 10px;
    position: relative;
    text-align: left;
  }

  .event-table td::before {
    content: attr(data-label);
    font-weight: bold;
    color: #f6d579;
    width: 40%;
    flex-shrink: 0;
    text-align: left;
  }

  .event-table td:not(:first-child) {
    border-top: none;
  }

  .event-table td:last-child {
    display: flex;
    justify-content: flex-start;
    gap: 8px;
  }

  .avatar {
    width: 50px;
    height: 50px;
  }

  .modal {
    width: 95%;
    padding: 15px;
    max-height: 100%;
  }

  .modal h3 {
    font-size: 1.3rem;
    text-align: left;
  }

  .avatar-container {
    align-items: flex-start;
  }

  .avatar-preview {
    width: 60px;
    height: 60px;
  }

  .modal input,
  .modal select,
  .modal textarea {
    font-size: 0.9rem;
    padding: 8px;
  }

  .modal-actions {
    flex-direction: column;
    justify-content: flex-start;
    align-items: stretch;
    gap: 8px;
  }

  .btn-success,
  .btn-secondary {
    padding: 8px;
    width: 100%;
    text-align: center;
  }
}

@media (max-width: 480px) {
  .main-content {
    padding: 8px;
  }

  .header h2 {
    font-size: 1.2rem;
  }

  .add-btn {
    width: 100%;
    text-align: center;
    padding: 10px;
  }

  .event-table td::before {
    width: 50%;
  }

  .modal {
    width: 98%;
    padding: 10px;
  }
}
</style>