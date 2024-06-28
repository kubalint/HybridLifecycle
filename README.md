Hybrid and Custom lifecycle implementation:

Hybrid Lifecycle
A hybrid lifecycle means that the service has different lifecycles in different contexts. In our HybridConfigurationService class, this is achieved as follows:

Singleton Instance:
If no scopeId is provided, the singleton instance is used. This instance will exist for the entire lifetime of the application and will be used for all requests.

"Scoped" instances:
If a scopeId is provided, a scoped  (not the default "scoped" DI lifecycle) instance is created and used. The same scoped instance is used for the same scopeId, while different scopeIds will result in different instances.

Custom Lifecycle
A custom lifecycle means that the lifecycle management is defined by the developer and does not follow the default DI lifecycles (e.g., singleton, scoped, transient). This is implemented in the following way:

Manual Management of Instances:
"Scoped" instances are created and stored based on custom logic in the HybridConfigurationService class.
Instances are manually disposed of at the end of their lifecycle or through explicit calls.

Automatic Cleanup:
Using a Timer, we periodically clean up expired scoped instances to avoid memory leaks.
