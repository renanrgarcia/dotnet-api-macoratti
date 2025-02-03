using CleanArch.Application.Members.Commands;
using CleanArch.Application.Members.Queries;
using CleanArch.Domain.Abstractions;
using CleanArch.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MembersController(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMember(int id)
        {
            var query = new GetMembersByIdQuery { Id = id };
            var member = await _mediator.Send(query);
            return member != null ? Ok(member) : NotFound("Member not found");
        }

        [HttpGet]
        public async Task<IActionResult> GetMembers()
        {
            var query = new GetMembersQuery();
            var members = await _mediator.Send(query);
            return Ok(members);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberCommand command)
        {
            var createdMember = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetMember), new { id = createdMember.Id }, createdMember);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(int id, UpdateMemberCommand command)
        {
            command.Id = id;

            var updatedMember = await _mediator.Send(command);

            return updatedMember != null ? Ok(updatedMember) : NotFound("Member not found");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var deletedMember = await _mediator.Send(new DeleteMemberCommand { Id = id });

            return deletedMember != null ? Ok(deletedMember) : NotFound("Member not found");
        }
    }
}