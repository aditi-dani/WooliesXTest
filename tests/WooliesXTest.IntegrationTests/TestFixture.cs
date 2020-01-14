using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace WooliesXTest.IntegrationTests
{
    public class TestFixture<TStartup> : IDisposable
    {
        public static string GetProjectPath(string projectRelativePath, Assembly startupAssembly)
        {
            var projectName = startupAssembly.GetName().Name;

            var applicationBasePath = AppContext.BaseDirectory;

            var directoryInfo = new DirectoryInfo(applicationBasePath);

            do
            {
                directoryInfo = directoryInfo.Parent;

                var projectDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, projectRelativePath));

                if (projectDirectoryInfo.Exists)
                    if (new FileInfo(Path.Combine(projectDirectoryInfo.FullName, "src", projectName, $"{projectName}.csproj")).Exists)
                        return Path.Combine(projectDirectoryInfo.FullName, "src", projectName);
            }
            while (directoryInfo.Parent != null);

            throw new Exception($"Project root could not be located using the application root {applicationBasePath}.");
        }

        private readonly TestServer _testServer;

        public TestFixture()
            : this(Path.Combine(""))
        {
        }

        public HttpClient Client { get; }

        public void Dispose()
        {
            Client.Dispose();
            _testServer.Dispose();
        }

        protected TestFixture(string relativeTargetProjectParentDir)
        {
            var startupAssembly = typeof(TStartup).GetTypeInfo().Assembly;
            var contentRoot = GetProjectPath(relativeTargetProjectParentDir, startupAssembly);

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(contentRoot)
                .AddJsonFile("appsettings.json");

            var webHostBuilder = new WebHostBuilder()
                .UseConfiguration(configurationBuilder.Build())
                .UseEnvironment("Development")
                .UseStartup(typeof(TStartup));

            // Create instance of test server
            _testServer = new TestServer(webHostBuilder);

            // Add configuration for client
            Client = _testServer.CreateClient();
            Client.BaseAddress = new Uri("http://localhost:5001");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
