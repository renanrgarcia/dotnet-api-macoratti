using System;
using CleanArch.Domain.Abstractions;
using CleanArch.Infrastructure.Context;

namespace CleanArch.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private IMemberRepository? _memberRepository;
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IMemberRepository MemberRepository
    {
        get
        {
            return _memberRepository = _memberRepository ??
                new MemberRepository(_context);
        }
    }

    public Task CommitAsync()
    {
        return _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
