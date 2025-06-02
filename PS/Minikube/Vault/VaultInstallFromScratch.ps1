# Ensure Helm is installed
if (-not (Get-Command helm -ErrorAction SilentlyContinue)) {
    Write-Error "Helm is not installed or not in PATH."
    exit 1
}

# Add the HashiCorp repo if it's not already added
$repoList = helm repo list
if ($repoList -notmatch "hashicorp") {
    helm repo add hashicorp https://helm.releases.hashicorp.com
}

# Update the Helm repo
helm repo update

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

# Add Kubernetes Auth configs
kubectl exec -it vault-0 -n vault -- /bin/sh -c "vault write auth/kubernetes/config token_reviewer_jwt=\"\$(cat /var/run/secrets/kubernetes.io/serviceaccount/token)\" kubernetes_host=\"https://\$KUBERNETES_PORT_443_TCP_ADDR:443\" kubernetes_ca_cert=@/var/run/secrets/kubernetes.io/serviceaccount/ca.crt"