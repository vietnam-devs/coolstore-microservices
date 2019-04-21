import os, subprocess, json, time

with open("configs/config.json") as config_file:
    config_data = json.load(config_file)

def run():    
    subprocess.call(['kubectl', 'config', 'use-context', config_data["aks"]["name"]])
    
    cmd = "curl -L https://git.io/getLatestIstio | ISTIO_VERSION=1.1.3 sh -"
    ps = subprocess.Popen(cmd, shell=True, stdout=subprocess.PIPE,stderr=subprocess.STDOUT)
    print(ps.communicate()[0])

    subprocess.call(['helm', 'upgrade', '--install', 'istio-init', './istio-1.1.3/install/kubernetes/helm/istio-init', '--namespace', 'istio-system'])

    time.sleep(60)

    subprocess.call(['helm', 'upgrade', '--install', 'istio', './istio-1.1.3/install/kubernetes/helm/istio', '--namespace', 'istio-system'])

    # remove istio-1.1.3 folder after install
    subprocess.call(['rm', '-rf', 'istio-1.1.3'])

if __name__ == "__main__":
    run()