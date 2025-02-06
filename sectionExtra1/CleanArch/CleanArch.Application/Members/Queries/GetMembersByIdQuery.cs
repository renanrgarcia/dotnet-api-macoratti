using System;
using CleanArch.Domain.Abstractions;
using CleanArch.Domain.Entities;
using MediatR;

namespace CleanArch.Application.Members.Queries;

public class GetMembersByIdQuery : IRequest<Member>
{
    public int Id { get; set; }
}

public class GetMembersByIdQueryHandler : IRequestHandler<GetMembersByIdQuery, Member>
{
    private readonly IMemberDapperRepository _memberDapperRepository;

    public GetMembersByIdQueryHandler(IMemberDapperRepository memberDapperRepository)
    {
        _memberDapperRepository = memberDapperRepository;
    }

    public async Task<Member> Handle(GetMembersByIdQuery request, CancellationToken cancellationToken)
    {
        var member = await _memberDapperRepository.GetMemberById(request.Id);
        return member;
    }
}