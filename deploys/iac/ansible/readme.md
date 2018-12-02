# Up and running Docker and Kubernetes with Ansible

- Step 1: Install `ansible` in WSL (Windows 10) as below

```
> sudo apt-get install software-properties-common
> sudo apt-add-repository ppa:ansible/ansible
> sudo apt-get update
> sudo apt-get install ansible
```

Read more information at https://www.vagrantup.com/docs/other/wsl.html

- Step 1: change config host in `hosts.yml` which points to the vagrant machine IP
- Step 2: run command as below to provision this machine:

```
> bash # on WSL (Windows 10) 
> sudo su
> ansible-playbook configuration.yml -i hosts.yml --ask-become-pass
> or ansible-playbook configuration.yml -i hosts.yml --ask-pass
```