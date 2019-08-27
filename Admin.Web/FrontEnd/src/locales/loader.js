import * as tslib_1 from "tslib";
export var LOCALES_KEYS;
(function (LOCALES_KEYS) {
    LOCALES_KEYS["EN_US"] = "en-US";
    LOCALES_KEYS["ZH_CN"] = "zh-CN";
})(LOCALES_KEYS || (LOCALES_KEYS = {}));
export const SUPPOER_LOCALES = [
    {
        name: 'English',
        value: LOCALES_KEYS.EN_US
    },
    {
        name: '简体中文',
        value: LOCALES_KEYS.ZH_CN
    }
];
export function getLocaleLoader(locale) {
    switch (locale) {
        case LOCALES_KEYS.ZH_CN:
            return new Promise((resolve) => tslib_1.__awaiter(this, void 0, void 0, function* () {
                const loc = yield import(/* webpackChunkName: "zh-CN" */ './zh_CN.json').then(m => m.default);
                const antdLoc = yield import(
                /* webpackChunkName: "antd-zh-CN" */ 'antd/lib/locale-provider/zh_CN').then(m => m.default);
                resolve({ localeData: loc, antdLocaleData: antdLoc });
            }));
        default:
            return new Promise((resolve) => tslib_1.__awaiter(this, void 0, void 0, function* () {
                const loc = yield import(/* webpackChunkName: "en-US" */ './en_US.json').then(m => m.default);
                const antdLoc = yield import(
                /* webpackChunkName: "antd-en-US" */ 'antd/lib/locale-provider/en_US').then(m => m.default);
                resolve({ localeData: loc, antdLocaleData: antdLoc });
            }));
    }
}
//# sourceMappingURL=loader.js.map