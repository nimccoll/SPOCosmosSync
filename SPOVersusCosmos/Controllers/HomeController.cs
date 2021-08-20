//===============================================================================
// Microsoft FastTrack for Azure
// SharePoint Online versus CosmosDB Samples
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using OfficeDevPnP.Core;
using SPOVersusCosmos.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SPOVersusCosmos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConfiguration _configuration;
        private AzureServiceTokenProvider _azureServiceTokenProvider;
        private KeyVaultClient _keyVaultClient;
        private AuthenticationManager _authenticationManager;
        private readonly string _listName = "ProjectList";
        private CosmosClient _cosmosClient;
        private readonly string _databaseName = "SharePointSync";
        private readonly string _containerName = "ProjectList";
        private Database _database;
        private Container _container;
        private static readonly JsonSerializer _jsonSerializer = new JsonSerializer();

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            // Authenticate to Key Vault using the application's Managed Identity
            _azureServiceTokenProvider = new AzureServiceTokenProvider();
            _keyVaultClient = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(
                    _azureServiceTokenProvider.KeyVaultTokenCallback));

            _authenticationManager = new AuthenticationManager();
            CosmosClientOptions options = new CosmosClientOptions() { AllowBulkExecution = true };
            //_cosmosClient = new CosmosClient(Environment.GetEnvironmentVariable("COSMOS_CONNECTIONSTRING"), options);
            _cosmosClient = new CosmosClient(Environment.GetEnvironmentVariable("COSMOS_CONNECTIONSTRING"));
            _database = _cosmosClient.GetDatabase(_databaseName);
            _container = _database.GetContainer(_containerName);
        }

        public IActionResult Index()
        {
            return View();
        }

        // Cache response using the default cache policy
        [ResponseCache(CacheProfileName = "DefaultCachePolicy")]
        public async Task<IActionResult> GetFromSPO()
        {
            List<Post> posted = new List<Post>();
            Stopwatch watch = new Stopwatch();

            // Retrieve the certificate for the application credentials from Key Vault
            SecretBundle certificateSecret = await _keyVaultClient.GetSecretAsync(Environment.GetEnvironmentVariable("KEYVAULT_ENDPOINT"), "nickoftime-certificate");
            byte[] privateKeyBytes = Convert.FromBase64String(certificateSecret.Value);
            X509Certificate2 certificate = new X509Certificate2(privateKeyBytes, (string)null);

            // Authenticate to SPO using App only credentials and retrieve list data
            using (ClientContext clientContext = new AuthenticationManager().GetAzureADAppOnlyAuthenticatedContext(Environment.GetEnvironmentVariable("SITE_URL"), Environment.GetEnvironmentVariable("CLIENT_ID"), Environment.GetEnvironmentVariable("TENANT"), certificate))
            {
                watch.Start();
                List projectList = clientContext.Web.Lists.GetByTitle(_listName);
                ListItem item1 = projectList.GetItemById(8);
                ListItem item2 = projectList.GetItemById(9);
                ListItem item3 = projectList.GetItemById(11);
                ListItem item4 = projectList.GetItemById(12);
                ListItem item5 = projectList.GetItemById(13);
                ListItem item6 = projectList.GetItemById(14);
                ListItem item7 = projectList.GetItemById(16);
                ListItem item8 = projectList.GetItemById(17);

                //CamlQuery projectListQuery = new CamlQuery();
                //projectListQuery.ViewXml = "<View><Query><OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy></Query></View>";
                //ListItemCollection projectListItems = projectList.GetItems(projectListQuery);
                //clientContext.Load(projectListItems);
                clientContext.Load(item1);
                clientContext.Load(item2);
                clientContext.Load(item3);
                clientContext.Load(item4);
                clientContext.Load(item5);
                clientContext.Load(item6);
                clientContext.Load(item7);
                clientContext.Load(item8);
                clientContext.ExecuteQuery();
                watch.Stop();
                //foreach (ListItem p in projectListItems)
                //{
                //    Post post = MapListItemToPost(p);
                //    posted.Add(post);
                //}
                Post post = MapListItemToPost(item1);
                posted.Add(post);
                post = MapListItemToPost(item2);
                posted.Add(post);
                post = MapListItemToPost(item3);
                posted.Add(post);
                post = MapListItemToPost(item4);
                posted.Add(post);
                post = MapListItemToPost(item5);
                posted.Add(post);
                post = MapListItemToPost(item6);
                posted.Add(post);
                post = MapListItemToPost(item7);
                posted.Add(post);
                post = MapListItemToPost(item8);
                posted.Add(post);
            };

            ViewBag.Title = "List Posts from SPO";
            _logger.LogInformation($"GetFromSPO Execution Time: {watch.ElapsedMilliseconds}");
            ViewBag.ExecutionTime = watch.ElapsedMilliseconds;

            return View("List", posted);
        }

        // Cache response using the default cache policy
        [ResponseCache(CacheProfileName = "DefaultCachePolicy")]
        public async Task<IActionResult> GetFromCosmos()
        {
            List<Post> posted = new List<Post>();
            Stopwatch watch = new Stopwatch();

            //using (
            //    ResponseMessage responseMessage = await _container.ReadItemStreamAsync(
            //    partitionKey: new PartitionKey("16"),
            //    id: "16"))
            //{
            //    // Item stream operations do not throw exceptions for better performance
            //    if (responseMessage.IsSuccessStatusCode)
            //    {
            //        ProjectListItem streamResponse = FromStream<ProjectListItem>(responseMessage.Content);

            //        // Log the diagnostics
            //        Console.WriteLine($"\n1.2.2 - Item Read Diagnostics: {responseMessage.Diagnostics.ToString()}");
            //    }
            //    else
            //    {
            //        Console.WriteLine($"Read item from stream failed. Status code: {responseMessage.StatusCode} Message: {responseMessage.ErrorMessage}");
            //    }
            //}
            //watch.Start();
            //List<Task<ItemResponse<ProjectListItem>>> concurrentTasks = new List<Task<ItemResponse<ProjectListItem>>>();
            //concurrentTasks.Add(_container.ReadItemAsync<ProjectListItem>("8", new PartitionKey("8")));
            //concurrentTasks.Add(_container.ReadItemAsync<ProjectListItem>("9", new PartitionKey("9")));
            //concurrentTasks.Add(_container.ReadItemAsync<ProjectListItem>("11", new PartitionKey("11")));
            //concurrentTasks.Add(_container.ReadItemAsync<ProjectListItem>("12", new PartitionKey("12")));
            //concurrentTasks.Add(_container.ReadItemAsync<ProjectListItem>("13", new PartitionKey("13")));
            //concurrentTasks.Add(_container.ReadItemAsync<ProjectListItem>("14", new PartitionKey("14")));
            //concurrentTasks.Add(_container.ReadItemAsync<ProjectListItem>("16", new PartitionKey("16")));
            //concurrentTasks.Add(_container.ReadItemAsync<ProjectListItem>("17", new PartitionKey("17")));

            //await Task.WhenAll(concurrentTasks);
            //watch.Stop();
            //foreach (Task<ItemResponse<ProjectListItem>> task in concurrentTasks)
            //{
            //    ProjectListItem p = task.Result.Resource;
            //    Post post = MapProjectListItemToPost(p);
            //    posted.Add(post);
            //}

            QueryDefinition query = new QueryDefinition(
                "SELECT * FROM ProjectList c where c.id IN(\"8\", \"9\", \"11\", \"12\", \"13\", \"14\", \"16\", \"17\")");

            List<ProjectListItem> selectedProjectListItems = new List<ProjectListItem>();
            watch.Start();
            using (FeedIterator<ProjectListItem> resultSet = _container.GetItemQueryIterator<ProjectListItem>(
                query))
            {
                while (resultSet.HasMoreResults)
                {
                    FeedResponse<ProjectListItem> response = await resultSet.ReadNextAsync();
                    ProjectListItem projectListItem = response.First();
                    selectedProjectListItems.AddRange(response);
                }
            }
            watch.Stop();
            foreach (ProjectListItem p in selectedProjectListItems)
            {
                Post post = MapProjectListItemToPost(p);
                posted.Add(post);
            }

            ViewBag.Title = "List Posts from CosmosDB";
            _logger.LogInformation($"GetFromCosmos Execution Time: {watch.ElapsedMilliseconds}");
            ViewBag.ExecutionTime = watch.ElapsedMilliseconds;

            return View("List", posted);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private Post MapListItemToPost(ListItem p)
        {
            Post post = new Post();

            post.ID = p.Id;
            post.Title = p["Title"].ToString();
            post.Description = p["Description"].ToString();
            post.Type = p["Type"].ToString();
            if (p["EffortHours"] != null) post.EffortHours = Convert.ToInt32(p["EffortHours"]);
            if (p["EffortMinutes"] != null) post.EffortMinutes = Convert.ToInt32(p["EffortMinutes"]);
            if (p["StartDate"] != null) post.StartDate = Convert.ToDateTime(p["StartDate"]);
            if (p["EndDate"] != null) post.EndDate = Convert.ToDateTime(p["EndDate"]);
            post.ExpirationDate = Convert.ToDateTime(p["ExpirationDate"]);
            post.Location = p["Location"].ToString();
            FieldUserValue userField = (FieldUserValue)p["PostedBy"];
            post.PostedBy = userField.LookupValue;
            post.PostedByID = userField.LookupId;
            post.PostedByEmailAddress = userField.Email;
            post.Status = p["Status"].ToString();
            post.Skills = new List<string>();
            for (int i = 1; i < 11; i++)
            {
                if (p[string.Format("Skill{0}", i)] != null)
                {
                    post.Skills.Add(p[string.Format("Skill{0}", i)].ToString());
                }
            }

            return post;
        }

        private Post MapProjectListItemToPost(ProjectListItem p)
        {
            Post post = new Post();
            post.ID = p.ID;
            post.Title = p.Title;
            post.Description = p.Description;
            post.Type = p.Type.Value;
            post.EffortHours = Convert.ToInt32(p.EffortHours);
            post.EffortMinutes = Convert.ToInt32(p.EffortMinutes);
            if (!string.IsNullOrEmpty(p.StartDate)) post.StartDate = Convert.ToDateTime(p.StartDate);
            if (!string.IsNullOrEmpty(p.EndDate)) post.EndDate = Convert.ToDateTime(p.EndDate);
            post.ExpirationDate = Convert.ToDateTime(p.ExpirationDate);
            post.Location = p.Location;
            post.PostedBy = p.PostedBy.DisplayName;
            if (p.PostedBy.Email != null) post.PostedByEmailAddress = p.PostedBy.Email.ToString();
            post.Status = p.Status.Value;
            post.Skills = new List<string>();
            if (!string.IsNullOrEmpty(p.Skill1)) post.Skills.Add(p.Skill1);
            if (!string.IsNullOrEmpty(p.Skill2)) post.Skills.Add(p.Skill2);
            if (!string.IsNullOrEmpty(p.Skill3)) post.Skills.Add(p.Skill3);
            if (!string.IsNullOrEmpty(p.Skill4)) post.Skills.Add(p.Skill4);
            if (!string.IsNullOrEmpty(p.Skill5)) post.Skills.Add(p.Skill5);
            if (!string.IsNullOrEmpty(p.Skill6)) post.Skills.Add(p.Skill6);
            if (!string.IsNullOrEmpty(p.Skill7)) post.Skills.Add(p.Skill7);
            if (!string.IsNullOrEmpty(p.Skill8)) post.Skills.Add(p.Skill8);
            if (!string.IsNullOrEmpty(p.Skill9)) post.Skills.Add(p.Skill9);
            if (!string.IsNullOrEmpty(p.Skill10)) post.Skills.Add(p.Skill10);

            return post;
        }

        private static T FromStream<T>(Stream stream)
        {
            using (stream)
            {
                if (typeof(Stream).IsAssignableFrom(typeof(T)))
                {
                    return (T)(object)stream;
                }

                using (StreamReader sr = new StreamReader(stream))
                {
                    string json = sr.ReadToEnd();
                    ProjectListItem p = JsonConvert.DeserializeObject<ProjectListItem>(json);
                    stream.Position = 0;
                    using (JsonTextReader jsonTextReader = new JsonTextReader(sr))
                    {
                        return _jsonSerializer.Deserialize<T>(jsonTextReader);
                    }
                }
            }
        }
    }
}
