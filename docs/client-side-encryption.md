# Client-side encryption

`StacyClouds.C4Sharp.Client` can encrypt a workspace before upload so the encrypted payload is stored remotely and the passphrase remains with the client.

> Note: client-side encryption depends on support from the target Structurizr-compatible service.

## Namespaces

```csharp
using StacyClouds.C4Sharp.Api;
using StacyClouds.C4Sharp.Encryption;
```

## Enable encryption

```csharp
StructurizrClient client = new StructurizrClient("key", "secret");
client.EncryptionStrategy = new AesEncryptionStrategy("password");
client.PutWorkspace(1234, workspace);
```

## Advanced configuration

`AesEncryptionStrategy` defaults to a 128-bit key size and 1000 iterations. Use the overload that accepts key size, iteration count, and passphrase when you need to control those values explicitly.

See `StacyClouds.C4Sharp.Examples/ClientSideEncryption.cs` for a complete example.
