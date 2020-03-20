using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HFFDCR.Core.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Client
{
    public class HFFDCRClient
    {
        private HttpClient _client = new HttpClient(new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = ((message, certificate2, arg3, arg4) => true)
        });
        private readonly string BaseAddress;

        public HFFDCRClient(string baseAddress)
        {
            BaseAddress = baseAddress;
        }

        private (string content, HttpStatusCode status) QueryRequest(string route, HttpMethod method, HttpContent content = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, route);
            request.Content = content ?? request.Content;
            
            Task<HttpResponseMessage> responseTask = _client.SendAsync(request);

            string responseContent = responseTask.Result.Content.ReadAsStringAsync().Result;
            HttpStatusCode status = responseTask.Result.StatusCode;
            
            request.Dispose();
            responseTask.Dispose();

            return (responseContent, status);
        }

        public IEnumerable<File> GetFiles()
        {
            string route = $"{BaseAddress}/File";
            var result = QueryRequest(route, HttpMethod.Get);

            if (result.status != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<File[]>(result.content);
        }

        public FileBlockInfo GetFileBlockContent(long fileId, long blockNumber)
        {
            string route = $"{BaseAddress}/File/{fileId}/FileBlock/{blockNumber}";
            var result = QueryRequest(route, HttpMethod.Get);

            if (result.status != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<FileBlockInfo>(result.content);
        }

        public bool SetFileBlockContent(long fileId, FileBlockInfo fileBlockInfo)
        {
            string route = $"{BaseAddress}/File/{fileId}/FileBlock";
            var result = QueryRequest(route, HttpMethod.Post,
                new StringContent(JsonConvert.SerializeObject(fileBlockInfo), Encoding.UTF8, "application/json"));

            if (result.status != HttpStatusCode.OK)
                return false;

            return true;
        }

        public IEnumerable<FileBlockInfo> GetFileBlockChecksums(long fileId)
        {
            string route = $"{BaseAddress}/File/{fileId}/Checksum";
            var result = QueryRequest(route, HttpMethod.Get);

            if (result.status != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<FileBlockInfo[]>(result.content);
        }
    }
}