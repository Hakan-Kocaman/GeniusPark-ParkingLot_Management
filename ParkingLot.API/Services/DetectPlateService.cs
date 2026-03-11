using ParkingLot.Core;
using System.Text.Json;

namespace ParkingLot.API.Services
{
    public class DetectPlateService
    {
        private readonly HttpClient _httpClient;

        public DetectPlateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public bool CheckPlateHaving(string plate) {
            return true;
        }

        public async Task<PlateResponse> DetectPlate(IFormFile carImage) {

            using var content = new MultipartFormDataContent();

            using var stream = carImage.OpenReadStream();

            using var streamContent = new StreamContent(stream);

            content.Add(streamContent, "file", carImage.FileName);

            var response = await _httpClient.PostAsync(
                "http://localhost:5001/api/plate/detect",
                content
            );

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PlateResponse>();

            if (result == null)
                throw new Exception("Failed to deserialize ML response");

            return result;
        
        }


    }
}
