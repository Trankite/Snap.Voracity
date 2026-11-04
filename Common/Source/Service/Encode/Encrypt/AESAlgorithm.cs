using System.Security.Cryptography;

namespace Common.Source.Service.Encode.Encrypt
{
    public sealed class AESAlgorithm : IDisposable
    {
        private readonly AesGcm Algorithm;

        public byte[] Tag { get; }

        public int TagSize => Tag.Length;

        public byte[] Nonce { get; }

        public int NonceSize => Nonce.Length;

        private AESAlgorithm(AesGcm algorithm, byte[] nonce, byte[] tag)
        {
            Tag = tag;
            Nonce = nonce;
            Algorithm = algorithm;
        }

        public static AESAlgorithm Create(byte[] key, int nonceSize = 12, int tagSize = 16)
        {
            return new AESAlgorithm(new AesGcm(key, tagSize), new byte[nonceSize], new byte[tagSize]);
        }

        public byte[] Encrypt(ReadOnlySpan<byte> plaintext, ReadOnlySpan<byte> associatedData = default)
        {
            byte[] ciphertext = new byte[plaintext.Length];
            Encrypt(ciphertext, plaintext, associatedData);
            return ciphertext;
        }

        public void Encrypt(Span<byte> ciphertext, ReadOnlySpan<byte> plaintext, ReadOnlySpan<byte> associatedData = default)
        {
            RandomNumberGenerator.Fill(Nonce);
            Algorithm.Encrypt(Nonce, plaintext, ciphertext, Tag, associatedData);
        }

        public byte[] Decrypt(ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> ciphertext, ReadOnlySpan<byte> tag, ReadOnlySpan<byte> associatedData = default)
        {
            byte[] plaintext = new byte[ciphertext.Length];
            Decrypt(nonce, ciphertext, tag, plaintext, associatedData);
            return plaintext;
        }

        public void Decrypt(ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> ciphertext, ReadOnlySpan<byte> tag, Span<byte> plaintext, ReadOnlySpan<byte> associatedData = default)
        {
            Algorithm.Decrypt(nonce, ciphertext, tag, plaintext, associatedData);
        }

        public void Dispose()
        {
            Algorithm.Dispose();
        }
    }
}