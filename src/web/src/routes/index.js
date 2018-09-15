import Vue from 'vue'
import Router from 'vue-router'
import { requireAuth } from '../auth/auth'

Vue.use(Router)

// route-level code splitting
const CartView = () => import('./Cart.vue')
const CallbackView = () => import('../components/Callback.vue')
const UnauthorizedView = () => import('../components/Unauthorized.vue')
const Review = () => import('../components/Review.vue')
const NewCatalog = () => import('./NewCatalog.vue')
const Index = () => import('./index.vue')

const router = new Router({
  mode: 'history',
  fallback: false,
  scrollBehavior: () => ({ y: 0 }),
  routes: [
    {
      path: '/',
      name: 'Home',
      component: Index,
      beforeEnter: requireAuth
    },
    {
      path: '/cart',
      name: 'Cart',
      component: CartView,
      beforeEnter: requireAuth
    },
    {
      path: '/callback',
      name: 'callback',
      component: CallbackView
    },
    {
      path: '/unauthorized',
      name: 'unauthorized',
      component: UnauthorizedView
    },
    {
      path: '/review/:id',
      name: 'reviewproduct',
      beforeEnter: requireAuth,
      component: Review
    },
    {
      path: '/new',
      name: 'new catalog',
      beforeEnter: requireAuth,
      component: NewCatalog
    }
  ]
})
export default router
