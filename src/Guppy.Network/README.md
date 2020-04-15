# Guppy.Network
Small networking based library implimenting simple Group based communication.

Simple call GuppyLoader.ConfigureNetwork to setup Guppy.Network in your Guppy instance.

## Message Structure
[MessageTarget(Peer)][MessageType(xxHashString)][CustomData]
[MessageTarget(Group)][GroupId(Guid)][MessageType(xxHashString)][CustomData] 