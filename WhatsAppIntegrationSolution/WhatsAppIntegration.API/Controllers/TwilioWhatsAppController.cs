using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using WhatsAppIntegration.API.Model;

namespace WhatsAppIntegration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwilioWhatsAppController : ControllerBase
    {

        private readonly IConfiguration _config;

        public TwilioWhatsAppController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("send")]
        public IActionResult SendTwilioMessage([FromBody] DtoWhatsAppMessage msg)
        {
            var sid = _config["TwilioSettings:AccountSid"];
            var token = _config["TwilioSettings:AuthToken"];
            var from = _config["TwilioSettings:FromNumber"];

            TwilioClient.Init(sid, token);

            var message = MessageResource.Create(
                from: new PhoneNumber(from),
                to: new PhoneNumber($"whatsapp:+{msg.To}"),
                body: msg.Body
            );

            return Ok($"Twilio WhatsApp message sent! SID: {message.Sid}");
        }
    }
}
