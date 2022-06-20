using AutoMapper;
using MediatR;
using ProductManager.Domain.Constansts;
using ProductManager.Domain.Entities;
using ProductManager.Features.Currency.Command;
using ProductManager.GenericResponse;
using ProductManager.Repositories.Interfaces;
using ProductManager.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductManager.Features.Products.Queries
{
    public class GetMostViewedProductsQuery : IRequest<Response<ICollection<ProductViewModel>>>
    {
        public int Count { get; set; } = 5;
        public CurrencyType? Currency { get; set; } = CurrencyType.USD;
    }

    public class GetMostViewedProductsQueryHandler : IRequestHandler<GetMostViewedProductsQuery, Response<ICollection<ProductViewModel>>>
    {
        private readonly IProductRepositoryAsync _productRepositoryAsync;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public GetMostViewedProductsQueryHandler(IProductRepositoryAsync productRepositoryAsync, IMapper mapper, IMediator mediator)
        {
            _productRepositoryAsync = productRepositoryAsync;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Response<ICollection<ProductViewModel>>> Handle(GetMostViewedProductsQuery request, CancellationToken cancellationToken)
        {
            var products = _mapper.Map<ICollection<ProductViewModel>>(await this._productRepositoryAsync.GetMostViewedProductsAsync(request.Count));
            if (products?.Count > 0)
            {
                if (request.Currency != null && request.Currency != CurrencyType.USD)
                {
                    foreach (var product in products.ToList())
                    {
                        var convertedDetails = await this._mediator.Send(new ConvertCurrencyCommand()
                        {
                            From = CurrencyType.USD,
                            To = request.Currency.Value,
                            Price = product.Price
                        });
                        if (convertedDetails != null && convertedDetails.ResultCode == ResultCode.Success)
                        {
                            product.Price = convertedDetails.Data;
                            product.Currency = request.Currency.Value.ToString();
                        }
                        else
                        {
                            throw new System.Exception($"failed to convert the currency for product:{product.Name}");
                        }
                    }
                }
                else
                {
                    products.ToList().ForEach(p =>
                    {
                        p.Currency = CurrencyType.USD.ToString();
                    });
                }
                return new Response<ICollection<ProductViewModel>>(ResultCode.Success, products);
            }
            else
            {
                return new Response<ICollection<ProductViewModel>>(ResultCode.Success, new List<ProductViewModel>());
            }
        }
    }
}
