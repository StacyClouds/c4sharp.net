using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StacyClouds.C4Sharp.Encryption;
using StacyClouds.C4Sharp.IO.Json;
using Xunit;

namespace StacyClouds.C4Sharp.Api.Tests
{
    public class StructurizrClientBehaviorTests
    {
        [Fact]
        public void Construction_sets_merge_from_remote_to_true_by_default()
        {
            TestStructurizrClient client = new TestStructurizrClient(new RecordingHttpMessageHandler(Array.Empty<HttpResponseMessage>()));
            Assert.True(client.MergeFromRemote);
        }

        [Fact]
        public void Lock_and_unlock_workspace_send_signed_requests()
        {
            RecordingHttpMessageHandler handler = new RecordingHttpMessageHandler(
                new[] { OkResponse("{\"success\":true}"), OkResponse("{\"success\":true}") });
            TestStructurizrClient client = new TestStructurizrClient(handler);

            Assert.True(client.LockWorkspace(42));
            Assert.True(client.UnlockWorkspace(42));

            Assert.Equal(2, handler.Requests.Count);
            Assert.Equal(HttpMethod.Put, handler.Requests[0].Method);
            Assert.Equal(HttpMethod.Delete, handler.Requests[1].Method);
            Assert.Contains("/workspace/42/lock?", handler.Requests[0].RequestUri.PathAndQuery);
            Assert.Equal("application/json; charset=utf-8", handler.Requests[0].ContentType.ToLowerInvariant());

            AssertSignedRequest(handler.Requests[0], "PUT", "", "");
            AssertSignedRequest(handler.Requests[1], "DELETE", "", "");
        }

        [Fact]
        public void GetWorkspace_reads_workspace_and_archives_response()
        {
            Workspace remoteWorkspace = new Workspace("Remote", "Remote desc");
            remoteWorkspace.Model.AddPerson("User");
            string json = SerializeWorkspace(remoteWorkspace);

            string archiveDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(archiveDirectory);

            try
            {
                RecordingHttpMessageHandler handler = new RecordingHttpMessageHandler(new[] { OkResponse(json) });
                TestStructurizrClient client = new TestStructurizrClient(handler)
                {
                    WorkspaceArchiveLocation = new DirectoryInfo(archiveDirectory),
                    IdGenerator = new PrefixIdGenerator("generated")
                };

                Workspace workspace = client.GetWorkspace(42);

                Assert.Equal("Remote", workspace.Name);
                Assert.Equal("generated-1", workspace.Model.AddPerson("Another").Id);
                Assert.Single(handler.Requests);
                AssertSignedRequest(handler.Requests[0], "GET", "", "");
                Assert.True(Directory.GetFiles(archiveDirectory, "structurizr-42-*.json").Single().Length > 0);
            }
            finally
            {
                if (Directory.Exists(archiveDirectory))
                {
                    Directory.Delete(archiveDirectory, true);
                }
            }
        }

        [Fact]
        public void GetWorkspace_throws_client_exception_on_non_ok_response()
        {
            RecordingHttpMessageHandler handler = new RecordingHttpMessageHandler(
                new[]
                {
                    new HttpResponseMessage(HttpStatusCode.Forbidden)
                    {
                        Content = new StringContent("{\"success\":false,\"message\":\"No access\"}", Encoding.UTF8, "application/json")
                    }
                });
            TestStructurizrClient client = new TestStructurizrClient(handler);

            StructurizrClientException exception = Assert.Throws<StructurizrClientException>(() => client.GetWorkspace(42));
            Assert.Equal("No access", exception.Message);
        }

        [Fact]
        public void GetWorkspace_with_encryption_strategy_reads_unencrypted_workspace_when_payload_is_plain_json()
        {
            Workspace remoteWorkspace = new Workspace("Remote", "Remote desc");
            remoteWorkspace.Model.AddPerson("User");
            string json = SerializeWorkspace(remoteWorkspace);

            RecordingHttpMessageHandler handler = new RecordingHttpMessageHandler(new[] { OkResponse(json) });
            TestStructurizrClient client = new TestStructurizrClient(handler)
            {
                EncryptionStrategy = new AesEncryptionStrategy("password")
            };

            Workspace workspace = client.GetWorkspace(42);
            Assert.Equal("Remote", workspace.Name);
            Assert.NotNull(workspace.Model.GetPersonWithName("User"));
        }

        [Fact]
        public void PutWorkspace_merges_remote_layout_and_writes_json()
        {
            Workspace remoteWorkspace = BuildWorkspaceWithContextView(PaperSize.A4_Landscape);
            Workspace localWorkspace = BuildWorkspaceWithContextView(null);
            string remoteJson = SerializeWorkspace(remoteWorkspace);

            RecordingHttpMessageHandler handler = new RecordingHttpMessageHandler(
                new[] { OkResponse(remoteJson), OkResponse("{\"success\":true}") });
            TestStructurizrClient client = new TestStructurizrClient(handler)
            {
                MergeFromRemote = true
            };

            client.PutWorkspace(42, localWorkspace);

            Assert.Equal(PaperSize.A4_Landscape, localWorkspace.Views.SystemContextViews.Single().PaperSize);
            Assert.Equal(2, handler.Requests.Count);
            Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
            Assert.Equal(HttpMethod.Put, handler.Requests[1].Method);
            AssertSignedRequest(handler.Requests[1], "PUT", handler.Requests[1].Body, handler.Requests[1].ContentType);
            Assert.Contains("\"paperSize\":\"A4_Landscape\"", handler.Requests[1].Body);
        }

