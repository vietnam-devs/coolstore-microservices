package main

import (
	"context"
	"log"
	"net/http"

	"github.com/dapr/go-sdk/service/common"
	daprd "github.com/dapr/go-sdk/service/http"
	"github.com/joho/godotenv"
)

var defaultSubscription = &common.Subscription{
	PubsubName: "pubsub",
	Topic:      "processing-order",
	Route:      "/orders",
}

func main() {
	if err := godotenv.Load(".devcontainer/.env"); err != nil {
		log.Println("warning .env file load error", err)
	}

	s := daprd.NewService(":5005")

	if err := s.AddTopicEventHandler(defaultSubscription, eventHandler); err != nil {
		log.Fatalf("error adding topic subscription: %v", err)
	}

	if err := s.Start(); err != nil && err != http.ErrServerClosed {
		log.Fatalf("error listenning: %v", err)
	}
}

func eventHandler(ctx context.Context, e *common.TopicEvent) (retry bool, err error) {
	log.Printf("event - PubsubName: %s, Topic: %s, ID: %s, Data: %s", e.PubsubName, e.Topic, e.ID, e.Data)
	return false, nil
}
