echo ===$ git pull
git pull

echo ===$ docker rmi discord-bot
docker rmi discord-bot

echo ===$ docker build -t discord-bot -f ./DiscordBot/Dockerfile .
docker build -t discord-bot -f ./DiscordBot/Dockerfile .

echo ===$ docker run --rm --name discord-bot -e DISCORD_TOKEN=$DISCORD_TOKEN discord-bot
docker run --rm --name discord-bot -e DISCORD_TOKEN=$DISCORD_TOKEN discord-bot