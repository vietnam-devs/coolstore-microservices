import axios from 'axios'

export function createAPI({ config, version }) {
  let api
  const baseUrl = `${config.databaseURL}${version}`
  // this piece of code may run multiple times in development mode,
  // so we attach the instantiated API to `process` to avoid duplications
  if (process.__API__) {
    api = process.__API__
  } else {
    api = {}
    api.onServer = true
  }

  api.get = function(path) {
    return new Promise(function(resolve, reject) {
      axios
        .get(`${baseUrl}/${path}`, {
          headers: {
            'Content-Type': 'application/json'
          }
        })
        .then(
          response => {
            response = response || {}
            response.data = response.data || {}
            resolve(response.data)
          },
          error => {
            reject()
            errorHandler(error)
          }
        )
    })
  }

  api.post = function(path, data) {
    return new Promise(function(resolve, reject) {
      axios
        .post(`${baseUrl}/${path}`, JSON.stringify(data), {
          headers: {
            'Content-Type': 'application/json'
          }
        })
        .then(
          response => {
            response = response || {}
            response.data = response.data || {}
            resolve(response.data)
          },
          error => {
            reject()
            errorHandler(error)
          }
        )
        .catch(error => {
          reject()
          errorHandler(error)
        })
    })
  }

  api.put = function(path, data) {
    return new Promise(function(resolve, reject) {
      axios
        .put(`${baseUrl}/${path}`, JSON.stringify(data), {
          headers: {
            'Content-Type': 'application/json'
          }
        })
        .then(
          response => {
            response = response || {}
            response.data = response.data || {}
            resolve(response.data)
          },
          error => {
            reject()
            errorHandler(error)
          }
        )
        .catch(error => {
          reject()
          errorHandler(error)
        })
    })
  }

  api.delete = function(path) {
    return new Promise(function(resolve, reject) {
      axios
        .delete(`${baseUrl}/${path}`)
        .then(
          response => {
            response = response || {}
            response.data = response.data || {}
            resolve(response.data)
          },
          error => {
            reject()
            errorHandler(error)
          }
        )
        .catch(error => {
          reject()
          errorHandler(error)
        })
    })
  }

  function errorHandler(error) {
    console.log(error)
  }

  return api
}
