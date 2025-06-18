using System.Linq.Expressions;

namespace RefactoringChallenge.Dal.Abstractions;

public interface IRepository<T> where T : class
{
    Task<List<T>> ItemsAsync(Expression<Func<T,bool>> predicate);
    Task<T?> FindAsync(int id);
    Task SaveAsync(T item);
}
