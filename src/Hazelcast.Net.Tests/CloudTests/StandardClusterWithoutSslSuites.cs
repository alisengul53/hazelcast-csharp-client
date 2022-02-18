// Copyright (c) 2008-2021, Hazelcast, Inc. All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Hazelcast.Testing.Cloud;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using Hazelcast.Examples;
using Hazelcast.Networking;
using Hazelcast.Testing.Remote;

namespace Hazelcast.Tests.CloudTests
{
    public class StandardClusterWithoutSslSuites: CloudRemoteTestBase
    {
        private CloudCluster _cloudCluster;
        private HazelcastOptions _options;
        private string _hzVersion;
        [OneTimeSetUp]
        public async Task CreateStandardCluster()
        {
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("standardClusterVersion")))
                _hzVersion = Environment.GetEnvironmentVariable("standardClusterVersion");
            
            //_cloudCluster = await RcClient.CreateHazelcastCloudStandardCluster(_hzVersion, false);
            _cloudCluster = await RcClient.GetHazelcastCloudCluster("1432");
            
            _options = new HazelcastOptionsBuilder()
                .WithConsoleLogger()
                .With("Logging:LogLevel:Hazelcast", "Information")
                .Build();
            _options.ClusterName = _cloudCluster.NameForConnect;

            // set the cloud discovery token and url
            _options.Networking.Cloud.DiscoveryToken = _cloudCluster.Token;
            _options.Networking.Cloud.Url = new Uri(Environment.GetEnvironmentVariable("uri"));
            _options.Networking.ReconnectMode = ReconnectMode.ReconnectAsync;
            _options.Metrics.Enabled = true;
        }

        [Test]
        public async Task TestCloudConnection()
        {
            Console.Write("Get and connect client...");
            await using var client = await HazelcastClientFactory.StartNewClientAsync(_options);

            await using var map = await client.GetMapAsync<string, string>("map1");

            await map.PutAsync("key1", "value1");
            Assert.AreEqual("value1", await map.GetAsync("key1"), "Gotten value from map is not expected");

            for (var i = 0; i < 100; i++)
            {
                await map.PutAsync("key_" + i, "value_" + i);

                await map.GetAsync("key_" + i);

                Console.WriteLine("Current map size: {0}", await map.GetSizeAsync());
            }
            Assert.AreEqual(100, await map.GetSizeAsync(), "Map size should be 100");
            
        }
    }
}
