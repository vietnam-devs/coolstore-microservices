FROM  rust:1.59-slim-buster

RUN USER=root cargo new --bin inventory
WORKDIR /inventory
COPY ./Cargo.toml ./Cargo.toml
RUN cargo build

RUN rm src/*.rs
COPY ./src ./src
RUN rm ./target/debug/deps/inventory*
RUN cargo build

RUN pwd
RUN ls -la target/debug

CMD ["./target/debug/server"]

EXPOSE 80
