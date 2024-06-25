namespace APBD_PROJEKT.model;

public class Contract
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client Client { get; set; }
    public int SoftwareId { get; set; }
    public Software Software { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public bool IsPaid { get; set; }
    public bool IsSigned => IsPaid;
    public int SupportYears { get; set; }
}