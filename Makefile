SHELL := /bin/bash

MAKEFLAGS := --no-print-directory

.DEFAULT_GOAL := help

.PHONY := help

help: ## Show the list of commands
	@echo "Please use 'make <target>' where <target> is one of"
	@awk 'BEGIN {FS = ":.*?## "} /^[a-zA-Z0-9\._-]+:.*?## / {printf "\033[36m%-30s\033[0m %s\n", $$1, $$2}' $(MAKEFILE_LIST)

build: ## Build the docker containers
	dotnet publish \
		MaartenH.Minor.Miffy.AuditLogging.Server/MaartenH.Minor.Miffy.AuditLogging.Server.csproj \
		-c Release \
		-o MaartenH.Minor.Miffy.AuditLogging.Server/obj/Docker/publish

	dotnet publish \
		ExampleService/ExampleService.csproj \
		-c Release \
		-o ExampleService/obj/Docker/publish

	docker-compose -f docker-compose.yaml -p auditlogger build

audit.logs: ## See the logs of the audit service
	docker logs -f auditlogger_auditlog_1

example.logs: ## See the logs of the example service
	docker logs -f auditlogger_exampleservice_1

up: ## Start the containers in docker-compose
	make build
	docker-compose -f docker-compose.yaml -p auditlogger up -d

down: ## Stop the containers
	docker-compose -f docker-compose.yaml -p auditlogger down
