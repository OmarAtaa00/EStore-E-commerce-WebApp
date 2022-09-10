using System;
using System.Linq;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery;


            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria); //ex  p => p.ProductTypeId == id;  
            }
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }
            //Paging must be after filtering and sorting other wise we would page the result then sort it "not right "
            if (spec.IsPaginationEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }



            //Current represent entity, include represent Expression
            //To replace the Include(p => p.productType etc) take expressions and aggregate them to query into database
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }

    }
}
