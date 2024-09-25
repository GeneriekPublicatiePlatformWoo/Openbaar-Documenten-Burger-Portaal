<template>
  <utrecht-article>
    <utrecht-heading :level="1">Het Woo Burgerportaal</utrecht-heading>

    <utrecht-paragraph>
      Zoek hier naar documenten die de gemeente openbaar heeft gemaakt. Deze documenten hebben te
      maken met de ideeën, plannen, regels en afspraken. Zoek op een trefwoord, bijvoorbeeld
      “zonnepanelen” of “windmolens”.
    </utrecht-paragraph>

    <utrecht-paragraph>
      <!-- <utrecht-button appearance="primary-action-button">Zoeken</utrecht-button> -->

      <router-link
        :to="{ name: 'zoeken' }"
        class="utrecht-button-link utrecht-button-link--html-a utrecht-button-link--primary-action"
        >Zoeken</router-link
      >
    </utrecht-paragraph>

    <utrecht-heading :level="2">Wat kunt u hier vinden?</utrecht-heading>

    <utrecht-paragraph>
      De Wet Open Overheid (Woo) regelt landelijk welke soorten informatie door overheden actief
      openbaar gemaakt moeten worden. De gemeente werkt de komende jaren aan de aanpassingen die
      nodig zijn om dit te doen. Er zijn 11 categorieën van informatie aangewezen die gemeenten
      actief openbaar moeten maken. We noemen dit Woo-categorieën. Naast Woo-categorieën heeft de
      gemeente ook andere onderwerpen waarover zij informatie actief openbaar maakt, die noemen we
      overige onderwerpen.
    </utrecht-paragraph>

    <utrecht-heading :level="2">Heeft u nog niet alles gevonden?</utrecht-heading>

    <utrecht-paragraph>
      Misschien vindt u de gezochte informatie op één van de volgende websites:
    </utrecht-paragraph>

    <utrecht-unordered-list>
      <utrecht-unordered-list-item>List item 1</utrecht-unordered-list-item>
      <utrecht-unordered-list-item>List item 2</utrecht-unordered-list-item>
      <utrecht-unordered-list-item>List item 3</utrecht-unordered-list-item>
    </utrecht-unordered-list>
  </utrecht-article>
</template>

<script setup lang="ts"></script>
