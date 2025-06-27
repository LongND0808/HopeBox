<template>
  <div class="cause-management">
    <div class="main-content">
      <div class="header">
        <h2>Quản lý chiến dịch</h2>
        <button class="add-btn" @click="openAddModal">+ Thêm chiến dịch</button>
      </div>

      <div class="table-container">
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
              <td data-label="Tiêu đề">{{ cause.title }}</td>
              <td data-label="Loại">{{ getCauseTypeLabel(cause.type) }}</td>
              <td data-label="Trạng thái">{{ getCauseStatusLabel(cause.status) }}</td>
              <td data-label="Mục tiêu (VND)">{{ formatCurrency(cause.targetAmount) }}</td>
              <td data-label="Hiện tại (VND)">{{ formatCurrency(cause.currentAmount) }}</td>
              <td data-label="Ngày bắt đầu">{{ formatDate(cause.startDate) }}</td>
              <td data-label="Ngày kết thúc">{{ formatDate(cause.endDate) }}</td>
              <td data-label="Tổ chức">{{ getOrganizationName(cause.organizationId) || 'N/A' }}</td>
              <td data-label="Người tạo">{{ getUserName(cause.createdBy) || 'N/A' }}</td>
              <td data-label="Hành động">
                <button class="btn edit" @click="editCause(cause)"><i class="fas fa-edit"></i></button>
                <button class="btn delete" @click="confirmDelete(cause.id)"><i class="fas fa-trash"></i></button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Modal for Cause -->
      <div v-if="showModal" class="modal-overlay">
        <div class="modal">
          <h3>{{ editingCause ? 'Cập nhật chiến dịch' : 'Thêm chiến dịch' }}</h3>
          <div class="image-container">
            <div class="image-group">
              <label>Ảnh tiêu điểm</label>
              <img :src="form.heroImage || '/images/placeholder.jpg'" alt="Hero Image" class="image-preview"
                @click="$refs.heroInput.click()" />
              <input type="file" ref="heroInput" accept="image/*" style="display: none"
                @change="handleHeroImageUpload" />
            </div>
            <div class="image-group">
              <label>Ảnh thách thức</label>
              <img :src="form.challengeImage || '/images/placeholder.jpg'" alt="Challenge Image"
                class="image-preview" @click="$refs.challengeInput.click()" />
              <input type="file" ref="challengeInput" accept="image/*" style="display: none"
                @change="handleChallengeImageUpload" />
            </div>
            <div class="image-group">
              <label>Ảnh tổng kết</label>
              <img :src="form.summaryImage || '/images/placeholder.jpg'" alt="Summary Image"
                class="image-preview" @click="$refs.summaryInput.click()" />
              <input type="file" ref="summaryInput" accept="image/*" style="display: none"
                @change="handleSummaryImageUpload" />
            </div>
          </div>
          <div class="form-group">
            <label>Tiêu đề</label>
            <input v-model="form.title" placeholder="Tiêu đề" required />
          </div>
          <div class="form-group">
            <label>Mô tả</label>
            <textarea v-model="form.description" placeholder="Mô tả"></textarea>
          </div>
          <div class="form-group">
            <label>Chi tiết</label>
            <textarea v-model="form.detail" placeholder="Chi tiết"></textarea>
          </div>
          <div class="form-group">
            <label>Thách thức</label>
            <textarea v-model="form.challenge" placeholder="Thách thức"></textarea>
          </div>
          <div class="form-group">
            <label>Tổng kết</label>
            <textarea v-model="form.summary" placeholder="Tóm tắt"></textarea>
          </div>
          <div class="form-group">
            <label>Loại</label>
            <select v-model.number="form.type">
              <option v-for="option in CausesTypeOptions" :key="option.value" :value="option.value">
                {{ option.label }}
              </option>
            </select>
          </div>
          <div class="form-group">
            <label>Ngày bắt đầu</label>
            <input type="date" v-model="form.startDate" />
          </div>
          <div class="form-group">
            <label>Ngày kết thúc</label>
            <input type="date" v-model="form.endDate" />
          </div>
          <div class="form-group">
            <label>Mục tiêu (VND)</label>
            <input v-model.number="form.targetAmount" type="number" placeholder="Mục tiêu (VND)" min="0" />
          </div>
          <div class="form-group">
            <label>Hiện tại (VND)</label>
            <input v-model.number="form.currentAmount" type="number" placeholder="Hiện tại (VND)" min="0" />
          </div>
          <div class="form-group">
            <label>Trạng thái</label>
            <select v-model.number="form.status">
              <option v-for="(label, value) in CausesStatusLabel" :key="value" :value="Number(value)">
                {{ label }}
              </option>
            </select>
          </div>
          <div class="form-group">
            <label>Tổ chức</label>
            <select v-model="form.organizationId" @change="onOrganizationChange" required>
              <option value="" disabled>Chọn tổ chức</option>
              <option v-for="org in organizations" :key="org.id" :value="org.id">
                {{ org.name }}
              </option>
            </select>
          </div>
          <div class="form-group">
            <label>Người tạo</label>
            <select v-model="form.createdBy" :disabled="!form.organizationId" required>
              <option value="" disabled>Chọn người tạo</option>
              <option v-for="user in filteredUsers" :key="user.id" :value="user.id">
                {{ user.fullName }}
              </option>
            </select>
          </div>
          <!-- Relief Packages Management -->
          <div class="form-group">
            <label>Quản lý gói cứu trợ</label>
            <button class="add-btn" @click="openPackageModal(null)">+ Thêm gói cứu trợ</button>
            <div class="table-container" v-if="reliefPackages.length > 0">
              <table class="package-table">
                <thead>
                  <tr>
                    <th>Tên</th>
                    <th>Mô tả</th>
                    <th>Phí bổ sung (VND)</th>
                    <th>Tổng giá (VND)</th>
                    <th>Số lượng mục tiêu</th>
                    <th>Hành động</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="pkg in reliefPackages" :key="pkg.id">
                    <td data-label="Tên">{{ pkg.name }}</td>
                    <td data-label="Mô tả">{{ pkg.description || 'N/A' }}</td>
                    <td data-label="Phí bổ sung (VND)">{{ formatCurrency(pkg.extraFee) }}</td>
                    <td data-label="Tổng giá (VND)">{{ formatCurrency(pkg.totalPrice) }}</td>
                    <td data-label="Số lượng mục tiêu">{{ pkg.targetQuantity }}</td>
                    <td data-label="Hành động">
                      <button class="btn edit" @click="openPackageModal(pkg)"><i class="fas fa-edit"></i></button>
                      <button class="btn delete" @click="confirmDeletePackage(pkg.id)"><i class="fas fa-trash"></i></button>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
            <p v-else class="text-white fst-italic">Chưa có gói cứu trợ nào.</p>
          </div>
          <div class="modal-actions">
            <button @click="saveCause" class="btn btn-success"
              :disabled="!form.organizationId || !form.createdBy">Lưu</button>
            <button @click="closeModal" class="btn btn-secondary">Hủy</button>
          </div>
        </div>
      </div>
      <!-- Modal for Relief Package -->
      <div v-if="showPackageModal" class="modal-overlay">
        <div class="modal">
          <h3>{{ editingPackage ? 'Cập nhật gói cứu trợ' : 'Thêm gói cứu trợ' }}</h3>
          <div class="form-group">
            <label>Tên gói</label>
            <input v-model="packageForm.name" placeholder="Tên gói" required />
          </div>
          <div class="form-group">
            <label>Mô tả</label>
            <textarea v-model="packageForm.description" placeholder="Mô tả"></textarea>
          </div>
          <div class="form-group">
            <label>Phí bổ sung (VND)</label>
            <input v-model.number="packageForm.extraFee" type="number" placeholder="Phí bổ sung" min="0" @input="updateTotalPrice" />
          </div>
          <div class="form-group">
            <label>Số lượng mục tiêu</label>
            <input v-model.number="packageForm.targetQuantity" type="number" placeholder="Số lượng mục tiêu" min="1" />
          </div>
          <div class="form-group">
            <label>Ảnh gói</label>
            <img :src="packageForm.image || '/images/placeholder.jpg'" alt="Package Image" class="image-preview"
              @click="$refs.packageImageInput.click()" />
            <input type="file" ref="packageImageInput" accept="image/*" style="display: none"
              @change="handlePackageImageUpload" />
          </div>
          <div class="form-group">
            <label>Vật phẩm trong gói</label>
            <div class="package-items">
              <div v-for="(item, index) in packageForm.packageItems" :key="index" class="package-item">
                <select v-model="item.reliefItemId" @change="updateTotalPrice">
                  <option value="" disabled>Chọn vật phẩm</option>
                  <option v-for="reliefItem in reliefItems" :key="reliefItem.id" :value="reliefItem.id">
                    {{ reliefItem.itemName }} ({{ formatCurrency(reliefItem.unitPrice) }} / {{ reliefItem.unit }})
                  </option>
                </select>
                <input v-model.number="item.quantity" type="number" placeholder="Số lượng" min="1" @input="updateTotalPrice" />
                <button class="btn delete" @click="removePackageItem(index)"><i class="fas fa-trash"></i></button>
              </div>
              <button class="add-btn" @click="addPackageItem">+ Thêm vật phẩm</button>
            </div>
          </div>
          <div class="form-group">
            <label>Tổng giá (VND)</label>
            <input v-model.number="packageForm.totalPrice" type="number" readonly />
          </div>
          <div class="modal-actions">
            <button @click="savePackage" class="btn btn-success"
              :disabled="!packageForm.name || !packageForm.targetQuantity">Lưu</button>
            <button @click="closePackageModal" class="btn btn-secondary">Hủy</button>
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
      reliefItems: [],
      reliefPackages: [],
      showModal: false,
      showPackageModal: false,
      editingCause: null,
      editingPackage: null,
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
      packageForm: {
        id: '',
        causeId: '',
        name: '',
        description: '',
        extraFee: 0,
        totalPrice: 0,
        image: '',
        currentQuantity: 0,
        targetQuantity: 1,
        packageItems: []
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
    async fetchReliefItems() {
      try {
        const res = await axios.get(`${BASE_URL}/api/ReliefItem/get-all`, {
          withCredentials: true
        });
        if (res.data.status === 200) {
          this.reliefItems = res.data.responseData;
        }
      } catch (err) {
        console.error('Lỗi tải danh sách vật phẩm cứu trợ:', err);
        await showErrorAlertDark('Lỗi', 'Không thể tải danh sách vật phẩm cứu trợ.');
      }
    },
    async fetchReliefPackages(causeId) {
      try {
        const res = await axios.get(`${BASE_URL}/api/ReliefPackage/get-relief-packages-by-cause-id?causeId=${causeId}`, {
          withCredentials: true
        });
        if (res.data.status === 200) {
          this.reliefPackages = await Promise.all(res.data.responseData.map(async (pkg) => {
            const itemsRes = await axios.get(`${BASE_URL}/api/ReliefPackageItem/get-by-package?packageId=${pkg.id}`, {
              withCredentials: true
            });
            return {
              ...pkg,
              packageItems: itemsRes.data.status === 200 ? itemsRes.data.responseData : []
            };
          }));
        }
      } catch (err) {
        console.error('Lỗi tải danh sách gói cứu trợ:', err);
        await showErrorAlertDark('Lỗi', 'Không thể tải danh sách gói cứu trợ.');
      }
    },
    async fetchReliefPackageItems(packageId) {
      try {
        const res = await axios.get(`${BASE_URL}/api/ReliefPackageItem/get-by-package?packageId=${packageId}`, {
          withCredentials: true
        });
        if (res.data.status === 200) {
          return res.data.responseData;
        }
        return [];
      } catch (err) {
        console.error('Lỗi tải danh sách vật phẩm gói cứu trợ:', err);
        await showErrorAlertDark('Lỗi', 'Không thể tải vật phẩm gói cứu trợ.');
        return [];
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
      this.reliefPackages = [];
      this.showModal = true;
    },
    async editCause(cause) {
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
      await this.fetchReliefPackages(cause.id);
      this.showModal = true;
    },
    async openPackageModal(pkg) {
      this.editingPackage = pkg;
      let packageItems = [];
      if (pkg) {
        packageItems = await this.fetchReliefPackageItems(pkg.id);
      }
      this.packageForm = pkg
        ? {
            id: pkg.id || '',
            causeId: this.form.id || this.editingCause?.id || '',
            name: pkg.name || '',
            description: pkg.description || '',
            extraFee: pkg.extraFee || 0,
            totalPrice: pkg.totalPrice || 0,
            image: pkg.image || '',
            currentQuantity: pkg.currentQuantity || 0,
            targetQuantity: pkg.targetQuantity || 1,
            packageItems: packageItems.map(item => ({
              id: item.id,
              reliefItemId: item.reliefItemId,
              quantity: item.quantity
            }))
          }
        : {
            id: '',
            causeId: this.form.id || this.editingCause?.id || '',
            name: '',
            description: '',
            extraFee: 0,
            totalPrice: 0,
            image: '',
            currentQuantity: 0,
            targetQuantity: 1,
            packageItems: []
          };
      this.updateTotalPrice();
      this.showPackageModal = true;
    },
    closePackageModal() {
      this.showPackageModal = false;
      this.editingPackage = null;
      this.packageForm = {
        id: '',
        causeId: '',
        name: '',
        description: '',
        extraFee: 0,
        totalPrice: 0,
        image: '',
        currentQuantity: 0,
        targetQuantity: 1,
        packageItems: []
      };
      if (this.$refs.packageImageInput) {
        this.$refs.packageImageInput.value = '';
      }
    },
    addPackageItem() {
      this.packageForm.packageItems.push({
        reliefItemId: '',
        quantity: 1
      });
    },
    removePackageItem(index) {
      this.packageForm.packageItems.splice(index, 1);
      this.updateTotalPrice();
    },
    updateTotalPrice() {
      const total = this.packageForm.packageItems.reduce((sum, item) => {
        const reliefItem = this.reliefItems.find(ri => ri.id === item.reliefItemId);
        return sum + (reliefItem ? reliefItem.unitPrice * item.quantity : 0);
      }, 0);
      this.packageForm.totalPrice = total + (this.packageForm.extraFee || 0);
    },
    async handlePackageImageUpload(event) {
      const file = event.target.files[0];
      if (!file) return;
      try {
        const formData = new FormData();
        formData.append('file', file);
        const res = await axios.post(
          `${BASE_URL}/api/ReliefPackage/change-image?packageId=${encodeURIComponent(this.packageForm.id || '')}`,
          formData,
          {
            headers: { 'Content-Type': 'multipart/form-data' },
            withCredentials: true
          }
        );
        if (res.data.status === 200) {
          this.packageForm.image = res.data.responseData;
          await showSuccessAlertDark('Thành công', 'Ảnh gói cứu trợ đã được cập nhật.');
        } else {
          await showErrorAlertDark('Lỗi', res.data.message);
        }
      } catch (err) {
        await showErrorAlertDark('Lỗi', err.message);
      }
    },
    async savePackage() {
      if (!this.packageForm.name || !this.packageForm.targetQuantity) {
        await showErrorAlertDark('Lỗi', 'Vui lòng nhập tên gói và số lượng mục tiêu.');
        return;
      }
      try {
        const selectedItems = this.packageForm.packageItems
          .filter(item => item.reliefItemId && item.quantity > 0)
          .reduce((acc, item) => {
            acc[item.reliefItemId] = item.quantity;
            return acc;
          }, {});
        const payload = {
          id: this.packageForm.id || '00000000-0000-0000-0000-000000000000',
          causeId: this.form.id || this.editingCause?.id || '00000000-0000-0000-0000-000000000000',
          name: this.packageForm.name,
          description: this.packageForm.description || null,
          extraFee: this.packageForm.extraFee || 0,
          totalPrice: this.packageForm.totalPrice || 0,
          image: this.packageForm.image || null,
          currentQuantity: this.packageForm.currentQuantity || 0,
          targetQuantity: this.packageForm.targetQuantity || 1,
          selectedItems
        };
        const url = this.editingPackage
          ? `${BASE_URL}/api/ReliefPackage/update-relief-package`
          : `${BASE_URL}/api/ReliefPackage/create-relief-package`;
        const res = await axios.post(url, payload, {
          withCredentials: true
        });
        if ([200, 201].includes(res.data.status)) {
          await this.fetchReliefPackages(this.form.id || this.editingCause?.id);
          await showSuccessAlertDark('Thành công', res.data.message);
          this.closePackageModal();
        } else {
          await showErrorAlertDark('Lỗi', res.data.message);
        }
      } catch (err) {
        await showErrorAlertDark('Lỗi', err.message);
      }
    },
    async confirmDeletePackage(id) {
      const result = await showConfirmDialogDark(
        'Bạn có chắc chắn?',
        'Hành động này sẽ xóa gói cứu trợ!',
        'Xóa',
        'Hủy'
      );
      if (result.isConfirmed) {
        try {
          const res = await axios.post(
            `${BASE_URL}/api/ReliefPackage/delete-relief-package`,
            id,
            {
              headers: { 'Content-Type': 'application/json' },
              withCredentials: true
            }
          );
          if (res.data.status === 200) {
            await this.fetchReliefPackages(this.form.id || this.editingCause?.id);
            await showSuccessAlertDark('Thành công', 'Gói cứu trợ đã được xóa.');
          } else {
            await showErrorAlertDark('Lỗi', res.data.message);
          }
        } catch (err) {
          await showErrorAlertDark('Lỗi', err.message);
        }
      }
    },
    onOrganizationChange() {
      this.form.createdBy = '';
    },
    async handleHeroImageUpload(event) {
      const file = event.target.files[0];
      if (!file) return;
      try {
        const formData = new FormData();
        formData.append('file', file);
        const causeId = this.form.id || this.editingCause?.id || '';
        const res = await axios.post(
          `${BASE_URL}/api/Cause/change-hero-image?causeId=${encodeURIComponent(causeId)}`,
          formData,
          {
            headers: { 'Content-Type': 'multipart/form-data' },
            withCredentials: true
          }
        );
        if (res.data.status === 200) {
          this.form.heroImage = res.data.responseData;
          await showSuccessAlertDark('Thành công', 'Ảnh tiêu điểm đã được cập nhật.');
          await this.fetchCauses();
        } else {
          await showErrorAlertDark('Lỗi', res.data.message);
        }
      } catch (err) {
        await showErrorAlertDark('Lỗi', err.message);
      }
    },
    async handleChallengeImageUpload(event) {
      const file = event.target.files[0];
      if (!file) return;
      try {
        const formData = new FormData();
        formData.append('file', file);
        const causeId = this.form.id || this.editingCause?.id || '';
        const res = await axios.post(
          `${BASE_URL}/api/Cause/change-challenge-image?causeId=${encodeURIComponent(causeId)}`,
          formData,
          {
            headers: { 'Content-Type': 'multipart/form-data' },
            withCredentials: true
          }
        );
        if (res.data.status === 200) {
          this.form.challengeImage = res.data.responseData;
          await showSuccessAlertDark('Thành công', 'Ảnh thách thức đã được cập nhật.');
          await this.fetchCauses();
        } else {
          await showErrorAlertDark('Lỗi', res.data.message);
        }
      } catch (err) {
        await showErrorAlertDark('Lỗi', err.message);
      }
    },
    async handleSummaryImageUpload(event) {
      const file = event.target.files[0];
      if (!file) return;
      try {
        const formData = new FormData();
        formData.append('file', file);
        const causeId = this.form.id || this.editingCause?.id || '';
        const res = await axios.post(
          `${BASE_URL}/api/Cause/change-summary-image?causeId=${encodeURIComponent(causeId)}`,
          formData,
          {
            headers: { 'Content-Type': 'multipart/form-data' },
            withCredentials: true
          }
        );
        if (res.data.status === 200) {
          this.form.summaryImage = res.data.responseData;
          await showSuccessAlertDark('Thành công', 'Ảnh tổng kết đã được cập nhật.');
          await this.fetchCauses();
        } else {
          await showErrorAlertDark('Lỗi', res.data.message);
        }
      } catch (err) {
        await showErrorAlertDark('Lỗi', err.message);
      }
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
      this.editingCause = null;
      if (this.$refs.heroInput) this.$refs.heroInput.value = '';
      if (this.$refs.challengeInput) this.$refs.challengeInput.value = '';
      if (this.$refs.summaryInput) this.$refs.summaryInput.value = '';
      this.reliefPackages = [];
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
    await Promise.all([this.fetchCauses(), this.fetchUsers(), this.fetchOrganizations(), this.fetchReliefItems()]);
  }
};
</script>

<style scoped>
.cause-management {
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
}

.add-btn:hover {
  background-color: #e0b450;
}

.table-container {
  overflow-x: auto;
  max-width: 1200px;
  margin: 0 auto;
}

.cause-table, .package-table {
  width: 100%;
  border-collapse: collapse;
  background: #222238;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.3);
}

.cause-table th,
.cause-table td,
.package-table th,
.package-table td {
  padding: 14px;
  text-align: left;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

.cause-table th,
.package-table th {
  background: #292b40;
  color: #f6d579;
  font-weight: 600;
}

.cause-table tbody tr:hover,
.package-table tbody tr:hover {
  background: #2a2a42;
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
  padding: 20px;
  border-radius: 12px;
  width: 90%;
  max-width: 800px;
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

.image-container {
  display: flex;
  flex-wrap: wrap;
  gap: 20px;
  margin-bottom: 20px;
}

.image-group {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 10px;
}

.image-preview {
  width: 80px;
  height: 80px;
  border-radius: 8px;
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

.package-items {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.package-item {
  display: flex;
  gap: 10px;
  align-items: center;
}

.package-item select,
.package-item input {
  flex: 1;
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

  .cause-table th,
  .cause-table td,
  .package-table th,
  .package-table td {
    padding: 10px;
  }

  .image-preview {
    width: 60px;
    height: 60px;
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

  .cause-table,
  .package-table {
    font-size: 0.9rem;
  }

  .cause-table th,
  .cause-table td,
  .package-table th,
  .package-table td {
    padding: 8px;
  }

  .cause-table thead,
  .package-table thead {
    display: none;
  }

  .cause-table tbody,
  .cause-table tr,
  .cause-table td,
  .package-table tbody,
  .package-table tr,
  .package-table td {
    display: block;
    width: 100%;
  }

  .cause-table tr,
  .package-table tr {
    margin-bottom: 15px;
    border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  }

  .cause-table td,
  .package-table td {
    display: flex;
    align-items: center;
    justify-content: flex-start;
    padding: 10px;
    position: relative;
    text-align: left;
  }

  .cause-table td::before,
  .package-table td::before {
    content: attr(data-label);
    font-weight: bold;
    color: #f6d579;
    width: 40%;
    flex-shrink: 0;
    text-align: left;
  }

  .cause-table td:not(:first-child),
  .package-table td:not(:first-child) {
    border-top: none;
  }

  .cause-table td:last-child,
  .package-table td:last-child {
    display: flex;
    justify-content: flex-start;
    gap: 8px;
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

  .image-container {
    flex-direction: column;
    align-items: flex-start;
  }

  .image-group {
    align-items: flex-start;
  }

  .image-preview {
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

  .package-item {
    flex-direction: column;
    align-items: flex-start;
  }

  .package-item select,
  .package-item input {
    width: 100%;
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

  .cause-table td::before,
  .package-table td::before {
    width: 50%;
  }

  .modal {
    width: 98%;
    padding: 10px;
  }
}
</style>