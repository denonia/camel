namespace Camel.Bancho.Services.Interfaces;

public interface ICryptoService
{
    public (string[], string) DecryptRijndaelData(byte[] iv, byte[] decryptionKey, byte[] scoreData, byte[] clientHash);
}