import {
    getProducts,
    setRating
} from '../api'

export default {
    // ensure data for rendering given list type
    FETCH_LIST_DATA: ({
        commit,
        dispatch,
        state
    }, {pageIndex}) => {
        return getProducts(pageIndex)
            .then(products => {
                if(products && products.value){
                    commit('SET_LIST', products.value );
                }
            })
    },

    SET_RATING_ITEM: ({
        commit,
        dispatch,
        state
    }, {itemId, rating})=>{
        return setRating(itemId, rating)
            .then(itemId => {
                commit('SET_RATE_ITEM_OF_LIST', itemId, rating)
            })
    }
}
