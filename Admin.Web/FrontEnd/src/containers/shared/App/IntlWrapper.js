import React from 'react';
import intl from 'react-intl-universal';
import { find } from 'lodash';
import { Select, LocaleProvider } from 'antd';
import styles from './index.scss';
import { useOnMount } from '@utils/reactExt';
import { setCookie } from '@utils/index';
import { COOKIE_KEYS } from '@constants/index';
import PageLoading from '@components/PageLoading';
import { SUPPOER_LOCALES, LOCALES_KEYS, getLocaleLoader } from '@locales/loader';
export default function IntlWrapper({ children }) {
    const [currentLocale, setCurrentLocale] = React.useState('');
    const [antdLocaleData, setAntdLocaleData] = React.useState(null);
    function loadLocales() {
        let targetLocale = intl.determineLocale({ cookieLocaleKey: COOKIE_KEYS.LANG });
        // default is English
        if (!find(SUPPOER_LOCALES, { value: targetLocale })) {
            targetLocale = LOCALES_KEYS.EN_US;
        }
        getLocaleLoader(targetLocale).then(res => {
            intl.init({ currentLocale: targetLocale, locales: { [targetLocale]: res.localeData } }).then(() => {
                setCurrentLocale(targetLocale);
                setAntdLocaleData(res.antdLocaleData);
            });
        });
    }
    function onSelectLocale(val) {
        setCookie(COOKIE_KEYS.LANG, val);
        location.reload();
    }
    useOnMount(loadLocales);
    if (!currentLocale) {
        return React.createElement(PageLoading, null);
    }
    const selectLanguage = (React.createElement(Select, { className: styles.intlSelect, onChange: onSelectLocale, value: currentLocale }, SUPPOER_LOCALES.map(l => (React.createElement(Select.Option, { key: l.value, value: l.value }, l.name)))));
    return (React.createElement(LocaleProvider, { locale: antdLocaleData },
        React.createElement(React.Fragment, null,
            selectLanguage,
            children)));
}
//# sourceMappingURL=IntlWrapper.js.map