import React, { useState, useEffect, memo } from 'react'
import { User } from 'oidc-client'
import {
  Jumbotron,
  Container,
  Navbar,
  Nav,
  NavItem,
  NavLink,
  UncontrolledDropdown,
  DropdownToggle,
  DropdownMenu,
  DropdownItem,
  Collapse
} from 'reactstrap'
import styled from 'styled-components'

import { AuthService } from 'services'

import { AppActions, useStore } from 'stores/store'
import { IAppUser } from 'stores/types'

const StyledHeader = styled.a`
  text-decoration: none;
  &:hover {
    text-decoration: none;
  }
`

const StyledNavBar = styled(Navbar)`
  margin-top: -3rem;
`

const StyledDropdown = styled(UncontrolledDropdown)`
  display: inherit;
`

const Header = () => {
  const [user, setUser] = useState<IAppUser>(null)
  const { dispatch } = useStore()

  useEffect(() => {
    if (user == null) {
      AuthService.getUser().then((user: User) => {
        let currentUser = { userName: user.profile.name, email: user.profile.email, accessToken: user.access_token }
        setUser(currentUser)
        dispatch(AppActions.loadUserLogin(currentUser))
      })
    }
  }, [user])

  return (
    <>
      <Jumbotron fluid>
        <Container fluid>
          <div>
            <h1 className="display-4">
              <StyledHeader href="/">CoolStore Microservices</StyledHeader>
            </h1>
            <p className="lead">
              A containerised microservices application consisting of services based on .NET Core, NodeJS and more
              running on Service Mesh. It demonstrates how to wire up small microservices into a larger application
              using microservice architectural principals
            </p>
          </div>
        </Container>
      </Jumbotron>

      <div>
        <StyledNavBar color="light" light expand="md">
          <span className="lead">
            Search Product&nbsp;<input type="textbox"></input>
          </span>
          <Collapse navbar>
            <Nav className="ml-auto">
              <StyledDropdown nav inNavbar>
                <NavItem>
                  <NavLink href="/cart">Cart</NavLink>
                </NavItem>
                <DropdownToggle nav caret>
                  {user != null ? user.userName : ''}
                </DropdownToggle>
                <DropdownMenu right>
                  <DropdownItem>
                    <a
                      href="#"
                      onClick={() => {
                        AuthService.signOut()
                      }}
                    >
                      Logout
                    </a>
                  </DropdownItem>
                </DropdownMenu>
              </StyledDropdown>
            </Nav>
          </Collapse>
        </StyledNavBar>
      </div>
    </>
  )
}

export default memo(Header)
