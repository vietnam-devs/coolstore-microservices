import React, { useState } from 'react'
import Button from '@material-ui/core/Button'
import Dialog from '@material-ui/core/Dialog'
import DialogActions from '@material-ui/core/DialogActions'
import DialogContent from '@material-ui/core/DialogContent'
import DialogContentText from '@material-ui/core/DialogContentText'
import DialogTitle from '@material-ui/core/DialogTitle'
import Typography from '@material-ui/core/Typography'
import { Theme } from '@material-ui/core/styles/createMuiTheme'
import createStyles from '@material-ui/core/styles/createStyles'
import withStyles from '@material-ui/core/styles/withStyles'
import withRoot from '../withRoot'
import { AuthService } from '../services/AuthService'

const authService = new AuthService()

const home = (props: any) => {
  const [open, setOpen] = useState(false)
  return (
    <div className={props.classes.root}>
      <Dialog open={open} onClose={() => setOpen(false)}>
        <DialogTitle>Super Secret Password</DialogTitle>
        <DialogContent>
          <DialogContentText>1-2-3-4-5</DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button color="primary" onClick={() => setOpen(false)}>
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
      <Button variant="contained" color="secondary" onClick={() => setOpen(true)}>
        Super Secret Password
      </Button>
      <Button variant="contained" color="primary" onClick={() => authService.login()}>
        Login
      </Button>
      <Button variant="contained" color="default" onClick={() => authService.logout()}>
        Logout
      </Button>
    </div>
  )
}

const styles = (theme: Theme) =>
  createStyles({
    root: {
      textAlign: 'center',
      paddingTop: theme.spacing.unit * 20
    }
  })

export default withRoot(withStyles(styles)(home))
