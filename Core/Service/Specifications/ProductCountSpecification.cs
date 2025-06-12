namespace Service.Specifications
{
    internal class ProductCountSpecification : BaseSpecifications<Product ,int>
    {

        public ProductCountSpecification(ProductQueryParams queryParams): base 
            (p =>
                (!queryParams.TypeId.HasValue || p.TypeId == queryParams.TypeId) &&
                (!queryParams.BrandId.HasValue || p.BrandId == queryParams.BrandId) &&
                (string.IsNullOrWhiteSpace(queryParams.Search) || p.Name.ToLower().Contains(queryParams.Search.ToLower()))
            )
        {

        }

    }
}
