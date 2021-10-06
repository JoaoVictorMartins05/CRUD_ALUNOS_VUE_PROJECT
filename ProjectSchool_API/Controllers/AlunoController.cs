using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectSchool_API.Data;
using ProjectSchool_API.Models;
using static System.Net.Mime.MediaTypeNames;

namespace ProjectSchool_API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AlunoController : Controller
  {
    public AlunoController(IRepository _repo)
    {
      this._repo = _repo;

    }
    public IRepository _repo { get; }


    [HttpGet]
    public async Task<IActionResult> Get()
    {

      //Console.WriteLine("A função que verifica se existe é: " + _repo.ifExistCpf("99261356095"));
      //Console.WriteLine("A função que verifica se é válido é: " + IsCpf("99261356095"));

      try
      {
        var result = await _repo.GetAllAlunosAsync(true);
        return Ok(result);
      }
      catch (System.Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
      }
    }

    [HttpGet("{AlunoId}")]
    public async Task<IActionResult> Get(int AlunoId)
    {

      try
      {
        var result = await _repo.GetAlunoAsyncById(AlunoId, true);
        //Image imagem = result.arquivo;
        return Ok(result);
      }
      catch (System.Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
      }

    }

    //------------------------------------
    [HttpGet("picture/{arquivo}")]
    public FileResult Get(string arquivo)
    {
      try
      {
        //string a = DES("aa");
        var caminhoDaImagem = @"./Resources/" + arquivo + "/a.jpg";
        byte[] dadosArquivo = System.IO.File.ReadAllBytes(caminhoDaImagem);
        return File(dadosArquivo, "image/png");
      }
      catch (System.Exception)
      {
        return null;
      }
    }
    //------------------------------------


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


    [HttpGet("ByProfessor/{ProfessorId}")]
    public async Task<IActionResult> GetByProfessorId(int ProfessorId)
    {

      try
      {
        var result = await _repo.GetAlunosAsyncByProfessorId(ProfessorId, true);
        return Ok(result);
      }
      catch (System.Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
      }

    }



    [HttpPost]
    public async Task<IActionResult> post(Aluno model)
    {
      if (IsCpf(model.cpf))
      {
        if (_repo.ifExistCpf(model.cpf))
        {
          try
          {
            model.arquivo = criptografia(model.cpf);
            _repo.Add(model);
            if (await _repo.SaveChangesAsync())
            {
              return Created($"/api/aluno/{model.Id}", model);
            }

          }
          catch (System.Exception)
          {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
          }
        }
      }

      return BadRequest();

    }


    [HttpPut("{AlunoId}")]
    public async Task<IActionResult> put(int AlunoId, Aluno model)
    {
      try
      {
        string old = "";
        var aluno = await _repo.GetAlunoAsyncById(AlunoId, false);

        if (aluno == null)
        {
          return NotFound();
        }

        if (aluno.cpf == "")
        {
          return BadRequest("CPF em Branco! Preencha e tente novamente.");
        }

        //81134145098
        if (IsCpf(model.cpf))
        {
          Console.WriteLine("1 if");
          if (_repo.ifExistCpf(model.cpf))
          {
            old = aluno.arquivo;
            Console.WriteLine("2 if");
            model.arquivo = criptografia(model.cpf);
            renomearPasta(old, model.arquivo);
            _repo.Update(model);
            if (await _repo.SaveChangesAsync())
            {
              aluno = await _repo.GetAlunoAsyncById(AlunoId, true);
              return Created($"/api/aluno/{model.Id}", model);
            }
          }
          else if (aluno.cpf == model.cpf)
          {
            Console.WriteLine("Entrei no ultimo " + aluno.Id + " e " + model.Id);
            model.arquivo = criptografia(model.cpf);
            //model.cpf = aluno.cpf;
            _repo.Update(model);
            if (await _repo.SaveChangesAsync())
            {
              aluno = await _repo.GetAlunoAsyncById(AlunoId, true);
              return Created($"/api/aluno/{model.Id}", model);
            }
          }
          return BadRequest("CPF já existe");
        }

        return BadRequest("Cpf Inválido");

      }
      catch (System.Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
      }

    }


    [HttpDelete("{AlunoId}")]
    public async Task<IActionResult> delete(int AlunoId)
    {
      try
      {
        var aluno = await _repo.GetAlunoAsyncById(AlunoId, false);

        if (aluno == null)
        {
          return NotFound();
        }

        _repo.Delete(aluno);

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
      {
        Console.WriteLine("CPF com menos de 11 digitos");
        return false;
      }


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

      bool verifica = cpf.EndsWith(digito);
      Console.WriteLine("O cpf é: " + verifica);
      return verifica;
    }


    [HttpPost("upload")]
    public async Task<IActionResult> Post(IFormFile file)
    {
      try
      {
        var filePath = @"./Resources/" + file.FileName + "/a.jpg";

        if (file.Length > 0)
        {
          using (var fileStream = new FileStream(filePath, FileMode.Create))
          {
            await file.CopyToAsync(fileStream);
            return Ok("Upload Sucess");
          }
        }
        return BadRequest("Upload Failed");
      }
      catch (System.Exception)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, "Nenhuma Imagem Selecionada");
      }
    }




  }
}