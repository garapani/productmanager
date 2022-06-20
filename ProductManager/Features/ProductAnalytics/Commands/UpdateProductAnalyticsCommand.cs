using MediatR;
using ProductManager.Repositories.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace ProductManager.Features.ProductAnalytics.Commands
{
    public class UpdateProductAnalyticsCommand : IRequest<bool>
    {
        public int ProductId { get; set; }
    }

    public class UpdateProductAnalyticsCommandHandler : IRequestHandler<UpdateProductAnalyticsCommand, bool>
    {
        private readonly IProductAnalyticsRepositoryAsync _productAnalyticsRepositoryAsync;
        public UpdateProductAnalyticsCommandHandler(IProductAnalyticsRepositoryAsync productAnalyticsRepositoryAsync)
        {
            this._productAnalyticsRepositoryAsync = productAnalyticsRepositoryAsync;
        }

        public async Task<bool> Handle(UpdateProductAnalyticsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var foundAnalytics = (await this._productAnalyticsRepositoryAsync.FindByConditionAsync(o => o.ProductId == request.ProductId).ConfigureAwait(false)).FirstOrDefault();
                if (foundAnalytics != null)
                {
                    foundAnalytics.ViewCount += 1;
                    await this._productAnalyticsRepositoryAsync.UpdateAsync(foundAnalytics);
                }
                else
                {
                    Domain.Entities.ProductAnalytics productAnalytics = await this._productAnalyticsRepositoryAsync.AddAsync(new Domain.Entities.ProductAnalytics()
                    {
                        ProductId = request.ProductId,
                        ViewCount = 1,
                    });
                }
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
