using BlazorApp1.Components;
using EasyCompressor;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDeflateCompressor("deflate");
builder.Services.AddGZipCompressor("gzip");
builder.Services.AddBrotliCompressor("brotli");
builder.Services.AddLZ4Compressor("lz4");
builder.Services.AddLZMACompressor("lzma");
builder.Services.AddLZMACompressor("zstd");
builder.Services.AddSnappierCompressor("snappy");

builder.Services
    .AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorApp1.Client._Imports).Assembly);

app.MapGet("/CompressAndCalculateHash/{compressorName}", (string compressorName, ICompressorProvider compressorProvider) =>
{
    var bytes = Encoding.UTF8.GetBytes("test data");

    var compressor = compressorProvider.GetCompressor(compressorName);
    var compressedBytes = compressor.Compress(bytes);

    var isSame = compressor.Decompress(compressedBytes).SequenceEqual(bytes);
    if (isSame is false)
        throw new Exception("Decompressed data is not the same as the original data.");

    var hashBytes = SHA256.HashData(compressedBytes);
    return Convert.ToHexStringLower(hashBytes);
});

await app.RunAsync();

public partial class Program;