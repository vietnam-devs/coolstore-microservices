package main

import (
	"log"

	"github.com/gofiber/fiber/v2"
	"github.com/gofiber/fiber/v2/middleware/cors"
	"github.com/gofiber/fiber/v2/middleware/logger"
	"github.com/joho/godotenv"
)

func main() {
	if err := godotenv.Load(".devcontainer/.env"); err != nil {
		log.Println("warning .env file load error", err)
	}

	app := fiber.New()

	app.Use(cors.New())
	app.Use(logger.New())

	log.Fatal(app.Listen(":5005"))
}
