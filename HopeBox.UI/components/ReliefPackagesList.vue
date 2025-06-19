<template>
    <div class="relief-packages-list">
        <div class="packages-header">
            <h5>Gói Cứu Trợ Có Sẵn</h5>
            <p>Chọn gói cứu trợ phù hợp để quyên góp cho sự kiện này</p>
        </div>
        
        <!-- Loading State -->
        <div v-if="loading" class="loading-state">
            <div class="loading-skeleton">
                <div class="skeleton-package" v-for="n in 3" :key="n">
                    <div class="skeleton-header"></div>
                    <div class="skeleton-description"></div>
                    <div class="skeleton-price"></div>
                    <div class="skeleton-button"></div>
                </div>
            </div>
        </div>
        
        <!-- Packages Grid -->
        <div v-else-if="packages.length > 0" class="packages-grid">
            <div v-for="pkg in packages" :key="pkg.id" class="package-item">
                <div class="package-header">
                    <h6>{{ pkg.name }}</h6>
                    <div class="package-price">{{ formatCurrency(pkg.totalAmount) }}</div>
                </div>
                
                <div class="package-content">
                    <!-- SỬA: Đổi package thành pkg -->
                    <p class="description">{{ pkg.description || 'Gói cứu trợ thiết yếu' }}</p>
                    
                    <!-- Package Items List -->
                    <!-- SỬA: Đổi package thành pkg -->
                    <div v-if="pkg.packageItems && pkg.packageItems.length > 0" class="package-items">
                        <h6>Nội dung gói:</h6>
                        <ul class="items-list">
                            <!-- SỬA: Đổi package thành pkg -->
                            <li v-for="item in pkg.packageItems" :key="item.id" class="item">
                                <span class="item-name">{{ item.reliefItem?.itemName || 'Vật phẩm' }}</span>
                                <span class="item-quantity">{{ item.quantity }} {{ getUnitLabel(item.reliefItem?.unit) }}</span>
                            </li>
                        </ul>
                    </div>
                    
                    <!-- Quantity Selector -->
                    <div class="quantity-selector">
                        <label>Số lượng gói:</label>
                        <div class="quantity-controls">
                            <button type="button" 
                                    class="quantity-btn" 
                                    @click="decreaseQuantity(pkg.id)"
                                    :disabled="getSelectedQuantity(pkg.id) <= 1">
                                <i class="fas fa-minus"></i>
                            </button>
                            <input type="number" 
                                   class="quantity-input" 
                                   :value="getSelectedQuantity(pkg.id)"
                                   @input="updateQuantity(pkg.id, $event.target.value)"
                                   min="1" 
                                   max="10">
                            <button type="button" 
                                    class="quantity-btn" 
                                    @click="increaseQuantity(pkg.id)"
                                    :disabled="getSelectedQuantity(pkg.id) >= 10">
                                <i class="fas fa-plus"></i>
                            </button>
                        </div>
                    </div>
                    
                    <!-- Total Price -->
                    <div class="package-total">
                        <!-- SỬA: Đổi package thành pkg -->
                        <strong>Tổng: {{ formatCurrency(pkg.totalAmount * getSelectedQuantity(pkg.id)) }}</strong>
                    </div>
                </div>
                
                <div class="package-footer">
                    <!-- SỬA: Đổi package thành pkg -->
                    <button @click="togglePackageSelection(pkg)" 
                            class="btn-select-package"
                            :class="{ 
                                'selected': isPackageSelected(pkg.id),
                                'btn-remove': isPackageSelected(pkg.id)
                            }">
                        <i :class="isPackageSelected(pkg.id) ? 'fas fa-check' : 'fas fa-plus'"></i>
                        {{ isPackageSelected(pkg.id) ? 'Đã chọn' : 'Chọn gói này' }}
                    </button>
                </div>
            </div>
        </div>
        
        <!-- No Packages State -->
        <div v-else class="no-packages">
            <div class="no-packages-content">
                <i class="fas fa-box-open fa-3x"></i>
                <h5>Chưa có gói cứu trợ</h5>
                <p>Hiện tại chưa có gói cứu trợ nào cho sự kiện này.</p>
            </div>
        </div>
        
        <!-- Selected Packages Summary -->
        <div v-if="selectedPackages.length > 0" class="selected-summary">
            <div class="summary-header">
                <h5>Gói đã chọn ({{ selectedPackages.length }})</h5>
                <button @click="clearAllSelections" class="btn-clear">Xóa tất cả</button>
            </div>
            
            <div class="summary-items">
                <div v-for="pkg in selectedPackages" :key="pkg.reliefPackageId" class="summary-item">
                    <span class="summary-name">{{ getPackageName(pkg.reliefPackageId) }}</span>
                    <span class="summary-quantity">x{{ pkg.quantity }}</span>
                    <span class="summary-price">{{ formatCurrency(getPackagePrice(pkg.reliefPackageId) * pkg.quantity) }}</span>
                    <button @click="removePackageSelection(pkg.reliefPackageId)" class="btn-remove-item">
                        <i class="fas fa-times"></i>
                    </button>
                </div>
            </div>
            
            <div class="summary-total">
                <strong>Tổng cộng: {{ formatCurrency(getTotalAmount()) }}</strong>
            </div>
            
            <div class="summary-actions">
                <button @click="proceedToDonation" class="btn-proceed">
                    <i class="fas fa-heart"></i>
                    Tiến hành quyên góp
                </button>
            </div>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import { Unit, UnitLabel } from '@/enums/enums';
