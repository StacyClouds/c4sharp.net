#!/usr/bin/env bash
set -euo pipefail

if [[ -x ./.dotnet-tools/dotnet-stryker ]]; then
	./.dotnet-tools/dotnet-stryker --config-file stryker-config.json
else
	dotnet stryker --config-file stryker-config.json
fi
