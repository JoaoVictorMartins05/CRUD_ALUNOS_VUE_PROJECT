import Vue from "vue";
import Router from "vue-router";
import Alunos from "./components/Aluno/Alunos.vue";
import Professor from "./components/Professor/Professor.vue";
import Sobre from "./components/Sobre/Sobre";
import  AlunoDetalhe from "./components/Aluno/AlunoDetalhe.vue"

Vue.use(Router);

export default new Router({
  routes:[
    {
      path: '/professores',
      nome: 'professores',
      component: Professor
    },
    {
      path: '/alunos/:prof_id',
      nome: 'alunos',
      component: Alunos
    },
    {
      path: '/todosalunos',
      nome: 'alunos',
      component: Alunos
    },
    {
      path: '/alunoDetalhe/:id',
      nome: 'AlunoDetalhe',
      component: AlunoDetalhe
    },
    {
      path: '/sobre',
      nome: 'sobre',
      component: Sobre
    }
  ]
});