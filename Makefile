WEB_CONFIG_DIR = $(shell pwd)/web_configs/node_local_web_server
WEB_CONFIG = .local-web-server.json

PROD_DIR = $(shell pwd)/production_build
CLIENT_SUBDIR = $(PROD_DIR)/client
SERVER_SUBDIR = $(PROD_DIR)/server

WS_PORT = 8000
SERVER_PORT = 5000
CLIENT_PORT = 5002
MOCK_PORT = 3333

DOTNET = dotnet

run: export ASPNETCORE_ENVIRONMENT = Development
run: build_dev
	cd ./angular-client && ng serve --port $(CLIENT_PORT) &
	cd ./dotnet-server/HospitalAllocation && $(DOTNET) run &
	ws \
		--rewrite '/api/* -> http://localhost:$(SERVER_PORT)/$$1' \
		--rewrite '/* -> http://localhost:$(CLIENT_PORT)/$$1'

run_demo: export ASPNETCORE_ENVIRONMENT = Development
run_demo: build_dev
		cd ./angular-client && ng serve --prod --no-aot --port $(CLIENT_PORT) &
		cd ./dotnet-server/HospitalAllocation && $(DOTNET) run &
		ws \
				--rewrite '/api/* -> http://localhost:$(SERVER_PORT)/$$1' \
				--rewrite '/* -> http://localhost:$(CLIENT_PORT)/$$1'

run_mock:
	cd ./angular-client && ng serve --port $(CLIENT_PORT) &
	cd ./mock-api && node server.js &
	ws \
		--rewrite '/api/* -> http://localhost:$(MOCK_PORT)/api/$$1' \
		--rewrite '/* -> http://localhost:$(CLIENT_PORT)/$$1'

build_common:
	cd ./dotnet-server/HospitalAllocation && $(DOTNET) restore
	cd ./angular-client && npm install

build_dev: build_common

build_prod: clean build_common
	cd ./angular-client && ng build --prod --output-path $(CLIENT_SUBDIR)&
	cd ./dotnet-server/HospitalAllocation && $(DOTNET) publish \
		-c Release \
		--output $(SERVER_SUBDIR) &

clean:
	if [ -e $(WEB_CONFIG) ]; \
		then \
			rm $(WEB_CONFIG) ; \
	fi;
	rm -rf $(PROD_DIR)

kill_ports:
	for port in $(WS_PORT) $(CLIENT_PORT) $(SERVER_PORT) $(MOCK_PORT) ; do \
		lsof -i tcp:$${port} | awk 'NR!=1 {print $$2}' | xargs kill -TERM ; \
	done

install_node_deps:
	npm install -g @angular/cli
	npm install -g local-web-server

test: test_backend

test_backend:
	cd ./dotnet-server/HospitalAllocation.Tests && $(DOTNET) restore
	cd ./dotnet-server/HospitalAllocation.Tests && $(DOTNET) test

remake_database: nuke_database make_database

nuke_database:
	cd ./dotnet-server/HospitalAllocation && $(DOTNET) restore
	cd ./dotnet-server/HospitalAllocation && $(DOTNET) ef database drop -f

make_database:
	cd ./dotnet-server/HospitalAllocation && $(DOTNET) restore
	cd ./dotnet-server/HospitalAllocation && $(DOTNET) ef database update
