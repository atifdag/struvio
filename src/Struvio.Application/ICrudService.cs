namespace Struvio.Application;

/// <summary>
/// CRUD işlemleri yapan sınıflar için arayüz
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICrudService<T> : ITransientDependency where T : class, ICrudModel, new()
{

    /// <summary>
    /// Filtreleme yaparak birden çok satır içeren liste modelini döndürür.
    /// </summary>
    /// <param name="filterModel">Filtreleme için Sınıf</param>
    /// <returns>T türünden liste modeli</returns>
    Task<ListModel<T>> GetAllAsync(FilterModel filterModel, CancellationToken cancellationToken = default);

    /// <summary>
    /// ID parametresi alarak tek satır içeren detay modelini döndürür.
    /// </summary>
    /// <param name="id">ID parametresi</param>
    /// <returns></returns>
    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ekleme yaparak sonucu modelle döndürür.
    /// </summary>
    /// <param name="addModel"></param>
    /// <returns>Ekleme işlemi sonucu oluşan model</returns>
    Task<T> CreateAsync(T model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Güncelleme yaparak sonucu modelle döndürür.
    /// </summary>
    /// <param name="model"></param>
    /// <returns>Güncelleme işlemi sonucu oluşan model</returns>
    Task<T> UpdateAsync(Guid id, T model, CancellationToken cancellationToken = default);

    /// <summary>
    /// ID parametresi alarak silme işlemini yapar.
    /// </summary>
    /// <param name="id">ID parametresi</param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

}
