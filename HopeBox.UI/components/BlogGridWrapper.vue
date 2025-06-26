<template>
  <section class="blog-grid-area">
    <div class="container">
      <div class="row">
        <div class="col-12">
          <div class="blog-filter-section">
            <div class="section-title text-center mb-4">
              <h5 class="subtitle">Khám phá</h5>
              <h2 class="title">Tìm kiếm <span>Bài viết</span> phù hợp</h2>
              <p>Sử dụng bộ lọc để tìm kiếm các bài viết theo chủ đề và nội dung mà bạn quan tâm.</p>
            </div>

            <div class="filter-controls">
              <div class="row">
                <div class="col-md-3 mb-3">
                  <label class="form-label">Tìm theo tiêu đề</label>
                  <input
                    v-model="filters.title" 
                    type="text" 
                    class="form-control"
                    placeholder="Nhập tiêu đề bài viết..."
                    @keyup.enter="handleSearch"
                  >
                </div>

                <div class="col-md-3 mb-3">
                  <label class="form-label">Tìm theo tags</label>
                  <input 
                    v-model="filters.tags" 
                    type="text" 
                    class="form-control"
                    placeholder="Nhập tags..."
                    @keyup.enter="handleSearch"
                  >
                </div>

                <div class="col-md-2 mb-3">
                  <label class="form-label">Trạng thái</label>
                  <select v-model="filters.isPublished" class="form-select">
                    <option :value="null">Tất cả</option>
                    <option :value="true">Đã xuất bản</option>
                    <option :value="false">Nháp</option>
                  </select>
                </div>

                <div class="col-md-2 mb-3">
                  <label class="form-label">Từ ngày</label>
                  <input 
                    v-model="filters.fromDate" 
                    type="date" 
                    class="form-control"
                  >
                </div>

                <div class="col-md-2 mb-3">
                  <label class="form-label">Đến ngày</label>
                  <input 
                    v-model="filters.toDate" 
                    type="date" 
                    class="form-control"
                  >
                </div>
              </div>

              <div class="row">
                <div class="col-md-2 mb-3">
                  <label class="form-label">Số bài/trang</label>
                  <select v-model="pagination.pageSize" class="form-select" @change="handlePageSizeChange">
                    <option v-for="size in pageSizeOptions" :key="size" :value="size">
                      {{ size }} bài/trang
                    </option>
                  </select>
                </div>

                <div class="col-md-10 mb-3 d-flex align-items-end gap-2">
                  <button 
                    class="btn btn-search" 
                    @click="handleSearch" 
                    :disabled="loading"
                  >
                    <span v-if="loading">
                      <i class="fa fa-spinner fa-spin"></i> Đang tìm...
                    </span>
                    <span v-else> Tìm kiếm
                    </span>
                  </button>
                  
                  <button 
                    class="btn btn-reset" 
                    @click="handleReset"
                    :disabled="loading"
                  >Reset
                  </button>
                </div>
              </div>
            </div>
          </div>

          <div class="loading-state" v-if="loading">
            <div class="text-center">
              <i class="fa fa-spinner fa-spin fa-3x"></i>
              <p>Đang tải danh sách bài viết...</p>
            </div>
          </div>

          <div class="blog-content-column" v-else-if="blogsData && blogsData.length > 0">
            <div class="blog-content-area post-items-style2">
              <div class="post-item" v-for="blog in blogsData" :key="blog.id">
                <div class="thumb">
                  <nuxt-link :to="`/blog-details?id=${blog.id}`">
                    <img :src="blog.imageUrl || '/images/default-blog.jpg'" :alt="blog.title" @error="handleImageError">
                  </nuxt-link>
                  <div class="meta-date">
                    <span>{{ formatDate(blog.createdAt).day }}</span>{{ formatDate(blog.createdAt).month }}
                  </div>
                  <div class="shape-line"></div>
                </div>
                <div class="content">
                  <div class="inner-content">
                    <div class="meta">
                      <span class="post-category">{{ blog.tags || 'Chưa phân loại' }}</span>
                      <span class="post-author">
                        <span class="icon">
                          <img class="icon-img" src="/images/icons/admin1.png" alt="Icon-Image">
                        </span>
                        By: {{ blog.creatorName || 'Ẩn danh' }}
                      </span>
                      <span class="post-status" :class="{ 'published': blog.isPublished, 'draft': !blog.isPublished }">
                        {{ blog.isPublished ? 'Đã xuất bản' : 'Nháp' }}
                      </span>
                    </div>
                    <h4 class="title">
                      <nuxt-link :to="`/blog-details?id=${blog.id}`">{{ blog.title }}</nuxt-link>
                    </h4>
                    <p>{{ blog.description }}</p>
                    
                    <div class="blog-stats mb-3">
                      <span class="stat-item">
                        <i class="fa fa-eye"></i> {{ blog.viewCount || 0 }}
                      </span>
                      <span class="stat-item">
                        <i class="fa fa-heart"></i> {{ blog.likeCount || 0 }}
                      </span>
                      <span class="stat-item">
                        <i class="fa fa-comment"></i> {{ blog.commentCount || 0 }}
                      </span>
                      <span class="stat-item">
                        <i class="fa fa-share"></i> {{ blog.shareCount || 0 }}
                      </span>
                      <span class="stat-item">
                        <i class="fa fa-clock"></i> {{ blog.readingTime || 0 }} phút đọc
                      </span>
                    </div>

                    <nuxt-link :to="`/blog-details?id=${blog.id}`" class="btn-theme btn-border-gradient btn-size-md">
                      <span>
                        Đọc thêm
                        <img class="icon icon-img" src="/images/icons/arrow-line-right-gradient.png" alt="Icon">
                      </span>
                    </nuxt-link>
                  </div>
                </div>
              </div>

              <div class="pagination-area pt-0 pb-0" v-if="totalPages > 1">
                <nav>
                  <ul class="page-numbers">
                    <li>
                      <a 
                        class="page-number prev" 
                        href="#" 
                        @click.prevent="goToPage(pagination.pageIndex - 1)" 
                        :class="{ 'disabled-link': pagination.pageIndex === 1 }"
                      >
                        <img src="/images/icons/test-arrow-left.png" alt="Previous">
                      </a>
                    </li>

                    <!-- Page Numbers -->
                    <li v-for="page in visiblePages" :key="page">
                      <a 
                        class="page-number" 
                        href="#" 
                        @click.prevent="goToPage(page)" 
                        :class="{ current: page === pagination.pageIndex }"
                      >
                        {{ page }}
                      </a>
                    </li>

                    <li>
                      <a 
                        class="page-number next" 
                        href="#" 
                        @click.prevent="goToPage(pagination.pageIndex + 1)" 
                        :class="{ 'disabled-link': pagination.pageIndex === totalPages }"
                      >
                        <img src="/images/icons/arrow-line-right-gradient.png" alt="Next">
                      </a>
                    </li>
                  </ul>
                </nav>

                <div class="pagination-info text-center mt-3">
                </div>
              </div>
            </div>
          </div>

          <div class="no-blogs-message" v-else-if="!loading">
            <div class="text-center">
              <i class="fa fa-info-circle fa-3x mb-3"></i>
              <h4>Không tìm thấy bài viết nào</h4>
              <p>Không có bài viết nào phù hợp với tiêu chí tìm kiếm của bạn.</p>
              <button class="btn btn-primary" @click="handleReset">
                <i class="fa fa-refresh"></i> Đặt lại bộ lọc
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
import axios from 'axios';
import { BASE_URL } from '@/utils/constants';
import { showErrorAlert, showSuccessAlert } from '@/utils/alertHelper';

