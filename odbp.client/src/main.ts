import "@utrecht/design-tokens/dist/index.css";
import "@utrecht/component-library-css";
import "./assets/design-tokens.scss";
import "./assets/main.scss";

import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import { registerComponents } from "@/components/register";

const app = createApp(App);

app.use(router);

registerComponents(app);

app.mount("#app");
