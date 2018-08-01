import { getItem } from '../helper/storage'
export default {
    isLoggedIn: state => {
        return getItem('accessToken') != null
    },

    accessToken: state => {
        return getItem('accessToken')
    },

    products: state => {
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

    productIds: state => state.products.map(product => product.id),

    ratingSet: state => {
        let ratingSet = state.ratings.reduce((obj, item) => {
            obj[item.productId] = item
            return obj
        }, {})
        return state.products.reduce((obj, item) => {
            ratingSet[item.id] = ratingSet[item.id] || {}
            obj[item.id] = ratingSet[item.id]
            return obj;
        }, {})
    }
    // state.ratings.reduce((obj, item) => {
    //     obj[item.productId] = item
    //     return obj
    // }, {})
}
