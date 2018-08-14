import ProductSchema from './../models/product'
import * as mongoose from 'mongoose'
import { Route, Get, Post, Delete, Body } from 'tsoa'
import { Product, ProductCreateRequest } from '../models/product'

const ProductMongoose = mongoose.model('Product', ProductSchema)

@Route('api/products')
export class ProductsController {
  /** Get the all product */
  @Get('')
  public async GetAll(): Promise<Product[]> {
    var products = await ProductMongoose.find({}).exec()
    return products
  }

  /** Get product by ID */
  @Get('{productId}')
  public async Get(productId: string): Promise<Product> {
   return await ProductMongoose.findProduct(productId).exec()
  }

  /**
   * Create a product
   * @param request This is a product creation request description
   */
  @Post()
  public async Create(@Body() request: ProductCreateRequest): Promise<Product> {
    var newProduct = new ProductMongoose()
    var product = newProduct.createProduct(request)
    return new Promise<Product>((resolve, reject) => {
      product.save(function (error) {
        if (error) {
          reject(error)
        } else {
          resolve(product)
        }
      })
    })

  }
}
