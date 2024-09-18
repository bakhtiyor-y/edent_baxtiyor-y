using Edent.Api.Infrastructure.Extensions;
using Edent.Api.Models;
using Edent.Api.Models.PlayMobile;
using Edent.Api.Services.Notification.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Edent.Api.Services.Notification.PlayMobile
{
    public class PlayMobileSmsSender : ISendSmsBehavior<PlayMobileSmsSender>
    {
        private readonly IOptionsSnapshot<PlayMobileProvider> _smsProvider;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<PlayMobileSmsSender> _logger;

        public PlayMobileSmsSender(IOptionsSnapshot<PlayMobileProvider> smsProvider,
            IHttpClientFactory clientFactory,
            ILogger<PlayMobileSmsSender> logger)
        {
            _smsProvider = smsProvider;
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<bool> SendAsync(NotificationMessage notification)
        {
            try
            {
                if (_smsProvider.Value != null)
                {
                    var message = getSmsMessage(notification);

                    HttpClient httpClient = _clientFactory.CreateClient();
                    httpClient.SetBasicAuthentication(_smsProvider.Value.UserName, _smsProvider.Value.Password);

                    HttpResponseMessage respopnseMessage = await httpClient.PostAsync(
                        _smsProvider.Value.Url,
                        new StringContent(JsonConvert.SerializeObject(message),
                        Encoding.UTF8, "application/json"));

                    if (!respopnseMessage.IsSuccessStatusCode)
                    {
                        string content = await respopnseMessage.Content.ReadAsStringAsync();
                        _logger.LogError($"------- Sms sender: {content} ------");
                    }
                    return respopnseMessage.IsSuccessStatusCode;
                }
                else
                {
                    _logger.LogError($"------- Sms sender: provider not found ------");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"------- Sms sender: {ex.Message} ------ InnerException:{ex.InnerException?.Message}");
                return false;
            }
        }

        private SmsMessage getSmsMessage(NotificationMessage notification)
        {
            SmsMessage smsMessage = new SmsMessage
            {
                Messages = new List<Message>
                {
                    new Message
                    {
                        MessageId = $"{DateTime.Now.Ticks}",
                        Recipient = notification.Recipient.TrimStart('+'),
                        Sms = new Sms
                        {
                            Originator = "3700",
                            Content = new Content
                            {
                                Text = notification.Message
                            }
                        }
                    }
                }
            };
            return smsMessage;
        }
    }
}
