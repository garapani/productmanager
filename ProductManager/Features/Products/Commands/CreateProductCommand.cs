using AutoMapper;
using MediatR;
using ProductManager.Domain.Entities;
using ProductManager.GenericResponse;
using ProductManager.Repositories.Interfaces;
using ProductManager.ViewModel;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace ProductManager.Features.Products.Commands
{
    public class CreateProductCommand : IRequest<Response<ProductViewModel>>
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Response<ProductViewModel>>
    {
        private readonly IProductRepositoryAsync _productRepositoryAsync;
        private readonly IMapper _mapper;
        public CreateProductCommandHandler(IProductRepositoryAsync productRepositoryAsync, IMapper mapper)
        {
            _productRepositoryAsync = productRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<ProductViewModel>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<CreateProductCommand, Product>(request);
            var existingProducts = await _productRepositoryAsync.FindByConditionAsync(o => o.Name.ToLower() == request.Name.ToLower());
            if (existingProducts?.Count() > 0)
            {
                throw new System.Exception($"Product is already exists, {product.Name}");
            }
            var addedProduct = await _productRepositoryAsync.AddAsync(product);
            if (addedProduct != null)
            {
                return new Response<ProductViewModel>(ResultCode.Success, this._mapper.Map<ProductViewModel>(addedProduct));
            }
            else
            {
                throw new System.Exception($"Failed to add a product, {product.Name}");
            }
        }
    }
}
