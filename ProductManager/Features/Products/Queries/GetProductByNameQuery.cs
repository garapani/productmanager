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
using System.Linq;

namespace ProductManager.Features.Products.Queries
{
    public class GetProductByNameQuery : IRequest<Response<ProductViewModel>>
    {
        public string Name { get; set; }
        public CurrencyType? Currency { get; set; } = CurrencyType.USD; 
    }

    public class GetProductByNameQueryHandler : IRequestHandler<GetProductByNameQuery, Response<ProductViewModel>>
    {
        private readonly IProductRepositoryAsync _productRepositoryAsync;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public GetProductByNameQueryHandler(IProductRepositoryAsync productRepositoryAsync, IMapper mapper, IMediator mediator)
        {
            this._productRepositoryAsync = productRepositoryAsync;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Response<ProductViewModel>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                throw new System.Exception("Name is required");
            }
            var productsDetails = await _productRepositoryAsync.FindByConditionAsync(o => o.Name.ToLower() ==  request.Name.ToLower());
            if (productsDetails?.Count() > 0)
            {
                var product = productsDetails.FirstOrDefault();
                if (product != null)
                {
                    var result = await this._mediator.Send(new UpdateProductAnalyticsCommand()
                    {
                        ProductId = product.Id
                    });

                    var productviewModel = this._mapper.Map<ProductViewModel>(product);
                    if (request.Currency != null && request.Currency != CurrencyType.USD)
                    {
                        var convertedDetails = await this._mediator.Send(new ConvertCurrencyCommand()
                        {
                            From = CurrencyType.USD,
                            To = request.Currency.Value,
                            Price = product.Price
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
                    throw new System.Exception($"Failed to find product with Name:{request.Name}");
                }
            }
            else
            {
                throw new System.Exception($"Failed to find product with Name:{request.Name}");
            }
        }
    }
}
