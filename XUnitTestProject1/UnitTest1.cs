using Microsoft.EntityFrameworkCore;
using System;
using WebApplication.Controllers;
using WebApplication.DTO;
using WebApplication.Models;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        [Fact]
        public async void Test1()
        {
            var builder = new DbContextOptionsBuilder<DataBaseContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            using (var context = new DataBaseContext(builder.Options))
            {
                AdvertisementsController advertisementsController = new AdvertisementsController(context);

                var request = await advertisementsController.GetAdvertisement("AFD45B0-0B0C-4A1B-A0D0-012EC2301C7A");

                Assert.Equal("fawwew", request.Value.Title);
            }
        }
    }
}
