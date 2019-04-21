import os
import json

from azure.common.credentials import ServicePrincipalCredentials
from azure.mgmt.resource import ResourceManagementClient
from azure.mgmt.network import NetworkManagementClient
from azure.mgmt.containerservice import ContainerServiceClient
from azure.mgmt.containerservice.models import ManagedCluster, ManagedClusterAgentPoolProfile, ManagedClusterAddonProfile, ManagedClusterServicePrincipalProfile

with open("configs/config.json") as config_file:
    config_data = json.load(config_file)


def get_credential():
    credentials = ServicePrincipalCredentials(
        client_id=config_data["azure"]["client_id"],
        secret=config_data["azure"]["client_secret"],
        tenant=config_data["azure"]["tenant_id"],
    )
    return credentials


def create_resource_group(credentials, subscription_id):
    resource_client = ResourceManagementClient(credentials, subscription_id)

    print("- Creating resource group: %s ..." %
          config_data["resource_group"]["name"])
    resource_client.resource_groups.create_or_update(
        config_data["resource_group"]["name"], config_data["resource_group"])


def create_virtual_network(credentials, subscription_id):
    network_client = NetworkManagementClient(credentials, subscription_id)

    print("- Creating virtual network...")
    async_vnet_creation = network_client.virtual_networks.create_or_update(
        config_data["resource_group"]["name"],
        config_data["virtual_network"]["vnet_name"],
        config_data["virtual_network"]
    )
    async_vnet_creation.wait()

    print("- Creating subnet...")
    async_subnet_creation = network_client.subnets.create_or_update(
        config_data["resource_group"]["name"],
        config_data["virtual_network"]["vnet_name"],
        config_data["virtual_network"]["subnet_1"]["name"],
        config_data["virtual_network"]["subnet_1"]
    )
    async_subnet_creation.wait()
    return async_subnet_creation.result()

def run():
    credentials = get_credential()
    subscription_id = config_data["azure"]["supscription_id"]

    create_resource_group(credentials, subscription_id)
    # subnet_info = create_virtual_network(credentials, subscription_id)

    container_service_client = ContainerServiceClient(
        credentials, subscription_id)

    addon_profiles = {}
    addon_profiles['httpapplicationrouting'] = ManagedClusterAddonProfile(
        enabled=False)
    managed_agent_pool = ManagedClusterAgentPoolProfile(
        name = "agentpool",
        count = config_data["aks"]["vm_count"],
        vm_size = config_data["aks"]["vm_size"],
        # vnet_subnet_id = subnet_info.id
    )

    managed_cluster = ManagedCluster(
        location=config_data["aks"]["location"],
        kubernetes_version=config_data["aks"]["k8s_version"],
        enable_rbac="true",
        dns_prefix="%s-dns" % config_data["aks"]["name"],
        service_principal_profile=ManagedClusterServicePrincipalProfile(
            client_id=config_data["azure"]["client_id"],
            secret=config_data["azure"]["client_secret"]
        ),
        addon_profiles=addon_profiles,
        agent_pool_profiles=[managed_agent_pool]
    )

    print("- Creating AKS...")
    async_aks_creation = container_service_client.managed_clusters.create_or_update(
        config_data["resource_group"]["name"],
        config_data["aks"]["name"],
        managed_cluster
    )
    async_aks_creation.wait()

if __name__ == "__main__":
    run()
