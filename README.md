# Guppy
A simple game framework built on top of MonoGame


## Custom Events
Custom events are events bound by a simple string and can dynamically be invoked at any time. They are implemented by the Driven class.

| Project | Class | Event | Arg | Description |
| ------- | ----- | ----- | --------- | ----------- |
| `Guppy` | `Creatable` | `disposing` |` Creatable` | Invoked when the child is disposed. |
| | | | |
| `Guppy` | `Frameable` | `changed:enabled` |` Boolean` | Invoked when the enabled value is updated. |
| `Guppy` | `Frameable` | `changed:visible` | `Boolean` | Invoked when the visible value is updated. |
| `Guppy` | `Frameable` | `changed:update-order` | `Int32` | Invoked when the update order value is changed. |
| `Guppy` | `Frameable` | `changed:draw` | `Int32` | Invoked when the draw value is updated. |
| | | | |
| `Guppy` | `Entity` | `changed:layer` | `Layer` | Invoked when the layer is updated. |
| | | | |
| `Guppy` | `UniqueCollection<T>` | `added` | `T` | Invoked when an item is added to the collection. |
| `Guppy` | `UniqueCollection<T>` | `removed` | `T` | Invoked when an item is removed from the collection. |
| | | | |
| `Guppy` | `PooledFactory<T>` | `pulled` | `T` | Invoked when an item is pulled from the factory. |
| | | | |
| `Guppy.Network` | `Peer` | `recieved:data` | `NetIncomingMessage` | Invoked when an the peer recieves a data message. |
| `Guppy.Network` | `Peer` | `recieved:error` | `NetIncomingMessage` | Invoked when an the peer recieves an error message. |
| `Guppy.Network` | `Peer` | `recieved:status-changed` | `NetIncomingMessage` | Invoked when an the peer recieves a status changed message. |
| `Guppy.Network` | `Peer` | `recieved:unconnected-data` | `NetIncomingMessage` | Invoked when an the peer recieves an unconnected data message. |
| `Guppy.Network` | `Peer` | `recieved:connection-approval` | `NetIncomingMessage` | Invoked when an the peer recieves a connection approval message. |
| `Guppy.Network` | `Peer` | `recieved:reciept` | `NetIncomingMessage` | Invoked when an the peer recieves a reciept message. |
| `Guppy.Network` | `Peer` | `recieved:discovery-request` | `NetIncomingMessage` | Invoked when an the peer recieves a discovery request message. |
| `Guppy.Network` | `Peer` | `recieved:discovery-response` | `NetIncomingMessage` | Invoked when an the peer recieves a discovery response message. |
| `Guppy.Network` | `Peer` | `recieved:verbose-debug-message` | `NetIncomingMessage` | Invoked when an the peer recieves a verbose debug message message. |
| `Guppy.Network` | `Peer` | `recieved:debug-message` | `NetIncomingMessage` | Invoked when an the peer recieves a debug message message. |
| `Guppy.Network` | `Peer` | `recieved:warning-message` | `NetIncomingMessage` | Invoked when an the peer recieves a warning message message. |
| `Guppy.Network` | `Peer` | `recieved:error-message` | `NetIncomingMessage` | Invoked when an the peer recieves an error message message. |
| `Guppy.Network` | `Peer` | `recieved:nat-introduction-success` | `NetIncomingMessage` | Invoked when an the peer recieves a nat introduction success message. |
| `Guppy.Network` | `Peer` | `recieved:connection-latency-updated` | `NetIncomingMessage` | Invoked when an the peer recieves a connection latency updated message. |
| | | | |