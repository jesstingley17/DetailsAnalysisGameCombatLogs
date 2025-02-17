param (
    [string]$HELM_NAME,
    [string]$HELM_CHART_PATH,
    [string]$HELM_IMAGE_TAGS_PATH,
    [string]$NAMESPACE
)

# Create namespace
kubectl create ns ${NAMESPACE}

# Helm install
helm install ${HELM_NAME} ${HELM_CHART_PATH} -f ${HELM_CHART_PATH}/values.yaml -f ${HELM_IMAGE_TAGS_PATH} -n ${NAMESPACE}

Write-Host "Create new namespace and helm install completed successfully."