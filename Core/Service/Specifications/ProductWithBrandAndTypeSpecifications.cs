namespace Service.Specifications
{
    internal class ProductWithBrandAndTypeSpecifications : BaseSpecifications<Product , int>
    {

        public ProductWithBrandAndTypeSpecifications(ProductQueryParams queryParams) : base
            ( p =>
                (!queryParams.TypeId.HasValue || p.TypeId == queryParams.TypeId) &&
                (!queryParams.BrandId.HasValue || p.BrandId == queryParams.BrandId) &&
                (string.IsNullOrWhiteSpace(queryParams.Search) || p.Name.ToLower().Contains(queryParams.Search.ToLower()))
            )
        {
            AddInclude(p => p.productType);
            AddInclude(p => p.productBrand);

            switch (queryParams.Sort)
            {
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDescending(p => p.Price);
                    break;
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDescending(p => p.Name);
                    break;
                default:
                    break;
            }

            ApplyPagination(queryParams.PageSize, queryParams.PageNumber);
        }
        public ProductWithBrandAndTypeSpecifications(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.productType);
            AddInclude(p => p.productBrand);
        }

    }
}
