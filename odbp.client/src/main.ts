import "@utrecht/design-tokens/dist/index.css";
import "@utrecht/component-library-css";
import "./assets/design-tokens.scss";
import "./assets/main.scss";

import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";

import {
  SkipLink as UtrechtSkipLink,
  Article as UtrechtArticle,
  Heading as UtrechtHeading,
  Paragraph as UtrechtParagraph,
  UnorderedList as UtrechtUnorderedList,
  UnorderedListItem as UtrechtUnorderedListItem,
  Button as UtrechtButton,
  FormField as UtrechtFormField,
  FormLabel as UtrechtFormLabel,
  Textbox as UtrechtTextbox
} from "@utrecht/component-library-vue";

const app = createApp(App);

app.use(router);

app
  .component("UtrechtSkipLink", UtrechtSkipLink)
  .component("UtrechtArticle", UtrechtArticle)
  .component("UtrechtHeading", UtrechtHeading)
  .component("UtrechtParagraph", UtrechtParagraph)
  .component("UtrechtUnorderedList", UtrechtUnorderedList)
  .component("UtrechtUnorderedListItem", UtrechtUnorderedListItem)
  .component("UtrechtButton", UtrechtButton)
  .component("UtrechtFormField", UtrechtFormField)
  .component("UtrechtFormLabel", UtrechtFormLabel)
  .component("UtrechtTextbox", UtrechtTextbox)
  .mount("#app");
