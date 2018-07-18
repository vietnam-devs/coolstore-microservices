export default {
    isLoggedIn: state => {
        if (typeof localStorage !== 'undefined') { 
            var accessToken = localStorage.getItem("accessToken");
            state.accessToken = accessToken;
        }
        return state.accessToken != null
    },

    accessToken: state => {
        if (typeof localStorage !== 'undefined') { 
            var accessToken = localStorage.getItem("accessToken");
            state.accessToken = accessToken;
        }
        return state.accessToken;
    }
}