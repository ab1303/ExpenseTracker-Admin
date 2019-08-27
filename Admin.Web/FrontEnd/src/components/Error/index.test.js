import React from 'react';
import enzyme from 'enzyme';
import styles from './index.scss';
import Error from './';
it('renders the correct text', () => {
    const component = enzyme.shallow(React.createElement(Error, null));
    const avatars = component.find(`.${styles.title}`);
    expect(avatars.text()).toEqual('Ooooops!');
});
//# sourceMappingURL=index.test.js.map