import { showErrorAlert, showSuccessAlert } from '@/utils/alertHelper';

export default {
    name: 'ReliefPackagesList',
    props: {
        eventId: {
            type: String,
            required: true
        }
    },
    data() {
        return {
            packages: [],
            loading: false,
            selectedPackages: [],
            packageQuantities: {}
        }
    },
    mounted() {
        this.fetchPackages();
    },
    methods: {
        /**
         * Fetch relief packages for event
         */
        async fetchPackages() {
            try {
                this.loading = true;
                const response = await axios.get(
                    `https://localhost:7213/api/Donation/get-event-relief-packages/${this.eventId}`
                );
                
                if (response.data.status === 200) {
                    this.packages = response.data.responseData;
                    this.initializeQuantities();
                    console.log('Packages loaded:', this.packages); // Debug log
                } else {
                    await showErrorAlert('Lỗi', 'Không thể tải danh sách gói cứu trợ');
                }
            } catch (error) {
                console.error('Error fetching packages:', error);
                await showErrorAlert('Lỗi', 'Không thể kết nối đến server');
            } finally {
                this.loading = false;
            }
        },

        /**
         * Initialize quantities for all packages
         */
        initializeQuantities() {
            const quantities = {};
            this.packages.forEach(pkg => {
                quantities[pkg.id] = 1;
            });
            this.packageQuantities = quantities;
        },

        /**
         * Get selected quantity for a package
         */
        getSelectedQuantity(packageId) {
            return this.packageQuantities[packageId] || 1;
        },

        /**
         * Update quantity for a package
         */
        updateQuantity(packageId, value) {
            const quantity = Math.max(1, Math.min(10, parseInt(value) || 1));
            this.packageQuantities = {
                ...this.packageQuantities,
                [packageId]: quantity
            };
            
            const selectedIndex = this.selectedPackages.findIndex(p => p.reliefPackageId === packageId);
            if (selectedIndex !== -1) {
                this.selectedPackages[selectedIndex].quantity = quantity;
            }
        },

        /**
         * Increase quantity
         */
        increaseQuantity(packageId) {
            const currentQuantity = this.getSelectedQuantity(packageId);
            if (currentQuantity < 10) {
                this.updateQuantity(packageId, currentQuantity + 1);
            }
        },

        /**
         * Decrease quantity
         */
        decreaseQuantity(packageId) {
            const currentQuantity = this.getSelectedQuantity(packageId);
            if (currentQuantity > 1) {
                this.updateQuantity(packageId, currentQuantity - 1);
            }
        },

        /**
         * Check if package is selected
         */
        isPackageSelected(packageId) {
            return this.selectedPackages.some(p => p.reliefPackageId === packageId);
        },

        /**
         * Toggle package selection
         */
        togglePackageSelection(pkg) {
            const packageId = pkg.id;
            const selectedIndex = this.selectedPackages.findIndex(p => p.reliefPackageId === packageId);
            
            if (selectedIndex !== -1) {
                this.selectedPackages.splice(selectedIndex, 1);
            } else {
                this.selectedPackages.push({
                    reliefPackageId: packageId,
                    quantity: this.getSelectedQuantity(packageId)
                });
            }
            
            this.$emit('packages-selected', this.selectedPackages);
        },

        /**
         * Remove package from selection
         */
        removePackageSelection(packageId) {
            const selectedIndex = this.selectedPackages.findIndex(p => p.reliefPackageId === packageId);
            if (selectedIndex !== -1) {
                this.selectedPackages.splice(selectedIndex, 1);
                this.$emit('packages-selected', this.selectedPackages);
            }
        },

        /**
         * Clear all selections
         */
        clearAllSelections() {
            this.selectedPackages = [];
            this.$emit('packages-selected', this.selectedPackages);
        },

        /**
         * Get package name by ID
         */
        getPackageName(packageId) {
            const pkg = this.packages.find(p => p.id === packageId);
            return pkg ? pkg.name : 'Gói cứu trợ';
        },

        /**
         * Get package price by ID
         */
        getPackagePrice(packageId) {
            const pkg = this.packages.find(p => p.id === packageId);
            return pkg ? pkg.totalAmount : 0;
        },

        /**
         * Get total amount of selected packages
         */
        getTotalAmount() {
            return this.selectedPackages.reduce((total, pkg) => {
                const packagePrice = this.getPackagePrice(pkg.reliefPackageId);
                return total + (packagePrice * pkg.quantity);
            }, 0);
        },

        /**
         * Proceed to donation with selected packages
         */
        async proceedToDonation() {
            if (this.selectedPackages.length === 0) {
                await showErrorAlert('Lỗi', 'Vui lòng chọn ít nhất một gói cứu trợ');
                return;
            }

            this.$emit('proceed-donation', {
                packages: this.selectedPackages,
                totalAmount: this.getTotalAmount()
            });
        },

        /**
         * Get unit label
         */
        getUnitLabel(unitValue) {
            return UnitLabel[unitValue] || 'cái';
        },

        /**
         * Format currency
         */
        formatCurrency(amount) {
            if (!amount || amount === 0) {
                return '0 ₫';
            }
            
            try {
                return new Intl.NumberFormat('vi-VN', {
                    style: 'currency',
                    currency: 'VND',
                    minimumFractionDigits: 0,
                    maximumFractionDigits: 0
                }).format(amount);
            } catch (error) {
                return amount.toLocaleString('vi-VN') + ' ₫';
            }
        }
    }
}
</script>

