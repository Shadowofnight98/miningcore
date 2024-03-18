using Miningcore.Contracts;
using Miningcore.Native;

namespace Miningcore.Crypto.Hashing.Algorithms;

[Identifier("fishhash")]
public unsafe class FishHash : IHashAlgorithm
{
    private IntPtr handle = IntPtr.Zero;
    private readonly object genLock = new();

    public FishHash()
    {
        lock(genLock)
        {
            this.handle = Multihash.fishhashGetContext();
        }
    }

    public void Digest(ReadOnlySpan<byte> data, Span<byte> result, params object[] extra)
    {
        Contract.Requires<ArgumentException>(this.handle != IntPtr.Zero);
        Contract.Requires<ArgumentException>(result.Length >= 32);

        fixed(byte* input = data)
        {
            fixed (byte* output = result)
            {
                Multihash.fishhash(output, this.handle, input, (uint) data.Length);
            }
        }
    }
}