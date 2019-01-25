$namespace = "packt"
$dockerRegistry = "localhost:5000"
$dockerusername = ""
$dockerpassword = ""
$pullSecret = "regsecret"
$fluentdConfigFile= '.\k8s\infra\efk\fluentd.conf'
$releaseVersion = "0.1.0"

Write-Host "Building docker images" -ForegroundColor Yellow
docker build -t $dockerRegistry/customerservice:$releaseVersion ../Customers.Web/
docker build -t $dockerRegistry/orderservice:$releaseVersion ../Orders.Web/
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build docker images." -ForegroundColor Red
    [Environment]::Exit(1)
} else {
    Write-Host "Successfully built docker images." -ForegroundColor Green
}

Write-Host "Publishing docker images" -ForegroundColor Yellow
docker push $dockerRegistry/customerservice:$releaseVersion
docker push $dockerRegistry/orderservice:$releaseVersion
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to publish docker images." -ForegroundColor Red
    [Environment]::Exit(1)
} else {
    Write-Host "Successfully published docker images." -ForegroundColor Green
}

Write-Host "Creating namespace in k8s" -ForegroundColor Yellow
& kubectl  create namespace $namespace 
& kubectl  get namespace $namespace
if ($LASTEXITCODE -ne 0) {
    Write-Host "$namespace cannot be created." -ForegroundColor Red
    [Environment]::Exit(1)
} else {
    Write-Host "Successfully created namespace $namespace" -ForegroundColor Green
}

Write-Host "Setup k8s requirements" -ForegroundColor Yellow
& kubectl config set-context $(& kubectl  config current-context) --namespace=$namespace
& kubectl create secret docker-registry $pullSecret --docker-server=$dockerRegistry --docker-username=$dockerusername --docker-password=$dockerpassword --docker-email="example@gmail.com" --namespace=$namespace
& kubectl create configmap fluentd-config --from-file=fluent.conf=$fluentdConfigFile -n $namespace
& kubectl get secret $pullSecret -n $namespace
If ($LASTEXITCODE -ne 0) {
    Write-Host "$pullSecret cannot be created in namespace '$namespace'." -ForegroundColor Red
    [Environment]::Exit(1)
} else {
    Write-Host "Successfully created secret $pullSecret" -ForegroundColor Green
}

Write-Host "Starting k8s deployment" -ForegroundColor Yellow
& kubectl apply -f "$PSScriptRoot\k8\infra"
& kubectl apply -f "$PSScriptRoot\k8s\infra\efk"
& kubectl apply -f "$PSScriptRoot\k8s"
If ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to create deployments in k8s" -ForegroundColor Red
    [Environment]::Exit(1)
} else {
    Write-Host "Completed deployments in k8s" -ForegroundColor Green
}
