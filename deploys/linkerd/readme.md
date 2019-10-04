### Up and running coolstore

```bash
$ kubectl apply -f coolstore-infrastructure.yaml
```

Waiting a minute until all pods running with UP status

```bash
$ kubectl apply -f coolstore-migration.yaml
```

```bash
$ kubectl apply -f coolstore.yaml
```

### Linkerd injection

```bash
$ kubectl get -n coolstore deploy -o yaml | linkerd inject - | kubectl apply -f -
$ linkerd dashboard --port 9999
```

### Clean up

```bash
$ kubectl delete -f coolstore.yaml
$ kubectl delete -f coolstore-migration.yaml
$ kubectl delete -f coolstore-infrastructure.yaml
```
