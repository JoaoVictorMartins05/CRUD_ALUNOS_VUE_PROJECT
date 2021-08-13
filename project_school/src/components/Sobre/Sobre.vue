<template>
  <div>
    <titulo texto="Sobre" :btnVoltar="true" />
    <p>
      Lorem ipsum dolor sit amet, consectetur adipisicing elit. Sint possimus,
      facere molestiae qui veniam error blanditiis esse aspernatur corporis in,
      ratione tempora aut non saepe recusandae architecto impedit. Nobis, nihil.
    </p>
    <p>
      Lorem ipsum dolor sit amet consectetur adipisicing elit. Molestiae sunt
      asperiores a officia ratione quam in perferendis quasi tempore esse
      incidunt architecto repellendus ipsa quos magnam sed voluptates, cum illo.
    </p>
  </div>
</template>

<script>
import Titulo from "../_share/Titulo.vue";

export default {
  components: {
    Titulo,
  },
  data() {
    return {};
  },
};
</script>

<style scoped>
</style>