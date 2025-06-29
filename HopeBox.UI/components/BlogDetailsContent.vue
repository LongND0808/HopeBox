<template>
  <section class="blog-details-area" v-if="blog">
    <div class="container">
      <div class="row">
        <div class="col-12">
          <div class="blog-details-column">
            <div class="post-details-content">
              <div class="post-details-body">
                <div class="thumb">
                  <img class="w-100" :src="blog.imageUrl || '/images/blog/g1.jpg'" :alt="blog.title" width="800"
                    height="400" loading="lazy" />
                </div>
                <div class="content">
                  <div class="meta">
                    <nuxt-link to="/blog" class="post-category">{{ blog.category || 'Chưa phân loại' }}</nuxt-link>
                    <nuxt-link to="/blog" class="post-author">
                      <span class="icon">
                        <img class="icon-img" src="/images/icons/admin1.png" alt="Icon-Image">
                      </span>Đăng bởi: {{ blog.authorName || 'Ẩn danh' }}
                    </nuxt-link>
                  </div>
                  <h2 class="title">{{ blog.title }}</h2>
                  <p>{{ blog.description }}</p>
                  <div class="blog-stats">
                    <span><i class="icofont-eye"></i> {{ blog.viewCount || 0 }} lượt xem</span>
                    <span><i class="icofont-heart"></i> {{ blog.likeCount || 0 }} lượt thích</span>
                    <span><i class="icofont-share"></i> {{ blog.shareCount || 0 }} lượt chia sẻ</span>
                  </div>
                  <div v-html="blog.content"></div>
                  <div class="blog-actions">
                    <button class="btn-theme btn-gradient btn-slide no-border" @click="toggleLike"
                      :disabled="actionLoading">
                      <span v-if="actionLoading && actionType === 'like'">Đang xử lý...</span>
                      <span v-else>{{ isLiked ? 'Bỏ thích' : 'Thích' }}</span>
                    </button>
                    <div class="social-items">
                      <a href="#" @click.prevent="shareBlog(0)"><i class="icofont-facebook"></i></a>
                      <a href="#" @click.prevent="shareBlog(1)"><i class="icofont-skype"></i></a>
                      <a href="#" @click.prevent="shareBlog(2)"><i class="icofont-twitter"></i></a>
                    </div>
                  </div>
                  <div class="blockquote-area">
                    <blockquote class="blockquote-style">
                      <p>{{ blog.blockquote || 'Mỗi hành động tử tế hôm nay, là hạt giống hy vọng cho ngày mai.' }}</p>
                      <div class="icon">“</div>
                    </blockquote>
                  </div>
                  <div class="row mb-32">
                    <div class="col-sm-4"
                      v-for="(img, i) in blog.extraImages || ['/images/blog/s1.jpg', '/images/blog/s2.jpg', '/images/blog/s3.jpg']"
                      :key="i">
                      <img class="w-100 mb-xs-30" :src="img" alt="Extra image" width="200" height="150" loading="lazy">
                    </div>
                  </div>
                  <div class="category-social-content">
                    <div class="social-items">
                      <a href="#" @click.prevent="shareBlog('Facebook')"><i class="icofont-facebook"></i></a>
                      <a href="#" @click.prevent="shareBlog('Skype')"><i class="icofont-skype"></i></a>
                      <a href="#" @click.prevent="shareBlog('Twitter')"><i class="icofont-twitter"></i></a>
                    </div>
                    <div class="category-items">
                      <span>Tags: </span>
                      <nuxt-link v-for="(tag, i) in parsedTags" :key="i" :to="`/blog?tags=${encodeURIComponent(tag)}`">
                        {{ tag }}<span v-if="i < parsedTags.length - 1">, </span>
                      </nuxt-link>
                    </div>
                  </div>
                </div>
                <div class="meta mt-3">
                  <div>Ngày đăng: {{ formatDate(blog.createdAt) }}</div>
                  <div>Tác giả: {{ blog.authorName || 'Ẩn danh' }}</div>
                </div>
                <div class="comment-area">
                  <h2 class="title">Comments <span>({{ blog.commentCount || 0 }})</span></h2>
                  <div class="comment-list" v-if="nestedComments.length">
                    <div v-for="comment in nestedComments" :key="comment.id" class="comment-item"
                      :style="{ 'margin-left': comment.level * 20 + 'px' }">
                      <div class="comment-content">
                        <p>
                          <strong>{{ comment.userName || 'Ẩn danh' }}</strong>
                          <span v-if="comment.addresseeName"> @{{ comment.addresseeName }}</span>
                          - {{ formatDate(comment.createdAt) }}
                        </p>
                        <p>{{ comment.content }}</p>
                        <div class="comment-actions">
                          <button @click="startReply(comment.id, comment.userName)" class="btn-reply">Trả lời</button>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div class="comment-form">
                    <h2 class="title">{{ replyingTo ? 'Trả lời bình luận' : 'Đăng bình luận' }}</h2>
                    <form @submit.prevent="submitComment">
                      <div class="comment-form-content">
                        <div class="row row-gutter-20">
                          <div class="col-md-12">
                            <div class="form-group mb-0">
                              <textarea v-model="commentForm.comment" class="form-control textarea" name="comment"
                                :placeholder="replyingTo ? 'Viết trả lời...' : 'Bình luận'" required
                                rows="7"></textarea>
                            </div>
                          </div>
                        </div>
                        <div class="row" v-if="replyingTo">
                          <div class="col-md-12">
                            <button type="button" @click="cancelReply" class="btn-cancel">Hủy trả lời</button>
                          </div>
                        </div>
                        <div class="row">
                          <div class="col-md-12">
                            <div class="form-group mb-0">
                              <button class="btn-theme btn-gradient btn-slide no-border" type="submit"
                                :disabled="commentLoading">
                                <span v-if="commentLoading">Đang gửi...</span>
                                <span v-else>{{ replyingTo ? 'Gửi trả lời' : 'Gửi bình luận' }}</span>
                              </button>
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
  import { BASE_URL } from '@/utils/constants';
  import { showErrorAlert, showSuccessAlert } from '@/utils/alertHelper';

  export default {
    data() {
      return {
        blog: null,
        comments: [],
        nestedComments: [],
        commentForm: {
          comment: '',
        },
        replyingTo: null,
        replyingToName: null,
        commentLoading: false,
        actionLoading: false,
        actionType: null,
        isLiked: false,
        loading: false,
      };
    },
    head() {
      return {
        title: this.blog?.title || 'Bài viết',
        meta: [
          {
            hid: 'description',
            name: 'description',
            content: this.blog?.metaDescription || 'Không có mô tả.',
          },
          {
            hid: 'keywords',
            name: 'keywords',
            content: this.blog?.metaKeywords || 'blog, bài viết',
          },
        ],
      };
    },
    computed: {
      parsedTags() {
        if (!this.blog || !this.blog.tags) return ['Chưa phân loại'];
        if (Array.isArray(this.blog.tags)) return this.blog.tags.length ? this.blog.tags : ['Chưa phân loại'];
        const tags = this.blog.tags.split(',').map((tag) => tag.trim()).filter((tag) => tag);
        return tags.length ? tags : ['Chưa phân loại'];
      },
      isAuthenticated() {
        // Replace with your actual authentication check (e.g., checking JWT in localStorage or Vuex/Pinia store)
        return !!localStorage.getItem('authToken'); // Example: Check for JWT token
      },
    },
    watch: {
      '$route.query.id': {
        handler: 'fetchBlogDetails',
        immediate: true,
      },
    },
    methods: {
      getAuthenticatedUserId() {
        // Replace with your actual method to retrieve the authenticated user's ID
        // Example: Decode JWT token or access from Vuex/Pinia store
        const token = localStorage.getItem('authToken');
        if (token) {
          try {
            const payload = JSON.parse(atob(token.split('.')[1])); // Decode JWT payload
            return payload.id || null;
          } catch {
            return null;
          }
        }
        return null;
      },
      async fetchBlogDetails() {
        const id = this.$route.query.id;
        if (!id) {
          this.blog = null;
          await showErrorAlert('Lỗi', 'Không tìm thấy ID bài viết.');
          return;
        }
        try {
          this.loading = true;
          this.blog = null;

          // Increment view count
          await this.incrementViewCount(id);

          const blogResponse = await axios.get(`${BASE_URL}/api/Blog/get-by-id?id=${id}`, {
            withCredentials: true,
          });
          if (blogResponse.data?.status === 200) {
            const blog = blogResponse.data.responseData;
            let authorName = blog.creatorName || 'Ẩn danh';

            // Fetch user data for author details
            if (blog.createdBy) {
              try {
                const userResponse = await axios.get(`${BASE_URL}/api/User/get-by-id?id=${blog.createdBy}`, {
                  withCredentials: true,
                });
                if (userResponse.data?.status === 200 && userResponse.data.responseData) {
                  authorName = userResponse.data.responseData.fullName || userResponse.data.responseData.userName || 'Ẩn danh';
                }
              } catch (userError) {
                console.warn('Không thể lấy thông tin tác giả:', userError);
              }
            }

            this.blog = {
              ...blog,
              authorName,
              imageUrl: blog.imageUrl || '/images/blog/g1.jpg',
              content: blog.content || '<p>Không có nội dung.</p>',
              description: blog.description || 'Không có mô tả.',
              category: blog.category || 'Chưa phân loại',
              viewCount: blog.viewCount || 0,
              likeCount: blog.likeCount || 0,
              shareCount: blog.shareCount || 0,
              commentCount: blog.commentCount || 0,
              metaDescription: blog.metaDescription || blog.description || 'Không có mô tả.',
              metaKeywords: blog.metaKeywords || blog.tags || 'blog, bài viết',
            };

            // Fetch comments
            await this.fetchComments(id);

            // Check if user has liked the blog
            if (this.isAuthenticated) {
              const likeResponse = await axios.get(`${BASE_URL}/api/Blog/get-by-id?id=${id}`, {
                withCredentials: true,
              });
              if (likeResponse.data?.status === 200) {
                this.isLiked = likeResponse.data.responseData.isLikedByCurrentUser || false;
              }
            }
          } else {
            await showErrorAlert('Lỗi', blogResponse.data?.message || 'Không tìm thấy bài viết.');
          }
        } catch (err) {
          await showErrorAlert('Lỗi', err.response?.data?.message || 'Không thể tải bài viết.');
          console.error('Lỗi tải bài viết:', err);
        } finally {
          this.loading = false;
        }
      },
      async fetchComments(blogId) {
        try {
          const response = await axios.get(`${BASE_URL}/api/Blog/${blogId}/comments?pageIndex=1&pageSize=10`, {
            withCredentials: true,
          });
          if (response.data?.status === 200) {
            this.comments = await this.transformComments(response.data.responseData || []);
            this.nestedComments = this.buildNestedComments(this.comments);
          } else {
            await showErrorAlert('Lỗi', response.data?.message || 'Không thể tải bình luận.');
            this.comments = [];
            this.nestedComments = [];
          }
        } catch (err) {
          await showErrorAlert('Lỗi', err.response?.data?.message || 'Không thể tải bình luận.');
          console.error('Lỗi tải bình luận:', err);
          this.comments = [];
          this.nestedComments = [];
        }
      },
      async transformComments(comments) {
        const transformed = [];
        for (const comment of comments) {
          let userName = 'Ẩn danh';
          let addresseeName = null;

          // Fetch user name for comment
          try {
            const userResponse = await axios.get(`${BASE_URL}/api/User/get-by-id?id=${comment.userId}`, {
              withCredentials: true,
            });
            if (userResponse.data?.status === 200 && userResponse.data.responseData) {
              userName = userResponse.data.responseData.fullName || userResponse.data.responseData.userName || 'Ẩn danh';
            }
          } catch (userError) {
            console.warn(`Không thể lấy thông tin người dùng ${comment.userId}:`, userError);
          }

          // Fetch addressee name for replies
          if (comment.parentCommentId) {
            const parentComment = comments.find(c => c.id === comment.parentCommentId);
            if (parentComment) {
              try {
                const parentUserResponse = await axios.get(`${BASE_URL}/api/User/get-by-id?id=${parentComment.userId}`, {
                  withCredentials: true,
                });
                if (parentUserResponse.data?.status === 200 && parentUserResponse.data.responseData) {
                  addresseeName = parentUserResponse.data.responseData.fullName || parentUserResponse.data.responseData.userName || 'Ẩn danh';
                }
              } catch (parentError) {
                console.warn(`Không thể lấy thông tin người dùng cho comment cha ${comment.parentCommentId}:`, parentError);
              }
            }
          }

          transformed.push({
            ...comment,
            userName,
            addresseeName,
          });
        }
        return transformed;
      },
      buildNestedComments(comments) {
        // Create a map for quick lookup
        const commentMap = new Map();
        comments.forEach(comment => commentMap.set(comment.id, { ...comment, replies: [] }));

        // Build the tree by grouping replies under their parents
        comments.forEach(comment => {
          if (comment.parentCommentId) {
            const parent = commentMap.get(comment.parentCommentId);
            if (parent) {
              parent.replies.push(commentMap.get(comment.id));
            }
          }
        });

        // Collect top-level comments (parentCommentId: null)
        const nested = [];
        commentMap.forEach(comment => {
          if (!comment.parentCommentId) {
            nested.push(comment);
          }
        });

        // Sort top-level comments by createdAt descending
        nested.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));

        // Flatten the tree while preserving hierarchy with level
        const flattened = [];
        const flattenComments = (comment, level = 0) => {
          comment.level = level; // Update level for display
          flattened.push(comment);
          if (comment.replies && comment.replies.length) {
            // Sort replies by createdAt ascending
            comment.replies.sort((a, b) => new Date(a.createdAt) - new Date(b.createdAt));
            comment.replies.forEach(reply => flattenComments(reply, level + 1));
          }
        };

        nested.forEach(comment => flattenComments(comment, comment.level || 0));

        return flattened;
      },
      async incrementViewCount(blogId) {
        try {
          const response = await axios.put(`${BASE_URL}/api/Blog/${blogId}/view`, null, {
            withCredentials: true,
          });
          if (response.data?.status !== 200) {
            console.warn('Không thể tăng lượt xem:', response.data?.message);
          }
        } catch (err) {
          console.error('Lỗi tăng lượt xem:', err);
        }
      },
      async toggleLike() {
        try {
          this.actionLoading = true;
          this.actionType = 'like';

          const response = await axios({
            url: `${BASE_URL}/api/Blog/${this.blog.id}/like`,
            method: this.isLiked ? 'delete' : 'post',
            withCredentials: true,
          });

          if (response.data?.status === 200) {
            this.isLiked = !this.isLiked;
            this.blog.likeCount = this.isLiked ? this.blog.likeCount + 1 : Math.max(0, this.blog.likeCount - 1);
            await showSuccessAlert('Thành công', this.isLiked ? 'Đã thích bài viết.' : 'Đã bỏ thích bài viết.');
          } else {
            await showErrorAlert('Lỗi', response.data?.message || 'Không thể thực hiện hành động.');
          }
        } catch (err) {
          if (err.response?.data?.message === 'Bạn đã like blog này rồi') {
            this.isLiked = true
          }
          await showErrorAlert('Lỗi', err.response?.data?.message || 'Không thể thực hiện hành động.');
          console.error('Lỗi thích/bỏ thích:', err);
        } finally {
          this.actionLoading = false;
          this.actionType = null;
        }
      },

      async shareBlog(platform) {
        try {
          const PLATFORM_NAME = {
            0: 'Facebook',
            1: 'Skype',
            2: 'Twitter'
          };
          this.actionLoading = true;
          this.actionType = 'share';
          const response = await axios.post(`${BASE_URL}/api/Blog/${this.blog.id}/share`, {
            userId: this.getAuthenticatedUserId(),
            platform,
            caption: `Chia sẻ bài viết: ${this.blog.title}`
          }, {
            withCredentials: true,
          });
          if (response.data?.status === 200) {
            this.blog.shareCount++;
            await showSuccessAlert(
              'Thành công',
              `Đã chia sẻ bài viết lên ${PLATFORM_NAME[platform] ?? 'nền tảng?'}.`
            );
          } else {
            await showErrorAlert('Lỗi', response.data?.message || 'Không thể chia sẻ bài viết.');
          }
        } catch (err) {
          await showErrorAlert('Lỗi', err.response?.data?.message || 'Không thể chia sẻ bài viết.');
          console.error('Lỗi chia sẻ:', err);
        } finally {
          this.actionLoading = false;
          this.actionType = null;
        }
      },
      startReply(commentId, userName) {
        this.replyingTo = commentId;
        this.replyingToName = userName;
        this.commentForm.comment = '';
      },
      cancelReply() {
        this.replyingTo = null;
        this.replyingToName = null;
        this.commentForm.comment = '';
      },
      async submitComment() {
        if (!this.commentForm.comment) {
          await showErrorAlert('Lỗi', 'Vui lòng nhập nội dung bình luận.');
          return;
        }
        try {
          this.commentLoading = true;
          const payload = {
            blogId: this.blog.id,
            content: this.commentForm.comment,
          };
          if (this.replyingTo) {
            payload.parentCommentId = this.replyingTo;
          }
          const response = await axios.post(`${BASE_URL}/api/Blog/${this.blog.id}/comment`, payload, {
            withCredentials: true,
          });
          if (response.data?.status === 200) {
            await showSuccessAlert('Thành công', this.replyingTo ? 'Trả lời đã được gửi.' : 'Bình luận đã được gửi.');
            this.commentForm.comment = '';
            this.replyingTo = null;
            this.replyingToName = null;
            await this.fetchBlogDetails();
          } else {
            await showErrorAlert('Lỗi', response.data?.message || 'Không thể gửi bình luận.');
          }
        } catch (err) {
          await showErrorAlert('Lỗi', err.response?.data?.message || 'Không thể gửi bình luận.');
          console.error('Lỗi gửi bình luận:', err);
        } finally {
          this.commentLoading = false;
        }
      },
      formatDate(dateStr) {
        if (!dateStr) return 'N/A';
        try {
          const date = new Date(dateStr);
          return date.toLocaleString('vi-VN', {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
            hour: '2-digit',
            minute: '2-digit',
          });
        } catch {
          return 'N/A';
        }
      },
    },
  };
