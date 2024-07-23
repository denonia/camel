using System.Text;
using Camel.Bancho.Services.Interfaces;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace Camel.Bancho.Services;

public class CryptoService : ICryptoService
{
    private const string LegacyDecryptionKey = "h89f2-890h2h89b34g-h80g134n90133";
    private const string NewDecryptionKey = "osu!-scoreburgr---------";

    private readonly CbcBlockCipher _blockCipher;

    public CryptoService()
    {
        var engine = new RijndaelEngine(256);
        _blockCipher = new CbcBlockCipher(engine);
    }

    public (string[], string) DecryptRijndaelData(byte[] iv, string? osuVersion, byte[] scoreData, byte[] clientHash)
    {
        var key = string.IsNullOrEmpty(osuVersion) ? LegacyDecryptionKey : NewDecryptionKey + osuVersion;
        var keyParam = new KeyParameter(Encoding.UTF8.GetBytes(key));
        var keyParamWithIV = new ParametersWithIV(keyParam, iv, 0, 32);

        var scoreDataBytes = Decrypt(_blockCipher, keyParamWithIV, scoreData);
        var clientHashBytes = Decrypt(_blockCipher, keyParamWithIV, clientHash);
        return (Encoding.UTF8.GetString(scoreDataBytes).Split(':'), Encoding.UTF8.GetString(clientHashBytes));
    }

    private byte[] Decrypt(CbcBlockCipher blockCipher, ParametersWithIV keyParamWithIV, byte[] inputBytes)
    {
        var cipher = new PaddedBufferedBlockCipher(blockCipher, new Pkcs7Padding());
        cipher.Init(false, keyParamWithIV);
        var result = new byte[cipher.GetOutputSize(inputBytes.Length)];
        var length = cipher.ProcessBytes(inputBytes, result, 0);
        cipher.DoFinal(result, length);
        return result;
    }
}