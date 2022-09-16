docker build -t webshopcontainers.azurecr.io/webshop/worker-low -f .\GrpcQueryServer\Dockerfile .

docker push webshopcontainers.azurecr.io/webshop/worker-low

az acr login --name webshopcontainers.azurecr.io