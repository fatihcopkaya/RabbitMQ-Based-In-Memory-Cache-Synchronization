# RabbitMQ-Based In-Memory Cache Synchronization (With Multi-Instance Support)
 This project demonstrates a solution to synchronize in-memory cache across multiple instances of the same application running on different servers, without using distributed cache. It uses RabbitMQ to propagate cache updates to other instances, ensuring cache consistency. Each instance connects to RabbitMQ with a unique queue and listens for cache update messages. When a cache update happens in one instance, the change is automatically propagated to all other instances using RabbitMQ's fanout exchange.  In this system, RabbitMQ's fanout exchange ensures that cache updates are broadcast to all instances. Each instance has its own unique queue, and all queues are bound to the same exchange. This way, when an update is made in one instance, it is automatically sent to all instances.
