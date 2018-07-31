import { getItem } from '../helper/storage'
export default {
    isLoggedIn: state => {
        return getItem('accessToken') != null
    },

    accessToken: state => {
        return getItem('accessToken')
    },

    products: state => {
        console.log('getproduct')
        return state.products.map(product => {
            let productDefault = {
                price: 0,
                availability: {
                    quantity: 0
                },
                rating: {
                    rate: 0,
                    count: 0
                },
                quantity: 1 //value default when add to cart
            }
            if (!product.availability) {
                product.availability = Object.assign(
                    productDefault.availability
                )
            }
            state.ratings = state.ratings || []
            let rating =
                state.ratings.find(item => {
                    return (item.productId = product.id)
                }) || {}
            product.rating = Object.assign(rating, product.rating)
            return Object.assign(productDefault, product)
        })
    },

    cartReducer: state => {
        state.cart = state.cart || {}
        state.cart.item = state.cart.item || []
        let flat = {
            byItemIds: state.cart.items.map(item => item.productId),
            itemsFlat: state.cart.items.reduce((obj, item) => {
                obj[item.productId] = item
                return obj
            }, {})
        }
        return Object.assign(state.cart, flat)
    },

    ratingsReducer: state => {
        console.log('get rating')
        state.ratings = state.ratings || []
        let flat = {
            byItemIds: state.ratings.items.map(item => item.productId),
            ratingFlats: state.ratings.items.reduce((obj, item) => {
                obj[item.productId] = item
                return obj
            }, {})
        }
        return Object.assign(state.ratings, flat)
    }
}
