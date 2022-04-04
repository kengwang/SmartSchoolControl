using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SmartSchoolControl.Common.Base.Abstractions;

namespace SmartSchoolControl.Client.Db;

public sealed class DbRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
{
    private DbSet<TEntity> Table => _context.Set<TEntity>();
    private readonly ClientDbContext _context;

    public DbRepository(ClientDbContext context)
    {
        _context = context;
    }


    public TEntity Insert(TEntity entity)
    {
        var newEntity = Table.Add(entity);
        _context.SaveChanges();
        return newEntity.Entity;
    }

    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        var newEntity = await Table.AddAsync(entity);
        await _context.SaveChangesAsync();
        return newEntity.Entity;
    }

    public void Delete(TEntity entity)
    {
        AttachIfNot(entity);
        Table.Remove(entity);
        _context.SaveChanges();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        AttachIfNot(entity);
        Table.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public void Delete(Expression<Func<TEntity, bool>> predicate)
    {
        // This will cause a cast
        Table.Where(predicate).ToList().ForEach(Delete);
    }

    public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        await Table.Where(predicate).ForEachAsync(Delete);
    }

    public TEntity Update(TEntity entity)
    {
        AttachIfNot(entity);
        _context.Entry(entity).State = EntityState.Modified;
        Save();
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        AttachIfNot(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await SaveAsync();

        return entity;
    }

    public void UpdateRange(List<TEntity> entities)
    {
        AttachIfNot(entities);
        entities.ForEach(t=>_context.Entry(t).State = EntityState.Modified);
        _context.UpdateRange(entities);
        _context.SaveChanges();
    }

    public async Task UpdateRangeAsync(List<TEntity> entities)
    {
        AttachIfNot(entities);
        entities.ForEach(t=>_context.Entry(t).State = EntityState.Modified);
        _context.UpdateRange(entities);
        await _context.SaveChangesAsync();
    }

    public IQueryable<TEntity> GetAll()
    {
        return Table;
    }

    public List<TEntity> GetAllList()
    {
        return Table.ToList();
    }

    public async Task<List<TEntity>> GetAllListAsync()
    {
        return await Table.ToListAsync();
    }

    public List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
    {
        return Table.Where(predicate).ToList();
    }

    public async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Table.Where(predicate).ToListAsync();
    }

    public TEntity First(Expression<Func<TEntity, bool>> predicate)
    {
        return Table.First(predicate);
    }

    public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Table.FirstAsync(predicate);
    }

    public TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return Table.FirstOrDefault(predicate);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Table.FirstOrDefaultAsync(predicate);
    }

    public int Count()
    {
        return Table.Count();
    }

    public async Task<int> CountAsync()
    {
        return await Table.CountAsync();
    }

    public int Count(Expression<Func<TEntity, bool>> predicate)
    {
        return Table.Count(predicate);
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Table.CountAsync(predicate);
    }

    public long CountLong()
    {
        return Table.LongCount();
    }

    public async Task<long> CountLongAsync()
    {
        return await Table.LongCountAsync();
    }

    public long CountLong(Expression<Func<TEntity, bool>> predicate)
    {
        return Table.LongCount(predicate);
    }

    public async Task<long> CountLongAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Table.LongCountAsync(predicate);
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    private void AttachIfNot(TEntity entity)
    {
        var entry = _context.ChangeTracker.Entries()
            .FirstOrDefault(ent => ent.Entity == entity);

        if (entry != null)
        {
            return;
        }

        Table.Attach(entity);
    }

    private void AttachIfNot(IEnumerable<TEntity> entities)
    {
        var entry = _context.ChangeTracker.Entries()
            .Where(ent => entities.Contains(ent.Entity));

        if (!entry.Any())
        {
            return;
        }

        Table.AttachRange(entities);
    }
}