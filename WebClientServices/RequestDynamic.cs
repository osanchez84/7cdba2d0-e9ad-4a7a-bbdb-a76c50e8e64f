using GuanajuatoAdminUsuarios.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GuanajuatoAdminUsuarios.WebClientServices
{
    public  class RequestDynamic : IRequestDynamic
    {
        private readonly IServiceAppSettingsService _serviceAppSettingsService;
        public RequestDynamic(IServiceAppSettingsService serviceAppSettingsService)
        {
            _serviceAppSettingsService = serviceAppSettingsService;
        }

        public async Task<string> EncryptionService(string UsuarioLog, string PasswordLog, string ReciboControlInterno, string FechaReversa,string urlName)
        {
            var serviceAppSettings  = _serviceAppSettingsService.GetSettingbyName(urlName);
            XDocument myxml = XDocument.Load(@"XMLRequest\ReversaDePagoRequest.xml");
            string XMLRequest = myxml.ToString();

            XMLRequest = string.Format(XMLRequest, UsuarioLog, PasswordLog, ReciboControlInterno, FechaReversa);


            var getEncryptionResponse = await PostSOAPRequestAsync(serviceAppSettings.SettingValue, XMLRequest);
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml(getEncryptionResponse);
            string encrypt = xmlDoc.InnerText;
            return encrypt;

        }

        private async Task<string> PostSOAPRequestAsync(string requestUri, string text)
        {
            using (HttpClient _httpClient = new HttpClient())
            {
                using (HttpContent content = new StringContent(text, System.Text.Encoding.UTF8, "text/xml"))
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri))
                {
                    request.Headers.Add("SOAPAction", "");
                    request.Content = content;
                    using (HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
        }

    }
}
