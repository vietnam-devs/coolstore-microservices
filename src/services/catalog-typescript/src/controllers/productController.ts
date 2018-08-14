import { Route, Get, Post, Body } from 'tsoa'
import { Product, ProductCreateRequest } from '../models/product'

@Route(`api/products`)
export class ProductController {
  /**
   * Get the all product
   */
  @Get()
  public async GetAll(): Promise<any> {
    var products = Product.find({}).exec()
    return Promise.resolve(products)
  }

  /**
   * Get product by Id
   * @param productId Product Id
   */
  @Get(`{productId}`)
  public Get(productId: string): Promise<any> {
    let product = Product.findOne({ productId }).exec()
    return Promise.resolve(product)
  }

  /**
   * Create a product
   * @param request This is a product creation request description
   */
  @Post()
  public Create(@Body() product: ProductCreateRequest): Promise<any> {
    var result = Product.createProduct(product)
    return Promise.resolve(result)
  }
}

export default new ProductController()
