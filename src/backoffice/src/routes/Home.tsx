import * as React from 'react'
import Button from '@material-ui/core/Button'
import Dialog from '@material-ui/core/Dialog'
import DialogActions from '@material-ui/core/DialogActions'
import DialogContent from '@material-ui/core/DialogContent'
import DialogContentText from '@material-ui/core/DialogContentText'
import DialogTitle from '@material-ui/core/DialogTitle'
import Typography from '@material-ui/core/Typography'
import { Theme } from '@material-ui/core/styles/createMuiTheme'
import createStyles from '@material-ui/core/styles/createStyles'
import withStyles, { WithStyles } from '@material-ui/core/styles/withStyles'
import withRoot from '../withRoot'
import { AuthService } from '../services/AuthService'

const styles = (theme: Theme) =>
  createStyles({
    root: {
      textAlign: 'center',
      paddingTop: theme.spacing.unit * 20
    }
  })

type State = {
  open: boolean
}

class Home extends React.Component<WithStyles<typeof styles>, State> {
  public authService: AuthService
  constructor(props: WithStyles<typeof styles>) {
    super(props)
    this.authService = new AuthService()
  }

  state = {
    open: false
  }

  handleClose = () => {
    this.setState({
      open: false
    })
  }

  handleClick = () => {
    this.setState({
      open: true
    })
  }

  public loginClick = async () => {
    await this.authService.login()
  }

  public logoutClick = async () => {
    await this.authService.logout()
  }

  render() {
    return (
      <div className={this.props.classes.root}>
        <Dialog open={this.state.open} onClose={this.handleClose}>
          <DialogTitle>Super Secret Password</DialogTitle>
          <DialogContent>
            <DialogContentText>1-2-3-4-5</DialogContentText>
          </DialogContent>
          <DialogActions>
            <Button color="primary" onClick={this.handleClose}>
              OK
            </Button>
          </DialogActions>
        </Dialog>
        <Typography variant="h4" gutterBottom>
          Material-UI
        </Typography>
        <Typography variant="subtitle1" gutterBottom>
          example project
        </Typography>
        <Button variant="contained" color="secondary" onClick={this.handleClick}>
          Super Secret Password
        </Button>
        <Button variant="contained" color="primary" onClick={this.loginClick}>
          Login
        </Button>
        <Button variant="contained" color="default" onClick={this.logoutClick}>
          Logout
        </Button>
      </div>
    )
  }
}

export default withRoot(withStyles(styles)(Home))
