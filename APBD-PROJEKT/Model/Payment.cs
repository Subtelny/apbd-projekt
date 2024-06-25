namespace APBD_PROJEKT.model;

public class Payment
{
    public int Id { get; set; }
    public int ContractId { get; set; }
    public Contract Contract { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
}
