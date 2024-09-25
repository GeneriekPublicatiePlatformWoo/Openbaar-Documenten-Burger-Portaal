<template>
  <nav class="utrecht-nav-bar" aria-label="Hoofdmenu">
    <div class="utrecht-nav-bar__content">
      <ul role="list" class="utrecht-nav-list" id="menu">
        <li class="utrecht-nav-list__item">
          <router-link
            :to="{ name: 'home' }"
            class="utrecht-link utrecht-link--html-a utrecht-nav-list__link"
            >Home</router-link
          >
        </li>

        <li class="utrecht-nav-list__item">
          <router-link
            :to="{ name: 'zoeken' }"
            class="utrecht-link utrecht-link--html-a utrecht-nav-list__link"
            >Zoeken</router-link
          >
        </li>
      </ul>
    </div>
  </nav>
</template>

<script lang="ts" setup></script>

<style lang="scss" scoped></style>
