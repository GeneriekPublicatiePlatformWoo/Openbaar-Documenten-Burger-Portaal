import { createRouter, createWebHistory } from "vue-router";
import HomeView from "../views/HomeView.vue";
import SearchView from "../views/SearchView.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      component: HomeView,
      meta: {
        title: "Homepage"
      }
    },
    {
      path: "/zoeken",
      name: "zoeken",
      component: SearchView,
      meta: {
        title: "Zoeken naar documenten"
      }
    }
  ]
});

router.beforeEach(async (to) => {
  document.title = `${to.meta?.title || ""} | ${import.meta.env.VITE_APP_TITLE}`;

  document.body.setAttribute("tabindex", "-1");
  document.body.focus();
  document.body.removeAttribute("tabindex");
});

export default router;
