### Up and running coolstore

```bash
$ kubectl apply -f coolstore.yaml
```

Waiting a minute until all pods running with UP status

```bash
$ kubectl apply -f coolstore-migration.yaml
```

### Linkerd injection

```bash
$ kubectl get -n coolstore deploy -o yaml | linkerd inject - | kubectl apply -f -
$ linkerd dashboard --port 9999
```
