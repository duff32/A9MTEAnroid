using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace App3
{
    public class MockDataStore : IDataStore<Item>
    {
        List<Item> items;
        HttpClient client;
        public MockDataStore()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;

            items = new List<Item>();
            var _items = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is a nice description"},
                new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is a nice description"},
                new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is a nice description"},
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is a nice description"},
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is a nice description"},
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is a nice description"},
            };

            foreach (Item item in _items)
            {
                items.Add(item);
            }
            
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var _item = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var _item = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            Item item;
            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            var response = await client.GetAsync("http://10.0.2.152:9999/api/Values/1");
            if (response.IsSuccessStatusCode)
            {
                item = JsonConvert.DeserializeObject<Item>(await response.Content.ReadAsStringAsync());
                items.Add(item);
            }
            return items;
        }
    }
}
