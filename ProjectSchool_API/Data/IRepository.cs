using System.Threading.Tasks;
using ProjectSchool_API.Models;

namespace ProjectSchool_API.Data
{
  public interface IRepository
  {
    //GERAL
    void Add<T>(T entity) where T : class;

    void Update<T>(T entity) where T : class;

    void Delete<T>(T entity) where T : class;

    Task<bool> SaveChangesAsync();

    //Aluno
    Task<Aluno[]> GetAllAlunosAsync(bool includeProfessor);

    Task<Aluno[]> GetAlunosAsyncByProfessorId(int ProfessorID, bool includeProfessor);

    Task<Aluno> GetAlunoAsyncById(int AlunoID, bool includeProfessor);

    //Professor
    Task<Professor[]> GetAllProfessoresAsync(bool includeAluno);
    Task<Professor> GetProfessorAsyncById(int AlunoID, bool includeAluno);

    //verificarSeCpfJáExiste
    bool ifExistCpf(string cpf);

    //bool verificaCpf(string cpf);

  }
}