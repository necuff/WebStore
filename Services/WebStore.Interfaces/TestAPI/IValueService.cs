using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WebStore.Interfaces.TestAPI
{
    public interface IValueService
    {
        IEnumerable<string> Get();

        string Get(int id);

        Uri Post(string Value);

        HttpStatusCode Update(int id, string value);

        HttpStatusCode Delete(int id);
    }
}
