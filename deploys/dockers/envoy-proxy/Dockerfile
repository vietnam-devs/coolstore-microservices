FROM envoyproxy/envoy:latest

COPY deploys/dockers/envoy-proxy/envoy.yaml /etc/envoy.yaml

CMD /usr/local/bin/envoy -c /etc/envoy.yaml -l debug --service-cluster envoy-proxy
