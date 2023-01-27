using Domain.Compartilhado;

namespace Domain.NotasFiscais;

public class NotaFiscal : Entidade
{
    public NotaFiscal(string cCodIntOs, int nCodOS)
    {
        this.cCodIntOs = cCodIntOs;
        if (string.IsNullOrEmpty(cCodIntOs))
            throw new Exception("Código interno do pedido não pode ser vazio");

        this.nCodOS = nCodOS;
        if (nCodOS <= 0)
            throw new Exception("Código do pedido não pode ser vazio");
    }
    public string cCodIntOs { get; private set; }
    public int nCodOS { get; private set; }
}
