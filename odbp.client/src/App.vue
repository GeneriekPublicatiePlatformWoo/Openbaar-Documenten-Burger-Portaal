<script setup lang="ts">
import { RouterView } from "vue-router";
import TheHeader from "./components/TheHeader.vue";
import TheNav from "./components/TheNav.vue";
import TheFooter from "./components/TheFooter.vue";
</script>

<template>
  <the-header />

  <the-nav />

  <main>
    <router-view />
  </main>

  <the-footer />
</template>

<style lang="scss" scoped>
nav {
  border: 1px solid var(--utrecht-color-red-40);
  border-left: none;
  border-right: none;
}
</style>
