import React, { memo } from 'react'
import { Container } from 'reactstrap'
import styled from 'styled-components'

const StyledFooter = styled.footer`
  margin: 3rem 0 1rem;
`

const Footer = () => {
  return (
    <StyledFooter className="footer text-center">
      <Container fluid className="clearfix">
        <span className="text-muted d-block text-center text-sm-left d-sm-inline-block">
          Copyright © 2019 CoolStore Application. All rights reserved.
        </span>
      </Container>
    </StyledFooter>
  )
}

export default memo(Footer)
