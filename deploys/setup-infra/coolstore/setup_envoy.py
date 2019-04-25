import os, subprocess, json, time, sys
from pathlib import Path

base_path = Path(__file__).parent
config_file_path = (base_path / '../configs/config.json').resolve()

with open(config_file_path) as config_file:
    config_data = json.load(config_file)

def run(args):
    subprocess.call(['kubectl', 'config', 'use-context', config_data["aks"]["name"]])

    istio_path = (base_path / '../../istio').resolve()
    print('Install envoy filter')
    subprocess.call(['kubectl', 'apply', '-f', Path.joinpath(istio_path, 'envoy-filter.yaml')])
    print('Install sidecar')
    subprocess.call(['kubectl', 'apply', '-f', Path.joinpath(istio_path, 'istio-sidecar-injector.yaml')])

if __name__ == "__main__":
    run(sys.argv[1:])