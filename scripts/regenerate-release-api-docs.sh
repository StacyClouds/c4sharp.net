#!/usr/bin/env bash

set -euo pipefail

SCRIPT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)
REPO_ROOT=$(cd "${SCRIPT_DIR}/.." && pwd)

PACKAGE_VERSION="${1:-${PACKAGE_VERSION:-}}"
DOCFX_TARGET_FRAMEWORK="${DOCFX_TARGET_FRAMEWORK:-net10.0}"
DOCUMENTED_PROJECTS=(
	"StacyClouds.C4Sharp.Core/StacyClouds.C4Sharp.Core.csproj"
	"StacyClouds.C4Sharp.Client/StacyClouds.C4Sharp.Client.csproj"
	"StacyClouds.C4Sharp.Renderer/StacyClouds.C4Sharp.Renderer.csproj"
	"StacyClouds.C4Sharp.Editor/StacyClouds.C4Sharp.Editor.csproj"
)

validate_package_version() {
	local package_version="$1"

	if [[ ! "${package_version}" =~ ^[0-9]+\.[0-9]+\.[0-9]+([-.][0-9A-Za-z.-]+)?(\+[0-9A-Za-z.-]+)?$ ]]; then
		echo "Invalid package version: ${package_version}" >&2
		return 1
	fi
}

write_release_notes() {
	local package_version="$1"
	local notes_file="$2"
	local release_date="${3:-$(date -u +"%Y-%m-%d")}"

	cat > "${notes_file}" <<EOF
---
title: Release ${package_version}
---

# C4Sharp.NET ${package_version} — Release Notes

Released: ${release_date}

## Package versions

All packages in this release are published at version \`${package_version}\`:

- [StacyClouds.C4Sharp.Core ${package_version}](https://www.nuget.org/packages/StacyClouds.C4Sharp.Core/${package_version})
- [StacyClouds.C4Sharp.Client ${package_version}](https://www.nuget.org/packages/StacyClouds.C4Sharp.Client/${package_version})
- [StacyClouds.C4Sharp.Renderer ${package_version}](https://www.nuget.org/packages/StacyClouds.C4Sharp.Renderer/${package_version})
- [StacyClouds.C4Sharp.Editor ${package_version}](https://www.nuget.org/packages/StacyClouds.C4Sharp.Editor/${package_version})

## API reference

Browse the [API reference](index.html) for the full public API at this version.
EOF
}

ensure_docfx_target_framework_supported() {
python - "${DOCFX_TARGET_FRAMEWORK}" "${DOCUMENTED_PROJECTS[@]}" <<'PY'
import sys
import xml.etree.ElementTree as ET

target_framework = sys.argv[1]
unsupported_projects = []

for project_path in sys.argv[2:]:
    root = ET.parse(project_path).getroot()
    frameworks = []

    for element in root.iter():
        if element.tag.endswith("TargetFramework") and element.text:
            frameworks.extend([value.strip() for value in element.text.split(";") if value.strip()])
        elif element.tag.endswith("TargetFrameworks") and element.text:
            frameworks.extend([value.strip() for value in element.text.split(";") if value.strip()])

    if target_framework not in frameworks:
        unsupported_projects.append(project_path)

if unsupported_projects:
    print(
        f"DOCFX_TARGET_FRAMEWORK '{target_framework}' is not supported by: "
        + ", ".join(unsupported_projects),
        file=sys.stderr,
    )
    sys.exit(1)
PY
}

main() {
	if [[ -z "${PACKAGE_VERSION}" ]]; then
		echo "Usage: scripts/regenerate-release-api-docs.sh <package-version>" >&2
		echo "Example: scripts/regenerate-release-api-docs.sh 0.9.7" >&2
		exit 1
	fi

	validate_package_version "${PACKAGE_VERSION}"

	cd "${REPO_ROOT}"
	ensure_docfx_target_framework_supported

	dotnet tool restore
	dotnet restore StacyClouds.C4Sharp.slnx -p:TargetFramework="${DOCFX_TARGET_FRAMEWORK}"
	dotnet build StacyClouds.C4Sharp.slnx --no-restore -c Release -p:TargetFramework="${DOCFX_TARGET_FRAMEWORK}"
	dotnet docfx metadata docfx.json --property "TargetFramework=${DOCFX_TARGET_FRAMEWORK}" --noRestore
	dotnet docfx build docfx.json

	NOTES_FILE="docs/api/release-notes-${PACKAGE_VERSION}.md"
	write_release_notes "${PACKAGE_VERSION}" "${NOTES_FILE}"

	echo "Regenerated docs/api/ for ${DOCFX_TARGET_FRAMEWORK} and wrote ${NOTES_FILE}"
}

if [[ "${BASH_SOURCE[0]}" == "${0}" ]]; then
	main "$@"
fi
