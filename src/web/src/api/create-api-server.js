import axios from 'axios'

export function createAPI({
    config,
    version
}) {
    let api
    var baseUrl = `${config.databaseURL}${version}`
    // this piece of code may run multiple times in development mode,
    // so we attach the instantiated API to `process` to avoid duplications
    if (process.__API__) {
        api = process.__API__
    } else {
        api = {};
        api.onServer = true
    }

    api.get = function(path){
        return axios.get(`${baseUrl}/${path}`)
        .then(api.then)
        .catch(api.error);
    }

    api.error = function(error) {
        console.log(error);
    }

    api.then = function(response){
        return new Promise((resolve, reject) => {
            console.log('Data: ', response.data)
            resolve(response.data);
        })
    }

    return api
}
