<template>
  <section class="blog-details-area" v-if="blog">
    <div class="container">
      <div class="row">
        <div class="col-12">
          <div class="blog-details-column">
            <div class="post-details-content">
              <div class="post-details-body">
                <div class="thumb">
                  <img class="w-100" :src="blog.imageUrl || '/images/blog/g1.jpg'" :alt="blog.title" />
                </div>
                <div class="content">
                  <div class="meta">
                    <nuxt-link to="/blog" class="post-category">{{ blog.category || 'Education' }}</nuxt-link>
                    <nuxt-link to="/blog" class="post-author">
                      <span class="icon">
                        <img class="icon-img" src="/images/icons/admin1.png" alt="Icon-Image">
                      </span>By: {{ blog.authorName || 'Ẩn danh' }}
                    </nuxt-link>
                  </div>
                  <h2 class="title">{{ blog.title }}</h2>
                  <p>{{ blog.description }}</p>
                  <div v-html="blog.content"></div>
                  <div class="blockquote-area">
                    <blockquote class="blockquote-style">
                      <p>{{ blog.blockquote || 'Contrary to popular belief not simply random has roots in a piece of classical Latin literature making it over 2000 years old Latin professor looked up one of the more.' }}</p>
                      <div class="icon">“</div>
                    </blockquote>
                  </div>
                  <!-- Các hình ảnh bổ sung hoặc section khác nếu có -->
                  <div class="row mb-32">
                    <div class="col-sm-4" v-for="(img, i) in blog.extraImages || ['/images/blog/s1.jpg','/images/blog/s2.jpg','/images/blog/s3.jpg']" :key="i">
                      <img class="w-100 mb-xs-30" :src="img" alt="image">
                    </div>
                  </div>
                  <div class="category-social-content">
                    <div class="social-items">
                      <a href="#"><i class="icofont-facebook"></i></a>
                      <a href="#"><i class="icofont-skype"></i></a>
                      <a href="#"><i class="icofont-twitter"></i></a>
                    </div>
                    <div class="category-items">
                      <span>Tags:</span>
                      <nuxt-link v-for="(tag, i) in blog.tags || ['Donation','Charity','Non Profit','Fund Raising']" :key="i" to="/blog">{{ tag }}<span v-if="i < (blog.tags?.length || 4)-1">,</span></nuxt-link>
                    </div>
                  </div>
                </div>
                <div class="meta mt-3">
                    <div>Ngày đăng: {{ formatDate(blog.createdAt) }}</div>
                    <div>Tác giả: {{ blog.authorName || 'Ẩn danh' }}</div>
                </div>
                <!-- Comments, form giữ nguyên như template tĩnh -->
                <div class="comment-area">
                  <h2 class="title">Comments <span>(04)</span></h2>
                  <!-- ... comments render here ... -->
                  <div class="comment-form">
                    <h2 class="title">Reply Comment</h2>
                    <form action="#" method="post">
                      <div class="comment-form-content">
                        <div class="row row-gutter-20">
                          <div class="col-md-6">
                            <div class="form-group">
                              <input class="form-control" type="text" placeholder="Name" required="">
                            </div>
                          </div>
                          <div class="col-md-6">
                            <div class="form-group">
                              <input class="form-control" type="email" placeholder="Email" required="">
                            </div>
                          </div>
                          <div class="col-md-12">
                            <div class="form-group mb-0">
                              <textarea class="form-control textarea" name="comment" id="comment" cols="45" rows="7" placeholder="Comment" required=""></textarea>
                            </div>
                          </div>
                        </div>
                        <div class="row">
                          <div class="col-md-12">
                            <div class="form-group mb-0">
                              <button class="btn-theme btn-gradient btn-slide no-border" type="submit">Send Comment</button>
                            </div>
                          </div>
                        </div>
                      </div>
                    </form>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
  <div v-else>
    <p>Không tìm thấy bài viết.</p>
  </div>
</template>

<script>
import axios from 'axios';
import SidebarWrapper from '@/components/SidebarWrapper';

export default {
  components: { SidebarWrapper },
  data() {
    return {
      blog: null,
    };
  },
  watch: {
    '$route.query.id': 'fetchBlogDetails'
  },
  mounted() {
    this.fetchBlogDetails();
  },
  methods: {
    async fetchBlogDetails() {
      const id = this.$route.query.id;
      if (!id) {
        this.blog = null;
        return;
      }
      try {
        const response = await axios.get(`https://localhost:7213/api/Blog/get-by-id?id=${id}`);
        if (response.data && response.data.status === 200) {
          const blog = response.data.responseData;
          // Lấy tên tác giả nếu muốn
          try {
            const authorRes = await axios.get(`https://localhost:7213/api/User/get-by-id?id=${blog.createdBy}`);
            if (authorRes.data && authorRes.data.responseData) {
              blog.authorName = authorRes.data.responseData.fullName || 'Ẩn danh';
            } else {
              blog.authorName = 'Ẩn danh';
            }
          } catch {
            blog.authorName = 'Ẩn danh';
          }
          // Nếu muốn có các trường như blockquote, extraImages, tags... thì bổ sung ở đây từ API hoặc gán mặc định
          // blog.blockquote = blog.blockquote || 'Nội dung blockquote mặc định...'
          // blog.extraImages = blog.extraImages || ['/images/blog/s1.jpg', ...]
          // blog.tags = blog.tags || ['Donation', ...]
          this.blog = blog;
        } else {
          this.blog = null;
        }
      } catch (err) {
        this.blog = null;
      }
    },
    formatDate(dateStr) {
      if (!dateStr) return '';
      const date = new Date(dateStr);
      return date.toLocaleDateString('vi-VN');
    },
  }
};
</script>
