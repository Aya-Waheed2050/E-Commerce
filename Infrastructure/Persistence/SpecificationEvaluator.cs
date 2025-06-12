using DomainLayer.Contracts;
using DomainLayer.Models.ProductModule;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Persistence
{
    static internal class SpecificationEvaluator
    {

        static public IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> InputQuery , ISpecifications<TEntity, TKey> Specifications)
            where TEntity : BaseEntity<TKey>
        {

            var Query = InputQuery;

            if (Specifications.Criteria is not null)
                Query = Query.Where(Specifications.Criteria);

            if(Specifications.OrderBy is not null)
                Query = Query.OrderBy(Specifications.OrderBy);

            if(Specifications.OrderByDescending is not null)
                Query = Query.OrderByDescending(Specifications.OrderByDescending);

            if (Specifications.IncludeExpression is not null && Specifications.IncludeExpression.Count > 0)
            {
                Query = Specifications.IncludeExpression.Aggregate(Query, (currentQuery , IncludeExp) => currentQuery.Include(IncludeExp));
            }
            
            if(Specifications.IsPaginated)
            {
                Query = Query.Skip(Specifications.Skip).Take(Specifications.Take);
            }

            return Query;
        } 


    }
}
