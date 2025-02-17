param (
    [string]$HELM_NAME,
    [string]$HELM_CHART_PATH,
    [string]$HELM_IMAGE_TAGS_PATH,
    [string]$NAMESPACE
)

# Helm upgrade
helm upgrade ${HELM_NAME} ${HELM_CHART_PATH} -f ${HELM_CHART_PATH}/values.yaml -f ${HELM_IMAGE_TAGS_PATH} -n ${NAMESPACE}

# Update deployment
kubectl rollout restart deploy -n ${NAMESPACE}

Write-Host "Upgrade completed successfully."