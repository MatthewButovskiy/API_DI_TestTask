namespace Scripts.Services.Dogs
{
    using System.Collections.Generic;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UnityEngine.Networking;
    using Newtonsoft.Json.Linq;

    public interface IDogService
    {
        UniTask<IReadOnlyList<DogBreedShort>> GetBreedsAsync(CancellationToken token);
        UniTask<DogBreedFull> GetBreedAsync(string id, CancellationToken token);
    }

    public struct DogBreedShort
    {
        public string Id;
        public string Name;
    }

    public struct DogBreedFull
    {
        public string Name;
        public string Description;
    }

    public sealed class DogService : IDogService
    {
        private const string BASE = "https://dogapi.dog/api/v2/";

        public async UniTask<IReadOnlyList<DogBreedShort>> GetBreedsAsync(CancellationToken token)
        {
            using var req = UnityWebRequest.Get($"{BASE}breeds?page[size]=10");
            await req.SendWebRequest().WithCancellation(token);

           

            var list = new List<DogBreedShort>();
            var arr  = JObject.Parse(req.downloadHandler.text)["data"];
            foreach (var b in arr)
            {
                list.Add(new DogBreedShort
                {
                    Id   = b["id"].ToString(),
                    Name = b["attributes"]["name"].ToString()
                });
            }
            return list;
        }

        public async UniTask<DogBreedFull> GetBreedAsync(string id, CancellationToken token)
        {
            using var req = UnityWebRequest.Get($"{BASE}breeds/{id}");
            await req.SendWebRequest().WithCancellation(token);
            

            var node = JObject.Parse(req.downloadHandler.text)["data"]["attributes"];
            return new DogBreedFull
            {
                Name        = node["name"].ToString(),
                Description = node["description"].ToString()
            };
        }
    }
}