<style scoped>
.relief-packages-list {
    .packages-header {
        text-align: center;
        margin-bottom: 30px;
        
        h5 {
            margin-bottom: 10px;
            color: #333;
            font-weight: 600;
            font-size: 24px;
        }
        
        p {
            color: #666;
            margin: 0;
        }
    }
}

/* ===== LOADING SKELETON ===== */
.loading-skeleton {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
    gap: 20px;
    
    .skeleton-package {
        background: #f8f9fa;
        padding: 20px;
        border-radius: 12px;
        border: 1px solid #e9ecef;
        
        .skeleton-header,
        .skeleton-description,
        .skeleton-price,
        .skeleton-button {
            background: linear-gradient(90deg, #f0f0f0 25%, #e0e0e0 50%, #f0f0f0 75%);
            background-size: 200% 100%;
            animation: loading 1.5s infinite;
            border-radius: 4px;
            margin-bottom: 10px;
        }
        
        .skeleton-header {
            height: 24px;
            width: 70%;
        }
        
        .skeleton-description {
            height: 16px;
            width: 100%;
        }
        
        .skeleton-price {
            height: 20px;
            width: 50%;
        }
        
        .skeleton-button {
            height: 40px;
            width: 100%;
            margin-bottom: 0;
        }
    }
}

@keyframes loading {
    0% { background-position: 200% 0; }
    100% { background-position: -200% 0; }
}

/* ===== PACKAGES GRID ===== */
.packages-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(320px, 1fr));
    gap: 25px;
    margin-bottom: 30px;
}

