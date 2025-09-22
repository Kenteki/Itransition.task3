using System.Numerics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// �������: /app/{sanitizedEmail}
// sanitizedEmail = ���� email � ������� ���� ��������, ����� a-z0-9, �� "_"
app.MapGet("/app/{sanitizedEmail}", async (HttpRequest req, HttpResponse res, string sanitizedEmail) =>
{
    var q = req.Query;

    // ��������� ������� ���������� x � y
    if (!q.TryGetValue("x", out var xs) || !q.TryGetValue("y", out var ys))
    {
        res.ContentType = "text/plain";
        await res.WriteAsync("NaN");
        return;
    }

    if (!BigInteger.TryParse(xs.ToString(), out BigInteger x) ||
        !BigInteger.TryParse(ys.ToString(), out BigInteger y))
    {
        res.ContentType = "text/plain";
        await res.WriteAsync("NaN");
        return;
    }
    if (x < 0 || y < 0)
    {
        res.ContentType = "text/plain";
        await res.WriteAsync("NaN");
        return;
    }

    BigInteger gcd = Gcd(x, y);

    BigInteger lcm = (x == 0 || y == 0) ? 0 : BigInteger.Divide(BigInteger.Multiply(x, y), gcd);

    res.ContentType = "text/plain";
    await res.WriteAsync(lcm.ToString());

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
