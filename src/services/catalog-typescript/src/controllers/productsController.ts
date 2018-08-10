import { ProductSchema } from './../models/product'
import * as mongoose from 'mongoose'
import { Route, Get, Post, Delete, Body } from 'tsoa'
import { Product, ProductCreateRequest } from '../models/product'

const ProductMongoose = mongoose.model('Product', ProductSchema)

@Route('api/products')
export class ProductsController {
  /** Get the current user */
  @Get('')
  public async GetAll(): Promise<Product[]> {
    var products = await ProductMongoose.find({}).exec()
    return products
  }

  /** Get user by ID */
  @Get('{productId}')
  public async Get(productId: number): Promise<Product> {
    return {
      id: 'd831e238-94ae-44cb-8ed9-16d6addf5876',
      name: 'Product Name',
      desc: 'Product Description',
      price: 200.25,
      imageUrl: 'Image Url'
    }
  }

  /**
   * Create a user
   * @param request This is a user creation request description
   */
  @Post()
  public async Create(@Body() request: ProductCreateRequest): Promise<Product> {
    return {
      id: 'd831e238-94ae-44cb-8ed9-16d6addf5876',
      name: 'Product Name',
      desc: 'Product Description',
      price: 200.25,
      imageUrl: 'Image Url'
    }
  }

  /** Delete a user by ID */
  @Delete('{productId}')
  public async Delete(productId: number): Promise<void> {
    return Promise.resolve()
  }
}
