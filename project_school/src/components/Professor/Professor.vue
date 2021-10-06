<template>
  <div>
    <titulo texto="Professores" :btnVoltar="true" />
    <table>
      <thead>
        <th>Cód.</th>
        <th>Nome</th>
        <th>Alunos</th>
      </thead>
      <tbody v-if="professores.length">
        <tr v-for="(professor, index) in professores" :key="index">
          <!-- <td>{{ index + 1 }}</td> -->
          <td class="colPequeno" style="text-align: center; width: 15%">
            {{ professor.id }}
          </td>

          <router-link
            v-bind:to="'/alunos/' + professor.id"
            tag="td"
            style="cursor: pointer"
          >
            {{ professor.nome }} {{ professor.sobrenome }}
          </router-link>

          <td class="colPequeno" style="text-align: center; width: 20%">
            {{ professor.qtdAlunos }}
          </td>
        </tr>
      </tbody>
      <tfoot v-else>
        <tr>
          <td colspan="3" style="text-align: center">
            <h5>Nenhum professor encontrado</h5>
          </td>
        </tr>
      </tfoot>
    </table>

    <img class="imagemDetalhe" :src="this.image" alt="Imagem" />
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
      image: "",
    };
  },
  created() {
    this.$http
      .get("http://localhost:5000/api/aluno")
      .then((res) => res.json())
      .then((alunos) => {
        this.alunos = alunos;
        this.carregarProfessores();
        this.image = `http://localhost:5000/api/professor/picture/0`;
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
        .get("http://localhost:5000/api/professor")
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