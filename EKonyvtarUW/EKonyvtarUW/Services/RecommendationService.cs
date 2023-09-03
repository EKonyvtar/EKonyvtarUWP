using EKonyvtarUW.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace EKonyvtarUW.Services
{
    public class RecommendationService
    {
        public static async Task<List<Book>> GetRecommendation()
        {
            var result = new List<Book>();
            try
            {
                StorageFile recommendationFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/recommendation.json"));
                string json = await FileIO.ReadTextAsync(recommendationFile);
                result = JsonConvert.DeserializeObject<List<Book>>(json);
            }
            catch (Exception)
            {
                //Unknown exception yet
            }
            return result;
        }
    }
}
