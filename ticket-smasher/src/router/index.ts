import { route } from 'quasar/wrappers';
import {
  createMemoryHistory,
  createRouter,
  createWebHashHistory,
  createWebHistory,
} from 'vue-router';
import routes from './routes';
import { getAuth } from 'firebase/auth';
import { initializeApp } from 'firebase/app';

export default route(function (/* { store, ssrContext } */) {
  const createHistory = process.env.SERVER
    ? createMemoryHistory
    : process.env.VUE_ROUTER_MODE === 'history'
      ? createWebHistory
      : createWebHashHistory;

  const Router = createRouter({
    scrollBehavior: () => ({ left: 0, top: 0 }),
    routes,

    // Leave this as is and make changes in quasar.conf.js instead!
    // quasar.conf.js -> build -> vueRouterMode
    // quasar.conf.js -> build -> publicPath
    history: createHistory(process.env.VUE_ROUTER_BASE),
  });

  // Initialize Firebase
  const app = initializeApp({
    apiKey: process.env.VUE_APP_FIREBASE_API_KEY ?? '',
    authDomain: process.env.VUE_APP_FIREBASE_AUTH_DOMAIN ?? '',
  });
  const auth = getAuth(app);

  // Add a global navigation guard
  Router.beforeEach((to, from, next) => {
    const user = auth.currentUser;

    // If the user is not authenticated and tries to access any page except /login, redirect to /login
    if (!user && to.path !== '/login') {
      next('/login');
    }
    // If the user is authenticated and tries to access the login page, redirect to the home page
    else if (user && to.path === '/login') {
      next('/');
    }
    else {
      next();
    }
  });

  return Router;
});
