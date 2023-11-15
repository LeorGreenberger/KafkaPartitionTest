using KafkaPartitionTest.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddFilter("Silverback", LogLevel.Trace);
builder.Services.AddMessaging();
var app = builder.Build();

await app.RunAsync();
