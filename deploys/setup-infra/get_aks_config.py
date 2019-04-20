import os
import json

from azure.cli.core import get_default_cli

with open("configs/config.json") as config_file:
    config_data = json.load(config_file)

def login_azure():
    get_default_cli().invoke(['login', "--service-principal", 
        "--username", config_data["azure"]["client_id"], 
        "--tenant", config_data["azure"]["tenant_id"],
        "--password", config_data["azure"]["client_secret"]])

def logout_azure():
    get_default_cli().invoke(['logout'])

def run():
    login_azure()

    get_default_cli().invoke(['aks', 'get-credentials', 
        '--resource-group', config_data['resource_group']['name'],
        '--name', config_data['aks']['name']])

    logout_azure()

if __name__ == "__main__":
    run()