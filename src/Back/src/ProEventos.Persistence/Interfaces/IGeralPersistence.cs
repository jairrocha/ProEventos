namespace ProEventos.Persistence.Interfaces;

public interface IGeralPersistence
{

   
    void Add<T>(T entity) where T : class;
    void Update<T>(T entity) where T : class;
    void Delete<T>(T entity) where T : class;
    void DeleteRange<T>(T entityArray) where T : class;
    Task<bool> SaveChangesAsync();

}
