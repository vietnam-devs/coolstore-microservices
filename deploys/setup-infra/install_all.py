import get_aks_config
import install_helm
import install_istio
import install_dashboard
import create_aks
import time

def run():
    create_aks.run()
    
    get_aks_config.run()

    install_helm.run()

    time.sleep(60)

    install_dashboard.run()

    install_istio.run()

if __name__ == "__main__":
    run()