<template>
    <div class="donation-form bgcolor-theme3">
        <div class="section-title">
            <h5 class="subtitle line-theme-color mb-14">Đóng Góp</h5>
            <h2 class="title text-white">Quyên Góp Ngay</h2>
            <img class="line-shape" src="/images/shape/line-t1.png" alt="image">
        </div>
        <form @submit.prevent="submitDonation">
            <div class="amount-info">
                <div class="donate-amount" @click="setAmount(20000)">20.000₫</div>
                <div class="donate-amount" @click="setAmount(50000)">50.000₫</div>
                <div class="donate-amount" @click="setAmount(100000)">100.000₫</div>
                <div class="donate-amount donate-custom-amount">
                    <input class="form-control" type="number" placeholder="Nhập số tiền"
                        v-model.number="donation.donationAmount" min="0">
                </div>
            </div>
            <div class="relief-packages mt-4">
                <h4 class="text-white mb-3">Chọn Gói Cứu Trợ (Tùy Chọn)</h4>
                <div class="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4">
                    <p v-if="reliefPackages.length === 0" class="text-white fst-italic mt-2">
                        Hiện tại không có gói cứu trợ nào khả dụng.
                    </p>
                    <div class="col" v-for="pkg in reliefPackages" :key="pkg.id">
                        <div class="pkg-card">
                            <img :src="pkg.image || '/images/placeholder.jpg'" alt="Package Image" class="pkg-image"
                                @mouseover="showTooltip(pkg.id)" @mouseleave="hideTooltip(pkg.id)">
                            <h5 class="pkg-name">{{ pkg.name }}</h5>
                            <p class="pkg-price">Giá: {{ formatCurrency(pkg.totalPrice + pkg.extraFee) }}</p>
                            <div class="quantity-control mt-2">
                                <button class="btn btn-sm btn-outline-secondary"
                                    @click.prevent="decreaseQuantity(pkg)">−</button>
                                <span class="quantity-display mx-2">
                                    {{ pkg.selectedQuantity }} / {{ getAvailableQuantity(pkg) }}
                                </span>
                                <button class="btn btn-sm btn-outline-secondary"
                                    @click.prevent="increaseQuantity(pkg)">+</button>
                            </div>
                            <div v-if="pkg.showTooltip" class="tooltip">
                                <div class="tooltip-inner">
                                    <p>Bao gồm:</p>
                                    <p v-for="item in pkg.packageItems" :key="item.reliefItemId">
                                        {{ getReliefItemName(item.reliefItemId) }} - {{ item.quantity }}
                                        {{ getReliefItemUnit(item.reliefItemId) }}
                                    </p>
                                </div>
                                <div class="tooltip-arrow"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="payment-method-wrp mt-4">
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
            <div class="personal-info mt-4">
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
            <div class="btn-wrp mt-4">
                <button type="submit" class="btn-theme btn-gradient btn-slide" :disabled="totalAmount <= 0">
                    Quyên Góp Ngay
                </button>
                <a class="btn-theme btn-gradient btn-border">Tổng tiền: {{ formatCurrency(totalAmount) }}</a>
            </div>
        </form>
        <div class="layer-style">
            <img class="layer-style1" src="/images/shape/form-shape1.png" alt="image">
            <img class="layer-style2" src="/images/shape/form-shape2.png" alt="image">
        </div>
    </div>
</template>

