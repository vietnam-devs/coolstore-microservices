/* tslint:disable */
import { ProductsController } from './controllers/productsController'
import { ValidateParam } from 'tsoa';
import { ProductCreateRequest } from './models/product';


export function RegisterRoutes(app: any, basePath: string) {
  console.log(`register get all ${basePath}api/products/:productId`)
  app.get(`${basePath}api/products/:productId`, async (req, res) => {
    console.info(req.params)
    const controller = new ProductsController()
    var product = await controller.Get.apply(controller,[req.params.productId])
    res.send(product)
  })

  app.get(`${basePath}api/products`, async (req, res) => {
    const controller = new ProductsController()
    var products = await controller.GetAll.apply(controller)
    res.send(products)
  })

  app.post(`${basePath}api/products`, async (request: any, response: any, next: any) => {
    let createRequest = <ProductCreateRequest> request.body;
    const controller = new ProductsController()
    try {
      var product = await controller.Create.apply(controller, [createRequest])
      response.send(product)
    } catch (error) {
      response.status(403).send(error)
    }
  })

  app.get(`${basePath}healthz`, (req, res) => {
    res.send({
      status: 'Healthy!'
    })
  })
}
