import * as mongoose from 'mongoose'
const Schema = mongoose.Schema

export interface Product {
  id: string
  name: string
  desc: string
  price: number
  imageUrl: string
}

export interface ProductCreateRequest {
  name: string
  desc: string
  price: number
  imageUrl: string
}

export const ProductSchema = new Schema({
  id: {
    type: String,
    required: 'Enter a id'
  },
  name: {
    type: String,
    required: 'Product name cannot be blank'
  },
  desc: {
    type: String
  },
  price: {
    type: Number,
    required: 'Price cannot be blank'
  },
  imageUrl: {
    type: String,
    required: 'Image cannot be blank'
  }
})
