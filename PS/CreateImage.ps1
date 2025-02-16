param (
    [string]$DOCKER_HUB_LOGIN,
    [string]$DOCKER_IMAGE_NAME,
    [string]$DOCKER_IMAGE_VERSION,
    [string]$PROJECT_DOCKER_PATH
)

# Create Docker Image
docker build -t ${DOCKER_HUB_LOGIN}/${DOCKER_IMAGE_NAME}:${DOCKER_IMAGE_VERSION} -f ${PROJECT_DOCKER_PATH} .

# Push docker Image
docker push ${DOCKER_HUB_LOGIN}/${DOCKER_IMAGE_NAME}:${DOCKER_IMAGE_VERSION}

Write-Host "Build and push Docker Image completed successfully."