namespace CTRs.Models.EnviarCacambaObra;

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

    private EnviaCacambaObra_LocalResponse enviaCacambaObra_LocalResponseField;

    [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://tempuri.org/")]
    public EnviaCacambaObra_LocalResponse EnviaCacambaObra_LocalResponse
    {
        get
        {
            return this.enviaCacambaObra_LocalResponseField;
        }
        set
        {
            this.enviaCacambaObra_LocalResponseField = value;
        }
    }
}

[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://tempuri.org/")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "http://tempuri.org/", IsNullable = false)]
public partial class EnviaCacambaObra_LocalResponse
{

    private EnviaCacambaObra_LocalResponseEnviaCacambaObra_LocalResult enviaCacambaObra_LocalResultField;

    public EnviaCacambaObra_LocalResponseEnviaCacambaObra_LocalResult EnviaCacambaObra_LocalResult
    {
        get
        {
            return this.enviaCacambaObra_LocalResultField;
        }
        set
        {
            this.enviaCacambaObra_LocalResultField = value;
        }
    }
}

[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://tempuri.org/")]
public partial class EnviaCacambaObra_LocalResponseEnviaCacambaObra_LocalResult
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

    private string mensagemField;

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
}

