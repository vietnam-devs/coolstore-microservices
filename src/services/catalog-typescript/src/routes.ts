/* tslint:disable */
import * as express from 'express'
import ProductController from './controllers/productController'

export default express
  .Router()
  .get('/', async (_: express.Request, res: express.Response) => {
    var products = await ProductController.GetAll()
    res.send(products)
  })
  .get('/:productId', async (req: express.Request, res: express.Response) => {
    var product = await ProductController.Get(req.params.productId)
    res.send(product)
  })
  .post('/', async (req: express.Request, res: express.Response) => {
    try {
      var product = await ProductController.Create(req.body)
      res.send(product)
    } catch (error) {
      res.status(400).send(error)
    }
  })
