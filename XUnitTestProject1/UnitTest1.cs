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
                .EnableSensitiveDataLogging();

            using (var context = new DataBaseContext(builder.Options))
            {
                AdvertisementsController advertisementsController = new AdvertisementsController(context);

                var request = await advertisementsController.GetAdvertisement("a3f01e0a-b424-4493-a676-2446e340f151");

                //Assert.Equal(4, request);
            }
        }
    }
}
