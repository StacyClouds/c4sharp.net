---
name: client
description: Use StacyClouds.C4Sharp.Client to publish, retrieve, and secure Structurizr workspaces.
license: MIT
compatibility: C4Sharp.NET repository context.
metadata:
  author: StacyClouds
  version: "1.0"
---

Use this skill for Structurizr API client integration.

## Install

```bash
dotnet add package StacyClouds.C4Sharp.Client
```

## Scope

- API client configuration
- Workspace upload/download flows
- Client-side encryption usage

## Prerequisite

The workspace is normally produced with `StacyClouds.C4Sharp.Core`.

## Guidance flow

1. Confirm target environment (cloud or on-prem).
2. Collect required identifiers and credentials.
3. Build upload/download flow around workspace lifecycle.
4. Add encryption guidance when security requirements exist.

## Usage checklist

- Confirm API URL/endpoint and workspace identifier.
- Avoid hardcoding credentials; use environment variables or secure secrets stores.
- Validate publish success and retrieve the same workspace for round-trip verification.

## API correctness protocol

- Verify client API signatures against package source and tests before giving code.
- Prefer guidance consistent with `docs/api-client.md` and `docs/client-side-encryption.md`.
- If authentication/encryption methods are uncertain, inspect implementation first and then answer.

## Authoritative references

- `docs/api-client.md`
- `docs/client-side-encryption.md`
- `StacyClouds.C4Sharp.Client/`
- `StacyClouds.C4Sharp.Client.Tests/`

## Expected output

- Minimal integration example using the Client package
- Required settings checklist (with validated key names)
- Validation steps for successful publish/retrieve operations
