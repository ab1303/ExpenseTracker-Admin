import React from 'react';
import { Button } from 'antd';
import styles from './index.scss';
import UserModal from './../UserModal';
function Header() {
    const [modalVisible, setModalVisible] = React.useState(false);
    function toggleModalVisible() {
        setModalVisible(visible => !visible);
    }
    return (React.createElement("div", { className: styles.header },
        React.createElement(Button, { type: "primary", onClick: toggleModalVisible }, "add user"),
        React.createElement(UserModal, { visible: modalVisible, onCancel: toggleModalVisible })));
}
export default Header;
//# sourceMappingURL=index.js.map