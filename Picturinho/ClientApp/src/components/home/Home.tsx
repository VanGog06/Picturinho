import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Link } from 'react-router-dom';

import { UserModel } from '../../models/user/UserModel';
import { deleteUser, getAll } from '../../store/user/UserActions';

export const Home: React.FC = () => {
  const user = useSelector((state: any) => state.user);
  const loggerUser = useSelector((state: any) => state.authentication.user);
  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(getAll());
  }, [dispatch]);

  function handleDeleteUser(
    event: React.MouseEvent<HTMLAnchorElement, MouseEvent>,
    id: number
  ): void {
    event.preventDefault();
    dispatch(deleteUser(id));
  }

  return (
    <div className="col-lg-8 offset-lg-2">
      <h1>Hi {loggerUser.firstName}!</h1>
      <p>You're logged in with React Hooks!!</p>
      <h3>All registered users:</h3>
      {user && user.loading && <em>Loading users...</em>}
      {user && user.error && (
        <span className="text-danger">ERROR: {user.error}</span>
      )}
      {user && user.items && (
        <ul>
          {user.items.map((user: UserModel) => (
            <li key={user.id}>
              {user.firstName + " " + user.lastName}
              {user.deleting ? (
                <em> - Deleting...</em>
              ) : (
                <span>
                  {" "}
                  -{" "}
                  <a
                    onClick={(
                      event: React.MouseEvent<HTMLAnchorElement, MouseEvent>
                    ) => handleDeleteUser(event, user.id)}
                    className="text-primary"
                    href=""
                  >
                    Delete
                  </a>
                </span>
              )}
            </li>
          ))}
        </ul>
      )}
      <p>
        <Link to="login">Logout</Link>
      </p>
    </div>
  );
};
