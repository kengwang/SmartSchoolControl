using System.Linq.Expressions;

namespace SmartSchoolControl.Common.Base.Abstractions;

/// <summary>
/// 仓储增删改查接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IRepository<TEntity, TKey> where TEntity : class
{
    #region Insert

    public TEntity Insert(TEntity entity);
    public Task<TEntity> InsertAsync(TEntity entity);

    #endregion
    #region Delete

    public void Delete(TEntity entity);
    public Task DeleteAsync(TEntity entity);

    public void Delete(Expression<Func<TEntity, bool>> predicate);
    public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

    #endregion
    #region Update

    public TEntity Update(TEntity entity);
    public Task<TEntity> UpdateAsync(TEntity entity);

    public void UpdateRange(List<TEntity> entities);
    public Task UpdateRangeAsync(List<TEntity> entities);

    #endregion
    #region Select

    public IQueryable<TEntity> GetAll();

    public List<TEntity> GetAllList();
    public Task<List<TEntity>> GetAllListAsync();
    
    public List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate);
    public Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);
    
    public TEntity First(Expression<Func<TEntity, bool>> predicate);
    public Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate);
    
    public TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

    #endregion
    #region Count

    public int Count();
    public Task<int> CountAsync();
    public int Count(Expression<Func<TEntity, bool>> predicate);
    public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
    
    public long CountLong();
    public Task<long> CountLongAsync();
    public long CountLong(Expression<Func<TEntity, bool>> predicate);
    public Task<long> CountLongAsync(Expression<Func<TEntity, bool>> predicate);
    
    #endregion
    #region Save

    public void Save();
    public Task SaveAsync();
    
    #endregion
}