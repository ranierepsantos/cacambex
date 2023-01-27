namespace Domain.Compartilhado;

public static class MD5Hash
{
    public static string Encrypt(this string password)
    {
        try
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(inputBytes);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error:", nameof(ex));
        }
    }
}
