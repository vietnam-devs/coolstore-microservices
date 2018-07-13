import Vue from 'vue'

export default {
    SET_LIST: (state, data) => {
        state.products = data
    },
    SET_RATE_ITEM_OF_LIST: (state, itemId, ratting) => {
        state.products.forEach(product => {
            if (product.itemId === itemId) {
                product.rating.rating = rating;
                product.rating.rated = true;
                product.rating.count++;
            }
        })
    },
}