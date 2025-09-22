using System.Numerics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/app/{email}", async (HttpRequest request, HttpResponse response, string email) =>
{
    var q = request.Query;

    if (!q.TryGetValue("x", out var xs) || !q.TryGetValue("y", out var ys))
    {
        response.ContentType = "text/plain";
        await response.WriteAsync("NaN");
        return;
    }

    if (!BigInteger.TryParse(xs.ToString(), out BigInteger x) ||
        !BigInteger.TryParse(ys.ToString(), out BigInteger y))
    {
        response.ContentType = "text/plain";
        await response.WriteAsync("NaN");
        return;
    }
    if (x < 0 || y < 0)
    {
        response.ContentType = "text/plain";
        await response.WriteAsync("NaN");
        return;
    }

    BigInteger gcd = Gcd(x, y);

    BigInteger lcm = (x == 0 || y == 0) ? 0 : BigInteger.Divide(BigInteger.Multiply(x, y), gcd);

    response.ContentType = "text/plain";
    await response.WriteAsync(lcm.ToString());

    static BigInteger Gcd(BigInteger a, BigInteger b)
    {
        a = BigInteger.Abs(a);
        b = BigInteger.Abs(b);
        while (b != 0)
        {
            var t = a % b;
            a = b;
            b = t;
        }
        return a;
    }
});

app.Run();