<script>
    import { PaymentMethod, PaymentMethodLabel, Unit, UnitLabel } from '@/enums/enums.js';
    import axios from 'axios';
    import { showSuccessAlert, showErrorAlert } from '@/utils/alertHelper.js';

    import { BASE_URL } from '@/utils/constants';

    export default {
        props: {
            causeId: {
                type: String,
                required: true
            }
        },
        data() {
            return {
                donation: {
                    causeId: this.causeId,
                    donationAmount: 50000,
                    paymentMethod: PaymentMethod.VNPAY,
                    isAnonymous: false,
                    message: ''
                },
                reliefPackages: [],
                reliefItems: [],
                paymentLabels: PaymentMethodLabel,
                tooltipTimeout: null,
                units: Unit,
                unitLabels: UnitLabel
            };
        },
        computed: {
            totalAmount() {
                const packageTotal = this.reliefPackages.reduce((sum, pkg) => {
                    return sum + (pkg.selectedQuantity || 0) * (pkg.totalPrice + (pkg.extraFee || 0));
                }, 0);
                return (this.donation.donationAmount || 0) + packageTotal;
            }
        },
        methods: {
            formatCurrency(amount) {
                return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(amount);
            },
            setAmount(value) {
                this.donation.donationAmount = value;
            },
            getAvailableQuantity(pkg) {
                return (pkg.targetQuantity || 0) - (pkg.currentQuantity || 0);
            },
            getReliefItemName(reliefItemId) {
                const item = this.reliefItems.find(i => i.id === reliefItemId);
                return item ? item.itemName : 'Unknown Item';
            },
            getReliefItemUnit(reliefItemId) {
                const item = this.reliefItems.find(i => i.id === reliefItemId);
                return item ? this.unitLabels[item.unit] : 'Unknown Item';
            },
            showTooltip(packageId) {
                clearTimeout(this.tooltipTimeout);
                this.tooltipTimeout = setTimeout(() => {
                    this.reliefPackages = this.reliefPackages.map(pkg => ({
                        ...pkg,
                        showTooltip: pkg.id === packageId
                    }));
                }, 200);
            },
            hideTooltip(packageId) {
                clearTimeout(this.tooltipTimeout);
                this.reliefPackages = this.reliefPackages.map(pkg => ({
                    ...pkg,
                    showTooltip: false
                }));
            },
            increaseQuantity(pkg) {
                const max = this.getAvailableQuantity(pkg);
                if ((pkg.selectedQuantity || 0) < max) {
                    pkg.selectedQuantity = (pkg.selectedQuantity || 0) + 1;
                }
            },
            decreaseQuantity(pkg) {
                if ((pkg.selectedQuantity || 0) > 0) {
                    pkg.selectedQuantity = (pkg.selectedQuantity || 0) - 1;
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
                    await showErrorAlert('Lỗi', 'Không thể tải danh sách vật phẩm cứu trợ.');
                }
            },
            async fetchReliefPackages() {
                try {
                    const res = await axios.get(`${BASE_URL}/api/ReliefPackage/get-relief-packages-by-cause-id?causeId=${this.causeId}`, {
                        withCredentials: true
                    });
                    if (res.data.status === 200) {
                        this.reliefPackages = await Promise.all(res.data.responseData.map(async (pkg) => {
                            const itemsRes = await axios.get(`${BASE_URL}/api/ReliefPackageItem/get-by-package?packageId=${pkg.id}`, {
                                withCredentials: true
                            });
                            return {
                                ...pkg,
                                selectedQuantity: 0,
                                showTooltip: false,
                                packageItems: itemsRes.data.status === 200 ? itemsRes.data.responseData : []
                            };
                        }));
                    }
                } catch (err) {
                    console.error('Lỗi tải danh sách gói cứu trợ:', err);
                    await showErrorAlert('Lỗi', 'Không thể tải danh sách gói cứu trợ.');
                }
            },
            async submitDonation() {

                try {
                    await axios.get(`${BASE_URL}/api/Authentication/me`, {
                        withCredentials: true
                    })

                } catch (err) {
                    await showErrorAlert('Lỗi', 'Làm ơn đăng nhập để quyên góp.');
                    return;
                }

                if (this.totalAmount <= 0 || (this.donation.donationAmount || 0) < 0) {
                    await showErrorAlert('Lỗi', 'Số tiền quyên góp phải lớn hơn 0.');
                    return;
                }
                try {
                    const payload = {
                        causeId: this.donation.causeId,
                        donationAmount: this.donation.donationAmount || 0,
                        amount: this.totalAmount,
                        paymentMethod: Number(this.donation.paymentMethod),
                        isAnonymous: this.donation.isAnonymous,
                        message: this.donation.message || null,
                        reliefPackages: this.reliefPackages
                            .filter(pkg => (pkg.selectedQuantity || 0) > 0)
                            .reduce((acc, pkg) => {
                                acc[pkg.id] = pkg.selectedQuantity;
                                return acc;
                            }, {})
                    };

                    const res = await axios.post(`${BASE_URL}/api/Donation/create-donation`, payload, {
                        withCredentials: true
                    });

                    if (res.data.status !== 200) {
                        throw new Error(res.data.message || 'Không thể tạo đơn quyên góp.');
                    }

                    const paymentUrl = res.data.responseData;
                    if (!paymentUrl) {
                        throw new Error('Không lấy được URL thanh toán.');
                    }

                    const result = await showSuccessAlert('Thành công', 'Tạo đơn quyên góp thành công!');
                    if (result.isConfirmed) {
                        window.location.href = paymentUrl;
                    }
                } catch (err) {
                    console.error('Lỗi:', err);
                    await showErrorAlert(
                        'Đã xảy ra lỗi',
                        err.message || err.response?.data?.message || 'Có lỗi xảy ra. Vui lòng thử lại.'
                    );
                }
            }
        },
        async mounted() {
            await Promise.all([this.fetchReliefItems(), this.fetchReliefPackages()]);
        }
    };
