namespace Domain.Omie;

public class OmieErrorResult
{
    public OmieErrorResult(string faultstring, string faultcode)
    {
        this.faultstring = faultstring;
        this.faultcode = faultcode;
    }

    public string faultstring { get; private set; }
    public string faultcode { get; private set; }
}
