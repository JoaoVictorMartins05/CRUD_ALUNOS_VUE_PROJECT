using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectSchool_API.Data;
using ProjectSchool_API.Models;

namespace ProjectSchool_API.Controllers
{

  [Route("api/[controller]")]
  [ApiController]
  public class ProfessorController : Controller
  {
    public IRepository _repo { get; }
    public ProfessorController(IRepository repo)
    {
      this._repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
      try
      {
        var result = await _repo.GetAllProfessoresAsync(true);
        return Ok(result);
      }
      catch (System.Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
      }
      //return BadRequest();
    }

    [HttpGet("{ProfessorId}")]
    public async Task<IActionResult> Get(int ProfessorId)
    {
      try
      {
        var result = await _repo.GetProfessorAsyncById(ProfessorId, true);
        return Ok(result);
      }
      catch (System.Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
      }
      //return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> post(Professor model)
    {
      try
      {
        _repo.Add(model);
        if (await _repo.SaveChangesAsync())
        {
          return Created($"/api/professor/{model.Id}", model);
        }

      }
      catch (System.Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
      }

      return BadRequest();
    }

    [HttpPut("{ProfessorId}")]
    public async Task<IActionResult> put(int ProfessorId, Professor model)
    {
      try
      {
        var professor = await _repo.GetProfessorAsyncById(ProfessorId, false);

        if (professor == null)
        {
          return NotFound();
        }

        _repo.Update(model);

        if (await _repo.SaveChangesAsync())
        {
          //professor = await _repo.GetProfessorAsyncById(ProfessorId, true);
          return Created($"/api/professor/{model.Id}", model);
        }
      }
      catch (System.Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
      }
      return BadRequest();
    }


    [HttpDelete("{ProfessorId}")]
    public async Task<IActionResult> delete(int ProfessorId)
    {
      try
      {
        var professor = await _repo.GetAlunoAsyncById(ProfessorId, false);

        if (professor == null)
        {
          return NotFound();
        }

        _repo.Delete(professor);

        if (await _repo.SaveChangesAsync())
        {
          return Ok();
        }
      }
      catch (System.Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
      }
      return BadRequest();
    }

    bool IsCpf(string cpf)
    {
      int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
      int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
      string tempCpf;
      string digito;
      int soma;
      int resto;

      cpf = cpf.Trim();
      cpf = cpf.Replace(".", "").Replace("-", "");

      if (cpf.Length != 11)
        return false;

      tempCpf = cpf.Substring(0, 9);
      soma = 0;

      for (int i = 0; i < 9; i++)
        soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
      resto = soma % 11;
      if (resto < 2)
        resto = 0;
      else
        resto = 11 - resto;

      digito = resto.ToString();

      tempCpf = tempCpf + digito;

      soma = 0;

      for (int i = 0; i < 10; i++)
        soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

      resto = soma % 11;

      if (resto < 2)
        resto = 0;
      else
        resto = 11 - resto;
      digito = digito + resto.ToString();
      return cpf.EndsWith(digito);
    }

    public string criptografia(string path)
    {
      System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
      byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(path);
      byte[] hash = md5.ComputeHash(inputBytes);
      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      for (int i = 0; i < hash.Length; i++)
      {
        sb.Append(hash[i].ToString("X2"));
      }
      string newPath = sb.ToString();
      Console.WriteLine("senha nova: " + newPath);
      return newPath;
    }

    public void renomearPasta(string old, string nova)
    {
      string oldFile = @"./Resources/" + old;
      Console.WriteLine(oldFile);
      string newFile = @"./Resources/" + nova;
      Console.WriteLine(newFile);
      System.IO.Directory.Move(oldFile, newFile);
    }

    //------------------------------------
    [HttpGet("picture/{arquivo}")]
    public FileResult Get(string arquivo)
    {
      try
      {
        //string a = DES("aa");
        var caminhoDaImagem = @"./Resources/245C49301EFD94305B0DB5FB2ABC9CD3/a.jpg";
        byte[] dadosArquivo = System.IO.File.ReadAllBytes(caminhoDaImagem);
        return File(dadosArquivo, "image/png");
      }
      catch (System.Exception)
      {
        return null;
      }
    }
    //------------------------------------

  }
}