// src/enums/index.js

export const Gender = {
  UNKNOWN: 0,
  MALE: 1,
  FEMALE: 2,
  OTHER: 3
};

export const GenderLabel = {
  [Gender.UNKNOWN]: 'Không xác định',
  [Gender.MALE]: 'Nam',
  [Gender.FEMALE]: 'Nữ',
  [Gender.OTHER]: 'Khác'
};

export const UserStatus = {
  ACTIVE: 1,
  INACTIVE: 2,
  SUSPENDED: 3,
  BANNED: 4,
  DELETED: 5
};

export const UserStatusLabel = {
  [UserStatus.ACTIVE]: 'Hoạt động',
  [UserStatus.INACTIVE]: 'Không hoạt động',
  [UserStatus.SUSPENDED]: 'Tạm khóa',
  [UserStatus.BANNED]: 'Cấm',
  [UserStatus.DELETED]: 'Đã xóa'
};

export const VolunteerStatus = {
  PENDING: 1,
  APPROVED: 2,
  REJECTED: 3
};

export const VolunteerStatusLabel = {
  [VolunteerStatus.PENDING]: 'Chờ duyệt',
  [VolunteerStatus.APPROVED]: 'Đã duyệt',
  [VolunteerStatus.REJECTED]: 'Từ chối'
};

export const CausesStatus = {
  PENDING: 1,
  APPROVED: 2,
  ONGOING: 3,
  COMPLETED: 4,
  CANCELED: 5
};

export const CausesStatusLabel = {
  [CausesStatus.PENDING]: 'Chờ duyệt',
  [CausesStatus.APPROVED]: 'Đã duyệt',
  [CausesStatus.ONGOING]: 'Đang diễn ra',
  [CausesStatus.COMPLETED]: 'Đã hoàn thành',
  [CausesStatus.CANCELED]: 'Đã hủy'
};

export const CausesType = {
  WATER: 1,
  FOOD: 2,
  MEDICINE: 3,
  EDUCATION: 4,
  SHELTER: 5,
  CLOTHING: 6
};

export const CausesTypeLabel = {
  [CausesType.WATER]: 'Nước sạch',
  [CausesType.FOOD]: 'Lương thực',
  [CausesType.MEDICINE]: 'Y tế',
  [CausesType.EDUCATION]: 'Giáo dục',
  [CausesType.SHELTER]: 'Nhà ở',
  [CausesType.CLOTHING]: 'Quần áo'
};

export const CausesTypeOptions = Object.entries(CausesType)
  .map(([key, value]) => ({
    value,
    label: CausesTypeLabel[value]
  }));

export const EventStatus = {
  UPCOMING: 1,
  ONGOING: 2,
  COMPLETED: 3,
  CANCELLED: 4
};

export const EventStatusLabel = {
  [EventStatus.UPCOMING]: 'Sắp diễn ra',
  [EventStatus.ONGOING]: 'Đang diễn ra',
  [EventStatus.COMPLETED]: 'Đã hoàn thành',
  [EventStatus.CANCELLED]: 'Đã hủy'
};

export const MediaType = {
  IMAGE: 1,
  VIDEO: 2,
  DOCUMENT: 3
};

export const MediaTypeLabel = {
  [MediaType.IMAGE]: 'Hình ảnh',
  [MediaType.VIDEO]: 'Video',
  [MediaType.DOCUMENT]: 'Tài liệu'
};

export const Unit = {
  KG: 1,
  PACK: 2,
  BAG: 3,
  BOX: 4,
  BOTTLE: 5,
  CARTON: 6
};

export const UnitLabel = {
  [Unit.KG]: 'kg',
  [Unit.PACK]: 'Gói',
  [Unit.BAG]: 'Túi',
  [Unit.BOX]: 'Hộp',
  [Unit.BOTTLE]: 'Chai',
  [Unit.CARTON]: 'Thùng'
};

export const PaymentMethod = {
  VNPAY: 1,
  MOMO: 2
};

export const PaymentMethodLabel = {
  [PaymentMethod.VNPAY]: 'VNPAY',
  [PaymentMethod.MOMO]: 'MOMO'
};
