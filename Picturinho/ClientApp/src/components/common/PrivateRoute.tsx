import React from 'react';
import { Redirect, Route } from 'react-router-dom';

export const PrivateRoute: React.FC<any> = ({
  Component,
  children,
  ...rest
}) => {
  const user: string | null = localStorage.getItem("user");

  return (
    <Route
      {...rest}
      render={(props) =>
        user ? (
          <Component {...props} />
        ) : (
          <Redirect
            to={{ pathname: "/login", state: { from: props.location } }}
          />
        )
      }
    />
  );
};
