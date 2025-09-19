// 代码生成时间: 2025-09-19 12:02:34
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;

namespace RestfulApiDemo
{
    // Define a model for the resource
    public class Resource
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    // Define the API controller
    public class ResourceApiController : ApiController
    {
        private static List<Resource> resources = new List<Resource>();

        // GET: Retrieve all resources
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllResources()
        {
            try
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(string.Join(", ", resources))
                };
            }
            catch (Exception ex)
            {
                // Log the exception and return an error response
                Console.WriteLine($"Error: {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // GET: Retrieve a single resource by ID
        [HttpGet("{id}")]
        public async Task<HttpResponseMessage> GetResourceById(int id)
        {
            var resource = resources.Find(r => r.Id == id);
            if (resource == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(resource))
                };
            }
        }

        // POST: Create a new resource
        [HttpPost]
        public async Task<HttpResponseMessage> CreateResource(Resource resource)
        {
            if (resource == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            else
            {
                resources.Add(resource);
                return new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(resource))
                };
            }
        }

        // PUT: Update an existing resource
        [HttpPut("{id}")]
        public async Task<HttpResponseMessage> UpdateResource(int id, Resource resource)
        {
            var existingResource = resources.Find(r => r.Id == id);
            if (existingResource == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            else
            {
                existingResource.Name = resource.Name;
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(existingResource))
                };
            }
        }

        // DELETE: Remove a resource by ID
        [HttpDelete("{id}")]
        public async Task<HttpResponseMessage> DeleteResource(int id)
        {
            var resource = resources.Find(r => r.Id == id);
            if (resource != null)
            {
                resources.Remove(resource);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }
    }
}
