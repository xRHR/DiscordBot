using System;
using System.Threading.Tasks;
using Discord.Interactions;
using global::Lavalink4NET.Players.Vote;
using global::Lavalink4NET.Players;
using global::Lavalink4NET.Rest.Entities.Tracks;
using global::Lavalink4NET;
using Lavalink4NET.DiscordNet;
using Lavalink4NET.Players;
using Lavalink4NET.Players.Vote;
using Lavalink4NET.Rest.Entities.Tracks;

namespace DiscordBot
{
    
    /// <summary>
    ///     Presents some of the main features of the Lavalink4NET-Library.
    /// </summary>
    [RequireContext(ContextType.Guild)]
    public sealed class MusicModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IAudioService _audioService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MusicModule"/> class.
        /// </summary>
        /// <param name="audioService">the audio service</param>
        /// <exception cref="ArgumentNullException">
        ///     thrown if the specified <paramref name="audioService"/> is <see langword="null"/>.
        /// </exception>
        public MusicModule(IAudioService audioService)
        {
            ArgumentNullException.ThrowIfNull(audioService);

            _audioService = audioService;
        }

        /// <summary>
        ///     Disconnects from the current voice channel connected to asynchronously.
        /// </summary>
        /// <returns>a task that represents the asynchronous operation</returns>
        [SlashCommand("съебни", "бот ливнет из голосового канала", runMode: RunMode.Async)]
        public async Task Disconnect()
        {
            if (!this._audioService.Players.HasPlayer(Context.Guild.Id))
            {
                await RespondAsync(this.RandomFuckOffString()).ConfigureAwait(false);
                return;
            }
            var player = await GetPlayerAsync().ConfigureAwait(false);

            if (player is null)
            {
                return;
            }
            await player.DisconnectAsync().ConfigureAwait(false);
            await RespondAsync("наберешь").ConfigureAwait(false);
        }

        /// <summary>
        ///     Plays music asynchronously.
        /// </summary>
        /// <param name="query">the search query</param>
        /// <returns>a task that represents the asynchronous operation</returns>
        [SlashCommand("вруби", description: "добавляет трек в очередь", runMode: RunMode.Async)]
        public async Task Play(string query)
        {
            await DeferAsync().ConfigureAwait(false);

            var player = await GetPlayerAsync(connectToVoiceChannel: true).ConfigureAwait(false);

            if (player is null)
            {
                return;
            }

            var track = await _audioService.Tracks
                .LoadTrackAsync(query, TrackSearchMode.YouTube)
                .ConfigureAwait(false);
            if (track is null)
            {
                await FollowupAsync("такой песни нет").ConfigureAwait(false);
                return;
            }

            var position = await player.PlayAsync(track).ConfigureAwait(false);

            if (position is 0)
            {
                await FollowupAsync($"врубаю: {track.Uri}").ConfigureAwait(false);
            }
            else
            {
                await FollowupAsync($"добавил в очередь: {track.Uri}").ConfigureAwait(false);
            }
        }

        /// <summary>
        ///     Shows the track position asynchronously.
        /// </summary>
        /// <returns>a task that represents the asynchronous operation</returns>
        [SlashCommand("позиция", description: "показывает текующую позицию в треке", runMode: RunMode.Async)]
        public async Task Position()
        {
            if (!this._audioService.Players.HasPlayer(Context.Guild.Id))
            {
                await RespondAsync(this.RandomFuckOffString()).ConfigureAwait(false);
                return;
            }
            var player = await GetPlayerAsync(connectToVoiceChannel: false).ConfigureAwait(false);

            if (player is null)
            {
                return;
            }

            if (player.CurrentItem is null)
            {
                await RespondAsync("ничего не играет").ConfigureAwait(false);
                return;
            }

            await RespondAsync($"позиция: {player.Position?.Position} / {player.CurrentTrack.Duration}.").ConfigureAwait(false);
        }

        /// <summary>
        ///     Stops the current track asynchronously.
        /// </summary>
        /// <returns>a task that represents the asynchronous operation</returns>
        [SlashCommand("стоп", description: "останавливает текущий трек", runMode: RunMode.Async)]
        public async Task Stop()
        {
            if (!this._audioService.Players.HasPlayer(Context.Guild.Id))
            {
                await RespondAsync(this.RandomFuckOffString()).ConfigureAwait(false);
                return;
            }
            var player = await GetPlayerAsync(connectToVoiceChannel: false);

            if (player is null)
            {
                return;
            }

            if (player.CurrentItem is null)
            {
                await RespondAsync("ничего не играет").ConfigureAwait(false);
                return;
            }

            await player.StopAsync().ConfigureAwait(false);
            await RespondAsync("остановил шарманку").ConfigureAwait(false);
        }

