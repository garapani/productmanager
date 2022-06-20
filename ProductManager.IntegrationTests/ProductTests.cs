using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProductManager.GenericResponse;
using ProductManager.ViewModel;
using Xunit;
using System.Linq;

namespace ProductManager.IntegrationTests
{
    [TestCaseOrderer("ProductManager.IntegrationTests.AlphabeticalOrderer", "ProductManager.IntegrationTests")]
    public class ProductTests : IClassFixture<TestFixture<Startup>>
    {
        private HttpClient Client;

        public ProductTests(TestFixture<Startup> fixture)
        {   
            Client = fixture.Client;
        }

        [Fact]
        public async Task Test1()
        {
            // Arrange
            var request = "/api/v1/Product/mostviewed";

            // Act
            var response = await Client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Test2()
        {
            // Arrange
            var request = "/api/v1/Product";
            ProductViewModel p = new ProductViewModel()
            {
                Name = "iPhone 13",
                Description = "latest iphone",
                Price= 1000,
            };
            HttpContent c = new StringContent(JsonConvert.SerializeObject(p), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync(request,c);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Test3()
        {
            // Arrange
            var request = "/api/v1/Product?Name=iPhone 13";
            ProductViewModel p = new ProductViewModel()
            {
                Name = "iPhone 13",
                Description = "latest iphone",
                Price = 1000,
            };
            
            // Act
            var response = await Client.GetStringAsync(request);
            // Assert
            var responseProduct = JsonConvert.DeserializeObject<Response<ProductViewModel>>(response);
            Assert.Equal(responseProduct.ResultCode, ResultCode.Success);
            Assert.Equal(responseProduct.Data.Name, p.Name);
        }

        [Fact]
        public async Task Test4()
        {
            // Arrange
            var request = "/api/v1/Product?Name=iPhone 13";            
            // Act
            var response = await Client.DeleteAsync(request);
            // Assert
            var responseProduct = JsonConvert.DeserializeObject<Response<ProductViewModel>>(await response.Content.ReadAsStringAsync());
            Assert.Equal(responseProduct.ResultCode, ResultCode.Success);

            var request1 = "/api/v1/Product?Name=iPhone 13";
            ProductViewModel p = new ProductViewModel()
            {
                Name = "iPhone 13",
                Description = "latest iphone",
                Price = 1000,
            };

            // Act
            var response1 = await Client.GetAsync(request);
            // Assert
            var responseProduct1 = JsonConvert.DeserializeObject<Response<ProductViewModel>>(await response1.Content.ReadAsStringAsync());
            Assert.Equal(responseProduct1.ResultCode, ResultCode.BusinessLogicError);
            Assert.Equal(responseProduct1.Data, null);
        }

        [Fact]
        public async Task Test5()
        {
            // Arrange
            var request = "/api/v1/Product";
            ProductViewModel p = new ProductViewModel()
            {
                Name = "iPhone 14",
                Description = "latest iphone",
                Price = 1000,
            };
            HttpContent c = new StringContent(JsonConvert.SerializeObject(p), Encoding.UTF8, "application/json");

            // Act
            var response = await Client.PostAsync(request, c);
            // Assert
            response.EnsureSuccessStatusCode();

            var request1 = "/api/v1/Product?Name=iPhone 14";

            // Act
            var response1 = await Client.GetStringAsync(request1);
            // Assert
            var responseProduct1 = JsonConvert.DeserializeObject<Response<ProductViewModel>>(response1);
            Assert.Equal(responseProduct1.ResultCode, ResultCode.Success);
            Assert.Equal(responseProduct1.Data.Name, p.Name);

            // Act
            var response2 = await Client.GetStringAsync(request1);
            // Assert
            var responseProduct2 = JsonConvert.DeserializeObject<Response<ProductViewModel>>(response1);
            Assert.Equal(responseProduct2.ResultCode, ResultCode.Success);
            Assert.Equal(responseProduct2.Data.Name, p.Name);

            // Arrange
            var request3 = "/api/v1/Product/mostviewed";
            // Act
            var response3 = await Client.GetStringAsync(request3);
            var responseProduct3 = JsonConvert.DeserializeObject<Response<ICollection<ProductViewModel>>>(response3);
            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(responseProduct3.ResultCode, ResultCode.Success);
            Assert.NotEqual(responseProduct3.Data,null);
            Assert.Equal(responseProduct3.Data.FirstOrDefault().ViewCount, 2);
        }
    }
}
