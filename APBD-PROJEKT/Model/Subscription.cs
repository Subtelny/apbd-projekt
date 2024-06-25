namespace APBD_PROJEKT.model;

public class Subscription
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client Client { get; set; }
    public int SoftwareId { get; set; }
    public Software Software { get; set; }
    public string RenewalPeriod { get; set; }  // ex: "monthly", "yearly"
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime NextRenewalDate { get; set; }
    public bool IsActive { get; set; }
}
