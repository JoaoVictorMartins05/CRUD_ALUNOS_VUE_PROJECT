using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProjectSchool_API.Models;

namespace ProjectSchool_API.Data

{
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    { }
    public DbSet<Aluno> Alunos { get; set; }
    public DbSet<Professor> Professores { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.Entity<Professor>().HasData(
          new List<Professor>(){
                new Professor(){
                    Id=1,
                    Nome= "Kakashi Hatake",
                    cpf= "47634928020"
                }
          }
      );

      builder.Entity<Aluno>().HasData(
        new List<Aluno>(){
                new Aluno(){
                    Id=1,
                    Nome= "Naruto",
                    Sobrenome="Uzumaki",
                    DataNasc="05/10/1999",
                    cpf= "99261356095",
                    arquivo = "./Resources/naruto.jpg",
                    ProfessorId=1
                },
                new Aluno(){
                    Id=2,
                    Nome= "Sakura",
                    Sobrenome="Haruno",
                    DataNasc="20/01/2000",
                    cpf="47293417080",
                    ProfessorId=1
                },
                new Aluno(){
                    Id=3,
                    Nome= "Sasuke",
                    Sobrenome="Uchiha",
                    DataNasc="01/01/1345",
                    cpf = "68439389060",
                    ProfessorId=1
                },
        }
    );


    }
  }
}