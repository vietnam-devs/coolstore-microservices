#############################################################################
# You should get ingress IP update to Helm chart, then update hosts IP first
#############################################################################

Write-Host "::Delete all coolstore resources..."
kubectl delete -f deploys/k8s/coolstore.aks.yaml

Write-Host "::Delete all coolstore-istio resources..."
kubectl delete -f deploys/k8s/coolstore-istio.aks.yaml

Write-Host "::Export coolstore chart with configuration..."
helm template deploys/charts/coolstore -f deploys/charts/coolstore/values.aks.yaml -f deploys/charts/coolstore/values.aks.local.yaml > deploys/k8s/coolstore.aks.yaml

Write-Host "::Export coolstore-istio chart with configuration..."
helm template deploys/charts/coolstore-istio -f deploys/charts/coolstore-istio/values.aks.yaml > deploys/k8s/coolstore-istio.aks.yaml

Write-Host "::Apply coolstore kubernetes with istio injected..."
istioctl kube-inject -f deploys/k8s/coolstore.aks.yaml | kubectl apply -f -

Write-Host "::Apply coolstore-istio kubernetes with istio injected..."
istioctl kube-inject -f deploys/k8s/coolstore-istio.aks.yaml | kubectl apply -f -
