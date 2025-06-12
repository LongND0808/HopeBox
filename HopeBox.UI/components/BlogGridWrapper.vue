<template>
  <section class="blog-grid-area">
    <div class="container">
      <div class="row">
        <div class="col-12">
          <div class="blog-content-column">
            <div class="blog-content-area post-items-style2">
              <div class="post-item" v-for="blog in paginatedBlogs" :key="blog.id">
                <div class="thumb">
                  <nuxt-link :to="`/blog-details?id=${blog.id}`">
                  <img :src="blog.imageUrl" :alt="blog.title">
                  </nuxt-link>
                  <div class="meta-date">
                    <span>{{ formatDate(blog.createdAt).day }}</span>{{ formatDate(blog.createdAt).month }}
                  </div>
                  <div class="shape-line"></div>
                </div>
                <div class="content">
                  <div class="inner-content">
                    <div class="meta">
                      <nuxt-link to="/blog" class="post-category">{{ blog.category || 'Chưa phân loại' }}</nuxt-link>
                      <nuxt-link to="/blog" class="post-author">
                        <span class="icon">
                          <img class="icon-img" src="/images/icons/admin1.png" alt="Icon-Image">
                        </span>By: {{ blog.authorName || 'Ẩn danh' }}
                      </nuxt-link>
                    </div>
                    <h4 class="title">
                       <nuxt-link :to="`/blog-details?id=${blog.id}`">{{ blog.title }}</nuxt-link>
                    </h4>
                    <p>{{ blog.description }}</p>
                  <nuxt-link :to="`/blog-details?id=${blog.id}`" class="btn-theme btn-border-gradient btn-size-md">
                    <span>
                      Đọc thêm
                       <img class="icon icon-img" src="/images/icons/arrow-line-right-gradient.png" alt="Icon">
                    </span>
                  </nuxt-link>
                  </div>
                </div>
              </div>

              <div v-if="paginatedBlogs.length === 0">
                <p>Không có bài viết nào.</p>
              </div>

              <div class="pagination-area pt-0 pb-0">
                <nav>
                  <ul class="page-numbers">
                    <li>
                      <a class="page-number prev" href="#" @click.prevent="prevPage" :class="{ 'disabled-link': currentPage === 1 }">
                        <img src="/images/icons/test-arrow-left.png" alt="Icon-Image"> </a>
                    </li>
                    <li v-for="page in totalPages" :key="page">
                      <a class="page-number" href="#" @click.prevent="goToPage(page)" :class="{ current: page === currentPage }">
                        {{ page }}
                      </a>
                    </li>
                    <li>
                      <a class="page-number next" href="#" @click.prevent="nextPage" :class="{ 'disabled-link': currentPage === totalPages }">
                        <img src="/images/icons/arrow-line-right-gradient.png" alt="Icon-Image">
                      </a>
                    </li>
                  </ul>
                </nav>
              </div>
            </div>

            
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
import SidebarWrapper from '@/components/SidebarWrapper';
import axios from 'axios';

export default {
  components: {
    SidebarWrapper,
  },
  data() {
    return {
      allBlogs: [],
      currentPage: 1,
      blogsPerPage: 3,
    };
  },
  computed: {
    totalPages() {
      return Math.ceil(this.allBlogs.length / this.blogsPerPage);
    },
    paginatedBlogs() {
      const startIndex = (this.currentPage - 1) * this.blogsPerPage;
      const endIndex = startIndex + this.blogsPerPage;
      return this.allBlogs.slice(startIndex, endIndex);
    },
  },
  async mounted() {
    try {
      const response = await axios.get('https://localhost:7213/api/Blog/get-all');
      if (response.data && response.data.status === 200) {
        const blogsRaw = response.data.responseData;

        const blogsWithAuthors = await Promise.all(
          blogsRaw.map(async (blog) => {
            let authorName = 'Ẩn danh';
            try {
              const userRes = await axios.get(`https://localhost:7213/api/User/get-by-id?id=${blog.createdBy}`);
              if (userRes.data && userRes.data.responseData) {
                authorName = userRes.data.responseData.fullName || authorName;
              }
            } catch {
            }
            return { ...blog, authorName };
          })
        );

        this.allBlogs = blogsWithAuthors;
      } else {
        console.warn('API Blog returned invalid data:', response);
      }
    } catch (error) {
      console.error('Error loading blog posts:', error);
    }
  },
  methods: {
    formatDate(dateStr) {
      const date = new Date(dateStr);
      const day = date.getDate();
      const month = date.toLocaleString('default', { month: 'short' });
      return { day, month };
    },
    goToPage(page) {
      this.currentPage = page;
    },
    prevPage() {
      if (this.currentPage > 1) {
        this.currentPage--;
      }
    },
    nextPage() {
      if (this.currentPage < this.totalPages) {
        this.currentPage++;
      }
    },
  },
};
</script>
<style scoped>
/* This is CSS */
.meta-date {
  /* Some CSS properties */
  position: absolute;
  top: 0;
  left: 0;
  /* ... */
}

/* This is the problematic line that should NOT be in <style> */
/* Remove this: */
/* <span>{{ formatDate(blog.createdAt).day }}</span>{{ formatDate(blog.createdAt).month }} */

.some-other-css-rule {
  /* More CSS */
}
</style>
