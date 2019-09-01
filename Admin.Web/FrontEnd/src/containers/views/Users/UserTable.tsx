import React from 'react'
import { Table, Divider, Popconfirm } from 'antd'

import { useOnMount } from '@utils/reactExt'
import { connect } from 'react-redux';

import UserActionCreatorService from '@reducers/userReducer/service'

// import UserModal from './UserModal'

// interface IProps {
//     scrollY: number,
//     userReducer: UserStore, 
// }

function UserTable({ scrollY, userReducer, getUsers, handleTableChange, deleteUser }) {

    const [modalVisible, setModalVisible] = React.useState(false)
    const [currentUser, setCurrentUser] = React.useState<IUserStore.IUser>(null)

    function modifyUser(user: IUserStore.IUser) {
        setCurrentUser(user)
        setModalVisible(true)
    }

    useOnMount(getUsers)

    return (
        <React.Fragment>
            <Table<IUserStore.IUser>
                className="center-table"
                style={{ width: '100%' }}
                bordered
                rowKey="_id"
                loading={userReducer.getUsersloading}
                dataSource={userReducer.users}
                scroll={{ y: scrollY }}
                pagination={{
                    current: userReducer.page.index,
                    showSizeChanger: true,
                    pageSize: userReducer.page.size,
                    pageSizeOptions: ['30', '20', '10'],
                    total: userReducer.total
                }}
                onChange={handleTableChange}
            >
                <Table.Column<IUserStore.IUser> key="account" title="Account" dataIndex="account" width={200} />
                <Table.Column<IUserStore.IUser> key="category" title="Category" dataIndex="category" width={100} />
                <Table.Column<IUserStore.IUser> key="createdAt" title="CreatedAt" dataIndex="createdAt" width={200} />
                <Table.Column<IUserStore.IUser>
                    key="action"
                    title="Action"
                    width={120}
                    render={(_, record) => (
                        <span>
                            <a href="javascript:;" onClick={() => modifyUser(record)}>
                                Modify
                            </a>
                            <Divider type="vertical" />
                            <Popconfirm
                                placement="top"
                                title="确认删除?"
                                onConfirm={() => deleteUser(record._id)}
                                okText="Yes"
                                cancelText="No"
                            >
                                <a href="javascript:;">Delete</a>
                            </Popconfirm>
                        </span>
                    )}
                />
            </Table>
            {/* <UserModal visible={modalVisible} onCancel={() => setModalVisible(false)} user={currentUser} /> */}
        </React.Fragment>
    )
}


function mapStateToProps(state) {
    const { userReducer } = state;
    return {
        userReducer,
    };
}

export default connect(mapStateToProps, {
    getUsers: UserActionCreatorService.getUsers,
    handleTableChange: UserActionCreatorService.handleTableChange,
    deleteUser: UserActionCreatorService.deleteUser,
})(UserTable)
