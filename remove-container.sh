ssh-keygen -R '[localhost]:58222'
pkill -f "ssh -i $(pwd)/ssh_tunnel/tunnel_rsa tunnel@localhost -p 58222"
docker stop dotnet_webapi_postgresql_entityframeworkcore -t 1
docker rm --force dotnet_webapi_postgresql_entityframeworkcore
docker rmi --force dotnet_webapi_postgresql_entityframeworkcore
docker stop dotnet_webapi_postgresql_entityframeworkcore_ssh -t 1
docker rm --force dotnet_webapi_postgresql_entityframeworkcore_ssh
docker rmi --force dotnet_webapi_postgresql_entityframeworkcore_ssh
docker rmi --force public.ecr.aws/o2c0x5x8/community-images-backup:lscr.io-linuxserver-openssh-server
docker rmi --force public.ecr.aws/o2c0x5x8/application-base:dotnet-webapi-postgresql-entityframeworkcore
docker system prune
docker volume prune