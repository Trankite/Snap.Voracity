using StarRailDamage.Source.Extension;
using System.Security.Cryptography;

namespace StarRailDamage.Source.Service.Encode.Encrypt
{
    public sealed class AESAlgorithm : IDisposable
    {
        private readonly AesGcm Algorithm;

        public byte[] Key { get; }

        public byte[] Nonce { get; }

        public byte[] Tag { get; }

        public AESAlgorithm(byte[] key, int nonceLength = 12, int tagLength = 16)
        {
            Key = key;
            Nonce = new byte[nonceLength];
            Algorithm = new AesGcm(key, tagLength);
            Tag = new byte[tagLength];
        }

        public byte[] GetEncrypt(ReadOnlySpan<byte> plaintext, ReadOnlySpan<byte> associatedData = default)
        {
            return GetEncrypt(Nonce, plaintext, Tag, associatedData);
        }

        public byte[] GetEncrypt(ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> plaintext, Span<byte> tag, ReadOnlySpan<byte> associatedData = default)
        {
            byte[] Ciphertext = new byte[plaintext.Length];
            Encrypt(nonce, plaintext, Ciphertext, tag, associatedData);
            return Ciphertext;
        }

        public void Encrypt(ReadOnlySpan<byte> plaintext, Span<byte> ciphertext, ReadOnlySpan<byte> associatedData = default)
        {
            Encrypt(Nonce, plaintext, ciphertext, Tag, associatedData);
        }

        public void Encrypt(ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> plaintext, Span<byte> ciphertext, Span<byte> tag, ReadOnlySpan<byte> associatedData = default)
        {
            RandomNumberGenerator.Fill(Nonce);
            Algorithm.Encrypt(nonce, plaintext, ciphertext, tag, associatedData);
        }

        public byte[] GetDecrypt(ReadOnlySpan<byte> ciphertext, ReadOnlySpan<byte> associatedData = default)
        {
            return GetDecrypt(Nonce, ciphertext, Tag, associatedData);
        }

        public byte[] GetDecrypt(ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> ciphertext, ReadOnlySpan<byte> tag, ReadOnlySpan<byte> associatedData = default)
        {
            byte[] Plaintext = new byte[ciphertext.Length];
            Decrypt(nonce, ciphertext, tag, Plaintext, associatedData);
            return Plaintext;
        }

        public void Decrypt(ReadOnlySpan<byte> ciphertext, Span<byte> plaintext, ReadOnlySpan<byte> associatedData = default)
        {
            Decrypt(Nonce, ciphertext, Tag, plaintext, associatedData);
        }

        public void Decrypt(ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> ciphertext, ReadOnlySpan<byte> tag, Span<byte> plaintext, ReadOnlySpan<byte> associatedData = default)
        {
            Algorithm.Decrypt(nonce, ciphertext, tag, plaintext, associatedData);
        }

        public AESAlgorithm SetNonce(byte[] value)
        {
            return this.Configure(Self => value.FillTo(Self.Nonce));
        }

        public AESAlgorithm SetTag(byte[] value)
        {
            return this.Configure(Self => value.FillTo(Self.Tag));
        }

        public void Dispose()
        {
            Algorithm.Dispose();
        }
    }
}