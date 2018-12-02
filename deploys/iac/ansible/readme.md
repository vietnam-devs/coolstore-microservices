# Up and running Docker and Kubernetes with Ansible

- Step 1: Install `ansible` in WSL (Windows 10) as below

```
> cd ~
> sudo apt-get install software-properties-common
> sudo apt-add-repository ppa:ansible/ansible
> sudo apt-get update
> sudo apt-get dist-upgrade -y # if install python
> sudo apt-get install python # if install python
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

### Install with Ubuntu

We have `master01`, `worker01` and `worker02`, let say the IPs of its like

```
> 192.168.137.1   master01
> 192.168.137.2   worker01
> 192.168.137.3   worker02
```

- Step 1: Copy `ansible scripts` into `master01`

```
> bash # in root folder, let say coolstore-microservices
> scp -r deploys/iac/ vagrant@192.168.137.1:/home/vagrant/iac/
```

- Step 2: `ssh` into `master01` (make it a controller), then

```
> ssh-keygen # confirm with all default settings
> ssh-copy-id vagrant@192.168.137.2
> ssh-copy-id vagrant@192.168.137.3
```

More information at https://www.digitalocean.com/community/tutorials/ssh-essentials-working-with-ssh-servers-clients-and-keys

- Step 3: Edit `/ect/hosts` to add 2 workers

```
> sudo nano /etc/hosts
```

Put below to this file

```
192.168.137.1   master01
192.168.137.2   worker01
192.168.137.3   worker02
```

Save it!

- Step 2: Run ansible scripts to provision Kubernetes cluster

```
> ansible-playbook -i hosts initial.yml
> ansible-playbook -i hosts kube-dependencies.yml
> ansible-playbook -i hosts master.yml
> ansible-playbook -i hosts workers.yml
```

More information at https://www.digitalocean.com/community/tutorials/how-to-create-a-kubernetes-1-11-cluster-using-kubeadm-on-ubuntu-18-04