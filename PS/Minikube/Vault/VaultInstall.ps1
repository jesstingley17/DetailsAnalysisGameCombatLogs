# Variables
$podName = "vault-0"
$namespace = "vault"

# Install Vault using Helm
helm install vault hashicorp/vault -f ..\..\..\charts\vault-chart\values.yaml -n vault --create-namespace

# Apply Secrets
kubectl apply -f ..\..\..\charts\vault-chart\templates\vaultInitSecret.yaml -n vault
kubectl apply -f ..\..\..\charts\api-chart\templates\chat\chatDbSecret.yaml -n vault
kubectl apply -f ..\..\..\charts\api-chart\templates\communication\communicationDbSecret.yaml -n vault
kubectl apply -f ..\..\..\charts\api-chart\templates\authenticationSecret.yaml -n vault
kubectl apply -f ..\..\..\charts\api-chart\templates\clientSecret.yaml -n vault

# Apply Service accounts
kubectl apply -f ..\..\..\charts\vault-chart\templates\vaultSA.yaml -n vault

# Apply Jobs
kubectl apply -f ..\..\..\charts\vault-chart\templates\vaultInitAuthJob.yaml -n vault
kubectl apply -f ..\..\..\charts\vault-chart\templates\vaultInitSecretsJob.yaml -n vault

# Wait for the pod to be Ready
Write-Host "Waiting for pod '$podName' in namespace '$namespace' to be ready..."
do {
    Start-Sleep -Seconds 2
    $status = kubectl get pod $podName -n $namespace -o json | ConvertFrom-Json
    $ready = $status.status.conditions | Where-Object { $_.type -eq "Ready" } | Select-Object -ExpandProperty status
} while ($ready -ne "True")

Write-Host "Pod is ready. Running command..."

# Add Kubernetes Auth configs
$vaultCommand = 'vault write auth/kubernetes/config token_reviewer_jwt="$(cat /var/run/secrets/kubernetes.io/serviceaccount/token)" kubernetes_host="https://$KUBERNETES_PORT_443_TCP_ADDR:443" kubernetes_ca_cert=@/var/run/secrets/kubernetes.io/serviceaccount/ca.crt'
kubectl exec -it $podName -n $namespace -- /bin/sh -c "$vaultCommand"