        [Fact]
        public void PutWorkspace_with_encryption_strategy_writes_encrypted_payload()
        {
            Workspace localWorkspace = BuildWorkspaceWithContextView(null);
            RecordingHttpMessageHandler handler = new RecordingHttpMessageHandler(new[] { OkResponse("{\"success\":true}") });
            TestStructurizrClient client = new TestStructurizrClient(handler)
            {
                MergeFromRemote = false,
                EncryptionStrategy = new AesEncryptionStrategy(128, 1000, "06DC30A48ADEEE72D98E33C2CEAEAD3E", "ED124530AF64A5CAD8EF463CF5628434", "password")
            };

            client.PutWorkspace(42, localWorkspace);

            string body = handler.Requests.Single().Body;
            Assert.Contains("\"ciphertext\":", body);
            Assert.Contains("\"encryptionStrategy\":", body);
        }

        [Fact]
        public void PutWorkspace_throws_client_exception_on_non_ok_response()
        {
            Workspace localWorkspace = BuildWorkspaceWithContextView(null);
            RecordingHttpMessageHandler handler = new RecordingHttpMessageHandler(
                new[]
                {
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("{\"success\":false,\"message\":\"Bad payload\"}", Encoding.UTF8, "application/json")
                    }
                });
            TestStructurizrClient client = new TestStructurizrClient(handler)
            {
                MergeFromRemote = false
            };

            StructurizrClientException exception = Assert.Throws<StructurizrClientException>(() => client.PutWorkspace(42, localWorkspace));
            Assert.Equal("There was an error putting the workspace: Bad payload", exception.Message);
        }

        private static Workspace BuildWorkspaceWithContextView(PaperSize paperSize)
        {
            Workspace workspace = new Workspace("Remote", "Remote desc");
            SoftwareSystem softwareSystem = workspace.Model.AddSoftwareSystem("System", "System");
            Person user = workspace.Model.AddPerson("User", "User");
            user.Uses(softwareSystem, "Uses");

            SystemContextView view = workspace.Views.CreateSystemContextView(softwareSystem, "context", "Context");
            view.AddAllElements();
            view.PaperSize = paperSize;
            return workspace;
        }

        private static string SerializeWorkspace(Workspace workspace)
        {
            using StringWriter writer = new StringWriter();
            new JsonWriter(false).Write(workspace, writer);
            return writer.ToString();
        }

        private static HttpResponseMessage OkResponse(string body)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };
        }

        private static void AssertSignedRequest(RecordedRequest request, string httpMethod, string content, string contentType)
        {
            string nonce = request.Headers["Nonce"];
            string expectedAuthorization = ComputeAuthorizationHeader("key", "secret", httpMethod, request.RequestUri.PathAndQuery, content, contentType, nonce);

            Assert.StartsWith("structurizr-dotnet/", request.Headers["User-Agent"]);
            Assert.Equal(expectedAuthorization, request.Headers["X-Authorization"]);
            Assert.False(string.IsNullOrWhiteSpace(nonce));
        }

        private static string ComputeAuthorizationHeader(string apiKey, string apiSecret, string httpMethod, string path, string content, string contentType, string nonce)
        {
            string contentMd5 = ComputeMd5(content);
            string hmacContent = string.Join("\n", new[] { httpMethod, path, contentMd5, contentType, nonce }) + "\n";

            using HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(apiSecret));
            string hash = BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(hmacContent))).Replace("-", "").ToLowerInvariant();
            return apiKey + ":" + Convert.ToBase64String(Encoding.UTF8.GetBytes(hash));
        }

        private static string ComputeMd5(string value)
        {
            using MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(value ?? ""));
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        private sealed class TestStructurizrClient : StructurizrClient
        {
            private readonly HttpMessageHandler _handler;

            public TestStructurizrClient(HttpMessageHandler handler) : base("https://localhost", "key", "secret")
            {
                _handler = handler;
            }

            protected override HttpClient createHttpClient()
            {
                return new HttpClient(_handler, disposeHandler: false);
            }
        }

        private sealed class RecordingHttpMessageHandler : HttpMessageHandler
        {
            private readonly Queue<HttpResponseMessage> _responses;
            public List<RecordedRequest> Requests { get; } = new List<RecordedRequest>();

            public RecordingHttpMessageHandler(IEnumerable<HttpResponseMessage> responses)
            {
                _responses = new Queue<HttpResponseMessage>(responses ?? Array.Empty<HttpResponseMessage>());
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                string body = request.Content == null ? "" : await request.Content.ReadAsStringAsync();
                string contentType = request.Content?.Headers?.ContentType?.ToString() ?? "";

                Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
                {
                    headers[header.Key] = string.Join(",", header.Value);
                }

                if (request.Content != null)
                {
                    foreach (KeyValuePair<string, IEnumerable<string>> header in request.Content.Headers)
                    {
                        headers[header.Key] = string.Join(",", header.Value);
                    }
                }

                Requests.Add(new RecordedRequest(request.Method, request.RequestUri, body, contentType, headers));
                return _responses.Count > 0 ? _responses.Dequeue() : OkResponse("{\"success\":true}");
            }
        }

        private sealed record RecordedRequest(
            HttpMethod Method,
            Uri RequestUri,
            string Body,
            string ContentType,
            IReadOnlyDictionary<string, string> Headers);

        private sealed class PrefixIdGenerator : IdGenerator
        {
            private readonly string _prefix;
            private int _counter;

            public PrefixIdGenerator(string prefix)
            {
                _prefix = prefix;
            }

            public string GenerateId(Element element)
            {
                _counter++;
                return _prefix + "-" + _counter;
            }

            public string GenerateId(Relationship relationship)
            {
                _counter++;
                return _prefix + "-" + _counter;
            }

            public void Found(string id)
            {
            }
        }
    }
}
