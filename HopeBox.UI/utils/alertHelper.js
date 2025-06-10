import Swal from 'sweetalert2';

export const showSuccessAlert = async (title = 'Thành công', message = '') => {
    return await Swal.fire({
        icon: 'success',
        title: title,
        text: message,
        confirmButtonText: 'OK',
        confirmButtonColor: '#f37224',
        background: '#0c1e25',
        color: '#fff'
    });
};

export const showErrorAlert = async (title = 'Lỗi', message = '') => {
    return await Swal.fire({
        icon: 'error',
        title: title,
        text: message,
        confirmButtonText: 'Đóng',
        confirmButtonColor: '#d33',
        background: '#0c1e25',
        color: '#fff'
    });
};

export const showWarningAlert = async (title = 'Cảnh báo', message = '') => {
    return await Swal.fire({
        icon: 'warning',
        title: title,
        text: message,
        confirmButtonText: 'Hiểu rồi',
        confirmButtonColor: '#f0ad4e',
        background: '#0c1e25',
        color: '#fff'
    });
};

export const showConfirmDialog = async (
    title = 'Bạn chắc chứ?',
    message = '',
    confirmText = 'Xác nhận',
    cancelText = 'Hủy'
) => {
    return await Swal.fire({
        icon: 'question',
        title: title,
        text: message,
        showCancelButton: true,
        confirmButtonText: confirmText,
        cancelButtonText: cancelText,
        confirmButtonColor: '#f37224',
        cancelButtonColor: '#6c757d',
        background: '#0c1e25',
        color: '#fff'
    });
};
