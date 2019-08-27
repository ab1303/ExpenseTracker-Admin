import React from 'react';
import { Table, Divider, Popconfirm } from 'antd';
import { observer } from 'mobx-react';
import { useOnMount } from '@utils/reactExt';
import useRootStore from '@store/useRootStore';
import UserModal from './UserModal';
function UserTable({ scrollY }) {
    const { userStore } = useRootStore();
    const [modalVisible, setModalVisible] = React.useState(false);
    const [currentUser, setCurrentUser] = React.useState(null);
    function modifyUser(user) {
        setCurrentUser(user);
        setModalVisible(true);
    }
    useOnMount(userStore.getUsers);
    return (React.createElement(React.Fragment, null,
        React.createElement(Table, { className: "center-table", style: { width: '100%' }, bordered: true, rowKey: "_id", loading: userStore.getUsersloading, dataSource: userStore.users, scroll: { y: scrollY }, pagination: {
                current: userStore.pageIndex,
                showSizeChanger: true,
                pageSize: userStore.pageSize,
                pageSizeOptions: ['30', '20', '10'],
                total: userStore.total
            }, onChange: userStore.handleTableChange },
            React.createElement(Table.Column, { key: "account", title: "Account", dataIndex: "account", width: 200 }),
            React.createElement(Table.Column, { key: "category", title: "Category", dataIndex: "category", width: 100 }),
            React.createElement(Table.Column, { key: "createdAt", title: "CreatedAt", dataIndex: "createdAt", width: 200 }),
            React.createElement(Table.Column, { key: "action", title: "Action", width: 120, render: (_, record) => (React.createElement("span", null,
                    React.createElement("a", { href: "javascript:;", onClick: () => modifyUser(record) }, "Modify"),
                    React.createElement(Divider, { type: "vertical" }),
                    React.createElement(Popconfirm, { placement: "top", title: "\u786E\u8BA4\u5220\u9664?", onConfirm: () => userStore.deleteUser(record._id), okText: "Yes", cancelText: "No" },
                        React.createElement("a", { href: "javascript:;" }, "Delete")))) })),
        React.createElement(UserModal, { visible: modalVisible, onCancel: () => setModalVisible(false), user: currentUser })));
}
export default observer(UserTable);
//# sourceMappingURL=UserTable.js.map