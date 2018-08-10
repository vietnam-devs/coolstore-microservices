/* tslint:disable */
import {
  Controller,
  ValidateParam,
  FieldErrors,
  ValidateError,
  TsoaRoute
} from 'tsoa'
import { ProductsController } from './controllers/productsController'

export function RegisterRoutes(app: any, basePath: string) {
  app.get(`${basePath}api/products/:productId`, async (req, res) => {
    console.info(req.params)
    const controller = new ProductsController()
    var product = await controller.Get.apply(controller)
    res.send(product)
  })

  app.get(`${basePath}api/products`, async (req, res) => {
    const controller = new ProductsController()
    var products = await controller.GetAll.apply(controller)
    res.send(products)
  })

  app.post(`${basePath}api/products`, async (req, res) => {
    console.info(req.body)
    const controller = new ProductsController()
    var product = await controller.Create.apply(controller)
    res.send(product)
  })

  app.get(`${basePath}healthz`, (req, res) => {
    res.send({
      status: 'Healthy!'
    })
  })
}
