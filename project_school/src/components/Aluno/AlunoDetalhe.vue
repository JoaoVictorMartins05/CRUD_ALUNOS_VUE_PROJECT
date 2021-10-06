<template>
  <div>
    <titulo :texto="'Aluno: ' + aluno.nome" :btnVoltar="!visualizando">
      <button v-show="visualizando" class="btn btnEditar" @click="editar()">
        Editar
      </button>
    </titulo>
    <table>
      <tbody>
        <tr>
          <td class="colPequeno">Matrícula</td>
          <td>
            <label>{{ aluno.id }}</label>
          </td>
        </tr>
        <tr>
          <td class="colPequeno">Nome</td>
          <td>
            <label v-if="visualizando">{{ aluno.nome }}</label>
            <input v-else v-model="aluno.nome" type="text" />
          </td>
        </tr>
        <tr>
          <td class="colPequeno">Sobrenome</td>
          <td>
            <label v-if="visualizando">{{ aluno.sobrenome }}</label>
            <input v-else v-model="aluno.sobrenome" type="text" />
          </td>
        </tr>
        <tr>
          <td class="colPequeno">Data Nascimento</td>
          <td>
            <label v-if="visualizando">{{ aluno.dataNasc }}</label>
            <input v-else v-model="aluno.dataNasc" type="text" />
          </td>
        </tr>
        <tr>
          <td class="colPequeno">CPF</td>
          <td>
            <label v-if="visualizando">{{ cpfFormatado }}</label>
            <input v-else v-model="cpfFormatado" type="text" />
          </td>
        </tr>

        <tr v-if="!visualizando">
          <td class="colPequeno">Imagem</td>
          <td>
            <input type="file" accept="image/jpeg" ref="files" />
            <button v-on:click="upload()">Upload</button>
          </td>
        </tr>
        <tr>
          <td class="colPequeno">Professor</td>
          <td>
            <label v-if="visualizando">{{ aluno.professor.nome }}</label>
            <select v-else v-model="aluno.professor.id">
              <option
                v-for="(professor, index) in professores"
                :key="index"
                v-bind:value="professor.id"
              >
                {{ professor.nome }}
              </option>
            </select>
          </td>
        </tr>
      </tbody>
    </table>

    <img class="imagemDetalhe" v-bind:src="this.image" alt="Imagem" />

    <div style="margin-top: 10px">
      <div v-if="!visualizando">
        <button class="btn btnSalvar" @click="salvar(aluno)">Salvar</button>
        <button class="btn btnCancelar" @click="cancelar()">Cancelar</button>
      </div>
    </div>
  </div>
</template>


<script>
import Titulo from "../_share/Titulo.vue";
import axios from "axios";

export default {
  components: {
    Titulo,
  },

  data() {
    return {
      aluno: { nome: undefined },
      image: "",
      cpfFormatado: "",
      professores: [],
      idAluno: this.$route.params.id,
      visualizando: true,
      file: {},
    };
  },

  created() {
    this.carregarProfessor();
  },

  methods: {
    carregarProfessor() {
      this.$http
        .get("http://localhost:5000/api/professor")
        .then((res) => res.json())
        .then((professor) => {
          this.professores = professor;
          this.carregarAluno();
        });
    },

    carregarAluno() {
      this.$http
        .get(`http://localhost:5000/api/aluno/${this.idAluno}`)
        .then((res) => res.json())
        .then((aluno) => {
          this.aluno = aluno;
          this.formatarCpf(this.aluno.cpf);
          this.loading = false;
          this.image = `http://localhost:5000/api/aluno/picture/${this.aluno.arquivo}`;
        });
    },

    formatarCpf(cpf) {
      let stringAux = cpf;
      this.cpfFormatado =
        stringAux.substr(0, 3) +
        "." +
        stringAux.substr(3, 3) +
        "." +
        stringAux.substr(6, 3) +
        "-" +
        stringAux.substr(9, 2);
      console.log(this.cpfFormatado);
    },

    upload() {
      console.log("olaaaa");
      var dataForm = new FormData();
      var str = this.aluno.arquivo;

      console.log(this.$refs.files.files);

      if (this.$refs.files.files.length == 0) {
        alert("Upload Failed");
        location.reload();
      }

      for (let file of this.$refs.files.files) {
        dataForm.append(`file`, file, str);
      }

      console.log(dataForm);

      axios
        .post("http://localhost:5000/api/aluno/upload", dataForm, {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        })
        .then((res) => {
          console.log("Upload da Imagem: " + res.statusText);
          if (res.status == 200) {
            this.image = `http://localhost:5000/api/aluno/picture/${this.aluno.arquivo}`;
            alert("Upload Sucess");
            location.reload();
          } else {
            alert("Upload Failed");
          }
        });
    },

    editar() {
      this.visualizando = !this.visualizando;
    },

    cpfParaBD(cpf) {
      let stringAux = cpf;

      if (cpf.length == 14) {
        this.aluno.cpf =
          stringAux.substr(0, 3) +
          stringAux.substr(4, 3) +
          stringAux.substr(8, 3) +
          stringAux.substr(12, 2);
        console.log(this.aluno.cpf);
      } else {
        this.aluno.cpf = cpf;
      }
    },

    salvar(_aluno) {
      this.cpfParaBD(this.cpfFormatado);

      let _alunoEditar = {
        id: _aluno.id,
        nome: _aluno.nome,
        sobrenome: _aluno.sobrenome,
        dataNasc: _aluno.dataNasc,
        cpf: this.aluno.cpf,
        professorid: _aluno.professor.id,
      };

      if (_aluno.cpf == "") {
        console.log("ERRO: CPF IS NULL");
        alert("Campo CPF vazio! Por Favor, preencha o CPF");
      } else {
        this.$http
          .put(
            `http://localhost:5000/api/aluno/${_alunoEditar.id}`,
            _alunoEditar
          )
          .then((res) => {
            res
              .json()
              .then((aluno) => {
                this.aluno = aluno;
                this.formatarCpf(this.aluno.cpf);
              })
              .then(() => (this.visualizando = true));
            console.log("CPF ACEITO");
          })
          .catch((error) => {
            console.log(error.data);
            alert(error.data);
            this.visualizando = !this.visualizando;
          });

        this.visualizando = !this.visualizando;
      }
    },

    cancelar() {
      this.visualizando = !this.visualizando;
    },
  },
};
</script>

<style scoped>
.btnSalvar {
  float: right;
  background-color: rgb(79, 196, 68);
}

.btnCancelar {
  float: left;
  background-color: rgb(249, 186, 92);
}

.btnEditar {
  float: right;
  background-color: rgb(76, 186, 249);
}

.colPequeno {
  width: 20%;
}

input,
select {
  margin: 0px;
  padding: 5px 10px;
  font-size: 0.9em;
  font-family: Montserrat;
  border-radius: 5px;
  border: 1px solid silver;
  width: 95%;
  background-color: rgb(245, 245, 245);
}

select {
  height: 38px;
  width: 103%;
}
</style>