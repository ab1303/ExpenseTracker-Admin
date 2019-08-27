import * as tslib_1 from "tslib";
import React from 'react';
import { observer } from 'mobx-react';
import { Modal, Form, Input, Select } from 'antd';
import useRootStore from '@store/useRootStore';
const FormItem = Form.Item;
const formItemLayout = {
    labelCol: {
        xs: { span: 24 },
        sm: { span: 5 }
    },
    wrapperCol: {
        xs: { span: 24 },
        sm: { span: 19 }
    }
};
const userCategory = ['user', 'admin'];
function UserModal({ visible, onCancel, user, form }) {
    const { userStore } = useRootStore();
    const [loading, setLoading] = React.useState(false);
    const typeIsAdd = user === undefined;
    function toggleLoading() {
        setLoading(l => !l);
    }
    function submit(e) {
        if (e) {
            e.preventDefault();
        }
        form.validateFields((err, values) => tslib_1.__awaiter(this, void 0, void 0, function* () {
            if (!err) {
                toggleLoading();
                try {
                    if (typeIsAdd) {
                        yield userStore.createUser(values);
                        userStore.changePageIndex(1);
                    }
                    else {
                        yield userStore.modifyUser(Object.assign({}, values, { _id: user._id }));
                        userStore.getUsers();
                    }
                    onCancel();
                }
                catch (err) { }
                toggleLoading();
            }
        }));
    }
    const { getFieldDecorator } = form;
    return (React.createElement(Modal, { title: typeIsAdd ? 'Add User' : 'Modify User', visible: visible, onOk: submit, onCancel: onCancel, okButtonProps: { loading } },
        React.createElement(Form, { onSubmit: submit },
            React.createElement(FormItem, Object.assign({}, formItemLayout, { label: "account" }), getFieldDecorator('account', {
                initialValue: user ? user.account : '',
                rules: [{ required: true }]
            })(React.createElement(Input, null))),
            typeIsAdd && (React.createElement(FormItem, Object.assign({}, formItemLayout, { label: "password" }), getFieldDecorator('password', {
                rules: [{ required: true }]
            })(React.createElement(Input, null)))),
            React.createElement(FormItem, Object.assign({}, formItemLayout, { label: "category" }), getFieldDecorator('category', {
                initialValue: user ? user.category : userCategory[0],
                rules: [{ required: true }]
            })(React.createElement(Select, null, userCategory.map(c => (React.createElement(Select.Option, { key: c, value: c }, c)))))))));
}
export default Form.create()(observer(UserModal));
//# sourceMappingURL=UserModal.js.map