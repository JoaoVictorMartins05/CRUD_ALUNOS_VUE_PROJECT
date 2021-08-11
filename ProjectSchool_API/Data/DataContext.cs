using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProjectSchool_API.Models;

namespace ProjectSchool_API.Data

{
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Aluno> Alunos { get; set; }
    public DbSet<Professor> Professores { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.Entity<Professor>().HasData(
          new List<Professor>(){
                new Professor(){
                    Id=1,
                    Nome= "Marcelo Deux"
                },
                new Professor(){
                    Id=2,
                    Nome= "Rodrigo Robozão"
                },
                new Professor(){
                    Id=3,
                    Nome= "Ze Ruim"
                },
          }
      );

      builder.Entity<Aluno>().HasData(
        new List<Aluno>(){
                new Aluno(){
                    Id=1,
                    Nome= "João",
                    Sobrenome="Martins",
                    DataNasc="05/10/1999",
                    ProfessorId=1
                },
                new Aluno(){
                    Id=2,
                    Nome= "Gabi",
                    Sobrenome="Gabry",
                    DataNasc="20/01/2000",
                    ProfessorId=2
                },
                new Aluno(){
                    Id=3,
                    Nome= "Carlim",
                    Sobrenome="Carvalho",
                    DataNasc="01/01/1345",
                    ProfessorId=3
                },
        }
    );



    }
  }
}