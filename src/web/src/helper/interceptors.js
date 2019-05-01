import axios from 'axios'
import store from '../stores'
import { signinSilent } from '../auth/usermanager'

export default function setup() {
  axios.interceptors.request.use(
    function(config) {
      const token = store.getters['account/accessToken']
      const tokenInfo = store.getters['account/tokenInfo']
      console.log(tokenInfo)
      if (token) {
        config.headers.Authorization = `Bearer ${token}`
      }
      if (tokenInfo && tokenInfo.username) {
        config.headers['X-UserName'] = tokenInfo.username
      }
      config.headers['Cache-Control'] = 'no-cache'
      return config
    },
    function(err) {
      return Promise.reject(err)
    }
  )

  axios.interceptors.response.use(
    function(response) {
      return response
    },
    function(error) {
      if (401 === error.response.status) {
        signinSilent(usermanager => {
          usermanager.then(response => {})
        })
      } else {
        return Promise.reject(error)
      }
    }
  )
}
