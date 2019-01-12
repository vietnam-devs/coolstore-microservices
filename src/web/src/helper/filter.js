import moment from 'moment'
import Vue from 'vue'

Vue.filter('formatDate', function(value, format) {
  if (!format) {
    format = 'MM/DD/YYYY hh:mm'
  }
  if (value) {
    return moment(String(value)).format(format)
  }
})

Vue.filter('toCurrency', function(value) {
  if (typeof value !== 'number') {
    return value
  }
  var formatter = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
    minimumFractionDigits: 0
  })
  return formatter.format(value)
})
