declare var require: any

var uuid = require('uuid')
var fs = require('fs')
import { Route, Get, Post, Body } from 'tsoa'
import { default as Product, ProductCreateRequest } from '../models/product'

@Route(`api/products`)
export class ProductController {
  /**
   * Get the all product
   */
  @Get()
  public async GetAll(): Promise<any> {
    // @ts-ignore
    var products = Product.find({}).exec()
    return Promise.resolve(products)
  }

  /**
   * Get product by Id
   * @param productId Product Id
   */
  @Get(`{productId}`)
  public Get(productId: string): Promise<any> {
    // @ts-ignore
    let product = Product.findOne({ id: productId }).exec()
    return Promise.resolve(product)
  }

  /**
   * Create a product
   * @param request This is a product creation request description
   */
  @Post()
  public Create(@Body() request: ProductCreateRequest): Promise<any> {
    let product = new Product({ _id: uuid.v1(), ...request })
    console.log(product)
    let result = Product.create(product)
    return Promise.resolve(result)
  }
}
