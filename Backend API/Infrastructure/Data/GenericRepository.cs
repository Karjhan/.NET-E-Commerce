﻿using Backend_API.DataContexts;
using Backend_API.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private StoreContext _context { get; }

    public GenericRepository(StoreContext context)
    {
        _context = context;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }
    
    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetEntityWithSpecification(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<T>> ListAllAsync(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).ToListAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> specification)
    {
        return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification);
    }
}