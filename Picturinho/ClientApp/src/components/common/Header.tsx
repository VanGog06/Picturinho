import React from 'react';
import { Nav, Navbar, NavDropdown } from 'react-bootstrap';
import { useSelector } from 'react-redux';
import { Link } from 'react-router-dom';

import { ApplicationRole } from '../../models/enums/ApplicationRole';
import { UserModel } from '../../models/user/UserModel';

export const Header: React.FC = (): JSX.Element => {
  const user: UserModel | undefined = useSelector(
    (state: any) => state.authentication?.user
  );

  return (
    <Navbar bg="dark" variant="dark" expand="lg">
      <Navbar.Brand as={Link} to="/">
        Picturinho
      </Navbar.Brand>
      <Navbar.Toggle aria-controls="basic-navbar-nav" />
      <Navbar.Collapse id="basic-navbar-nav">
        <Nav className="mr-auto">
          <Nav.Link as={Link} to="/home">
            Home
          </Nav.Link>
          {user && user.username ? (
            <>
              <NavDropdown title="Albums" id="albums">
                <NavDropdown.Item as={Link} to="/albums">
                  View
                </NavDropdown.Item>
                <NavDropdown.Item as={Link} to="/albums/create">
                  Create
                </NavDropdown.Item>
              </NavDropdown>
              <Nav.Link as={Link} to="/explore">
                Explore
              </Nav.Link>
              {user.role === ApplicationRole.Administrator ? (
                <NavDropdown title="Admin" id="admin">
                  <NavDropdown.Item as={Link} to="/admin/users">
                    Users
                  </NavDropdown.Item>
                </NavDropdown>
              ) : undefined}
            </>
          ) : undefined}
        </Nav>
        <Nav>
          {user && user.username ? (
            <>
              <Nav.Link as={Link} to="/">
                {user.username}
              </Nav.Link>
              <Nav.Link as={Link} to="/login">
                Logout
              </Nav.Link>
            </>
          ) : (
            <>
              <Nav.Link as={Link} to="/login">
                Login
              </Nav.Link>
              <Nav.Link as={Link} to="/register">
                Register
              </Nav.Link>
            </>
          )}
        </Nav>
      </Navbar.Collapse>
    </Navbar>
  );
};
