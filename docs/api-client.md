# Client package guide

Install `StacyClouds.C4Sharp.Client` when your application needs to read or publish workspaces through a Structurizr-compatible API.

```bash
dotnet add package StacyClouds.C4Sharp.Client
```

## Namespaces

```csharp
using StacyClouds.C4Sharp;
using StacyClouds.C4Sharp.Api;
```

## Create a client

Use the two-argument constructor for the hosted API endpoint.

```csharp
StructurizrClient client = new StructurizrClient("key", "secret");
```

Use the three-argument constructor when targeting a different base URL.

```csharp
StructurizrClient client = new StructurizrClient("https://your-structurizr-instance", "key", "secret");
```

## Download a workspace

```csharp
Workspace workspace = client.GetWorkspace(1234);
```

By default, downloaded JSON is archived to the current working directory through `WorkspaceArchiveLocation`. Set that property to `null` to disable archiving, or point it at a different directory.

## Publish a workspace

```csharp
client.PutWorkspace(1234, workspace);
```

`MergeFromRemote` defaults to `true`, which preserves matching remote layout information where possible when you upload an updated workspace.

## Locking and unlocking

Use locking when the target workspace supports shared editing and you want to reduce concurrent update conflicts.

```csharp
bool locked = client.LockWorkspace(1234);
bool unlocked = client.UnlockWorkspace(1234);
```

## Related topics

- [Client-side encryption](client-side-encryption.md)
- [Core package guide](getting-started.md)
