using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace WhatsAppIntegration.Web.Pages
{
    public class MetaWhatsAppModel : PageModel
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _client;

        public MetaWhatsAppModel(IConfiguration config)
        {
            _config = config;
            _client = new HttpClient();
        }

        [BindProperty]
        public string To { get; set; } = string.Empty;

        [BindProperty]
        public string Body { get; set; } = string.Empty;

        public string? ResponseMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var payload = new
            {
                To = To,
                Body = Body
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var baseUrl = _config["APISettings:BaseUrl"];
            var apiUrl = $"{baseUrl}MetaWhatsApp/send";

            var response = await _client.PostAsync(apiUrl, content);

            ResponseMessage = await response.Content.ReadAsStringAsync();
            return Page();
        }
    }
}
