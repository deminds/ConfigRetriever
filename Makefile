.PHONY: unit-test
unit-test:
	dotnet test --filter TestCategory!=integration *.Tests

.PHONY: integration-test
integration-test:
	sudo docker-compose -f docker-compose-integration-test.yml up --abort-on-container-exit
	sudo docker-compose -f docker-compose-integration-test.yml down --rmi local
