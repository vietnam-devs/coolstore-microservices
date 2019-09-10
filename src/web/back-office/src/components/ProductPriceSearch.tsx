import React from 'react'

import { createStyles, withStyles, WithStyles } from '@material-ui/core/styles'
import Paper from '@material-ui/core/Paper'
import InputBase from '@material-ui/core/InputBase'
import SearchIcon from '@material-ui/icons/Search'
import IconButton from '@material-ui/core/IconButton'

const styles = () =>
  createStyles({
    root: {
      padding: '2px 4px',
      display: 'flex',
      alignItems: 'center',
      width: 400
    },
    input: {
      marginLeft: 8,
      flex: 1
    },
    iconButton: {
      padding: 10
    },
    divider: {
      width: 1,
      height: 28,
      margin: 4
    }
  })

interface ProductPriceSearchProps extends WithStyles<typeof styles> {
  onPriceChange: (event: React.ChangeEvent<HTMLInputElement>) => void
  price: number
}

const ProductPriceSearch: React.FC<ProductPriceSearchProps> = (props: ProductPriceSearchProps) => {
  const { classes } = props
  return (
    <div>
      <Paper className={classes.root} elevation={1}>
        <InputBase
          value={props.price}
          onChange={props.onPriceChange}
          className={classes.input}
          placeholder="Search product price"
        />
        <IconButton className={classes.iconButton} aria-label="Search">
          <SearchIcon />
        </IconButton>
      </Paper>
    </div>
  )
}

export default withStyles(styles)(ProductPriceSearch)
