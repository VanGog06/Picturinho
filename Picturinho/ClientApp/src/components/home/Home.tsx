import React from 'react';
import { useSelector } from 'react-redux';

export const Home: React.FC = (): JSX.Element => {
  const loggedUser = useSelector((state: any) => state.authentication.user);

  return (
    <div className="col-lg-8 offset-lg-2">
      <h1>Hi, {loggedUser.firstName}!</h1>
      <h2>Welcome to Picturinho</h2>
    </div>
  );
};
