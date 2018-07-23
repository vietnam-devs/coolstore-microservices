import axios from 'axios'

export function createAPI({ config, version }) {
    let api
    var baseUrl = `${config.databaseURL}${version}`
    // this piece of code may run multiple times in development mode,
    // so we attach the instantiated API to `process` to avoid duplications
    if (process.__API__) {
        api = process.__API__
    } else {
        api = {}
        api.onServer = true
    }

    api.get = function(path) {
        return axios.get(`${baseUrl}/${path}`)
    }

    api.post = function(path, data) {
        return axios.post(`${baseUrl}/${path}`, JSON.stringify(data), {
            headers: {
                'Content-Type': 'application/json'
            }
        })
    }

    api.put = function(path, data) {
        return axios.put(`${baseUrl}/${path}`, JSON.stringify(data), {
            headers: {
                'Content-Type': 'application/json'
            }
        })
    }

    api.delete = function(path) {
        return axios.delete(`${baseUrl}/${path}`)
    }

    return api
}
