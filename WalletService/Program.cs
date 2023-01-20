
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http.Headers;
using RestSharp;
using System.ComponentModel.DataAnnotations;
using System.Formats.Asn1;
using System.Globalization;
using System.Reflection;
using WalletService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Aud"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/GetWallet", [Authorize] async (GetWalletModel wallet) =>
{
    RequestHeader requestHeader = new RequestHeader();
    RestClient restClient = new RestClient();
    RestRequest request = new RestRequest("https://api.bscscan.com/" +
        "api?module=account&action=balance&" +
        "address="+ wallet.wallet);
    RestRequest request2 = new RestRequest("https://api.bscscan.com/" +
        "api?module=account&action=tokenbalance&" +
        "contractaddress=0x1236a887ef31B4d32E1F0a2b5e4531F52CeC7E75&" +
        "address="+ wallet.wallet + "&" +
        "tag=latest");
    restClient.AddDefaultHeaders(requestHeader.BSCRequestHeader);
    var BNB_Balance = await restClient.ExecutePostAsync(request);
    var Gami_balance = await restClient.ExecutePostAsync(request);
    Int32 responseHttpStatusCode = (Int32)BNB_Balance.StatusCode;
    return new[] { BNB_Balance.Content, Gami_balance.Content };
}).WithName("GetWallet");

app.MapPost("/CsvTest", async (CsvRequestModel data) =>
{
    //Kütüphane kullanmadan basit bir yöntemle çektim.
    var textReader = new StringReader(data.data);

    CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = false,
        Delimiter = ","
    };

    var csvr = new CsvReader(textReader, csvConfiguration);
    var records = csvr.GetRecords<CsvOutput>().ToList();

    CsvOutput csvOutput = new CsvOutput();
    csvOutput.OutA=records.First().OutA;
    var set = new HashSet<int>();
    foreach (var record in records)
    {
        if (csvOutput.OutA > record.OutA)
            csvOutput.OutA = record.OutA;
        csvOutput.OutB += record.OutB;
        set.Add(record.OutC);
    }
    csvOutput.OutC = set.Count();

   // List<int> field = data.data.Split("\n")?.Select(Int32.Parse)?.ToList();

    //CsvColumn csvColumn = new CsvColumn();
    //List<string> csvList = data.data.Split('\n').ToList();
    

    return csvOutput;
}).WithName("CsvTest");
app.UseAuthentication();
app.UseAuthorization();
app.Run();

internal record CsvRequestModel([Required] string data);
internal record GetWalletModel([Required] string wallet);