        /// <summary>
        ///     Updates the player volume asynchronously.
        /// </summary>
        /// <param name="volume">the volume (1 - 1000)</param>
        /// <returns>a task that represents the asynchronous operation</returns>
        [SlashCommand("громкость", description: "устанавливает громкость (0 - 1000%)", runMode: RunMode.Async)]
        public async Task Volume(int volume = 100)
        {
            if (volume is > 1000 or < 0)
            {
                await RespondAsync("громкость должна быть в диапазоне 0% - 1000%").ConfigureAwait(false);
                return;
            }

            var player = await GetPlayerAsync(connectToVoiceChannel: false).ConfigureAwait(false);

            if (player is null)
            {
                return;
            }

            await player.SetVolumeAsync(volume / 100f).ConfigureAwait(false);
            await RespondAsync($"установлена громкость {volume}%").ConfigureAwait(false);
        }

        [SlashCommand("скип", description: "скипает хуйню", runMode: RunMode.Async)]
        public async Task Skip()
        {
            if (!this._audioService.Players.HasPlayer(Context.Guild.Id))
            {
                await RespondAsync(this.RandomFuckOffString()).ConfigureAwait(false);
                return;
            }
            var player = await GetPlayerAsync(connectToVoiceChannel: false);

            if (player is null)
            {
                return;
            }

            if (player.CurrentItem is null)
            {
                await RespondAsync("ничего не играет").ConfigureAwait(false);
                return;
            }

            await player.SkipAsync().ConfigureAwait(false);

            var track = player.CurrentItem;

            if (track is not null)
            {
                await RespondAsync($"скипнул, сейчас играет: {track.Track!.Uri}").ConfigureAwait(false);
            }
            else
            {
                await RespondAsync("скипнул, играть нечего").ConfigureAwait(false);
            }
        }

        [SlashCommand("пауза", description: "", runMode: RunMode.Async)]
        public async Task PauseAsync()
        {
            if (!this._audioService.Players.HasPlayer(Context.Guild.Id))
            {
                await RespondAsync(this.RandomFuckOffString()).ConfigureAwait(false);
                return;
            }
            var player = await GetPlayerAsync(connectToVoiceChannel: false);

            if (player is null)
            {
                return;
            }

            if (player.State is PlayerState.Paused)
            {
                await RespondAsync("уже на паузе").ConfigureAwait(false);
                return;
            }

            await player.PauseAsync().ConfigureAwait(false);
            await RespondAsync("поставил паузу").ConfigureAwait(false);
        }

        [SlashCommand("рестарт", description: "снимает паузу", runMode: RunMode.Async)]
        public async Task ResumeAsync()
        {
            if (!this._audioService.Players.HasPlayer(Context.Guild.Id))
            {
                await RespondAsync(this.RandomFuckOffString()).ConfigureAwait(false);
                return;
            }
            var player = await GetPlayerAsync(connectToVoiceChannel: false);

            if (player is null)
            {
                return;
            }

            if (player.State is not PlayerState.Paused)
            {
                await RespondAsync("итак не на паузе").ConfigureAwait(false);
                return;
            }

            await player.ResumeAsync().ConfigureAwait(false);
            await RespondAsync("играю дальше").ConfigureAwait(false);
        }

        /// <summary>
        ///     Gets the guild player asynchronously.
        /// </summary>
        /// <param name="connectToVoiceChannel">
        ///     a value indicating whether to connect to a voice channel
        /// </param>
        /// <returns>
        ///     a task that represents the asynchronous operation. The task result is the lavalink player.
        /// </returns>
        private async ValueTask<VoteLavalinkPlayer?> GetPlayerAsync(bool connectToVoiceChannel = true)
        {
            Console.WriteLine("GetPlayerAsync started");
            var retrieveOptions = new PlayerRetrieveOptions(
                ChannelBehavior: connectToVoiceChannel ? PlayerChannelBehavior.Join : PlayerChannelBehavior.None);

            var result = await _audioService.Players
                .RetrieveAsync(Context, playerFactory: PlayerFactory.Vote, retrieveOptions)
                .ConfigureAwait(false);
            Console.WriteLine("GetPlayerAsync RetrieveAsync");

            if (!result.IsSuccess)
            {
                var errorMessage = result.Status switch
                {
                    PlayerRetrieveStatus.UserNotInVoiceChannel => "в войс зайди придурошный",
                    PlayerRetrieveStatus.BotNotConnected => "бот не соединен или чето такое я хуй его",
                    _ => "неизвестная ошибка",
                };

                await FollowupAsync(errorMessage).ConfigureAwait(false);
                return null;
            }

            return result.Player;
        }

        private List<string> fuckOffStrings = new List<string>
        {
            "ебнутый?",
            "ща подожди 15 минут",
            "ай донт ноу",
            "отъебись",
            "ежжи бля",
            "че нахуй",
            "э бля ты на кого балоны катишь",
        };
        private string RandomFuckOffString()
        {
            Random r = new Random();
            return fuckOffStrings[r.Next(this.fuckOffStrings.Count - 1)];
        }
    }
}
