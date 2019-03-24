FROM brennovich/protobuf-tools

COPY src/grpc/v1/*.proto ./v1/
COPY src/grpc/third_party/googleapis ./googleapis/
COPY src/grpc/third_party/grpc-gateway ./grpc-gateway/

CMD protoc -I./v1 -I/usr/local/include -I./googleapis  -I./grpc-gateway \
  --include_imports --include_source_info \
  --descriptor_set_out=/tmp/proto.pb ./v1/*.proto
