using StarRailDamage.Source.Extension;

namespace StarRailDamage.Source.Service.Encode.Encrypt
{
    public static class AESAlgorithmExtension
    {
        public static string EncryptToBase64String(this AESAlgorithm algorithm, byte[] plaintext)
        {
            int Offset = algorithm.Nonce.Length + algorithm.Tag.Length;
            byte[] Ciphertext = new byte[Offset + plaintext.Length];
            algorithm.Encrypt(plaintext, Ciphertext.AsSpan()[Offset..]);
            Ciphertext.FillFrom(algorithm.Nonce, algorithm.Tag);
            return Convert.ToBase64String(Ciphertext);
        }

        public static byte[] DecryptFromBase64String(this AESAlgorithm algorithm, string data)
        {
            ReadOnlySpan<byte> Original = Convert.FromBase64String(data);
            ReadOnlySpan<byte> Nonce = Original[..algorithm.Nonce.Length];
            ReadOnlySpan<byte> Tag = Original.Slice(algorithm.Nonce.Length, algorithm.Tag.Length);
            ReadOnlySpan<byte> Ciphertext = Original[(algorithm.Nonce.Length + algorithm.Tag.Length)..];
            return algorithm.GetDecrypt(Nonce, Ciphertext, Tag);
        }
    }
}