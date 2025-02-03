using CleanArch.Domain.Abstractions;
using CleanArch.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MembersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetMembers()
        {
            var members = await _unitOfWork.MemberRepository.GetMembers();
            return Ok(members);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMemberById(int id)
        {
            var member = await _unitOfWork.MemberRepository.GetMemberById(id);
            return member != null ? Ok(member) : NotFound("Member not found");
        }

        [HttpPost]
        public async Task<IActionResult> AddMember(Member member)
        {
            if (member == null)
                return BadRequest("Invalid member data.");

            var newMember = await _unitOfWork.MemberRepository.AddMember(member);
            await _unitOfWork.CommitAsync();
            return CreatedAtAction(nameof(GetMemberById), new { id = newMember.Id }, newMember);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(int id, Member member)
        {
            var memberToUpdate = await _unitOfWork.MemberRepository.GetMemberById(id);
            if (memberToUpdate == null)
                return NotFound("Member not found");

            memberToUpdate.Update(member.FirstName, member.LastName, member.Gender, member.Email, member.IsActive);
            _unitOfWork.MemberRepository.UpdateMember(memberToUpdate);
            await _unitOfWork.CommitAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var deletedMember = await _unitOfWork.MemberRepository.DeleteMember(id);
            if (deletedMember == null)
                return NotFound("Member not found");

            await _unitOfWork.CommitAsync();
            return Ok();
        }
    }
}
