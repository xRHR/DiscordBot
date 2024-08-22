git pull
docker rmi discord-bot:0.1
docker build -t discord-bot:0.1 -f ./DiscordBot/Dockerfile .
docker run --rm --name discord-bot -e DISCORD_TOKEN=$DISCORD_TOKEN discord-bot:0.1