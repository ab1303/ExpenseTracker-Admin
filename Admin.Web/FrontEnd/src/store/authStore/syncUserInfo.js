import { LOCALSTORAGE_KEYS } from '@constants/index';
export const initialUserInfo = (() => {
    const localUserInfo = localStorage.getItem(LOCALSTORAGE_KEYS.USERINFO);
    const _userInfo = localUserInfo ? JSON.parse(localUserInfo) : {};
    return _userInfo;
})();
export let userInfo = initialUserInfo;
/**
 * syncUserInfo for http
 *
 * @export
 * @param {IAuthStore.UserInfo} data
 */
export function syncUserInfo(data) {
    userInfo = data;
}
//# sourceMappingURL=syncUserInfo.js.map