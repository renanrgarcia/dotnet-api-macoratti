using AlunosApi.Models;
using AlunosApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlunosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Produces("application/json")]
    public class AlunosController : ControllerBase
    {
        private IAlunoService _alunoService;
        public AlunosController(IAlunoService alunoService)
        {
            _alunoService = alunoService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAlunos()
        {
            try
            {
                var alunos = await _alunoService.GetAlunos();
                return Ok(alunos);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter alunos");
            }
        }

        [HttpGet("alunosPorNome")]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>>
            GetAlunosByName([FromQuery] string nome)
        {
            try
            {
                var alunos = await _alunoService.GetAlunosByNome(nome);
                if (alunos == null)
                    return NotFound($"Não não existe alunos com o critério: {nome}");
                return Ok(alunos);
            }
            catch
            {
                return BadRequest("Requisição inválida");
            }
        }

        [HttpGet("{id:int}", Name = "GetAlunoById")]
        public async Task<ActionResult<Aluno>> GetAlunoById(int id)
        {
            try
            {
                var aluno = await _alunoService.GetAlunoById(id);
                if (aluno == null)
                    return NotFound($"Não foi encontrado um aluno com o id: {id}");
                return Ok(aluno);
            }
            catch
            {
                return BadRequest("Requisição inválida");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateAluno(Aluno aluno)
        {
            try
            {
                await _alunoService.CreateAluno(aluno);
                return CreatedAtRoute(nameof(GetAlunoById), new { id = aluno.Id }, aluno);
            }
            catch
            {
                return BadRequest("Requisição inválida");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateAluno(int id, [FromBody] Aluno aluno)
        {
            try
            {
                if (id != aluno.Id)
                    return BadRequest("Dados do id inválidos");
                await _alunoService.UpdateAluno(aluno);
                return Ok(aluno);
            }
            catch
            {
                return BadRequest("Requisição inválida");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAluno(int id)
        {
            try
            {
                var aluno = await _alunoService.GetAlunoById(id);
                if (aluno == null)
                    return NotFound($"Não foi encontrado um aluno com o id: {id}");
                await _alunoService.DeleteAluno(aluno);
                return Ok(aluno);
            }
            catch
            {
                return BadRequest("Requisição inválida");
            }
        }
    }
}