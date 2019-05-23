using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Xml.Linq;
using System.Xml;

namespace MaCoCo
{
    public class Utilities
    {
        public static async Task<string> GetString(string url)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return "";
            }
        }

        public static async Task<XElement> GetXML(string url)
        {
            return XElement.Parse(await GetString(url));
        }
    }
}
