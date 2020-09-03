<template>
  <div class="btn-link">
    <button
      @click="visitRoute"
      :class="['solar-button', { 'full-width': isFullWidth }]"
    >
      <slot></slot>
    </button>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop } from "vue-property-decorator";

@Component({
  name: "SunnyButton",
  components: {}
})
export default class SunnyButton extends Vue {
  @Prop({ required: false, type: String })
  link?: string;

  @Prop({ required: false, type: Boolean, default: false })
  isFullWidth!: boolean;

  visitRoute() {
    this.$router.push(this.link!);
  }
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";

.solar-button {
  background-color: lighten($color: $sunny-blue, $amount: 10%);
  color: white;
  padding: 0.8rem;
  transition: background-color 0.5s;
  margin: 0.3rem 0.2rem;
  display: inline-block;
  cursor: pointer;
  font-size: 1rem;
  min-width: 100px;
  border: none;
  border-bottom: 2px solid darken($color: $sunny-blue, $amount: 20%);
  border-radius: 3px;

  &:hover {
    background: lighten($sunny-blue, 20%);
    transition: background-color 0.5s;
  }

  &:disabled {
    background: lighten($sunny-blue, 15%);
    border-bottom: 2px solid lighten($color: $sunny-blue, $amount: 20%);
  }

  &:active {
    background: lighten($sunny-blue, 20%);
    border-bottom: 2px solid lighten($color: $sunny-yellow, $amount: 20%);
  }
}

.full-width {
  display: block;
  width: 100%;
}
</style>
