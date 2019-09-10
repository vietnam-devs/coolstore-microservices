import React from 'react'

import { createStyles, withStyles, WithStyles } from '@material-ui/core/styles'
import Card from '@material-ui/core/Card'
import CardActionArea from '@material-ui/core/CardActionArea'
import CardActions from '@material-ui/core/CardActions'
import CardContent from '@material-ui/core/CardContent'
import CardMedia, { CardMediaProps } from '@material-ui/core/CardMedia'
import Button from '@material-ui/core/Button'
import Typography from '@material-ui/core/Typography'

import withRoot from '../withRoot'

const styles = () =>
  createStyles({
    card: {
      maxWidth: 345
    },
    cardStyle: {
      maxWidth: 345,
      transitionDuration: '0.3s'
      //height: '30vw'
    },
    media: {
      objectFit: 'cover'
    }
  })

interface ProductItemProps extends WithStyles<typeof styles & CardMediaProps> {
  name: string
  description: string
  imageUrl: string
  price: number
}

const ProductItem: React.FC<ProductItemProps> = (props: ProductItemProps) => {
  return (
    <Card className={props.classes.cardStyle}>
      <CardActionArea>
        <CardMedia component="img" className={props.classes.media} image={props.imageUrl} title={props.name} />
        <CardContent>
          <Typography gutterBottom variant="h5" component="h2">
            {props.name}
          </Typography>
          <Typography component="p">{props.description}</Typography>
          <Typography component="p">${props.price}</Typography>
        </CardContent>
      </CardActionArea>
      <CardActions>
        <Button size="small" variant="outlined" color="secondary">
          Edit
        </Button>
        <Button size="small" variant="outlined" color="default">
          Delete
        </Button>
      </CardActions>
    </Card>
  )
}

export default withRoot(withStyles(styles)(ProductItem))
