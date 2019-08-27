import http from '@services/http';
export default {
    getUsers(data) {
        return http.get('user', data || {});
    },
    createUser(data) {
        return http.post('user/create', data || {});
    },
    modifyUser(data) {
        return http.post('user/modify', data || {});
    },
    deleteUser(data) {
        return http.post('user/delete', data || {});
    }
};
//# sourceMappingURL=user.js.map