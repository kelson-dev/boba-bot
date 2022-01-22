using Boba.Discord;
using Discord;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;

string key = await LoadKey();

DiscordSocketConfig client_config = new DiscordSocketConfig()
{
    DefaultRetryMode = RetryMode.Retry502 | RetryMode.RetryTimeouts,
};

var client = new DiscordSocketClient(client_config);

new MyBot(client);

await client.LoginAsync(TokenType.Bot, key);
await Console.Out.WriteLineAsync("Logged in");
await client.StartAsync();
await Console.Out.WriteLineAsync("Client started");
await Console.In.ReadLineAsync();

static async Task<string> LoadKey()
{
    const string credentials_file = "bot.credentials";
    if (File.Exists(credentials_file))
        return File.ReadAllText(credentials_file);
    await Console.Out.WriteLineAsync("Enter token: ");
    string? key = await Console.In.ReadLineAsync();
    await File.WriteAllTextAsync(credentials_file, key);
    return key!;
}