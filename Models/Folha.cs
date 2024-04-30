using System.ComponentModel.DataAnnotations;

public class Folha
{
    [Key]
    public int Id { get; set; }
    public float Valor { get; set; }
    public int Quantidade { get; set; }
    public int Mes { get; set; }
    public int Ano { get; set; }
    public int FuncionarioId { get; set; }
    public Funcionario? Funcionario { get; set; }
}