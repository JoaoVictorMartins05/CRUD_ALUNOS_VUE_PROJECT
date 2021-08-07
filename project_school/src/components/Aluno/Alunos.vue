<template>
  <div>
    <titulo
      :texto="
        professorid != undefined
          ? 'Professor: ' + professor.nome
          : 'Todos os Alunos'
      "
    />
    <div v-if="professorid != undefined">
      <input
        type="text"
        placeholder="Nome do Aluno"
        v-model="nome"
        v-on:keyup.enter="addAluno()"
      />

      <button class="btn btnInput" @click="addAluno()">Adicionar</button>
    </div>

    <table>
      <thead>
        <th>Mat.</th>
        <th>Nome</th>
        <th>Opções</th>
      </thead>
      <tbody v-if="alunos.length">
        <tr v-for="(aluno, index) in alunos" :key="index">
          <!-- <td>{{ index + 1 }}</td> -->
          <td class="colPequeno" style="text-align: center; width: 15%">
            {{ aluno.id }}
          </td>
          <router-link
            :to="`/alunoDetalhe/${aluno.id}`"
            tag="td"
            style="cursor: pointer"
          >
            {{ aluno.nome }} {{ aluno.sobrenome }}
          </router-link>
          <td>
            <button class="btn btn_danger" @click="remover(aluno)">
              Remover
            </button>
          </td>
        </tr>
      </tbody>
      <tfoot v-else>
        Nenhum aluno encontrado
      </tfoot>
    </table>
  </div>
</template>

<script>
//import Titulo from "../components/_share/Titulo.vue";
import Titulo from "../_share/Titulo.vue";

export default {
  components: { Titulo },
  data() {
    return {
      titulo: "Aluno",
      nome: "",
      professor: {},
      professorid: this.$route.params.prof_id,
      alunos: [],
    };
  },

  created() {
    if (this.professorid) {
      this.carregarProfessores();
      this.$http
        .get("http://localhost:3000/alunos?professor.id=" + this.professorid)
        .then((res) => res.json())
        .then((alunos) => (this.alunos = alunos));
    } else {
      this.$http
        .get("http://localhost:3000/alunos")
        .then((res) => res.json())
        .then((alunos) => (this.alunos = alunos));
    }
  },

  props: {},

  methods: {
    addAluno() {
      let _aluno = {
        nome: this.nome,
        sobrenome: "",
        professor: {
          id: this.professor.id,
          nome: this.professor.nome,
        },
      };

      this.$http
        .post("http://localhost:3000/alunos", _aluno)
        .then((res) => res.json())
        .then((alunoRetornado) => {
          this.alunos.push(alunoRetornado);
          this.nome = "";
        });

      // this.alunos.forEach((aluno) => {
      //   console.log(aluno);
      // });
    },

    remover(aluno) {
      this.$http.delete(`http://localhost:3000/alunos/${aluno.id}`).then(() => {
        let indice = this.alunos.indexOf(aluno);
        this.alunos.splice(indice, 1);
      });
    },

    carregarProfessores() {
      this.$http
        .get("http://localhost:3000/professores/" + this.professorid)
        .then((res) => res.json())
        .then((professor) => {
          this.professor = professor;
        });
    },
  },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
input {
  width: calc(100% - 195px);
  border: 0;
  padding: 20px;
  font-size: 1.3em;
  color: #687f7f;
  display: inline;
  border-radius: 5px;
}

.btnInput {
  width: 150px;
  /* margin-left: 4px; */
  border: 0px;
  padding: 20px;
  font-size: 1.3em;
  display: inline;
  background-color: rgb(116, 115, 115);
}

.btnInput:hover {
  padding: 20px;
  margin: 0px;
  border: 0px;
}
</style>
