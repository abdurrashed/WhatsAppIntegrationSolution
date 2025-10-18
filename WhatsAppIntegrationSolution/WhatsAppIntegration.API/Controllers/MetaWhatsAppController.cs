using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WhatsAppIntegration.API.Model;

namespace WhatsAppIntegration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetaWhatsAppController : ControllerBase
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public MetaWhatsAppController(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMetaMessage([FromBody] DtoWhatsAppMessage message)
        {
            var accessToken = _config["WhatsAppSettings:AccessToken"];
            var phoneNumberId = _config["WhatsAppSettings:PhoneNumberId"];

            var url = $"https://graph.facebook.com/v20.0/{phoneNumberId}/messages";

            var payload = new
            {
                messaging_product = "whatsapp",
                to = message.To,
                type = "text",
                text = new { body = message.Body }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
                return Ok(" WhatsApp message sent successfully!");
            else
                return BadRequest(await response.Content.ReadAsStringAsync());
        }
    }
}

