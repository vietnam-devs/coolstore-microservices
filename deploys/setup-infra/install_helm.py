import os, subprocess
import json

with open("configs/config.json") as config_file:
    config_data = json.load(config_file)

def run():    
    subprocess.call(['kubectl', 'config', 'use-context', config_data["aks"]["name"]])
    subprocess.call(['kubectl', 'apply', '-f', 'configs/helm_init.yaml'])
    subprocess.call(['helm', 'init', '--service-account', 'tiller'])

if __name__ == "__main__":
    run()