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
    public class DeleteProductByIdCommand : IRequest<Response<ProductViewModel>>
    {
        public int Id { get; set; }
    }

    public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, Response<ProductViewModel>>
    {
        private readonly IProductRepositoryAsync _productRepositoryAsync;
        private readonly IMapper _mapper;
        public DeleteProductByIdCommandHandler(IProductRepositoryAsync productRepositoryAsync, IMapper mapper)
        {
            _productRepositoryAsync = productRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<ProductViewModel>> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
        {
            var productsToDelete = await _productRepositoryAsync.FindByConditionAsync(o => o.Id == request.Id, "Analytics");
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
                throw new System.Exception($"Failed to delete a product, {request.Id}");
            }
        }
    }
}
