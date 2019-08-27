import http from '@services/http';
export default {
    login(data) {
        return http.post('auth/login', data || {});
    }
};
//# sourceMappingURL=auth.js.map