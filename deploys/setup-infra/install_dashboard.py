import os, subprocess
import json

with open("configs/config.json") as config_file:
    config_data = json.load(config_file)

def run():    
    subprocess.call(['kubectl', 'config', 'use-context', config_data["aks"]["name"]])
    subprocess.call(['kubectl', 'create', 
        'clusterrolebinding', 
        'kubernetes-dashboard', 
        '-n', 'kube-system',
        '--clusterrole', 'cluster-admin',
        '--serviceaccount', 'kube-system:kubernetes-dashboard'])    

if __name__ == "__main__":
    run()