</script>

<style scoped>
  .blog-details-area {
    padding: 40px 0;
    background: #f5f5f5;
  }

  .container {
    max-width: 1200px;
    margin: 0 auto;
  }

  .post-details-content {
    background: #fff;
    padding: 30px;
    border-radius: 8px;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
  }

  .thumb img {
    width: 100%;
    height: auto;
    aspect-ratio: 2 / 1;
    object-fit: cover;
    border-radius: 8px;
    margin-bottom: 20px;
  }

  .content {
    padding: 20px 0;
  }

  .meta {
    display: flex;
    gap: 15px;
    margin-bottom: 20px;
    flex-wrap: wrap;
  }

  .post-category {
    background: #f6d579;
    color: #111226;
    padding: 5px 10px;
    border-radius: 4px;
    text-decoration: none;
  }

  .post-author {
    color: #666;
    text-decoration: none;
    display: flex;
    align-items: center;
    gap: 5px;
  }

  .post-author .icon-img {
    width: 20px;
    height: 20px;
  }

  .title {
    font-size: 2rem;
    margin-bottom: 20px;
    color: #111226;
  }

  .blog-stats {
    display: flex;
    gap: 15px;
    margin-bottom: 20px;
    color: #666;
    font-size: 0.9rem;
    flex-wrap: wrap;
  }

  .blog-stats span {
    display: flex;
    align-items: center;
    gap: 5px;
  }

  .blog-actions {
    display: flex;
    align-items: center;
    gap: 20px;
    margin-bottom: 20px;
  }

  .blockquote-area {
    margin: 30px 0;
  }

  .blockquote-style {
    position: relative;
    padding: 20px;
    background: #f6d579;
    border-radius: 8px;
    color: #111226;
    font-style: italic;
  }

  .blockquote-style .icon {
    position: absolute;
    top: -10px;
    left: 20px;
    font-size: 3rem;
    color: #111226;
    opacity: 0.3;
  }

  .row.mb-32 {
    margin-bottom: 32px;
  }

  .row.mb-32 img {
    width: 100%;
    height: auto;
    aspect-ratio: 4 / 3;
    object-fit: cover;
    border-radius: 8px;
  }

  .category-social-content {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-top: 20px;
    flex-wrap: wrap;
    gap: 20px;
  }

  .social-items {
    display: flex;
    gap: 10px;
  }

  .social-items a {
    color: #111226;
    font-size: 1.2rem;
    transition: color 0.3s;
  }

  .social-items a:hover {
    color: #f6d579;
  }

  .category-items {
    display: flex;
    gap: 10px;
    align-items: center;
    flex-wrap: wrap;
  }

  .category-items span {
    font-weight: bold;
    color: #111226;
  }

  .category-items a {
    color: #666;
    text-decoration: none;
  }

  .category-items a:hover {
    color: #f6d579;
  }

  .meta.mt-3 {
    display: flex;
    gap: 20px;
    color: #666;
  }

  .comment-area {
    margin-top: 40px;
  }

  .comment-area .title {
    font-size: 1.5rem;
    margin-bottom: 20px;
  }

  .comment-list {
    margin-bottom: 20px;
  }

  .comment-item {
    padding: 10px 0;
    border-bottom: 1px solid #ddd;
  }

  .comment-content p {
    margin: 5px 0;
  }

  .comment-content p:first-child {
    font-size: 0.9rem;
    color: #666;
  }

  .comment-content p:first-child span {
    color: #f6d579;
  }

  .comment-actions {
    margin-top: 5px;
  }

  .btn-reply,
  .btn-cancel {
    background: none;
    border: none;
    color: #f6d579;
    cursor: pointer;
    font-size: 0.9rem;
    padding: 0;
    margin-right: 10px;
  }

  .btn-reply:hover,
  .btn-cancel:hover {
    color: #e0b450;
  }

  .comment-form {
    margin-top: 30px;
  }

  .comment-form-content {
    margin-bottom: 20px;
  }

  .form-group {
    margin-bottom: 15px;
  }

  .form-control {
    width: 100%;
    padding: 10px;
    border: 1px solid #ddd;
    border-radius: 4px;
    font-size: 1rem;
  }

  .textarea {
    resize: vertical;
  }

  .btn-theme {
    background: linear-gradient(45deg, #f6d579, #fbc658);
    color: #111226;
    padding: 10px 20px;
    border: none;
    border-radius: 4px;
    cursor: pointer;
    transition: background 0.3s;
  }

  .btn-theme:hover {
    background: linear-gradient(45deg, #e0b450, #e0b450);
  }

  .btn-theme:disabled {
    background: #e0e0e0;
    cursor: not-allowed;
  }

  @media (max-width: 768px) {
    .post-details-content {
      padding: 20px;
    }

    .title {
      font-size: 1.5rem;
    }

    .meta {
      flex-direction: column;
      gap: 10px;
    }

    .category-social-content {
      flex-direction: column;
      align-items: flex-start;
    }

    .row.mb-32 {
      margin-bottom: 20px;
    }

    .row.mb-32 .col-sm-4 {
      margin-bottom: 20px;
    }

    .blog-stats {
      flex-direction: column;
      gap: 10px;
    }

    .blog-actions {
      flex-direction: column;
      align-items: flex-start;
    }
  }
</style>