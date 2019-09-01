import React from 'react'
import { Modal, Form, Input, Select } from 'antd'
import { FormComponentProps } from 'antd/lib/form'

import { connect } from 'react-redux';
import UserActionCreatorService from '@reducers/userReducer/service'

const FormItem = Form.Item

const formItemLayout = {
    labelCol: {
        xs: { span: 24 },
        sm: { span: 5 }
    },
    wrapperCol: {
        xs: { span: 24 },
        sm: { span: 19 }
    }
}

const userCategory = ['user', 'admin']

interface IProps extends FormComponentProps {
    visible: boolean
    onCancel: () => void
    user?: IUserStore.IUser
}

function UserModal({ visible, onCancel, user, form, getUsers, modifyUser, createUser, handleTableChange }) {

    const [loading, setLoading] = React.useState(false)

    const typeIsAdd = user === undefined

    function toggleLoading() {
        setLoading(l => !l)
    }

    function submit(e?: React.FormEvent<any>) {
        if (e) {
            e.preventDefault()
        }
        form.validateFields(
            async (err, values): Promise<any> => {
                if (!err) {
                    toggleLoading()
                    try {
                        if (typeIsAdd) {
                            await createUser(values)
                            handleTableChange({ current : 1})
                        } else {
                            await modifyUser({ ...values, _id: user._id })
                            getUsers()
                        }
                        onCancel()
                    } catch (err) {}
                    toggleLoading()
                }
            }
        )
    }

    const { getFieldDecorator } = form
    return (
        <Modal
            title={typeIsAdd ? 'Add User' : 'Modify User'}
            visible={visible}
            onOk={submit}
            onCancel={onCancel}
            okButtonProps={{ loading }}
        >
            <Form onSubmit={submit}>
                <FormItem {...formItemLayout} label="account">
                    {getFieldDecorator('account', {
                        initialValue: user ? user.account : '',
                        rules: [{ required: true }]
                    })(<Input />)}
                </FormItem>
                {typeIsAdd && (
                    <FormItem {...formItemLayout} label="password">
                        {getFieldDecorator('password', {
                            rules: [{ required: true }]
                        })(<Input />)}
                    </FormItem>
                )}
                <FormItem {...formItemLayout} label="category">
                    {getFieldDecorator('category', {
                        initialValue: user ? user.category : userCategory[0],
                        rules: [{ required: true }]
                    })(
                        <Select>
                            {userCategory.map(c => (
                                <Select.Option key={c} value={c}>
                                    {c}
                                </Select.Option>
                            ))}
                        </Select>
                    )}
                </FormItem>
            </Form>
        </Modal>
    )
}



function mapStateToProps(state) {
    const { userReducer } = state;
    return {
        userReducer,
    };
}

const connectedUserModal =  connect(mapStateToProps, {
    getUsers: UserActionCreatorService.getUsers,
    createUser: UserActionCreatorService.createUser,
    modifyUser: UserActionCreatorService.modifyUser,
    handleTableChange: UserActionCreatorService.handleTableChange,
})(UserModal)


export default Form.create<IProps>()(connectedUserModal)
