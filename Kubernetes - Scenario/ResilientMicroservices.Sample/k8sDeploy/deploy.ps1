$namespace = "packt"
$dockerRegistry = "localhost:5000"
$releaseVersion = "0.1.0"
$ErrorActionPreference = "Stop"

Write-Host "Building docker images" -ForegroundColor Yellow
docker build -t $dockerRegistry/customerservice:$releaseVersion ..\ -f ..\Customers.Web\DockerFile
docker build -t $dockerRegistry/orderservice:$releaseVersion ..\ -f ..\Orders.Web\DockerFile
if ($LASTEXITCODE -ne 0) {
    throw "Failed to build docker images."
} else {
    Write-Host "Successfully built docker images." -ForegroundColor Green
}

Write-Host "Publishing docker images" -ForegroundColor Yellow
docker push $dockerRegistry/customerservice:$releaseVersion
docker push $dockerRegistry/orderservice:$releaseVersion
if ($LASTEXITCODE -ne 0) {
    throw "Failed to publish docker images."
} else {
    Write-Host "Successfully published docker images." -ForegroundColor Green
}

Write-Host "Creating namespace in k8s" -ForegroundColor Yellow
& kubectl  create namespace $namespace 
& kubectl  get namespace $namespace
if ($LASTEXITCODE -ne 0) {
    throw "$namespace cannot be created."
} else {
    Write-Host "Successfully created namespace $namespace" -ForegroundColor Green
}

Write-Host "Setup k8s requirements" -ForegroundColor Yellow
& kubectl config set-context $(& kubectl  config current-context) --namespace=$namespace

Write-Host "Starting k8s deployment" -ForegroundColor Yellow
& kubectl apply -f "$PSScriptRoot\k8s"
If ($LASTEXITCODE -ne 0) {
    throw "Failed to create deployments in k8s"
} else {
    Write-Host "Completed deployments in k8s" -ForegroundColor Green
}
