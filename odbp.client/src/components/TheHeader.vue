<template>
  <header>
    <p><utrecht-skip-link href="#main">Naar inhoud</utrecht-skip-link></p>

    <p><utrecht-skip-link href="#menu">Naar menu</utrecht-skip-link></p>

    <svg width="300" height="100" xmlns="http://www.w3.org/2000/svg">
      <rect width="90" height="100" fill="white" />

      <polygon points="0,0 90,0 0,100" fill="#c00" />

      <text
        x="55%"
        y="85%"
        text-anchor="middle"
        font-family="Lucida Grande, Lucida Sans Unicode, Lucida Sans, Arial, sans-serif"
        font-size="24"
        fill="#c00"
      >
        Gemeente Demodam
      </text>
    </svg>

    <utrecht-heading :level="1">Wet Open Overheid Burgerportaal</utrecht-heading>
  </header>
</template>

<script lang="ts" setup></script>

<style lang="scss" scoped>
header {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(var(--section-width), 1fr));

  h1 {
    align-self: center;
  }
}
</style>