</script>

<style scoped>
    .donation-form {
        position: relative;
        padding: 40px;
        border-radius: 12px;
        overflow: hidden;
    }

    .section-title {
        text-align: center;
        margin-bottom: 30px;
    }

    .subtitle {
        font-size: 1rem;
        font-weight: 600;
        text-transform: uppercase;
    }

    .title {
        font-size: 2rem;
        font-weight: 700;
    }

    .line-shape {
        margin-top: 10px;
    }

    .amount-info {
        display: flex;
        flex-wrap: wrap;
        gap: 10px;
        justify-content: center;
    }

    .donate-amount {
        background: rgba(255, 255, 255, 0.1);
        color: #ffffff;
        padding: 0px 20px;
        border-radius: 8px;
        cursor: pointer;
        font-size: 1rem;
        transition: background 0.3s ease;
    }

    .donate-amount:hover {
        background: #fbc658;
        color: #111226;
    }

    .donate-custom-amount {
        background: transparent;
    }

    .donate-custom-amount input {
        background: rgba(255, 255, 255, 0.1);
        color: #ffffff;
        border: none;
        padding: 10px;
        border-radius: 8px;
        width: 150px;
    }

    .donate-custom-amount input::placeholder {
        color: rgba(255, 255, 255, 0.6);
    }

    .relief-packages {
        margin-bottom: 30px;
    }

    .relief-packages h4 {
        font-size: 1.25rem;
        font-weight: 600;
        color: #fbc658;
        margin-bottom: 20px;
    }

    .pkg-card {
        background: rgba(255, 255, 255, 0.05);
        border-radius: 8px;
        padding: 15px;
        text-align: center;
        transition: transform 0.3s ease;
        position: relative;
    }

    .pkg-card:hover {
        transform: translateY(-5px);
    }

    .pkg-image {
        width: 100%;
        height: 150px;
        object-fit: cover;
        border-radius: 8px;
        margin-bottom: 10px;
        cursor: pointer;
    }

    .pkg-name {
        font-size: 1.1rem;
        font-weight: 600;
        color: #fbc658;
        margin-bottom: 5px;
    }

    .pkg-price {
        font-size: 1rem;
        color: #ffffff;
        margin-bottom: 10px;
    }

    .quantity-control {
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 5px;
    }

    .quantity-control .btn {
        background: rgba(255, 255, 255, 0.1);
        border: 1px solid #fbc658;
        color: #fbc658;
        padding: 5px 12px;
        font-size: 1rem;
        border-radius: 4px;
        transition: background 0.3s ease;
    }

    .quantity-control .btn:hover {
        background: #fbc658;
        color: #111226;
    }

    .quantity-display {
        font-size: 1rem;
        color: #ffffff;
        min-width: 60px;
        text-align: center;
    }

    .tooltip {
        position: absolute;
        bottom: calc(100% + 10px);
        background: rgba(0, 0, 0, 0.9);
        color: #ffffff;
        padding: 12px;
        border-radius: 6px;
        font-size: 16px;
        z-index: 1000;
        text-align: left;
        border: 1px solid #fbc658;
        opacity: 0;
        visibility: hidden;
        transition: opacity 0.2s ease, visibility 0.2s ease;
        width: 100%;
    }

    .pkg-card:hover .tooltip {
        opacity: 1;
        visibility: visible;
    }

    .tooltip-inner p {
        margin: 3px 0;
        text-align: left;
        line-height: 1.4;
    }

    .tooltip-arrow {
        position: absolute;
        bottom: -6px;
        left: 50%;
        transform: translateX(-50%);
        width: 0;
        height: 0;
        border-left: 6px solid transparent;
        border-right: 6px solid transparent;
        border-top: 6px solid #fbc658;
    }

    .payment-method-wrp h4 {
        font-size: 1.25rem;
        font-weight: 600;
        color: #fbc658;
        margin-bottom: 15px;
    }

    .payment-method {
        display: flex;
        align-items: center;
        justify-content: space-between;
        flex-wrap: wrap;
    }

    .payment-type {
        display: flex;
        gap: 15px;
    }

    .form-check-label {
        color: #ffffff;
        margin-left: 5px;
    }

    .personal-info .form-check-label {
        font-size: 1rem;
    }

    .textarea {
        background: rgba(255, 255, 255, 0.1);
        color: #ffffff;
        border: none;
        padding: 10px;
        border-radius: 8px;
        resize: vertical;
        min-height: 100px;
    }

    .textarea::placeholder {
        color: rgba(255, 255, 255, 0.6);
    }

    .btn-wrp {
        display: flex;
        gap: 15px;
        justify-content: center;
        flex-wrap: wrap;
    }

    .btn-theme {
        padding: 12px 30px;
        font-size: 1rem;
        font-weight: 600;
        border-radius: 8px;
        text-transform: uppercase;
    }

    .btn-gradient {
        background: linear-gradient(90deg, #fbc658, #f6a93b);
        color: #111226;
        border: none;
    }

    .btn-border {
        background: transparent;
        border: 2px solid #fbc658;
        color: #fbc658;
    }

    .layer-style {
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        pointer-events: none;
    }

    .layer-style1 {
        position: absolute;
        top: 0;
        left: 0;
    }

    .layer-style2 {
        position: absolute;
        bottom: 0;
        right: 0;
    }

    @media (max-width: 768px) {
        .donation-form {
            padding: 20px;
        }

        .pkg-image {
            height: 120px;
        }

        .quantity-control .btn {
            padding: 3px 10px;
        }

        .payment-method {
            flex-direction: column;
            align-items: flex-start;
            gap: 15px;
        }

        .btn-wrp {
            flex-direction: column;
            align-items: center;
        }

        .tooltip {
            max-width: 100%;
        }
    }

    @media (max-width: 576px) {
        .pkg-image {
            height: 100px;
        }

        .pkg-name {
            font-size: 1rem;
        }

        .pkg-price {
            font-size: 0.9rem;
        }

        .quantity-display {
            font-size: 0.9rem;
        }

        .tooltip {
            font-size: 0.8rem;
            max-width: 100%;
        }
    }
</style>