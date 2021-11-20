string key = await LoadKey();

DiscordSocketConfig client_config = new()
{
    DefaultRetryMode = RetryMode.Retry502 | RetryMode.RetryTimeouts,
};

var client = new DiscordSocketClient(client_config);

client.MessageReceived += async (message) =>
{
    if (message is not SocketUserMessage user_message)
        return;

    if (user_message.MentionedUsers.Any(user => user.Id == client.CurrentUser.Id))
        if (user_message.Content.Contains("ping"))
            await user_message.ReplyAsync("pong");
};

await client.LoginAsync(TokenType.Bot, key);
await client.StartAsync();
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