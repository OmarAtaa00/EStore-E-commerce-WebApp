using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class TypesAndBrandsSpec : BaseSpecification<Product>
    {
        // Criteria is basically a where clause
        // base: calling for base constructor 
        // if the left is false execute whatever on the right of this >>> "||" or else expression 
        // after || create a query to get all the products that match brandId and typeId that we are passing

        public TypesAndBrandsSpec(ProductSpecParams productParams)
        : base(x =>
            //Search 
            (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search))
            &&
            (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId)
            &&
            (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)

        )
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
            AddOrderBy(p => p.Name);

            //ex if the page is 1 so 5 * 1 = skip first 5  don't want that
            // so -1 so it is 0 * 5 is 0 we skip nothing and get first 5
            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);


            //Checking for the sort value to match to order accordingly 
            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "PriceLow":
                        AddOrderBy(p => p.Price);
                        break;
                    case "PriceHigh":
                        AddOrderByDescending(p => p.Price);
                        break;


                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
        }

        public TypesAndBrandsSpec(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);

        }


    }
}
