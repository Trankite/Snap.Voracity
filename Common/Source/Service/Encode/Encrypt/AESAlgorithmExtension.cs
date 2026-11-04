namespace Common.Source.Service.Encode.Encrypt
{
    public static class AESAlgorithmExtension
    {
        public static string EncryptToBase64String(this AESAlgorithm algorithm, byte[] plaintext)
        {
            int offset = algorithm.NonceSize + algorithm.TagSize;
            Span<byte> ciphertext = new byte[offset + plaintext.Length];
            algorithm.Encrypt(plaintext, ciphertext[offset..]);
            algorithm.Nonce.CopyTo(ciphertext);
            algorithm.Tag.CopyTo(ciphertext[algorithm.NonceSize..]);
            return Convert.ToBase64String(ciphertext);
        }

        public static byte[] DecryptFromBase64String(this AESAlgorithm algorithm, string data)
        {
            int offset = algorithm.NonceSize + algorithm.TagSize;
            ReadOnlySpan<byte> plaintext = Convert.FromBase64String(data);
            ReadOnlySpan<byte> nonce = plaintext[..algorithm.NonceSize];
            ReadOnlySpan<byte> tag = plaintext[algorithm.NonceSize..offset];
            ReadOnlySpan<byte> ciphertext = plaintext[offset..];
            return algorithm.Decrypt(nonce, ciphertext, tag);
        }
    }
}