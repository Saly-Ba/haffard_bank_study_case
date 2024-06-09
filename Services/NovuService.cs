using Novu.DTO;
using Novu.DTO.Topics;
using Novu.Models;
using Novu;

namespace HaffardBankApi.Services;
public class EmailService
{
    private readonly NovuClient _novuClient;
    private readonly string _novuApiKey = "a2cb0cc1244a2400cbc61c7b3522c739"; 

    public EmailService()
    {
        _novuClient = new NovuClient(new NovuClientConfiguration{ApiKey = _novuApiKey});
    }

    


}