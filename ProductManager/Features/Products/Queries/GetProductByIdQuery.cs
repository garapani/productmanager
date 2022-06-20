using AutoMapper;
using MediatR;
using ProductManager.Domain.Constansts;
using ProductManager.Domain.Entities;
using ProductManager.Features.Currency.Command;
using ProductManager.Features.ProductAnalytics.Commands;
using ProductManager.GenericResponse;
using ProductManager.Repositories.Interfaces;
using ProductManager.ViewModel;
using RestSharp;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ProductManager.Features.Products.Queries
{
    public class GetProductByIdQuery : IRequest<Response<ProductViewModel>>
    {
        public int Id { get; set; }
        public CurrencyType? Currency { get; set; } = CurrencyType.USD; 
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Response<ProductViewModel>>
    {
        private readonly IProductRepositoryAsync _productRepositoryAsync;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public GetProductByIdQueryHandler(IProductRepositoryAsync productRepositoryAsync, IMapper mapper, IMediator mediator)
        {
            this._productRepositoryAsync = productRepositoryAsync;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Response<ProductViewModel>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var productDetails = await _productRepositoryAsync.GetByIdAsync(request.Id);
            if (productDetails != null)
            {
                var result = await this._mediator.Send(new UpdateProductAnalyticsCommand()
                {
                    ProductId = request.Id
                });

                var productviewModel = this._mapper.Map<ProductViewModel>(productDetails);
                if (request.Currency != null && request.Currency != CurrencyType.USD)
                {
                    var convertedDetails = await this._mediator.Send(new ConvertCurrencyCommand()
                    {
                        From = CurrencyType.USD,
                        To = request.Currency.Value,
                        Price = productDetails.Price
                    });
                    if (convertedDetails != null && convertedDetails.ResultCode == ResultCode.Success)
                    {
                        productviewModel.Price = convertedDetails.Data;
                        productviewModel.Currency = request.Currency.Value.ToString();
                        return new Response<ProductViewModel>(ResultCode.Success, productviewModel);
                    }
                    else
                    {
                        return new Response<ProductViewModel>(ResultCode.InternalServerError, productviewModel, "Failed to convert the currency");
                    }
                }
                else
                {
                    productviewModel.Currency = CurrencyType.USD.ToString();
                    return new Response<ProductViewModel>(ResultCode.Success, productviewModel);
                }
            }
            else
            {
                throw new System.Exception($"Failed to find product with Id:{request.Id}");
            }
        }
    }
}
