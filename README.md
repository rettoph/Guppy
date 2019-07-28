# Guppy
A simple game framework built on top of MonoGame


## Custom Events
Custom events are events bound by a simple string and can dynamically be invoked at any time. The are implemented by the Driven class.

| Project | Class | Event | Parameter | Description |
| Guppy | ----- | ----- | --------- | ----------- |
| Guppy | ZFrameable | set:enabled | Boolean | Invoked when the enabled value is updated. |
| Guppy | ZFrameable | set:visible | Boolean | Invoked when the visible value is updated. |
| Guppy | ZFrameable | set:update-order | Int32 | Invoked when the update order value is changed. |
| Guppy | ZFrameable | set:draw | Int32 | Invoked when the draw value is updated. |
| Guppy | | | |
| Guppy | Scene | set:active | Boolean | Invoked when the scene is marked as active or inactive. |
| Guppy | | | |
| Guppy | Entity | changed:layer-depth | Entity | Invoked when the entity's layer depth is changed. |
