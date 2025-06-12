namespace Service
{
    public class ProductService(IUnitOfWork _unitOfWork , IMapper _mapper) : IProductService
    {
       
        public async Task<PaginatedResult<ProductDto>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<Product, int>();
            var Specification = new ProductWithBrandAndTypeSpecifications(queryParams);
            var AllProducts = await repo.GetAllAsync(Specification);
            var Data = _mapper.Map<IEnumerable<Product> ,IEnumerable <ProductDto>>(AllProducts);
            var ProductCount = AllProducts.Count();
            var totalCount = await repo.CountAsync(new ProductCountSpecification(queryParams));
            return new PaginatedResult<ProductDto>
            (
                queryParams.PageNumber,
                ProductCount,
                totalCount,
                Data
            );
        }

        public async Task<ProductDto> GetProductAsync(int id)
        {
            var specification = new ProductWithBrandAndTypeSpecifications(id);
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(specification);
            if (product is null)
                throw new ProductNotFoundException(id);

            return _mapper.Map<Product, ProductDto>(product);
        }

        public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
        {
            var repo = _unitOfWork.GetRepository<ProductType, int>();
            var types = await repo.GetAllAsync();
            var TypeDto = _mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDto>>(types);
            return TypeDto;
        }

        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
            var repo = _unitOfWork.GetRepository<ProductBrand, int>();
            var Brands = await repo.GetAllAsync();
            var BrandDto = _mapper.Map<IEnumerable<ProductBrand> , IEnumerable<BrandDto>>(Brands);
            return BrandDto;
        }
        
    }
}
