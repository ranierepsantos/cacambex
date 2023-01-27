namespace CTRs.Models.SolicitarCTR;

[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2003/05/soap-envelope")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2003/05/soap-envelope", IsNullable = false)]
public partial class Envelope
{

    private EnvelopeBody bodyField;

    public EnvelopeBody Body
    {
        get
        {
            return this.bodyField;
        }
        set
        {
            this.bodyField = value;
        }
    }
}

[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2003/05/soap-envelope")]
public partial class EnvelopeBody
{

    private SolicitaCTRResponse solicitaCTRResponseField;

    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/")]
    public SolicitaCTRResponse SolicitaCTRResponse
    {
        get
        {
            return this.solicitaCTRResponseField;
        }
        set
        {
            this.solicitaCTRResponseField = value;
        }
    }
}

[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://tempuri.org/")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://tempuri.org/", IsNullable = false)]
public partial class SolicitaCTRResponse
{

    private SolicitaCTRResponseSolicitaCTRResult solicitaCTRResultField;

    public SolicitaCTRResponseSolicitaCTRResult SolicitaCTRResult
    {
        get
        {
            return this.solicitaCTRResultField;
        }
        set
        {
            this.solicitaCTRResultField = value;
        }
    }
}

[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://tempuri.org/")]
public partial class SolicitaCTRResponseSolicitaCTRResult
{

    private resultado resultadoField;

    [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
    public resultado resultado
    {
        get
        {
            return this.resultadoField;
        }
        set
        {
            this.resultadoField = value;
        }
    }
}

[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class resultado
{

    private byte codigoField;
    private string id_ctr;
    private string mensagemField;
    private string numero_CTRField;

    public string ID_CTR
    {
        get
        {
            return this.id_ctr;
        }
        set
        {
            this.id_ctr = value;
        }
    }
    public byte codigo
    {
        get
        {
            return this.codigoField;
        }
        set
        {
            this.codigoField = value;
        }
    }

    public string mensagem
    {
        get
        {
            return this.mensagemField;
        }
        set
        {
            this.mensagemField = value;
        }
    }

    public string numero_CTR
    {
        get
        {
            return this.numero_CTRField;
        }
        set
        {
            this.numero_CTRField = value;
        }
    }
}

