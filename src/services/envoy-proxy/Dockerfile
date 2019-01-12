FROM envoyproxy/envoy:latest

COPY src/services/envoy-proxy/envoy.yaml /etc/envoy.yaml
COPY src/services/envoy-proxy/proto.pb /tmp/envoy/proto.pb

CMD /usr/local/bin/envoy -c /etc/envoy.yaml -l debug --service-cluster envoy-proxy
