import moment from 'moment'
import Vue from 'vue'

Vue.filter('formatDate', function(value, format) {
    debugger;
    if (!format) {
        format = 'MM/DD/YYYY hh:mm'
    }
    if (value) {
        return moment(String(value)).format(format)
    }
})
