﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using System.Configuration;

namespace Nether.Analytics.EventProcessor
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    internal static class Program
    {
        private static string s_webJobDashboardAndStorageConnectionString;
        private static string s_ingestEventHubConnectionString;
        private static string s_ingestEventHubName;

        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        public static void Main()
        {
            Greet();

            var jobHostConfig = Configure();

            // Run and block
            var host = new JobHost(jobHostConfig);
            host.RunAndBlock();
        }

        private static JobHostConfiguration Configure()
        {  
            //TODO: Make all configuration work in the same way across Nether
            Console.WriteLine("Configuring WebJob (from Environment Variables");

            s_webJobDashboardAndStorageConnectionString = ConfigResolver.Resolve("NETHER_WEBJOB_DASHBOARD_AND_STORAGE_CONNECTIONSTRING");
            Console.WriteLine($"webJobDashboardAndStorageConnectionString:");
            Console.WriteLine($"  {s_webJobDashboardAndStorageConnectionString}");

            s_ingestEventHubConnectionString = ConfigResolver.Resolve("NETHER_INGEST_EVENTHUB_CONNECTIONSTRING");
            Console.WriteLine($"ingestEventHubConnectionString:");
            Console.WriteLine($"  {s_ingestEventHubConnectionString}");

            s_ingestEventHubName = ConfigResolver.Resolve("NETHER_INGEST_EVENTHUB_NAME");
            Console.WriteLine($"ingestEventHubName:");
            Console.WriteLine($"  {s_ingestEventHubName}");

            Console.WriteLine();

            // Setup Web Job Config
            var jobHostConfig = new JobHostConfiguration(s_webJobDashboardAndStorageConnectionString)
            {
                NameResolver = new NameResolver()
            };
            var eventHubConfig = new EventHubConfiguration();
            eventHubConfig.AddReceiver(s_ingestEventHubName, s_ingestEventHubConnectionString);

            jobHostConfig.UseEventHub(eventHubConfig);

            if (jobHostConfig.IsDevelopment)
            {
                jobHostConfig.UseDevelopmentSettings();
            }

            return jobHostConfig;
        }

        private static void Greet()
        {
            Console.WriteLine();
            Console.WriteLine(@" _   _      _   _               ");
            Console.WriteLine(@"| \ | | ___| |_| |__   ___ _ __ ");
            Console.WriteLine(@"|  \| |/ _ \ __| '_ \ / _ \ '__|");
            Console.WriteLine(@"| |\  |  __/ |_| | | |  __/ |   ");
            Console.WriteLine(@"|_| \_|\___|\__|_| |_|\___|_|   ");
            Console.WriteLine(@"- Analytics Event Processor -");
            Console.WriteLine();
        }
        
    }
}
