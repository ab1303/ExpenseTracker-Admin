import axios from 'axios';
import qs from 'qs';
import { message } from 'antd';
import { userInfo } from '@store/authStore/syncUserInfo';
var HTTPERROR;
(function (HTTPERROR) {
    HTTPERROR[HTTPERROR["LOGICERROR"] = 0] = "LOGICERROR";
    HTTPERROR[HTTPERROR["TIMEOUTERROR"] = 1] = "TIMEOUTERROR";
    HTTPERROR[HTTPERROR["NETWORKERROR"] = 2] = "NETWORKERROR";
})(HTTPERROR || (HTTPERROR = {}));
const TOKENERROR = [401, 402, 403];
const DEFAULTCONFIG = {
    baseURL: process.env.BASEURL
};
const http = {};
const methods = ['get', 'post', 'put', 'delete'];
let authTimer = null;
const isSuccess = res => res.errCode === 0;
const resFormat = res => res.response || res.data || {};
methods.forEach(v => {
    http[v] = (url, data, baseUrl) => {
        const axiosConfig = {
            method: v,
            url,
            baseURL: baseUrl || DEFAULTCONFIG.baseURL,
            headers: { Authorization: `Bearer ${userInfo.token}` }
        };
        const instance = axios.create(DEFAULTCONFIG);
        // Add a request interceptor
        instance.interceptors.request.use(cfg => {
            cfg.params = Object.assign({}, cfg.params, { ts: Date.now() / 1000 });
            return cfg;
        }, error => Promise.reject(error));
        // Add a response interceptor
        instance.interceptors.response.use(response => {
            const rdata = typeof response.data === 'object' && !isNaN(response.data.length) ? response.data[0] : response.data;
            if (!isSuccess(rdata)) {
                return Promise.reject({
                    msg: rdata.msg,
                    errCode: rdata.errCode,
                    type: HTTPERROR[HTTPERROR.LOGICERROR],
                    config: response.config
                });
            }
            return resFormat(rdata);
        }, error => {
            if (TOKENERROR.includes(error.response.status)) {
                message.destroy();
                message.error('Authentication failure, Please relogin!');
                clearTimeout(authTimer);
                authTimer = setTimeout(() => {
                    location.replace('/#/login');
                }, 300);
                return;
            }
            return Promise.reject({
                msg: error.response.statusText || error.message || 'network error',
                type: /^timeout of/.test(error.message)
                    ? HTTPERROR[HTTPERROR.TIMEOUTERROR]
                    : HTTPERROR[HTTPERROR.NETWORKERROR],
                config: error.config
            });
        });
        if (v === 'get') {
            axiosConfig.params = data;
        }
        else if (data instanceof FormData) {
            axiosConfig.data = data;
        }
        else {
            axiosConfig.data = qs.stringify(data);
        }
        axiosConfig.startTime = new Date();
        return instance
            .request(axiosConfig)
            .then(res => res)
            .catch(err => {
            message.destroy();
            message.error(err.response || err.msg || err.stack || 'unknown error');
            return Promise.reject(axiosConfig.url.includes('autoScript.set') ? { err } : { err, stack: err.msg || err.stack || '' });
        });
    };
});
export default http;
//# sourceMappingURL=http.js.map