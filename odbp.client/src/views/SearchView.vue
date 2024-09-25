<template>
  <utrecht-article>
    <utrecht-heading :level="1">Zoeken</utrecht-heading>

    <form class="utrecht-form">
      <div class="utrecht-form-fieldset">
        <fieldset class="utrecht-form-fieldset__fieldset utrecht-form-fieldset--html-fieldset">
          <utrecht-form-field
            ><utrecht-form-label
              for="92eb76ee-c52f-4dc2-b3db-257ab2cba897"
              aria-hidden="true"
              hidden
              >Zoekterm</utrecht-form-label
            >
            <utrecht-textbox
              id="92eb76ee-c52f-4dc2-b3db-257ab2cba897"
              aria-placeholder="Hier zoeken"
              placeholder="Hier zoeken"
              v-model="zoekTerm"
          /></utrecht-form-field>
        </fieldset>
      </div>
    </form>
  </utrecht-article>
</template>

<script setup lang="ts">
import { ref } from "vue";

const zoekTerm = ref<string>("");
</script>
