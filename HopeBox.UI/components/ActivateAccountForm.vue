<template>
    <div class="login-container">
        <div class="login-box">
            <h2 class="title">Kích Hoạt Tài Khoản</h2>

            <form @submit.prevent="handleActivate">
                <div class="form-group email-group">
                    <label for="email">Email</label>
                    <div class="input-with-button">
                        <input type="email" id="email" v-model="form.confirmEmail" placeholder="Nhập email đã đăng ký"
                            required />
                        <span class="send-code" :class="{ disabled: countdown > 0 }" @click="handleSendCode">
                            {{ countdown > 0 ? countdown + 's' : 'Gửi' }}
                        </span>
                    </div>
                </div>

                <div class="form-group">
                    <label for="code">Mã Kích Hoạt</label>
                    <input type="text" id="code" v-model="form.confirmCode" placeholder="Nhập mã kích hoạt"
                        required />
                </div>

                <button type="submit" class="btn-login">Kích Hoạt</button>

                <div class="signup-link">
                    <nuxt-link to="/login">Quay lại đăng nhập</nuxt-link>
                </div>
            </form>
        </div>
    </div>
</template>

<script>
    import axios from 'axios'
    import { showSuccessAlert, showErrorAlert } from '@/utils/alertHelper.js'

    export default {
        data() {
            return {
                form: {
                    confirmEmail: '',
                    confirmCode: ''
                },
                message: '',
                isError: false,
                countdown: 0,
                timer: null
            }
        },
        methods: {
            async handleActivate() {
                try {
                    const response = await axios.post(
                        'https://localhost:7213/api/Authentication/confirm-email',
                        this.form
                    )

                    if (response.data.status === 200) {
                        await showSuccessAlert('Thành công', 'Tài khoản đã được kích hoạt!')
                        this.$router.push('/login')
                    } else {
                        this.message = response.data.message || 'Kích hoạt thất bại.'
                        this.isError = true
                    }
                } catch (err) {
                    this.message = err.response?.data?.message || 'Lỗi khi kích hoạt tài khoản.'
                    this.isError = true
                }
            },

            async handleSendCode() {
                if (!this.form.confirmEmail || this.countdown > 0) return

                try {
                    const response = await axios.post(
                        'https://localhost:7213/api/Authentication/send-confirmation-code',
                        { email: this.form.confirmEmail }
                    )

                    if (response.data.status === 200) {
                        await showSuccessAlert('Đã gửi mã', 'Vui lòng kiểm tra email của bạn.')
                        this.startCountdown()
                    } else {
                        await showErrorAlert('Thất bại', response.data.message || 'Không gửi được mã.')
                    }
                } catch (err) {
                    await showErrorAlert('Lỗi', err.response?.data?.message || 'Có lỗi xảy ra.')
                }
            },

            startCountdown() {
                this.countdown = 60
                this.timer = setInterval(() => {
                    if (this.countdown > 0) {
                        this.countdown--
                    } else {
                        clearInterval(this.timer)
                    }
                }, 1000)
            }
        },
        beforeDestroy() {
            if (this.timer) {
                clearInterval(this.timer)
            }
        }
    }
</script>

<style scoped>
    .login-container {
        display: flex;
        justify-content: center;
        align-items: center;
        padding: 100px 0;
        background-color: #fef9f6;
        min-height: 70vh;
    }

    .login-box {
        background: white;
        padding: 40px 30px;
        border-radius: 20px;
        box-shadow: 0 15px 30px rgba(0, 0, 0, 0.05);
        max-width: 420px;
        width: 100%;
    }

    .title {
        text-align: center;
        font-size: 28px;
        font-weight: 700;
        color: #232323;
        margin-bottom: 30px;
    }

    .form-group {
        margin-bottom: 20px;
    }

    label {
        font-weight: 600;
        margin-bottom: 8px;
        color: #444;
        display: block;
    }

    input {
        width: 100%;
        padding: 10px 15px;
        border: 1px solid #ddd;
        border-radius: 10px;
        transition: 0.3s;
    }

    input:focus {
        outline: none;
        border-color: #ff7a18;
        box-shadow: 0 0 0 3px rgba(255, 122, 24, 0.1);
    }

    .btn-login {
        width: 100%;
        padding: 12px;
        background: linear-gradient(to right, #ff7a18, #ffb347);
        border: none;
        border-radius: 30px;
        color: white;
        font-weight: bold;
        font-size: 16px;
        cursor: pointer;
        margin-top: 10px;
        transition: 0.3s;
    }

    .btn-login:hover {
        opacity: 0.9;
    }

    .signup-link {
        margin-top: 20px;
        text-align: center;
        font-size: 14px;
    }

    .signup-link a {
        color: #ff7a18;
        font-weight: bold;
        margin-left: 5px;
        text-decoration: none;
    }

    .message {
        margin-bottom: 20px;
        text-align: center;
        font-weight: bold;
    }

    .success {
        color: #27ae60;
    }

    .error {
        color: #d63031;
    }

    .input-with-button {
        display: flex;
        align-items: center;
        position: relative;
    }

    .input-with-button input {
        flex: 1;
        padding-right: 60px;
        /* để chữ Gửi không đè lên text */
    }

    .send-code {
        position: absolute;
        right: 10px;
        color: #ff7a18;
        font-weight: bold;
        cursor: pointer;
        user-select: none;
    }

    .send-code.disabled {
        color: #aaa;
        cursor: not-allowed;
    }
</style>