echo ===$ git pull
git pull
echo ===$ docker rmi discord-bot:0.1
docker rmi discord-bot:0.1
echo ===$ docker build -t discord-bot:0.1 -f ./DiscordBot/Dockerfile .
docker build -t discord-bot:0.1 -f ./DiscordBot/Dockerfile .
echo ===$ docker run --rm --name discord-bot -e DISCORD_TOKEN=$DISCORD_TOKEN LAVALINK_HOST_URL=$LAVALINK_HOST_URL LAVALINK_HOST_PASSWORD=$LAVALINK_HOST_PASSWORD discord-bot:0.1
docker run --rm --name discord-bot -e DISCORD_TOKEN=$DISCORD_TOKEN LAVALINK_HOST_URL=$LAVALINK_HOST_URL LAVALINK_HOST_PASSWORD=$LAVALINK_HOST_PASSWORD discord-bot:0.1