export default {
  data() {
    return {
      blogsData: [],
      loading: false,

      filters: {
        title: '',
        tags: '',
        isPublished: true,
        createdBy: null,
        fromDate: '',
        toDate: ''
      },

      pagination: {
        pageIndex: 1,
        pageSize: 6
      },

      totalPages: 0,
      totalRecords: 0,

      pageSizeOptions: [3, 6, 9, 12, 15],

      searchTimeout: null
    };
  },

  computed: {
    visiblePages() {
      const pages = [];
      const start = Math.max(1, this.pagination.pageIndex - 2);
      const end = Math.min(this.totalPages, this.pagination.pageIndex + 2);

      for (let i = start; i <= end; i++) {
        pages.push(i);
      }
      return pages;
    },

    // Build request payload theo đúng API
    requestPayload() {
      const payload = {
        pageIndex: this.pagination.pageIndex,
        pageSize: this.pagination.pageSize
      };

      // Chỉ thêm filter nếu có giá trị
      if (this.filters.title && this.filters.title.trim()) {
        payload.title = this.filters.title.trim();
      }

      if (this.filters.tags && this.filters.tags.trim()) {
        payload.tags = this.filters.tags.trim();
      }

      if (this.filters.isPublished !== null) {
        payload.isPublished = this.filters.isPublished;
      }

      if (this.filters.createdBy) {
        payload.createdBy = this.filters.createdBy;
      }

      if (this.filters.fromDate) {
        payload.fromDate = new Date(this.filters.fromDate).toISOString();
      }

      if (this.filters.toDate) {
        payload.toDate = new Date(this.filters.toDate).toISOString();
      }

      return payload;
    }
  },

  mounted() {
    this.fetchBlogsData();
  },

  methods: {
    async fetchBlogsData() {
      try {
        this.loading = true;

        // Sử dụng GET với query parameters thay vì POST
        const params = new URLSearchParams();
        
        Object.keys(this.requestPayload).forEach(key => {
          if (this.requestPayload[key] !== null && this.requestPayload[key] !== undefined) {
            params.append(key, this.requestPayload[key]);
          }
        });

        const response = await axios.get(
          `${BASE_URL}/api/Blog/filter?${params.toString()}`,
          {
            headers: {
              'Content-Type': 'application/json'
            }
          }
        );

        if (response.data && response.data.status === 200) {
          const responseData = response.data.responseData;
          
          // Xử lý response data theo cấu trúc API
          this.blogsData = responseData.pagedData || [];
          this.totalRecords = responseData.totalRecords || 0;
          this.totalPages = responseData.totalPages || 0;

          // Enrich với thông tin creator
          await this.enrichBlogsWithCreatorInfo();

          // Scroll to top nếu không phải trang đầu
          if (this.pagination.pageIndex > 1) {
            this.scrollToTop();
          }

        } else {
          throw new Error(response.data.message || 'Không thể tải danh sách bài viết');
        }
      } catch (error) {
        console.error('Lỗi khi lấy dữ liệu blogs:', error);
        
        // Reset data khi có lỗi
        this.blogsData = [];
        this.totalRecords = 0;
        this.totalPages = 0;

        await showErrorAlert(
          'Lỗi tải dữ liệu',
          error.response?.data?.message || error.message || 'Không thể kết nối đến server'
        );
      } finally {
        this.loading = false;
      }
    },

    async enrichBlogsWithCreatorInfo() {
      if (!this.blogsData.length) return;

      try {
        const enrichedBlogs = await Promise.all(
          this.blogsData.map(async (blog) => {
            let creatorName = 'Ẩn danh';
            try {
              const userRes = await axios.get(`${BASE_URL}/api/User/get-by-id?id=${blog.createdBy}`);
              if (userRes.data && userRes.data.status === 200 && userRes.data.responseData) {
                creatorName = userRes.data.responseData.fullName || 
                             userRes.data.responseData.userName || 
                             'Ẩn danh';
              }
            } catch (error) {
              console.warn('Không thể lấy thông tin creator cho blog:', blog.id);
            }
            return { ...blog, creatorName };
          })
        );
        
        this.blogsData = enrichedBlogs;
      } catch (error) {
        console.error('Lỗi khi enrich creator info:', error);
      }
    },

    // Search với debounce
    handleSearch() {
      // Clear timeout cũ
      if (this.searchTimeout) {
        clearTimeout(this.searchTimeout);
      }

      // Reset về trang đầu khi search
      this.pagination.pageIndex = 1;

      // Debounce search
      this.searchTimeout = setTimeout(() => {
        this.fetchBlogsData();
      }, 300);
    },

    // Reset tất cả filters
    async handleReset() {
      this.filters = {
        title: '',
        tags: '',
        isPublished: true,
        createdBy: null,
        fromDate: '',
        toDate: ''
      };

      this.pagination = {
        pageIndex: 1,
        pageSize: 6
      };

      await this.fetchBlogsData();
      await showSuccessAlert('Thành công', 'Đã đặt lại bộ lọc');
    },

    // Thay đổi page size
    async handlePageSizeChange() {
      this.pagination.pageIndex = 1;
      await this.fetchBlogsData();
    },

    // Chuyển trang
    async goToPage(page) {
      if (page >= 1 && page <= this.totalPages && page !== this.pagination.pageIndex && !this.loading) {
        this.pagination.pageIndex = page;
        await this.fetchBlogsData();
      }
    },

    // Scroll to top
    scrollToTop() {
      const element = this.$el.querySelector('.blog-filter-section');
      if (element) {
        element.scrollIntoView({ behavior: 'smooth', block: 'start' });
      }
    },

    // Format date
    formatDate(dateStr) {
      if (!dateStr) return { day: '', month: '' };
      
      try {
        const date = new Date(dateStr);
        const day = date.getDate().toString().padStart(2, '0');
        const month = date.toLocaleString('vi-VN', { month: 'short' });
        return { day, month };
      } catch (error) {
        return { day: '', month: '' };
      }
    },

    // Handle image error
    handleImageError(event) {
      event.target.src = '/images/default-blog.jpg';
    }
  },

  // Cleanup
  beforeDestroy() {
    if (this.searchTimeout) {
      clearTimeout(this.searchTimeout);
    }
  }
};
</script>

<style scoped>
@import '@/assets/scss/component/_blog.scss';
</style>