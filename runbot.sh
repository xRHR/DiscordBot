echo ===$ git pull
git pull

echo ===$ docker rmi discord-bot
docker rmi discord-bot

echo ===$ docker build -t discord-bot -f ./DiscordBot/Dockerfile .
docker build -t discord-bot -f ./DiscordBot/Dockerfile .

echo ===$ docker run --rm --name discord-bot -e DISCORD_TOKEN=$DISCORD_TOKEN LAVALINK_HOST_URL=$LAVALINK_HOST_URL LAVALINK_HOST_PASSWORD=$LAVALINK_HOST_PASSWORD discord-bot
docker run --rm --name discord-bot -e DISCORD_TOKEN=$DISCORD_TOKEN -e LAVALINK_HOST_URL=$LAVALINK_HOST_URL -e LAVALINK_HOST_PASSWORD=$LAVALINK_HOST_PASSWORD discord-bot