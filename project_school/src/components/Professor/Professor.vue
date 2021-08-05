<template>
  <div>
    <titulo texto="Professores" />
    <table>
      <thead>
        <th>Cód.</th>
        <th>Nome</th>
        <th>Alunos</th>
      </thead>
      <tbody v-if="professores.length">
        <tr v-for="(professor, index) in professores" :key="index">
          <!-- <td>{{ index + 1 }}</td> -->
          <td>{{ professor.id }}</td>

          <router-link
            v-bind:to="'/alunos/' + professor.id"
            tag="td"
            style="cursor: pointer"
          >
            {{ professor.nome }} {{ professor.sobrenome }}
          </router-link>

          <td>{{ professor.qtdAlunos }}</td>
        </tr>
      </tbody>
      <tfoot v-else>
        Nenhum professor encontrado
      </tfoot>
    </table>
  </div>
</template>

<script>
import Titulo from "../_share/Titulo.vue";

export default {
  components: { Titulo },
  data() {
    return {
      titulo: "Professores",
      professores: [],
      alunos: [],
    };
  },
  created() {
    this.$http
      .get("http://localhost:3000/alunos")
      .then((res) => res.json())
      .then((alunos) => {
        this.alunos = alunos;
        this.carregarProfessores();
      });
  },

  props: {},
  methods: {
    pegarQtdAlunoPorProfessor() {
      this.professores.forEach((professor, index) => {
        professor = {
          id: professor.id,
          nome: professor.nome,
          qtdAlunos: this.alunos.filter(
            (aluno) => aluno.professor.id == professor.id
          ).length,
        };
        this.professores[index] = professor;
      });
    },

    carregarProfessores() {
      this.$http
        .get("http://localhost:3000/professores")
        .then((res) => res.json())
        .then((professor) => {
          this.professores = professor;
          this.pegarQtdAlunoPorProfessor();
        });
    },
  },
};
</script>

<style scoped>
</style>