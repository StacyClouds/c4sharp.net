#!/usr/bin/env bash

set -euo pipefail

SCRIPT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)
REPO_ROOT=$(cd "${SCRIPT_DIR}/.." && pwd)

PACKAGE_VERSION="${1:-${PACKAGE_VERSION:-}}"

if [[ -z "${PACKAGE_VERSION}" ]]; then
	echo "Usage: scripts/regenerate-release-api-docs.sh <package-version>" >&2
	echo "Example: scripts/regenerate-release-api-docs.sh 0.9.7" >&2
	exit 1
fi

if [[ ! "${PACKAGE_VERSION}" =~ ^[0-9]+\.[0-9]+\.[0-9]+([-.][0-9A-Za-z.-]+)?$ ]]; then
	echo "Invalid package version: ${PACKAGE_VERSION}" >&2
	exit 1
fi

cd "${REPO_ROOT}"

dotnet tool restore
dotnet restore StacyClouds.C4Sharp.slnx -p:TargetFramework=net10.0
dotnet build StacyClouds.C4Sharp.slnx --no-restore -c Release -p:TargetFramework=net10.0
dotnet docfx metadata docfx.json
dotnet docfx build docfx.json

RELEASE_DATE=$(date -u +"%Y-%m-%d")
NOTES_FILE="docs/api/release-notes-${PACKAGE_VERSION}.md"

cat > "${NOTES_FILE}" <<EOF
---
title: Release ${PACKAGE_VERSION}
---

# C4Sharp.NET ${PACKAGE_VERSION} — Release Notes

Released: ${RELEASE_DATE}

## Package versions

All packages in this release are published at version \`${PACKAGE_VERSION}\`:

- [StacyClouds.C4Sharp.Core ${PACKAGE_VERSION}](https://www.nuget.org/packages/StacyClouds.C4Sharp.Core/${PACKAGE_VERSION})
- [StacyClouds.C4Sharp.Client ${PACKAGE_VERSION}](https://www.nuget.org/packages/StacyClouds.C4Sharp.Client/${PACKAGE_VERSION})
- [StacyClouds.C4Sharp.Renderer ${PACKAGE_VERSION}](https://www.nuget.org/packages/StacyClouds.C4Sharp.Renderer/${PACKAGE_VERSION})
- [StacyClouds.C4Sharp.Editor ${PACKAGE_VERSION}](https://www.nuget.org/packages/StacyClouds.C4Sharp.Editor/${PACKAGE_VERSION})

## API reference

Browse the [API reference](index.html) for the full public API at this version.
EOF

echo "Regenerated docs/api/ and wrote ${NOTES_FILE}"
