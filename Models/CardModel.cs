namespace HaffardBankApi.Models;

public class CardModel {
    public long Id {get; set;}
    public long Pin {get; set;}
    public bool IsActive {get; set;}
    public long ClientId {get; set;}
}