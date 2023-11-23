namespace Domain.Omie;
public class OmieConfigurations
{
    public OmieConfigurations(string oMIE_URL, string aPP_KEY, string aPP_SECRET, string oMIE_URL_TEST = "https://n8n.agenciadynamics.com.br/webhook-test/")
    {
        OMIE_URL = oMIE_URL;
        OMIE_URL_TEST = oMIE_URL_TEST;
        APP_KEY = aPP_KEY;
        APP_SECRET = aPP_SECRET;
    }

    public string OMIE_CALL { get; set; } = string.Empty;
    public string OMIE_URL { get; set; } = string.Empty;
    public string OMIE_URL_TEST { get; set; } = "https://n8n.agenciadynamics.com.br/";
    public string APP_KEY { get; set; } = string.Empty;
    public string APP_SECRET { get; set; } = string.Empty;
}
