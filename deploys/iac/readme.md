# Up and running 3 nodes (1 master and 2 workers) with Vagrant

- Step 1: Install `Vagrant`
- Step 2: Enabled `Hyper-V`
- Step 3: Create `Internal Network Switch` with named `sample_switch`, more information at https://gist.github.com/savishy/8ed40cd8692e295d64f45e299c2b83c9
- Step 4: Up and running as below

```
> vagrant up
```
- Step 5: Now we have `vagrant/vagrant` to work.

More work on making Vagrant works with Hyper-V and Ansible on Windows at:
- https://blog.zencoffee.org/2016/08/ansible-vagrant-windows/
- https://gist.github.com/jmyoung/fb034677ad332da5809fed4698ce55dc