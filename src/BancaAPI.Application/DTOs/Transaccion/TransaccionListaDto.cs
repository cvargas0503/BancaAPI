public class TransaccionListaDto
{
    public Guid Id { get; set; }
    public string Tipo { get; set; }
    public decimal Monto { get; set; }
    public decimal SaldoDespues { get; set; }
    public DateTime Fecha { get; set; }
}
