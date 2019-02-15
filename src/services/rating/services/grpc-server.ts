import * as uuid from 'uuid'
import * as protoLoader from '@grpc/proto-loader'
import * as grpc from 'grpc'
import { default as Logger } from './logger'
import { default as ratingSchema, RatingModel } from '../models/rating'
import { RatingService } from './rating'

const RatingProtoServices = {
  getRatings: async (call: any, callback: any) => {
    Logger.info('request', call.request)
    const ratings = await ratingSchema.find({}).exec()
    const results = ratings.map((rating: any) => {
      return {
        id: rating._id,
        product_id: rating.productId,
        user_id: rating.userId,
        cost: rating.cost
      }
    })
    Logger.info(results)
    callback(null, { ratings: results })
  },
  getRatingByProductId: async (call: any, callback: any) => {
    Logger.info('request', call.request)
    const rating: any = await RatingService.findRatingByProductId(call.request.product_id)

    if (rating == null) {
      callback(null, [])
    }

    callback(null, {
      rating: {
        id: rating._id,
        product_id: rating.productId,
        user_id: rating.userId,
        cost: rating.cost
      }
    })
  },
  createRating: async (call: any, callback: any) => {
    Logger.info('request', call.request)
    const model: RatingModel = {
      id: uuid.v1(),
      productId: call.request.product_id,
      userId: call.request.user_id,
      cost: call.request.cost
    }
    const rating: any = await RatingService.createRating(model)
    callback(null, {
      rating: {
        id: rating._id,
        product_id: rating.productId,
        user_id: rating.userId,
        cost: rating.cost
      }
    })
  },
  updateRating: async (call: any, callback: any) => {
    Logger.info('request', call.request)
    const model: RatingModel = {
      id: call.request.id,
      productId: call.request.product_id,
      userId: call.request.user_id,
      cost: call.request.cost
    }
    const rating: any = await RatingService.updateRating(model)
    callback(null, {
      rating: {
        id: rating._id,
        product_id: rating.productId,
        user_id: rating.userId,
        cost: rating.cost
      }
    })
  }
}

const getProto = (protofile: any) => {
  const packageDefinition = protoLoader.loadSync(protofile, {
    keepCase: true,
    longs: String,
    enums: String,
    defaults: true,
    oneofs: true,
    includeDirs: ['node_modules/google-proto-files', 'proto']
  })
  return grpc.loadPackageDefinition(packageDefinition)
}

export default async () => {
  const server = new grpc.Server()
  const proto: any = getProto('../proto/rating.proto')

  Logger.info(RatingProtoServices)
  await server.addService(proto.coolstore.RatingService.service, {
    ...RatingProtoServices
  })

  server.bind(`0.0.0.0:${process.env.PORT || 5007}`, grpc.ServerCredentials.createInsecure())
  server.start()
  Logger.info(`gRPC server running on port ${process.env.PORT}.\n Press CTRL-C to stop.\n`)
}
