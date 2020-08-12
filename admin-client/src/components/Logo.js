import React from 'react';
import logo from 'src/logo.svg';

const Logo = (props) => {
  return (
    <img
      alt="Logo"
      src={logo}
      {...props}
    />
  );
};

export default Logo;
