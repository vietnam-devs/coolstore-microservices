import {
    getProducts
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
}
