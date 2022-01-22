
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Boba.Discord
{
    public interface IMyStateMachine
    {
        IMyStateMachine Update(string message);
    }

    public class MyBot
    {
        private readonly DiscordSocketClient _client;

        private readonly Dictionary<ulong, IMyStateMachine> _stateMachines = new();

        public MyBot(DiscordSocketClient client)
        {
            _client = client;

            _client.MessageReceived += HandleMessage;
        }

        public async Task<ulong> GetThreadId(IMessage message)
        {
            while (message.Reference?.MessageId.IsSpecified ?? false)
            {
                if (await GetPreviousInThread(message) is IMessage previous)
                    message = previous;
                else
                    break;
            }
            return message.Id;
        }

        public static async Task<IMessage?> GetPreviousInThread(IMessage message)
        {
            if (message.Reference is null)
                return null;
            if (!message.Reference.MessageId.IsSpecified)
                return null;
            if (message is IUserMessage userMessage && userMessage.ReferencedMessage is not null)
                return userMessage.ReferencedMessage;
            return await message.Channel.GetMessageAsync(message.Reference.MessageId.Value);
        }

        public async Task HandleMessage(SocketMessage message)
        {
            await Console.Out.WriteLineAsync($"[{DateTimeOffset.Now:f}] {message.GetJumpUrl()}");
            if (message is not SocketUserMessage user_message)
                return;

            if (user_message.Author.IsBot)
                return;

            if (await GetPreviousInThread(user_message) is IMessage previous)
            {
                await HandleBotReply(user_message, previous);
                return;
            }
            
            if (user_message.Mentions(_client.CurrentUser.Id))
            {
                await HandleBotPing(user_message);
                return;
            }
        }

        public async Task HandleBotPing(SocketUserMessage userMessage)
        {
            if (userMessage.Content.ToLowerInvariant().Contains("what time is it"))
            {
                var now = DateTimeOffset.Now;
                var timestamp = now.ToUnixTimeSeconds();
                await userMessage.ReplyAsync($"It is <t:{timestamp}:f>", allowedMentions: AllowedMentions.None);
            }
        }

        public async Task HandleBotReply(SocketUserMessage userMessage, IMessage previous)
        {
            var thread_id = await GetThreadId(previous);
            await userMessage.ReplyAsync(
                $"Your thread id is {thread_id}",
                allowedMentions: AllowedMentions.None);

            if (_stateMachines.TryGetValue(thread_id, out var machine))
            {
                _stateMachines[thread_id] = machine.Update(userMessage.Content);
            }
        }
    }
}
