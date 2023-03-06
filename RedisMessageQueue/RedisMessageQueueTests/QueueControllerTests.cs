using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using RedisMessageQueue.Controllers;
using RedisMessageQueue.Infrastructure;
using StackExchange.Redis;

namespace RedisMessageQueueTests
{
    public class QueueControllerTests
    {
        public QueueControllerTests()
        {
            var config = InitConfiguration();

            var redisCache = ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                EndPoints = { config.GetValue<string>("RedisSettings:ConnectionString") },
                AllowAdmin = true
            });

            // clear the server for testing
            var server = redisCache.GetServer(redisCache.GetEndPoints().FirstOrDefault());
            server.FlushAllDatabases();

            var queueRepository = new QueueRepository(redisCache);
            _queueController = new QueueController(queueRepository);
        }

        [Test, Order(1)]
        public async Task TestPushMessageAsync()
        {
            // act
            var result = await _queueController.PushAsync(TestMessage);
            var statusCodeActionResult = result as IStatusCodeActionResult;

            // assert
            Assert.IsNotNull(statusCodeActionResult);
            Assert.That(statusCodeActionResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test, Order(2)]
        public async Task TestCountAsync()
        {
            // act
            var result = await _queueController.CountAsync();
            var statusCodeActionResult = result as IStatusCodeActionResult;
            var valueResult = result as OkObjectResult;

            // assert
            Assert.IsNotNull(statusCodeActionResult);
            Assert.IsNotNull(valueResult);
            Assert.That(statusCodeActionResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(Convert.ToInt32(valueResult.Value), Is.EqualTo(1));
        }

        [Test]
        public async Task TestPopAsync()
        {
            // act
            var result = await _queueController.PopAsync();
            var statusCodeActionResult = result as IStatusCodeActionResult;
            var valueResult = result as OkObjectResult;

            // assert
            Assert.IsNotNull(statusCodeActionResult);
            Assert.IsNotNull(valueResult);
            Assert.That(statusCodeActionResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(valueResult.Value, Is.EqualTo(TestMessage));
        }

        private QueueController _queueController;
        private const string TestMessage = "This is a test message";

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.test.json")
                .AddEnvironmentVariables()
                .Build();

            return config;
        }
    }
}