import React from 'react'
import PropTypes from 'prop-types'
import Link from 'gatsby-link'

const buildLinks = links => {
  return links.map(link => (
    <li key={link.url}>
      <Link to={link.url}>{link.title}</Link>
    </li>
  ))
}

const Header = ({ siteTitle, links }) => (
  <nav className="header">
    <ul>{buildLinks(links)}</ul>
  </nav>
)

Header.propTypes = {
  siteTitle: PropTypes.string,
  links: PropTypes.array
}

export default Header
