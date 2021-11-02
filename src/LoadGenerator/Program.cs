using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Presence.LoadGenerator;
using Presence.Shared;

Console.Title = "LoadGenerator";

await Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    // this hosted service connects and disconnects from the cluster along with the host
                    // it also exposes the cluster client to other services that request it
                    services.AddSingleton<ClusterClientHostedService>();
                    services.AddSingleton<IHostedService>(_ => _.GetService<ClusterClientHostedService>());
                    services.AddSingleton(_ => _.GetService<ClusterClientHostedService>().Client);

                    // this hosted service run the load generation using the available cluster client
                    services.AddSingleton<IHostedService, LoadGeneratorHostedService>();
                })
                .ConfigureLogging(builder => builder.AddConsole())
                .RunConsoleAsync();
