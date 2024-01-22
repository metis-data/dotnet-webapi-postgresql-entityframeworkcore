curl -sSfL https://github.com/open-telemetry/opentelemetry-dotnet-instrumentation/releases/latest/download/otel-dotnet-auto-install.sh -O
sh ./otel-dotnet-auto-install.sh
chmod +x $HOME/.otel-dotnet-auto/instrument.sh
. $HOME/.otel-dotnet-auto/instrument.sh
OTEL_TRACES_EXPORTER="otlp" \
	OTEL_EXPORTER_OTLP_PROTOCOL=http/protobuf \
	OTEL_EXPORTER_OTLP_ENDPOINT="http://127.0.0.1:4318" \
	OTEL_SERVICE_NAME="entityframeworkcore" \
    dotnet run --project src/dotnet-webapi-postgresql-entityframeworkcore.csproj