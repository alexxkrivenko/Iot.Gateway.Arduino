# https://www.appveyor.com/docs/environment-variables/

# Script static variables
$dockerImageName = 
$dockerPass = $env:DOCKER_PASS

$projectDir = $buildDir + "\";


# Build docker image
Write-Host "Build docker image" -ForegroundColor Green
docker build -t $env:DOCKER_USER/$env:DOCKER_IMAGE_NAME .

# Done
Write-Host "Done!" -ForegroundColor Green