import * as tslib_1 from "tslib";
import React from 'react';
import { observer } from 'mobx-react';
import { Form, Icon, Input, Button } from 'antd';
import intl from 'react-intl-universal';
import styles from './index.scss';
import useRootStore from '@store/useRootStore';
const FormItem = Form.Item;
function Login({ form }) {
    const { authStore } = useRootStore();
    const [loading, setLoading] = React.useState(false);
    const submit = (e) => {
        e.preventDefault();
        form.validateFields((err, values) => tslib_1.__awaiter(this, void 0, void 0, function* () {
            if (!err) {
                setLoading(true);
                try {
                    yield authStore.login(values);
                }
                finally {
                    setLoading(false);
                }
            }
        }));
    };
    const { getFieldDecorator } = form;
    return (React.createElement("div", { className: styles.login },
        React.createElement(Form, { onSubmit: submit, className: styles.form },
            React.createElement("div", { className: styles.logoBox },
                React.createElement(Icon, { type: "ant-design" })),
            React.createElement(FormItem, { hasFeedback: true }, getFieldDecorator('account', {
                rules: [{ required: true }]
            })(React.createElement(Input, { prefix: React.createElement(Icon, { type: "user", style: { color: 'rgba(0,0,0,.25)' } }), placeholder: "account" }))),
            React.createElement(FormItem, { hasFeedback: true }, getFieldDecorator('password', {
                rules: [{ required: true }]
            })(React.createElement(Input, { prefix: React.createElement(Icon, { type: "lock", style: { color: 'rgba(0,0,0,.25)' } }), type: "password", placeholder: "password" }))),
            React.createElement(FormItem, null,
                React.createElement("div", { className: styles.tips },
                    React.createElement("span", null,
                        intl.get('USERNAME'),
                        ": admin"),
                    React.createElement("span", null,
                        intl.get('PASSWORD'),
                        ": admin")),
                React.createElement(Button, { type: "primary", htmlType: "submit", block: true, loading: loading }, intl.get('LOGIN'))))));
}
export default Form.create()(observer(Login));
//# sourceMappingURL=index.js.map