import os, subprocess, json, time, sys
from pathlib import Path

base_path = Path(__file__).parent
config_file_path = (base_path / '../configs/config.json').resolve()

with open(config_file_path) as config_file:
    config_data = json.load(config_file)

def run(args):
    subprocess.call(['kubectl', 'config', 'use-context', config_data["aks"]["name"]])
    
    ns = args[0]

    print('Create namespace %s' % ns)
    subprocess.call(['kubectl', 'create', 'namespace', ns])
    subprocess.call(['kubectl', 'label', 'namespace', ns, 'istio-injection=enabled'])

    print('Setup Coolstore data storage')
    coolstore_path = (base_path / '../../charts/coolstore-datastorage').resolve()
    subprocess.call(['helm', 'upgrade', '--install', 'coolstore-datastorage-%s' %ns , coolstore_path, '--namespace', ns, '-f', '%s/values.aks.yaml' % coolstore_path])
    print('Setup Coolstore')
    coolstore_path = (base_path / '../../charts/coolstore').resolve()
    subprocess.call(['helm', 'upgrade', '--install', 'coolstore-%s' %ns , coolstore_path, '--namespace', ns, '-f', '%s/values.aks.yaml' % coolstore_path])
    print('Setup traffic')
    coolstore_traffic_path = (base_path / '../../istio/traffic-management').resolve()
    subprocess.call(['helm', 'upgrade', '--install', 'coolstore-traffic-%s' %ns , coolstore_traffic_path, '--namespace', ns, '-f', '%s/values.aks.yaml' % coolstore_path])

if __name__ == "__main__":
    run(sys.argv[1:])