# Guppy
A simple game framework built on top of MonoGame


## Custom Events
Custom events are events bound by a simple string and can dynamically be invoked at any time. They are implemented by the Driven class.

| Project | Class | Event | Arg | Description |
| ------- | ----- | ----- | --------- | ----------- |
| `Guppy` | `Reusable` | `disposing` |` DateTime` | Invoked when the child is disposed. |
| `Guppy` | `Reusable` | `changed:enabled` |` Boolean` | Invoked when the enabled value is updated. |
| `Guppy` | `Reusable` | `changed:visible` | `Boolean` | Invoked when the visible value is updated. |
| `Guppy` | `Reusable` | `changed:update-order` | `Int32` | Invoked when the update order value is changed. |
| `Guppy` | `Reusable` | `changed:draw` | `Int32` | Invoked when the draw value is updated. |
| | | | |
| `Guppy` | `FrameableCollection<T>` | `added` | `T` | Invoked when an item is added to the collection. |
| `Guppy` | `FrameableCollection<T>` | `removed` | `T` | Invoked when an item is removed from the collection. |
| | | | |
