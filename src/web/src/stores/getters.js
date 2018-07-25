import { getItem } from '../helper/storage'
export default {
    isLoggedIn: state => {
        return getItem('accessToken') != null
    },

    accessToken: state => {
        return getItem('accessToken')
    },

    products: state => {
        var products = []
        state.products.forEach(product => {
            var productDefault = {
                name: undefined,
                desc: undefined,
                price: 0,
                availability: {
                    location: undefined,
                    quantity: 0,
                    link: undefined
                },
                rating: {
                    id: undefined,
                    rate: 0,
                    count: 0
                },
                quantity: 1 //value default when add to cart
            }
            if (!product.availability) {
                product.availability = Object.assign(
                    productDefault.availability,
                    product.availability
                )
            }
            if (!product.rating) {
                product.rating = Object.assign(
                    productDefault.rating,
                    product.rating
                )
            }
            products.push(Object.assign(productDefault, product))
        })
        return products
    },

    // cartReducer: state => {
    //     return {
    //         byItemIds: state.cart.items.map(item => item.id),
    //         items: state.cart.items.reduce((obj, item) => {
    //             obj[post.id] = item
    //             return obj
    //         }, {})
    //     }
    //     state.cart.items.forEach(item => {})
    //     return products
    // }
}
