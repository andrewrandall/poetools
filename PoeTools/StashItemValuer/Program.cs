using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace StashItemValuer
{
    class Program
    {
        static void Main(string[] args)
        {
            //File.WriteAllText("testtab.json", GetTestTab());
            //var json = File.ReadAllText("testtab.json");
            var tabs = GetAllTabs();
            var json = JsonConvert.SerializeObject(tabs);
            File.WriteAllText("stash.json", json);
            //var rootModel = JsonConvert.DeserializeObject<ApiModel.RootObject>(json);
            //var x = rootModel.items.Where(i => i.name == "Plague Horn").ToArray();
        }

        static string GetTestTab()
        { 
            var url = @"https://www.pathofexile.com/character-window/get-stash-items?accountName=drewstroyer&realm=pc&league=Synthesis&tabs=0&tabIndex=20&public=false";
            var cookie = @"__cfduid=db5f1b5c7c73088d4ccedecec82ad9d911550498337; _ga=GA1.2.560615242.1550498339; stored_data=1; _gid=GA1.2.719260019.1555062840; cf_clearance=39db0555548cedbecb79ba435dad9b214d8dd23c-1555062854-300-150; POESESSID=373b58be228e39e7d29520918823f5aa; _gat=1";

            var cookieJar = new CookieContainer();
            foreach (var c in cookie.Split(';'))
            {
                var parts = c.Split('=');
                cookieJar.Add(new Uri("https://www.pathofexile.com"), new Cookie(parts[0].Trim(), parts[1].Trim()));
            }
            var handler = new HttpClientHandler() { CookieContainer = cookieJar };
            var client = new HttpClient(handler);
            var result = client.GetAsync(url).Result;
            var json = result.Content.ReadAsStringAsync().Result;
            return json;
        }

        static IEnumerable<StashTab> GetAllTabs()
        {
            var firstUrl = @"https://www.pathofexile.com/character-window/get-stash-items?accountName=drewstroyer&realm=pc&league=Synthesis&tabs=1&tabIndex=0&public=false";

            var cookie = @"__cfduid=db5f1b5c7c73088d4ccedecec82ad9d911550498337; _ga=GA1.2.560615242.1550498339; stored_data=1; _gid=GA1.2.719260019.1555062840; cf_clearance=39db0555548cedbecb79ba435dad9b214d8dd23c-1555062854-300-150; POESESSID=373b58be228e39e7d29520918823f5aa; _gat=1";

            var cookieJar = new CookieContainer();
            foreach (var c in cookie.Split(';'))
            {
                var parts = c.Split('=');
                cookieJar.Add(new Uri("https://www.pathofexile.com"), new Cookie(parts[0].Trim(), parts[1].Trim()));
            }
            var handler = new HttpClientHandler() { CookieContainer = cookieJar };
            var client = new HttpClient(handler);

            var result = client.GetAsync(firstUrl).Result;
            var json = result.Content.ReadAsStringAsync().Result;
            var firstTab = JsonConvert.DeserializeObject<ApiModel.RootObject>(json);

            var tabs = new ConcurrentDictionary<int, StashTab>();

            foreach (var tab in firstTab.tabs)
            {
                tabs.TryAdd(tab.i, new StashTab
                {
                    Index = tab.i,
                    Name = tab.n
                });
            }

            tabs[0].Items = firstTab.items;

            Parallel.ForEach(Enumerable.Range(1, firstTab.numTabs - 1), i =>
            {
                var url = string.Format(@"https://www.pathofexile.com/character-window/get-stash-items?accountName=drewstroyer&realm=pc&league=Synthesis&tabs=0&tabIndex={0}&public=false", i);
                var resultN = client.GetAsync(firstUrl).Result;
                var jsonN = result.Content.ReadAsStringAsync().Result;
                var tab = JsonConvert.DeserializeObject<ApiModel.RootObject>(json);
                tabs[i].Items = tab.items;
            });

            return tabs.Values;
        }
    }
}
