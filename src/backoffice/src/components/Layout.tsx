import * as React from 'react'
import { Link } from 'react-router-dom'
import Typography from '@material-ui/core/Typography'
import { Theme } from '@material-ui/core/styles/createMuiTheme'
import withStyles, { WithStyles } from '@material-ui/core/styles/withStyles'
import Drawer from '@material-ui/core/Drawer'
import AppBar from '@material-ui/core/AppBar'
import Toolbar from '@material-ui/core/Toolbar'
import List from '@material-ui/core/List'
import ListItem from '@material-ui/core/ListItem'
import withRoot from '../withRoot'

const drawerWidth = 240

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

// https://vectr.com/tmp/b2TDDwHxI4/c1cGF9wCaD.svg?width=640&height=640&select=c1cGF9wCaDpage0
class Layout extends React.Component<WithStyles<typeof styles>> {
  render() {
    return (
      <div className={this.props.classes.root}>
        <AppBar position="fixed" className={this.props.classes.appBar}>
          <Toolbar>
            <Typography variant="h6" color="inherit" noWrap>
              CoolStore
            </Typography>
          </Toolbar>
        </AppBar>
        <Drawer
          className={this.props.classes.drawer}
          variant="permanent"
          classes={{
            paper: this.props.classes.drawerPaper
          }}
        >
          <div className={this.props.classes.toolbar} />
          <List>
            <ListItem button key={'home'}>
              <Link to="/">Home</Link>
            </ListItem>
            <ListItem button key={'another'}>
              <Link to="/products">Products</Link>
            </ListItem>
          </List>
        </Drawer>
        <main className={this.props.classes.content}>
          <div className={this.props.classes.toolbar} />
          {this.props.children}
        </main>
      </div>
    )
  }
}

export default withRoot(withStyles(styles)(Layout))
