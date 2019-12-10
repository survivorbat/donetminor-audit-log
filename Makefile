SHELL := /bin/bash

MAKEFLAGS := --no-print-directory

.DEFAULT_GOAL := help

.PHONY := help

help: ## Show the list of commands
	@echo "Please use 'make <target>' where <target> is one of"
	@awk 'BEGIN {FS = ":.*?## "} /^[a-zA-Z0-9\._-]+:.*?## / {printf "\033[36m%-30s\033[0m %s\n", $$1, $$2}' $(MAKEFILE_LIST)

build: ## Build the docker containers
	dotnet publish \
		src/MaartenH.Minor.Miffy.AuditLogging.Server/MaartenH.Minor.Miffy.AuditLogging.Server.csproj \
		-c Release \
		-o src/MaartenH.Minor.Miffy.AuditLogging.Server/obj/Docker/publish

	dotnet publish \
		src/ExampleService/ExampleService.csproj \
		-c Release \
		-o src/ExampleService/obj/Docker/publish

	docker-compose -f src/docker-compose.yaml -p auditlogger build

up: ## Start the containers in docker-compose
	make build
	docker-compose -f src/docker-compose.yaml -p auditlogger up -d

down: ## Stop the containers
	docker-compose -f src/docker-compose.yaml -p auditlogger down