.package-item {
    background: white;
    border: 2px solid #e9ecef;
    border-radius: 12px;
    overflow: hidden;
    transition: all 0.3s ease;
    
    &:hover {
        border-color: #ff6b1f;
        transform: translateY(-2px);
        box-shadow: 0 8px 25px rgba(255, 107, 31, 0.15);
    }
    
    .package-header {
        background: linear-gradient(135deg, #ff7a36, #ff6b1f);
        color: white;
        padding: 20px;
        text-align: center;
        
        h6 {
            margin-bottom: 10px;
            color: white;
            font-weight: 600;
            font-size: 18px;
        }
        
        .package-price {
            font-size: 24px;
            font-weight: 700;
            margin: 0;
        }
    }
    
    .package-content {
        padding: 20px;
        
        .description {
            color: #666;
            font-size: 14px;
            margin-bottom: 15px;
            line-height: 1.5;
        }
        
        .package-items {
            margin-bottom: 20px;
            
            h6 {
                color: #333;
                font-size: 16px;
                margin-bottom: 10px;
                font-weight: 600;
            }
            
            .items-list {
                list-style: none;
                padding: 0;
                margin: 0;
                
                .item {
                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                    padding: 8px 0;
                    border-bottom: 1px solid #f0f0f0;
                    
                    &:last-child {
                        border-bottom: none;
                    }
                    
                    .item-name {
                        color: #333;
                        font-weight: 500;
                    }
                    
                    .item-quantity {
                        color: #ff6b1f;
                        font-weight: 600;
                        font-size: 14px;
                    }
                }
            }
        }
        
        .quantity-selector {
            margin-bottom: 15px;
            
            label {
                display: block;
                margin-bottom: 8px;
                color: #333;
                font-weight: 500;
            }
            
            .quantity-controls {
                display: flex;
                align-items: center;
                justify-content: center;
                gap: 10px;
                
                .quantity-btn {
                    width: 35px;
                    height: 35px;
                    border: 2px solid #ff6b1f;
                    background: white;
                    color: #ff6b1f;
                    border-radius: 6px;
                    cursor: pointer;
                    transition: all 0.3s ease;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    
                    &:hover:not(:disabled) {
                        background: #ff6b1f;
                        color: white;
                    }
                    
                    &:disabled {
                        opacity: 0.5;
                        cursor: not-allowed;
                    }
                }
                
                .quantity-input {
                    width: 60px;
                    height: 35px;
                    text-align: center;
                    border: 2px solid #e9ecef;
                    border-radius: 6px;
                    font-weight: 600;
                    
                    &:focus {
                        outline: none;
                        border-color: #ff6b1f;
                    }
                }
            }
        }
        
        .package-total {
            text-align: center;
            padding: 10px;
            background: #f8f9fa;
            border-radius: 6px;
            
            strong {
                color: #ff6b1f;
                font-size: 18px;
            }
        }
    }
    
    .package-footer {
        padding: 0 20px 20px;
        
        .btn-select-package {
            width: 100%;
            padding: 12px;
            border: 2px solid #ff6b1f;
            background: white;
            color: #ff6b1f;
            border-radius: 8px;
            cursor: pointer;
            font-weight: 600;
            transition: all 0.3s ease;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
            
            &:hover {
                background: #ff6b1f;
                color: white;
                transform: translateY(-1px);
            }
            
            &.selected {
                background: #28a745;
                border-color: #28a745;
                color: white;
                
                &:hover {
                    background: #dc3545;
                    border-color: #dc3545;
                }
            }
        }
    }
}

/* ===== NO PACKAGES STATE ===== */
.no-packages {
    text-align: center;
    padding: 60px 20px;
    
    .no-packages-content {
        i {
            color: #ccc;
            margin-bottom: 20px;
        }
        
        h5 {
            color: #666;
            margin-bottom: 10px;
        }
        
        p {
            color: #999;
            margin: 0;
        }
    }
}

/* ===== SELECTED SUMMARY ===== */
.selected-summary {
    background: white;
    border: 2px solid #ff6b1f;
    border-radius: 12px;
    padding: 20px;
    margin-top: 30px;
    
    .summary-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 15px;
        
        h5 {
            color: #ff6b1f;
            margin: 0;
            font-weight: 600;
        }
        
        .btn-clear {
            background: none;
            border: 1px solid #dc3545;
            color: #dc3545;
            padding: 5px 10px;
            border-radius: 4px;
            cursor: pointer;
            font-size: 12px;
            
            &:hover {
                background: #dc3545;
                color: white;
            }
        }
    }
    
    .summary-items {
        margin-bottom: 15px;
        
        .summary-item {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 10px 0;
            border-bottom: 1px solid #f0f0f0;
            
            &:last-child {
                border-bottom: none;
            }
            
            .summary-name {
                flex: 1;
                color: #333;
                font-weight: 500;
            }
            
            .summary-quantity {
                color: #666;
                margin: 0 15px;
            }
            
            .summary-price {
                color: #ff6b1f;
                font-weight: 600;
                margin-right: 10px;
            }
            
            .btn-remove-item {
                background: none;
                border: none;
                color: #dc3545;
                cursor: pointer;
                padding: 5px;
                
                &:hover {
                    color: #c82333;
                }
            }
        }
    }
    
    .summary-total {
        text-align: center;
        padding: 15px;
        background: #f8f9fa;
        border-radius: 6px;
        margin-bottom: 20px;
        
        strong {
            color: #ff6b1f;
            font-size: 20px;
        }
    }
    
    .summary-actions {
        text-align: center;
        
        .btn-proceed {
            background: linear-gradient(135deg, #ff7a36, #ff6b1f);
            color: white;
            border: none;
            padding: 15px 30px;
            border-radius: 8px;
            cursor: pointer;
            font-weight: 600;
            font-size: 16px;
            transition: all 0.3s ease;
            display: inline-flex;
            align-items: center;
            gap: 8px;
            
            &:hover {
                transform: translateY(-2px);
                box-shadow: 0 6px 20px rgba(255, 107, 31, 0.3);
            }
        }
    }
}

/* ===== RESPONSIVE DESIGN ===== */
@media (max-width: 767px) {
    .packages-grid {
        grid-template-columns: 1fr;
        gap: 20px;
    }
    
    .package-item {
        .package-content {
            padding: 15px;
        }
        
        .package-footer {
            padding: 0 15px 15px;
        }
    }
    
    .selected-summary {
        padding: 15px;
        
        .summary-header {
            flex-direction: column;
            gap: 10px;
            align-items: stretch;
            
            .btn-clear {
                align-self: center;
            }
        }
        
        .summary-item {
            flex-wrap: wrap;
            gap: 5px;
            
            .summary-name {
                flex-basis: 100%;
            }
        }
    }
}
</style>
