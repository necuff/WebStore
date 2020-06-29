﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using WebStore.Clients.Base;
using WebStore.Interfaces.TestAPI;

namespace WebStore.Clients.Values
{
    public class ValuesClient : BaseClient, IValueService
    {
        public ValuesClient(IConfiguration Configuration) : base(Configuration, "api/values") { }

        public HttpStatusCode Delete(int id)
        {
            var responce = _Client.DeleteAsync($"{_ServiceAddress}/{id}").Result;
            return responce.StatusCode;
                 
        }

        public IEnumerable<string> Get()
        {
            var response = _Client.GetAsync(_ServiceAddress).Result;
            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<IEnumerable<string>>().Result;

            return Enumerable.Empty<string>();
        }

        public string Get(int id)
        {
            var response = _Client.GetAsync($"{_ServiceAddress}/{id}").Result;

            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<string>().Result;

            return string.Empty;
        }

        public Uri Post(string value)
        {
            var response = _Client.PostAsJsonAsync(_ServiceAddress, value).Result;

            return response.EnsureSuccessStatusCode().Headers.Location;
        }

        public HttpStatusCode Update(int id, string value)
        {
            var response = _Client.PutAsJsonAsync($"{_ServiceAddress}/{id}", value).Result;

            return response.EnsureSuccessStatusCode().StatusCode;
        }
    }
}
