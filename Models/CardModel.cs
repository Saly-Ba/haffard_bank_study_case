namespace HaffardBankApi.Models;

public class CardModel {
    public long Id {get; set;}
    public string? Pin {get; set;}
    public bool IsActive {get; set;}
    public long ClientId {get; set;}
    public ClientModel? ClientModel {get; set;}

    public CardModel() { }
    public CardModel(string? pin, bool isActive,long clientId ,ClientModel? clientModel){
        this.Pin = pin;
        this.IsActive = isActive;
        this.ClientId = clientId;
        this.ClientModel = clientModel;
    }
}