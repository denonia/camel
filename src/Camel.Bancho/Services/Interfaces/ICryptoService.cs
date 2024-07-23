namespace Camel.Bancho.Services.Interfaces;

public interface ICryptoService
{
    public (string[], string) DecryptRijndaelData(byte[] iv, string? osuVersion, byte[] scoreData, byte[] clientHash);
}