<template>
  <div class="blog-management">
    <div class="main-content">
      <div class="header">
        <h2>Quản lý Blog</h2>
        <button class="add-btn" @click="openAddModal">
          <i class="fas fa-plus"></i> Thêm Blog
        </button>
      </div>

      <div class="table-container">
        <table class="blog-table">
          <thead>
            <tr>
              <th>Hình ảnh</th>
              <th>Tiêu đề</th>
              <th>Nội dung</th>
              <th>Ngày đăng</th>
              <th>Lượt xem</th>
              <th>Trạng thái</th>
              <th>Người tạo</th>
              <th>Hành động</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="blog in blogs" :key="blog.id">
              <td data-label="Hình ảnh">
                <img :src="blog.imageUrl || '/images/placeholder.jpg'" class="avatar" alt="Hình ảnh Blog" />
              </td>
              <td data-label="Tiêu đề">{{ blog.title }}</td>
              <td data-label="Nội dung">{{ blog.content || 'N/A' }}</td>
              <td data-label="Ngày đăng">{{ formatDate(blog.createdAt) }}</td>
              <td data-label="Lượt xem">{{ blog.viewCount || 0 }}</td>
              <td data-label="Trạng thái">
                <span :class="getStatusClass(blog.isPublished)">
                  {{ getStatusLabel(blog.isPublished) }}
                </span>
              </td>
              <td data-label="Người tạo">{{ getUserName(blog.createdBy) || 'N/A' }}</td>
              <td data-label="Hành động">
                <button class="btn edit" @click="editBlog(blog)">
                  <i class="fas fa-edit"></i>
                </button>
                <button class="btn delete" @click="confirmDelete(blog.id)">
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
          <h3>{{ editingBlog ? 'Cập nhật Blog' : 'Thêm Blog' }}</h3>
          <div class="avatar-container">
            <img :src="form.imageUrl || '/images/placeholder.jpg'" alt="Hình ảnh Blog" class="avatar-preview"
              @click="$refs.fileInput.click()" />
            <input type="file" ref="fileInput" accept="image/*" style="display: none" @change="handleImageUpload" />
          </div>
          <div class="form-group">
            <label>Tiêu đề</label>
            <input v-model="form.title" placeholder="Tiêu đề Blog" required />
          </div>
          <div class="form-group">
            <label>Mô tả</label>
            <textarea v-model="form.description" placeholder="Mô tả Blog" rows="3"></textarea>
          </div>
          <div class="form-group">
            <label>Nội dung</label>
            <textarea v-model="form.content" placeholder="Nội dung Blog" rows="5"></textarea>
          </div>
          <div class="form-group">
            <label>Slug</label>
            <input v-model="form.slug" placeholder="Slug (URL thân thiện)" />
          </div>
          <div class="form-group">
            <label>Tags (phân cách bằng dấu phẩy)</label>
            <input v-model="form.tags" placeholder="Ví dụ: tin tức, công nghệ" />
          </div>
          <div class="form-group">
            <label>Mô tả SEO</label>
            <textarea v-model="form.metaDescription" placeholder="Mô tả SEO" rows="3"></textarea>
          </div>
          <div class="form-group">
            <label>Từ khóa SEO (phân cách bằng dấu phẩy)</label>
            <input v-model="form.metaKeywords" placeholder="Ví dụ: blog, tin tức" />
          </div>
          <div class="form-group">
            <label>Thời gian đọc (phút)</label>
            <input type="number" v-model.number="form.readingTime" placeholder="Thời gian đọc (phút)" min="0" />
          </div>
          <div class="form-group">
            <label>Trạng thái</label>
            <select v-model="form.isPublished" required>
              <option :value="true">Đã đăng</option>
              <option :value="false">Bản nháp</option>
            </select>
          </div>
          <div class="form-group">
            <label>Người tạo</label>
            <select v-model="form.createdBy" required>
              <option value="" disabled>Chọn người tạo</option>
              <option v-for="user in users" :key="user.id" :value="user.id">
                {{ user.fullName }}
              </option>
            </select>
          </div>
          <div class="modal-actions">
            <button @click="saveBlog" class="btn btn-success"
              :disabled="!form.title || !form.content || !form.createdBy">Lưu</button>
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

  export default {
    data() {
      return {
        blogs: [],
        users: [],
        showModal: false,
        editingBlog: null,
        form: {
          id: null,
          title: '',
          content: '',
          description: '',
          imageUrl: '',
          slug: '',
          viewCount: 0,
          likeCount: 0,
          commentCount: 0,
          shareCount: 0,
          tags: '',
          metaDescription: '',
          metaKeywords: '',
          readingTime: 0,
          createdAt: null,
          updatedAt: null,
          createdBy: null,
          creatorName: null,
          isPublished: false
        },
        pendingImage: null // Store file for new blog image uploads
      };
    },
    methods: {
      async fetchBlogs() {
        try {
          const response = await axios.get(`${BASE_URL}/api/blog/get-all`, {
            withCredentials: true
          });
          if (response.data.status === 200) {
            this.blogs = response.data.responseData.map(blog => ({
              ...blog,
              imageUrl: blog.imageUrl || '/images/placeholder.jpg'
            }));
          } else {
            await showErrorAlertDark('Lỗi', response.data.message || 'Không thể tải danh sách blog.');
          }
        } catch (error) {
          await showErrorAlertDark('Lỗi', 'Không thể tải danh sách blog.');
          console.error('Lỗi tải danh sách blog:', error);
        }
      },
      async fetchUsers() {
        try {
          const response = await axios.get(`${BASE_URL}/api/User/get-all`, {
            withCredentials: true
          });
          if (response.data.status === 200) {
            this.users = response.data.responseData;
          } else {
            await showErrorAlertDark('Lỗi', response.data.message || 'Không thể tải danh sách người dùng.');
          }
        } catch (error) {
          await showErrorAlertDark('Lỗi', 'Không thể tải danh sách người dùng.');
          console.error('Lỗi tải danh sách người dùng:', error);
        }
      },
      getUserName(userId) {
        const user = this.users.find(u => u.id === userId);
        return user ? user.fullName : null;
      },
      getStatusLabel(isPublished) {
        return isPublished ? 'Đã đăng' : 'Bản nháp';
      },
      getStatusClass(isPublished) {
        return isPublished ? 'blog-status-active' : 'blog-status-pending';
      },
      openAddModal() {
        this.editingBlog = null;
        this.form = {
          id: null,
          title: '',
          content: '',
          description: '',
          imageUrl: '',
          slug: '',
          viewCount: 0,
          likeCount: 0,
          commentCount: 0,
          shareCount: 0,
          tags: '',
          metaDescription: '',
          metaKeywords: '',
          readingTime: 0,
          createdAt: null,
          updatedAt: null,
          createdBy: null,
          creatorName: null,
          isPublished: false
        };
        this.pendingImage = null;
        if (this.$refs.fileInput) this.$refs.fileInput.value = '';
        this.showModal = true;
      },
      editBlog(blog) {
        this.form = {
          id: blog.id || null,
          title: blog.title || '',
          content: blog.content || '',
          description: blog.description || '',
          imageUrl: blog.imageUrl || '',
          slug: blog.slug || '',
          viewCount: blog.viewCount || 0,
          likeCount: blog.likeCount || 0,
          commentCount: blog.commentCount || 0,
          shareCount: blog.shareCount || 0,
          tags: blog.tags || '',
          metaDescription: blog.metaDescription || '',
          metaKeywords: blog.metaKeywords || '',
          readingTime: blog.readingTime || 0,
          createdAt: blog.createdAt ? new Date(blog.createdAt).toISOString() : null,
          updatedAt: blog.updatedAt ? new Date(blog.updatedAt).toISOString() : null,
          createdBy: blog.createdBy || null,
          creatorName: blog.creatorName || null,
          isPublished: blog.isPublished || false
        };
        this.editingBlog = null;
        this.pendingImage = null;
        this.showModal = true;
      },
      async handleImageUpload(event) {
        const file = event.target.files[0];
        if (!file) return;

        if (this.form.id) {
          // For existing blogs, upload immediately
          try {
            const formData = new FormData();
            formData.append('file', file);
            const response = await axios.post(
              `${BASE_URL}/api/blog/change-image?blogId=${encodeURIComponent(this.form.id)}`,
              formData,
              {
                headers: { 'Content-Type': 'multipart/form-data' },
                withCredentials: true
              }
            );

            if (response.data.status === 200 && response.data.responseData) {
              this.form.imageUrl = response.data.responseData;
              await showSuccessAlertDark('Thành công', 'Ảnh bài viết đã được cập nhật.');
              this.fetchBlogs();
            } else {
              await showErrorAlertDark('Lỗi', response.data.message || 'Không thể tải ảnh lên.');
            }
          } catch (error) {
            await showErrorAlertDark('Lỗi', 'Không thể tải ảnh lên.');
            console.error('Lỗi tải ảnh:', error);
          }
        } else {
          // For new blogs, store the file temporarily
          this.pendingImage = file;
          this.form.imageUrl = URL.createObjectURL(file); // Preview the image
        }
      },
      async saveBlog() {
        if (!this.form.title || !this.form.content || !this.form.createdBy) {
          await showErrorAlertDark('Lỗi', 'Vui lòng nhập đầy đủ thông tin bắt buộc: Tiêu đề, Nội dung, Người tạo.');
          return;
        }

        try {
          const blogDto = {
            id: this.form.id || null,
            title: this.form.title,
            content: this.form.content,
            description: this.form.description || null,
            imageUrl: this.form.imageUrl && !this.pendingImage ? this.form.imageUrl : null,
            slug: this.form.slug || null,
            viewCount: this.form.viewCount || 0,
            likeCount: this.form.likeCount || 0,
            commentCount: this.form.commentCount || 0,
            shareCount: this.form.shareCount || 0,
            tags: this.form.tags || null,
            metaDescription: this.form.metaDescription || null,
            metaKeywords: this.form.metaKeywords || null,
            readingTime: this.form.readingTime || 0,
            createdAt: this.form.id && this.form.createdAt ? new Date(this.form.createdAt).toISOString() : new Date().toISOString(),
            updatedAt: this.form.id && this.form.updatedAt ? new Date(this.form.updatedAt).toISOString() : null,
            createdBy: this.form.createdBy || null,
            isPublished: this.form.isPublished
            // creatorName is set by the backend
          };

          const payload = blogDto;

          const url = this.form.id ? `${BASE_URL}/api/blog/update` : `${BASE_URL}/api/blog/add`;

          const response = await axios.post(url, payload, {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true
          });

          if ([200, 201].includes(response.data.status)) {
            let blogId = this.form.id || response.data.responseData?.id;

            // If there's a pending image for a new blog, upload it now
            if (!this.form.id && this.pendingImage) {
              const formData = new FormData();
              formData.append('file', this.pendingImage);
              const imageResponse = await axios.post(
                `${BASE_URL}/api/blog/change-image?blogId=${encodeURIComponent(blogId)}`,
                formData,
                {
                  headers: { 'Content-Type': 'multipart/form-data' },
                  withCredentials: true
                }
              );

              if (imageResponse.data.status === 200 && imageResponse.data.responseData) {
                this.form.imageUrl = imageResponse.data.responseData;
              } else {
                console.error('Failed to upload image:', imageResponse.data.message);
              }
            }

            await showSuccessAlertDark('Thành công', this.form.id ? 'Cập nhật blog thành công!' : 'Thêm blog thành công!');
            this.fetchBlogs();
            this.closeModal();
          } else {
            await showErrorAlertDark('Lỗi', response.data.message || 'Không thể lưu thông tin blog.');
          }
        } catch (error) {
          await showErrorAlertDark('Lỗi', 'Không thể lưu thông tin blog.');
          console.error('Lỗi lưu blog:', error);
        }
      },
      async confirmDelete(id) {
        const result = await showConfirmDialogDark(
          'Bạn có chắc chắn?',
          'Hành động này sẽ xóa bài viết!',
          'Xóa',
          'Hủy'
        );

        if (result.isConfirmed) {
          try {
            const response = await axios.post(
              `${BASE_URL}/api/blog/delete`,
              id,
              {
                headers: { 'Content-Type': 'application/json' },
                withCredentials: true
              }
            );
            if (response.data.status === 200) {
              await showSuccessAlertDark('Thành công', 'Xóa blog thành công!');
              this.fetchBlogs();
            } else {
              await showErrorAlertDark('Lỗi', response.data.message || 'Không thể xóa blog.');
            }
          } catch (error) {
            await showErrorAlertDark('Lỗi', 'Không thể xóa blog.');
            console.error('Lỗi xóa blog:', error);
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
        this.editingBlog = null;
        this.pendingImage = null;
        if (this.$refs.fileInput) this.$refs.fileInput.value = '';
      }
    },
    async mounted() {
      await Promise.all([this.fetchBlogs(), this.fetchUsers()]);
    }
  };
</script>

<style scoped>
  .blog-management {
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

  .blog-table {
    width: 100%;
    border-collapse: collapse;
    background: #222238;
    border-radius: 8px;
    overflow: hidden;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.3);
  }

  .blog-table th,
  .blog-table td {
    padding: 14px;
    text-align: left;
    border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  }

  .blog-table th {
    background: #292b40;
    color: #f6d579;
    font-weight: 600;
    position: sticky;
    top: 0;
    z-index: 1;
  }

  .blog-table tbody tr:hover {
    background: #2a2a42;
  }

  .avatar {
    width: 40px;
    height: 40px;
    border-radius: 50%;
    object-fit: cover;
  }

  .blog-status-active {
    color: #28a745;
    font-weight: bold;
  }

  .blog-status-pending {
    color: #dad00b;
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
    min-height: 120px;
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

    .blog-table th,
    .blog-table td {
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

    .blog-table {
      font-size: 0.9rem;
    }

    .blog-table th,
    .blog-table td {
      padding: 8px;
    }

    .blog-table thead {
      display: none;
    }

    .blog-table tbody,
    .blog-table tr,
    .blog-table td {
      display: block;
      width: 100%;
    }

    .blog-table tr {
      margin-bottom: 15px;
      border-bottom: 1px solid rgba(255, 255, 255, 0.1);
    }

    .blog-table td {
      display: flex;
      align-items: center;
      justify-content: flex-start;
      padding: 10px;
      position: relative;
      text-align: left;
    }

    .blog-table td::before {
      content: attr(data-label);
      font-weight: bold;
      color: #f6d579;
      width: 40%;
      flex-shrink: 0;
      text-align: left;
    }

    .blog-table td:not(:first-child) {
      border-top: none;
    }

    .blog-table td:last-child {
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

    .blog-table td::before {
      width: 50%;
    }

    .modal {
      width: 98%;
      padding: 10px;
    }
  }
</style>