using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddDeflateCompressor("deflate");
builder.Services.AddGZipCompressor("gzip");
builder.Services.AddBrotliCompressor("brotli");
builder.Services.AddLZ4Compressor("lz4");
builder.Services.AddLZMACompressor("lzma");
builder.Services.AddLZMACompressor("zstd");
builder.Services.AddSnappierCompressor("snappy");

await builder.Build().RunAsync();
