using GSOP.Domain.Contracts;
using GSOP.Domain.Contracts.FilmTypes;
using GSOP.Domain.Contracts.FilmTypes.Models;
using GSOP.Infrastructure.DataAccess.Customers;
using LinqToDB;

namespace GSOP.Infrastructure.DataAccess.FilmTypes;

/// <inheritdoc/>
public class FilmTypeRepository : IFilmTypeRepository
{
    private readonly DatabaseConnection _connection;

    public FilmTypeRepository(DatabaseConnection connection)
    {
        _connection = connection;
    }

    /// <inheritdoc/>
    public Task<long> Create(IFilmType filmType)
    {
        return _connection.InsertWithInt64IdentityAsync(new CustomerPOCO() { Name = filmType.Article });
    }

    /// <inheritdoc/>
    public async Task<bool> Delete(ID id)
    {
        return await _connection.FilmTypes
            .Where(x => x.ID == id)
            .DeleteAsync() == 1;
    }

    /// <inheritdoc/>
    public Task<FilmTypeDTO?> Get(ID id)
    {
        return _connection.FilmTypes
            .Where(x => x.ID == id)
            .Select(x => new FilmTypeDTO { Article = x.Article })
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<FilmTypeInfo>> GetInfos()
    {
        return await _connection.FilmTypes
            .Select(x => new FilmTypeInfo { ID = x.ID, Name = x.Article })
            .ToListAsync();
    }

    /// <inheritdoc/>
    public Task Update(ID id, IFilmType filmType)
    {
        return _connection.FilmTypes
            .Where(x => x.ID == id)
            .Set(x => x.Article, filmType.Article)
            .UpdateAsync();
    }

    /// <inheritdoc/>
    public Task<bool> IsArticleExsits(FilmTypeArticle filmTypeArticle)
    {
        return _connection.FilmTypes
            .Where(x => x.Article == filmTypeArticle)
            .AnyAsync();
    }
}
