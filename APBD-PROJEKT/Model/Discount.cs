namespace APBD_PROJEKT.model;

public class Discount
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string OfferType { get; set; }  // "upfront" lub "subscription"
    public decimal Value { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}