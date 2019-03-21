import * as React from 'react'
import { ReactNode } from 'react'
import { Link } from 'react-router-dom'

import Typography from '@material-ui/core/Typography'
import { Theme } from '@material-ui/core/styles/createMuiTheme'
import withStyles, { WithStyles } from '@material-ui/core/styles/withStyles'
import Drawer from '@material-ui/core/Drawer'
import AppBar from '@material-ui/core/AppBar'
import Toolbar from '@material-ui/core/Toolbar'
import List from '@material-ui/core/List'
import ListItem from '@material-ui/core/ListItem'
import ListItemIcon from '@material-ui/core/ListItemIcon'
import ListItemText from '@material-ui/core/ListItemText'

import withRoot from '../withRoot'

const drawerWidth = 180

const styles = (theme: Theme) => ({
  root: {
    display: 'flex'
  },
  appBar: {
    zIndex: theme.zIndex.drawer + 1
  },
  drawer: {
    width: drawerWidth,
    flexShrink: 0
  },
  drawerPaper: {
    width: drawerWidth
  },
  content: {
    flexGrow: 1,
    padding: theme.spacing.unit * 3
  },
  toolbar: theme.mixins.toolbar
})

interface LayoutProps extends WithStyles<typeof styles> {
  children: ReactNode
}

// https://vectr.com/tmp/b2TDDwHxI4/c1cGF9wCaD.svg?width=640&height=640&select=c1cGF9wCaDpage0
const Layout: React.FC<LayoutProps> = (props: LayoutProps) => {
  return (
    <div className={props.classes.root}>
      <AppBar position="fixed" className={props.classes.appBar}>
        <Toolbar>
          <Typography variant="h6" color="inherit" noWrap>
            <b>CoolStore Backoffice</b>
          </Typography>
        </Toolbar>
      </AppBar>
      <Drawer
        className={props.classes.drawer}
        variant="permanent"
        classes={{
          paper: props.classes.drawerPaper
        }}
      >
        <div className={props.classes.toolbar} />
        <List>
          <ListItemLink to="/" primary="Home">
            Home
          </ListItemLink>
          <ListItemLink to="/products" secondary="Products">
            Products
          </ListItemLink>
        </List>
      </Drawer>
      <main className={props.classes.content}>
        <div className={props.classes.toolbar} />
        {props.children}
      </main>
    </div>
  )
}

const ListItemLink = (props: any) => {
  const renderLink = (itemProps: any) => <Link to={props.to} {...itemProps} />
  const { icon, primary, secondary } = props

  return (
    <li>
      <ListItem button component={renderLink}>
        <ListItemText primary={primary} secondary={secondary} />
      </ListItem>
    </li>
  )
}

export default withRoot(withStyles(styles)(Layout))
