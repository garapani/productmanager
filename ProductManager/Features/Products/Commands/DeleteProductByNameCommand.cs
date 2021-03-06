using AutoMapper;
using MediatR;
using ProductManager.Domain.Entities;
using ProductManager.GenericResponse;
using ProductManager.Repositories.Interfaces;
using ProductManager.ViewModel;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace ProductManager.Features.Products.Commands
{
    public class DeleteProductByNameCommand : IRequest<Response<ProductViewModel>>
    {
        public string Name { get; set; }
    }

    public class DeleteProductByNameCommandHandler : IRequestHandler<DeleteProductByNameCommand, Response<ProductViewModel>>
    {
        private readonly IProductRepositoryAsync _productRepositoryAsync;
        private readonly IMapper _mapper;
        public DeleteProductByNameCommandHandler(IProductRepositoryAsync productRepositoryAsync, IMapper mapper)
        {
            _productRepositoryAsync = productRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<ProductViewModel>> Handle(DeleteProductByNameCommand request, CancellationToken cancellationToken)
        {
            var productsToDelete = await _productRepositoryAsync.FindByConditionAsync(o => o.Name == request.Name,"Analytics");
            if (productsToDelete?.Count() > 0)
            {
                var productToDelete = productsToDelete.FirstOrDefault();
                productToDelete.IsDeleted = true;
                productToDelete.Analytics.ViewCount = 0;
                await _productRepositoryAsync.UpdateAsync(productToDelete);
                return new Response<ProductViewModel>(ResultCode.Success, this._mapper.Map<ProductViewModel>(productToDelete));
            }
            else
            {
                throw new System.Exception($"Failed to delete a product, {request.Name}");
            }
        }
    }
}
