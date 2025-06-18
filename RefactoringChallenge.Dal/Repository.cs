using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RefactoringChallenge.Dal.Abstractions;
using RefactoringChallenge.Domain;

namespace RefactoringChallenge.Dal;

public class Repository<T>(RefactoringChallengeDbContext context) : IRepository<T> where T : Entity
{
    public Task<List<T>> ItemsAsync(Expression<Func<T,bool>> predicate)
    {
        return context.Set<T>().Where(predicate).ToListAsync();
    }

    public Task<T?> FindAsync(int id)
    {
        return context.Set<T>().SingleOrDefaultAsync(x => x.Id == id);
    }

    public Task SaveAsync(T item)
    {
        context.Update(item);
        return context.SaveChangesAsync();
    }
}
