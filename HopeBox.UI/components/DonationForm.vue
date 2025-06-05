<template>
    <div class="donation-form bgcolor-theme3">
        <div class="section-title">
            <h5 class="subtitle line-theme-color mb-14">Đóng Góp</h5>
            <h2 class="title text-white">Quyên Góp Ngay</h2>
            <img class="line-shape" src="/images/shape/line-t3.png" alt="image">
        </div>
        <form @submit.prevent="submitDonation">
            <div class="amount-info">
                <div class="donate-amount" @click="setAmount(20000)">20.000₫ </div>
                <div class="donate-amount" @click="setAmount(50000)">50.000₫ </div>
                <div class="donate-amount" @click="setAmount(100000)">100.00₫ </div>
                <div class="donate-amount donate-custom-amount">
                    <input class="form-control" type="number" placeholder="Nhập số tiền"
                        v-model.number="donation.amount">
                </div>
            </div>
            <div class="payment-method-wrp">
                <h4>Phương Thức Thanh Toán:</h4>
                <div class="payment-method">
                    <div class="payment-type">
                        <div class="form-check form-check-inline" v-for="(label, value) in paymentLabels" :key="value">
                            <input class="form-check-input" type="radio" :id="'payment_' + value" :value="value"
                                v-model="donation.paymentMethod">
                            <label class="form-check-label" :for="'payment_' + value">
                                {{ label }}
                            </label>
                        </div>
                    </div>
                    <img src="/images/photos/payment-card.png" alt="image">
                </div>
            </div>
            <div class="personal-info">
                <div class="row row-gutter-20">
                    <div class="form-group form-check mb-3">
                        <input type="checkbox" class="form-check-input" id="anonymousCheck"
                            v-model="donation.isAnonymous">
                        <label class="form-check-label text-white" for="anonymousCheck">
                            Ẩn danh khi quyên góp
                        </label>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group mb-0">
                            <textarea class="form-control textarea" placeholder="Lời nhắn (không bắt buộc)"
                                v-model="donation.message"></textarea>
                        </div>
                    </div>
                </div>
            </div>
            <div class="btn-wrp">
                <button type="submit" class="btn-theme btn-gradient btn-slide">
                    Quyên Góp Ngay
                    <img class="icon icon-img" src="/images/icons/arrow-line-right2.png" alt="Icon">
                </button>
                <a href="#" class="btn-theme btn-gradient btn-border">Tổng tiền: {{ donation.amount }}₫ </a>
            </div>
        </form>
        <div class="layer-style">
            <img class="layer-style1" src="/images/shape/form-shape1.png" alt="image">
            <img class="layer-style2" src="/images/shape/form-shape2.png" alt="image">
        </div>
    </div>
</template>

<script>
    import { PaymentMethod, PaymentMethodLabel } from '@/enums/enums.js';
    import axios from 'axios';
    import Swal from 'sweetalert2'


    export default {
        props: {
            causesId: {
                type: String,
                required: false
            }
        },
        data() {
            return {
                donation: {
                    userId: '00000000-0000-0000-0000-000000000000',
                    causesId: this.causesId,
                    amount: 50000,
                    donationDate: new Date().toISOString(),
                    paymentMethod: PaymentMethod.VNPAY,
                    transactionId: null,
                    tradingCode: null,
                    status: 0,
                    message: '',
                    isAnonymous: false
                },
                paymentLabels: PaymentMethodLabel
            };
        },
        methods: {
            setAmount(value) {
                this.donation.amount = value;
            },
            submitDonation: async function () {
                try {
                    const payload = {
                        causesId: this.donation.causesId,
                        amount: this.donation.amount,
                        donationDate: this.donation.donationDate,
                        paymentMethod: this.donation.paymentMethod,
                        transactionId: this.donation.transactionId,
                        tradingCode: this.donation.tradingCode,
                        status: this.donation.status,
                        isAnonymous: this.donation.isAnonymous,
                        message: this.donation.message
                    };

                    const res = await axios.post('https://localhost:7213/api/Donation/create-donation', payload, {
                        withCredentials: true
                    });

                    const message = res.data?.message || "Cảm ơn bạn đã quyên góp!";
                    const donationId = res.data?.responseData;

                    if (!donationId) {
                        throw new Error("Không lấy được ID donation từ server.");
                    }

                    const paymentRes = await axios.get(`https://localhost:7213/api/Donation/create-payment-url?donationId=${donationId}`);
                    const paymentUrl = paymentRes.data?.responseData;

                    if (!paymentUrl) {
                        throw new Error("Không lấy được URL thanh toán.");
                    }

                    Swal.fire({
                        title: 'Tạo đơn quyên góp thành Công',
                        text: message,
                        icon: 'success',
                        confirmButtonText: 'Thanh Toán Ngay',
                        confirmButtonColor: '#f37224', 
                        background: '#0c1e25',
                        color: '#fff'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location.href = paymentUrl;
                        }
                    });

                } catch (err) {
                    console.error("Lỗi:", err);
                    Swal.fire({
                        title: 'Đã xảy ra lỗi',
                        text: err.response?.data?.message || "Có lỗi xảy ra. Vui lòng thử lại.",
                        icon: 'error',
                        confirmButtonText: 'Đóng',
                        confirmButtonColor: '#d33',
                        background: '#0c1e25',
                        color: '#fff'
                    });
                }
            }

        }
    };
</script>