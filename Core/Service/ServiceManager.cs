namespace Service
{
    public class ServiceManager(IUnitOfWork _unitOfWork , IMapper _mapper , IBasketRepositories _basketRepositories ,
           UserManager<ApplicationUser> _userManager , IConfiguration _configuration) 
        
    {
        private readonly Lazy<IProductService> _lazyProductService = 
            new Lazy<IProductService>(() => new ProductService(_unitOfWork , _mapper));

        private readonly Lazy<IBasketService> _lazyBasketService =
            new Lazy<IBasketService>(() => new BasketService(_basketRepositories , _mapper));

        private readonly Lazy<IAuthenticationService> _lazyAuthenticationService =
            new Lazy<IAuthenticationService>(() => new AuthenticationService(_userManager , _configuration , _mapper));

        private readonly Lazy<IOrderService> _lazyOrderService = 
            new Lazy<IOrderService>(() => new OrderService(_mapper , _basketRepositories ,_unitOfWork));

        public IProductService ProductService => _lazyProductService.Value;

        public IBasketService BasketService => _lazyBasketService.Value;

        public IAuthenticationService authenticationService => _lazyAuthenticationService.Value;

        public IOrderService OrderService => _lazyOrderService.Value;
    }
}
