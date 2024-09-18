import type { App } from "vue";

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

export const registerComponents = (app: App): void => {
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
    .component("UtrechtTextbox", UtrechtTextbox);
};
